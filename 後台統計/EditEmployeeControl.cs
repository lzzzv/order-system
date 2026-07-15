using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace 後台統計
{
    public partial class EditEmployeeControl : UserControl
    {
        public EditEmployeeControl()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        void BindDataGridView()
        {
            try
            {
                DataTable oEmployeeTable = new DataTable();
                OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
                oConnection.Open();
                OleDbCommand oCommand = new OleDbCommand("SELECT * FROM EMPLOYEE", oConnection);
                OleDbDataAdapter oAdapter = new OleDbDataAdapter(oCommand);
                oAdapter.Fill(oEmployeeTable);
                oConnection.Close();
                dataGridView1.DataSource = oEmployeeTable;
            }
            catch {}
        }
        int AddRowToDB(string strName, string strRole)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("INSERT INTO EMPLOYEE(E_ACCOUNT, E_ROLE) VALUES ('{0}', '{1}');", strName, strRole);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            strCommand = "SELECT @@IDENTITY";
            oCommand = new OleDbCommand(strCommand, oConnection);
            object objID = oCommand.ExecuteScalar();
            oConnection.Close();
            return ((int)objID);
        }
        void EditRowToDB(int nID, string strName, string strRole)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("UPDATE EMPLOYEE SET E_ACCOUNT = '{0}', E_ROLE='{1}' WHERE E_ID = {2};", strName, strRole, nID);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            oConnection.Close();
        }
        void DeleteRow(int nID)
        {
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("DELETE FROM EMPLOYEE WHERE E_ID = {0};", nID);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            oCommand.ExecuteNonQuery();
            oConnection.Close();
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].IsNewRow)
                return;
            // 新增或修改
            object nID = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            string strName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string strRole = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            if (nID == DBNull.Value)
            {
                // 新增
                int nNewID = AddRowToDB(strName, strRole);
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = nNewID;
            }
            else
            {
                // 修改
                EditRowToDB((int)nID, strName, strRole);
            }
        }

        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].IsNewRow)
                return;
            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value == DBNull.Value)
                return;
            int nID = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            string strName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i == e.RowIndex || dataGridView1.Rows[i].IsNewRow)
                    continue;
                if (strName == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    // 發現重複，取消驗證並提示
                    MessageBox.Show("姓名重複，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    break;
                }
            }
            if (dataGridView1.Rows[e.RowIndex].Cells[2].Value == DBNull.Value)
            {
                MessageBox.Show("請選擇角色！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }

            if (MainForm.m_nLoginUserID == nID && dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() != "系統管理員")
            {
                MessageBox.Show("角色不允許修改！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.IsNewRow)
                return;
            int nID = (int)e.Row.Cells[0].Value;
            if (MainForm.m_nLoginUserID == nID)
            {
                MessageBox.Show("不允許刪除自己！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else
            {
                DeleteRow(nID);
            }
        }

        private void EditEmployeeControl_Load(object sender, EventArgs e)
        {
            BindDataGridView();
        }
    }
}
