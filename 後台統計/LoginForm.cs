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
    public partial class LoginForm : Form
    {
        public string m_strRole = string.Empty;
        public string m_strLoginName = string.Empty;
        public int m_nLoginID = 0;
        public LoginForm()
        {
            InitializeComponent();
        }
        public string GetMD5(string strText)
        {
            string strResult = "";
            try
            {
                System.Security.Cryptography.MD5 oMD5 = System.Security.Cryptography.MD5.Create();
                byte[] oBytes = System.Text.Encoding.ASCII.GetBytes(strText);
                byte[] oEncodedBytes = oMD5.ComputeHash(oBytes);
                strResult = System.Convert.ToBase64String(oEncodedBytes);
            }
            catch { }
            return (strResult);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string strAccount = txtAccount.Text;
            string strPassword = txtPassword.Text;
            string strEncodedPassword = GetMD5(strPassword);
            OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
            oConnection.Open();
            string strCommand = string.Format("SELECT E_ID, E_ROLE FROM EMPLOYEE WHERE E_ACCOUNT='{0}' AND E_PASSWORD='{1}'", strAccount, strEncodedPassword);
            OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
            OleDbDataAdapter oAdapter = new OleDbDataAdapter(oCommand);
            DataTable oEmployee = new DataTable();
            oAdapter.Fill(oEmployee);
            oConnection.Close();
            if (oEmployee.Rows.Count == 0)
            {
                MessageBox.Show("帳號或密碼錯誤，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                m_nLoginID = (int)oEmployee.Rows[0][0];      
                m_strRole = oEmployee.Rows[0][1].ToString();      
                m_strLoginName = strAccount;
                Close();
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordForm oForm = new ChangePasswordForm();
            oForm.ShowDialog(this);
        }
    }
}
