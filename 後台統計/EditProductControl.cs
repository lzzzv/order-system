using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 後台統計
{
    public partial class EditProductControl : UserControl
    {
        public EditProductControl()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        private void EditProductControl_Load(object sender, EventArgs e)
        {
            BindDataGridView();
        }
        Dictionary <string, int> m_oCategoryNameID = new Dictionary<string, int>();
        bool m_bBinding = false;
        void BindDataGridView()
        {
            m_bBinding = true;
            try
            {
                DataTable oCategoryTable = new DataTable();
                DataTable oProductTable = new DataTable();
                OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
                oConnection.Open();
                OleDbCommand oCommand = new OleDbCommand("SELECT PRODUCT.*, CATEGORY.CAT_NAME FROM PRODUCT INNER JOIN CATEGORY ON PRODUCT.CAT_ID = CATEGORY.CAT_ID", oConnection);
                OleDbDataAdapter oAdapter = new OleDbDataAdapter(oCommand);
                oAdapter.Fill(oProductTable);
                oCommand = new OleDbCommand("SELECT * FROM CATEGORY", oConnection);
                oAdapter = new OleDbDataAdapter(oCommand);
                oAdapter.Fill(oCategoryTable);
                oConnection.Close();
                CAT_NAME.DataSource = oCategoryTable;
                CAT_NAME.DisplayMember = "CAT_NAME";
                dataGridView1.DataSource = oProductTable;
                m_oCategoryNameID.Clear();
                foreach (DataRow oRow in oCategoryTable.Rows)
                {
                    m_oCategoryNameID.Add(oRow[1].ToString(), (int)oRow[0]);
                }
            }
            catch { }
            m_bBinding = false;
        }
        int AddRowToDB(string strName, Decimal fPrice, int nCatID, bool bEnabled)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("INSERT INTO PRODUCT(P_NAME, P_PRICE, CAT_ID, P_ENABLE) VALUES ('{0}', {1}, {2}, {3});", 
                                              strName, fPrice, nCatID, bEnabled);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            strCommand = "SELECT @@IDENTITY";
            oCommand = new OleDbCommand(strCommand, oConnection);
            object objID = oCommand.ExecuteScalar();
            oConnection.Close();
            return ((int)objID);
        }

        void EditRowToDB(int nID, string strName, Decimal fPrice, int nCatID, bool bEnabled)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("UPDATE PRODUCT SET P_NAME = '{0}', P_PRICE = {1}, CAT_ID = {2}, P_ENABLE = {3} WHERE P_ID = {4};",
                                               strName, fPrice, nCatID, bEnabled, nID);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            oConnection.Close();
        }
        void DeleteRow(int nID)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("DELETE FROM PRODUCT WHERE P_ID = {0};", nID);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            oConnection.Close();
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.IsNewRow)
                return;
            int nID = (int)e.Row.Cells[0].Value;
            DeleteRow(nID);
        }

        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (m_bBinding)
                return;
            if (dataGridView1.Rows[e.RowIndex].IsNewRow ||
                dataGridView1.Rows[e.RowIndex].Cells[0].Value is null)
                return;
            string strName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (strName.Trim().Length == 0)
            {
                MessageBox.Show("品項名稱不可空白，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }
            string strPrice = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            Decimal fPrice;
            if (!Decimal.TryParse(strPrice, out fPrice))
            {
                MessageBox.Show("單價格式需為數值，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i == e.RowIndex || dataGridView1.Rows[i].IsNewRow)
                    continue;
                if (strName == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    // 發現重複，取消驗證並提示
                    MessageBox.Show("品項名稱重複，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (m_bBinding)
                return;
            if (dataGridView1.Rows[e.RowIndex].IsNewRow ||
                dataGridView1.Rows[e.RowIndex].Cells[0].Value is null)
                return;
            // 新增或修改
            object nID = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            string  strName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            Decimal fPrice = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            
            bool    bEnabled = (dataGridView1.Rows[e.RowIndex].Cells[4].Value != DBNull.Value &&
                                (bool)dataGridView1.Rows[e.RowIndex].Cells[4].Value == true);

            int     nCatID;
            
            if (dataGridView1.Rows[e.RowIndex].Cells[3].Value == DBNull.Value)
                nCatID = 1;
            else
                nCatID = m_oCategoryNameID[dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()];
            if (nID == DBNull.Value)
            {
                // 新增
                int nNewID = AddRowToDB(strName, fPrice, nCatID, bEnabled);
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = nNewID;
            }
            else
            {
                // 修改
                EditRowToDB((int)nID, strName, fPrice, nCatID, bEnabled);
            }

        }

        private void dataGridView1_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
                BindDataGridView();
        }
    }
}
