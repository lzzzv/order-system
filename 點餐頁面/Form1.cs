using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace 點餐頁面
{
    public partial class 點餐介面 : Form
    {
        DataTable cartTable;

        string connStr =
            @"Provider=Microsoft.ACE.OLEDB.12.0;
              Data Source=E:\系統分析期末報告\點餐系統\點餐系統資料庫.accdb";

        public 點餐介面()
        {
            InitializeComponent();
        }

        // =====================
        // 表單載入
        // =====================
        private void 點餐介面_Load(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
            }

            LoadCategory();
            InitCart();
        }

        // =====================
        // 載入分類
        // =====================
        private void LoadCategory()
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string sql = "SELECT CAT_ID, CAT_NAME FROM CATEGORY";
                OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCategory.DisplayMember = "CAT_NAME";
                cmbCategory.ValueMember = "CAT_ID";
                cmbCategory.DataSource = dt;
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        // =====================
        // 載入商品（一定要有 P_ID）
        // =====================
        private void LoadProduct()
        {
            if (cmbCategory.SelectedValue == null) return;

            int catId = Convert.ToInt32(cmbCategory.SelectedValue);

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string sql =
                    "SELECT P_ID, P_NAME, P_PRICE " +
                    "FROM PRODUCT WHERE CAT_ID = ? AND P_ENABLE = true";

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.Parameters.Add("@p1", OleDbType.Integer).Value = catId;

                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvProduct.DataSource = dt;
            }
        }

        // =====================
        // 初始化購物車
        // =====================
        private void InitCart()
        {
            cartTable = new DataTable();
            cartTable.Columns.Add("P_ID", typeof(int));
            cartTable.Columns.Add("P_NAME");
            cartTable.Columns.Add("PRICE", typeof(decimal));
            cartTable.Columns.Add("QTY", typeof(int));
            cartTable.Columns.Add("SUBTOTAL", typeof(decimal));

            dgvCart.DataSource = cartTable;
            dgvCart.ReadOnly = true;
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            UpdateTotal();
        }

        // =====================
        // 商品雙擊 → 加入購物車
        // =====================
        private void dgvProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int pid = Convert.ToInt32(dgvProduct.Rows[e.RowIndex].Cells["P_ID"].Value);
            string name = dgvProduct.Rows[e.RowIndex].Cells["P_NAME"].Value.ToString();
            decimal price = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["P_PRICE"].Value);

            foreach (DataRow row in cartTable.Rows)
            {
                if (Convert.ToInt32(row["P_ID"]) == pid)
                {
                    row["QTY"] = Convert.ToInt32(row["QTY"]) + 1;
                    row["SUBTOTAL"] = Convert.ToInt32(row["QTY"]) * price;
                    UpdateTotal();
                    return;
                }
            }

            DataRow newRow = cartTable.NewRow();
            newRow["P_ID"] = pid;
            newRow["P_NAME"] = name;
            newRow["PRICE"] = price;
            newRow["QTY"] = 1;
            newRow["SUBTOTAL"] = price;
            cartTable.Rows.Add(newRow);

            UpdateTotal();
        }

        // =====================
        // 金額
        // =====================
        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                total += Convert.ToDecimal(row["SUBTOTAL"]);
            }
            lblTotal.Text = $"總金額：{total} 元";
        }

        private decimal GetTotal()
        {
            decimal total = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                total += Convert.ToDecimal(row["SUBTOTAL"]);
            }
            return total;
        }

        // =====================
        // 結帳
        // =====================
        private void btncheckout_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("購物車是空的");
                return;
            }

            decimal total = GetTotal();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                OleDbTransaction tran = conn.BeginTransaction();

                try
                {
                    // ORDERS
                    string sqlOrder =
                        "INSERT INTO ORDERS (O_DATE, O_TOTAL, C_ID, E_ID, O_STATUS) " +
                        "VALUES (?, ?, ?, ?, ?)";

                    OleDbCommand cmdOrder = new OleDbCommand(sqlOrder, conn, tran);
                    cmdOrder.Parameters.Add("@p1", OleDbType.Date).Value = DateTime.Now;
                    cmdOrder.Parameters.Add("@p2", OleDbType.Currency).Value = total;
                    cmdOrder.Parameters.Add("@p3", OleDbType.Integer).Value = 2; // CUSTOMER
                    cmdOrder.Parameters.Add("@p4", OleDbType.Integer).Value = 1; // EMPLOYEE
                    cmdOrder.Parameters.Add("@p5", OleDbType.VarChar).Value = "PAID";
                    cmdOrder.ExecuteNonQuery();

                    OleDbCommand cmdGetId =
                        new OleDbCommand("SELECT @@IDENTITY", conn, tran);
                    int orderId = Convert.ToInt32(cmdGetId.ExecuteScalar());

                    // 訂單明細（⚠️ 表名請確認）
                    foreach (DataRow row in cartTable.Rows)
                    {
                        string sqlDetail =
                            "INSERT INTO [訂單明細] (O_ID, P_ID, QTY, SUBTOTAL) VALUES (?, ?, ?, ?)";

                        OleDbCommand cmdDetail =
                            new OleDbCommand(sqlDetail, conn, tran);

                        cmdDetail.Parameters.Add("@p1", OleDbType.Integer).Value = orderId;
                        cmdDetail.Parameters.Add("@p2", OleDbType.Integer).Value = row["P_ID"];
                        cmdDetail.Parameters.Add("@p3", OleDbType.Integer).Value = row["QTY"];
                        cmdDetail.Parameters.Add("@p4", OleDbType.Currency).Value = row["SUBTOTAL"];

                        cmdDetail.ExecuteNonQuery();
                    }
                    string sqlPay =
                        "INSERT INTO PAYMENT (O_ID, PAY_WAY, PAY_TIME) VALUES (?, ?, ?)";

                    OleDbCommand cmdPay =
                        new OleDbCommand(sqlPay, conn, tran);

                    cmdPay.Parameters.Add("@p1", OleDbType.Integer).Value = orderId;
                    cmdPay.Parameters.Add("@p2", OleDbType.VarChar).Value = "現金";
                    cmdPay.Parameters.Add("@p3", OleDbType.Date).Value = DateTime.Now;
                    cmdPay.ExecuteNonQuery();

                    tran.Commit();

                    MessageBox.Show("結帳完成！");
                    cartTable.Clear();
                    UpdateTotal();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("結帳失敗：" + ex.Message);
                }
            }
        }
    }
}
