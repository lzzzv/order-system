using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace 後台統計
{
    public partial class EditCategoryControl : UserControl
    {
        public EditCategoryControl()
        {
            InitializeComponent();
        }
        void BindDataGridView()
        {
            try
            {
                DataTable oCategoryTable = new DataTable();
                OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
                oConnection.Open();
                OleDbCommand oCommand = new OleDbCommand("SELECT * FROM CATEGORY", oConnection);
                OleDbDataAdapter oAdapter = new OleDbDataAdapter(oCommand);
                oAdapter.Fill(oCategoryTable);
                oConnection.Close();
                dataGridView1.DataSource = oCategoryTable;
            }
            catch {}
        }
        int AddRowToDB(string strName)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("INSERT INTO CATEGORY(CAT_NAME) VALUES ('{0}');", strName);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            strCommand = "SELECT @@IDENTITY";
            oCommand = new OleDbCommand(strCommand, oConnection);
            object objID = oCommand.ExecuteScalar();
            oConnection.Close();
            return ((int)objID);
        }
        void EditRowToDB(int nID, string strName)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("UPDATE CATEGORY SET CAT_NAME = '{0}' WHERE CAT_ID = {1};", strName, nID);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            oConnection.Close();
        }
        void DeleteRow(int nID)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("UPDATE PRODUCT SET CAT_ID = 1 WHERE CAT_ID = {0};", nID);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            strCommand = string.Format("DELETE FROM CATEGORY WHERE CAT_ID = {0};", nID);
            oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            oConnection.Close();
        }

        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].IsNewRow ||
                dataGridView1.Rows[e.RowIndex].Cells[0].Value is null)
                return;
            string strName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i == e.RowIndex || dataGridView1.Rows[i].IsNewRow)
                    continue;
                if (strName == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    // 發現重複，取消驗證並提示
                    MessageBox.Show("類別名稱重複，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].IsNewRow ||
                dataGridView1.Rows[e.RowIndex].Cells[0].Value is null)
                return;
            // 新增或修改
            object nID = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            string strName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (nID == DBNull.Value)
            {
                // 新增
                int nNewID = AddRowToDB(strName);
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = nNewID;
            }
            else
            {
                // 修改
                EditRowToDB((int)nID, strName);
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.IsNewRow)
                return;
            int nID = (int)e.Row.Cells[0].Value;
            if (nID == 1) // 未分類不可刪除
            {
                MessageBox.Show("未分類不可刪除！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else
            {
                DeleteRow(nID);
            }
        }

        private void EditCategoryControl_Load(object sender, EventArgs e)
        {
            BindDataGridView();
        }
    }
}
