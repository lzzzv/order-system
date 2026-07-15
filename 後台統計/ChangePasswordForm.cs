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
    public partial class ChangePasswordForm : Form
    {
        public ChangePasswordForm()
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            string strAccount = txtAccount.Text;
            string strOldPassword = txtOldPassword.Text;
            string strNewPassword = txtNewPassword.Text.Trim();
            string strNewPassword2 = txtNewPassword2.Text.Trim();
            string strEncodedOldPassword = GetMD5(strOldPassword);
            string strEncodedNewPassword = GetMD5(strNewPassword);

            if (strAccount.Length == 0)
            {
                MessageBox.Show("請輸入帳號，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (strNewPassword.Length == 0)
            {
                MessageBox.Show("新密碼不可為空白，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (strNewPassword != strNewPassword2)
            {
                MessageBox.Show("新密碼不一致，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
                oConnection.Open();
                string strCommand = string.Format("SELECT E_ID FROM EMPLOYEE WHERE E_ACCOUNT='{0}' AND (E_PASSWORD='{1}' OR E_PASSWORD IS NULL)", strAccount, strEncodedOldPassword);
                OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
                object objID = oCommand.ExecuteScalar();
                if (objID == null)
                {
                    MessageBox.Show("帳號或密碼錯誤，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else 
                {
                    int nID = (int)objID;
                    strCommand = string.Format("UPDATE EMPLOYEE SET E_PASSWORD='{0}' WHERE E_ID = {1}", strEncodedNewPassword, nID);
                    oCommand = new OleDbCommand(strCommand, oConnection);
                    oCommand.ExecuteNonQuery();
                    MessageBox.Show("修改成功，請重新登入！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                oConnection.Close();
            }
        }
    }
}
