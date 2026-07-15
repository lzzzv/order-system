using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 點餐頁面
{
    public static class Db
    {
        public static string DbFileName = "點餐系統資料庫.accdb";

        private static string GetDbPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string p = Path.GetFullPath(Path.Combine(baseDir, @"..\..\", DbFileName));
            if (!File.Exists(p)) throw new FileNotFoundException("找不到資料庫檔案", p);
            return p;
        }

        private static OleDbConnection Open()
        {
            string dbPath = GetDbPath();
            string connStr =
                "Provider=Microsoft.ACE.OLEDB.12.0;" +
                $"Data Source={dbPath};" +
                "Persist Security Info=False;";
            var cn = new OleDbConnection(connStr);
            cn.Open();
            return cn;
        }

        private static int ScalarInt(OleDbConnection cn, string sql)
        {
            using (var cmd = new OleDbCommand(sql, cn))
            {
                object v = cmd.ExecuteScalar();
                if (v == null || v == DBNull.Value) return 0;
                return Convert.ToInt32(v);
            }
        }

        public static string DebugCounts()
        {
            var sb = new StringBuilder();
            sb.AppendLine("DB Path:");
            sb.AppendLine(GetDbPath());
            sb.AppendLine();

            using (var cn = Open())
            {
                sb.AppendLine($"PRODUCT: {ScalarInt(cn, "SELECT COUNT(*) FROM PRODUCT")}");
                sb.AppendLine($"CATEGORY: {ScalarInt(cn, "SELECT COUNT(*) FROM CATEGORY")}");
                sb.AppendLine($"ORDERS: {ScalarInt(cn, "SELECT COUNT(*) FROM [ORDERS]")}");
                sb.AppendLine($"訂單明細: {ScalarInt(cn, "SELECT COUNT(*) FROM [訂單明細]")}");
                sb.AppendLine($"PAYMENT: {ScalarInt(cn, "SELECT COUNT(*) FROM PAYMENT")}");
                sb.AppendLine();
                sb.AppendLine($"PAID ORDERS: {ScalarInt(cn, "SELECT COUNT(*) FROM [ORDERS] WHERE O_STATUS='PAID'")}");
                sb.AppendLine($"SUM(QTY) in 訂單明細: {ScalarInt(cn, "SELECT SUM(QTY) FROM [訂單明細]")}");
            }

            return sb.ToString();
        }

        public class CatRow
        {
            public int CatId { get; set; }
            public string CatName { get; set; }
        }

        public class ProductRow
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int CatId { get; set; }
            public string ImgFile { get; set; } 
        }
        public static List<CatRow> GetCategories()
        {
            var list = new List<CatRow>();

            using (var cn = Open())
            using (var cmd = new OleDbCommand("SELECT CAT_ID, CAT_NAME FROM CATEGORY ORDER BY CAT_ID", cn))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    list.Add(new CatRow
                    {
                        CatId = Convert.ToInt32(rd["CAT_ID"]),
                        CatName = Convert.ToString(rd["CAT_NAME"])
                    });
                }
            }
            return list;
        }

        public static List<ProductRow> GetProducts(int? catId)
        {
            var list = new List<ProductRow>();

            var sql = new StringBuilder();
            sql.Append("SELECT P_ID, P_NAME, P_PRICE AS PriceNum, CAT_ID, P_IMG_URL ");
            sql.Append("FROM PRODUCT ");
            sql.Append("WHERE P_ENABLE=True ");
            if (catId.HasValue) sql.Append("AND CAT_ID=? ");
            sql.Append("ORDER BY P_ID");

            using (var cn = Open())
            using (var cmd = new OleDbCommand(sql.ToString(), cn))
            {
                if (catId.HasValue)
                    cmd.Parameters.Add("?", OleDbType.Integer).Value = catId.Value;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        decimal price = 0m;
                        if (rd["PriceNum"] != DBNull.Value)
                            price = Convert.ToDecimal(rd["PriceNum"]);

                        list.Add(new ProductRow
                        {
                            ProductId = Convert.ToInt32(rd["P_ID"]),
                            Name = Convert.ToString(rd["P_NAME"]),
                            Price = price,
                            CatId = Convert.ToInt32(rd["CAT_ID"]),
                            ImgFile = Convert.ToString(rd["P_IMG_URL"])
                        });
                    }
                }
            }

            return list;
        }

        public static int CreateOrderWithPayment(
            Dictionary<int, (int qty, decimal price)> items,
            decimal total,
            int customerId,
            int employeeId,
            string payWay,
            DateTime payTime,
            string orderStatus = "PAID"
        )
        {
            if (items == null || items.Count == 0)
                throw new InvalidOperationException("items 為空");

            using (var cn = Open())
            using (var tx = cn.BeginTransaction())
            {
                try
                {
 
                    using (var cmd = new OleDbCommand(
                        "INSERT INTO [ORDERS] (O_DATE, O_TOTAL, C_ID, E_ID, O_STATUS) VALUES (?, ?, ?, ?, ?);",
                        cn, tx))
                    {
                        cmd.Parameters.Add("?", OleDbType.Date).Value = payTime;              
                        cmd.Parameters.Add("?", OleDbType.Currency).Value = total;          
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = customerId;       
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = employeeId;        
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = orderStatus;      
                        cmd.ExecuteNonQuery();
                    }

                    int orderId;
                    using (var cmd = new OleDbCommand("SELECT @@IDENTITY;", cn, tx))
                    {
                        orderId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    foreach (var kv in items)
                    {
                        int productId = kv.Key;
                        int qty = kv.Value.qty;
                        decimal price = kv.Value.price;
                        decimal subtotal = qty * price;

                        using (var cmd = new OleDbCommand(
                            "INSERT INTO [訂單明細] (O_ID, P_ID, QTY, SUBTOTAL) VALUES (?, ?, ?, ?);",
                            cn, tx))
                        {
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = orderId;        
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = productId;     
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = qty;            
                            cmd.Parameters.Add("?", OleDbType.Currency).Value = subtotal;      
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (var cmd = new OleDbCommand(
                        "INSERT INTO PAYMENT (O_ID, PAY_WAY, PAY_TIME) VALUES (?, ?, ?);",
                        cn, tx))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = orderId;
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = payWay;
                        cmd.Parameters.Add("?", OleDbType.Date).Value = payTime;
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                    return orderId;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public static List<ProductSale> GetTopSellers(int topN)
        {
            if (topN < 1) topN = 1;
            if (topN > 20) topN = 20;

            string sql =
             $@"SELECT TOP {topN}
             p.P_ID AS ProductId,
             p.P_NAME AS Name,
             IIf(IsNull(p.P_PRICE), 0, p.P_PRICE) AS PriceNum,
             p.CAT_ID AS CatId,
             Sum(IIf(IsNull(d.QTY),0,d.QTY)) AS SoldQty,
             IIf(IsNull(p.P_IMG_URL),'',p.P_IMG_URL) AS ImgUrl
             FROM (PRODUCT AS p
             INNER JOIN [訂單明細] AS d ON d.P_ID = p.P_ID)
             INNER JOIN [ORDERS] AS o ON d.O_ID = o.O_ID
             WHERE o.O_STATUS='PAID'
             GROUP BY p.P_ID, p.P_NAME, p.P_PRICE, p.CAT_ID, p.P_IMG_URL
             ORDER BY Sum(IIf(IsNull(d.QTY),0,d.QTY)) DESC;";

            var list = new List<ProductSale>();

            using (var cn = Open())
            using (var cmd = new OleDbCommand(sql, cn))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    var ps = new ProductSale();

                    ps.ProductId = Convert.ToInt32(rd["ProductId"]);
                    ps.Name = Convert.ToString(rd["Name"]);
                    ps.Price = (rd["PriceNum"] == DBNull.Value) ? 0m : Convert.ToDecimal(rd["PriceNum"]);
                    ps.CatId = Convert.ToInt32(rd["CatId"]);
                    ps.SoldQty = (rd["SoldQty"] == DBNull.Value) ? 0 : Convert.ToInt32(rd["SoldQty"]);
                    ps.ImgUrl = (rd["ImgUrl"] == DBNull.Value) ? "" : Convert.ToString(rd["ImgUrl"]);

                    list.Add(ps);
                }
            }

            return list;
        }
    }
}