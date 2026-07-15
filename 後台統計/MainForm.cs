using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 後台統計
{
    public partial class MainForm : Form
    {
        static public int m_nLoginUserID = 2;
        static public string m_strLoginUserName;
        static public string m_strLoginUserRole;
        static public string m_strConnectionString = "";

        public MainForm()
        {
            // Build Connection String 
            string strDBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\點餐系統資料庫.accdb");
            m_strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=True", strDBPath);
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {   
            LoginForm oLoginForm = new LoginForm();
            oLoginForm.ShowDialog(this);
            if (oLoginForm.m_strRole.Length == 0)
            {
                Close();
            }
            m_nLoginUserID = oLoginForm.m_nLoginID;
            m_strLoginUserName = oLoginForm.m_strLoginName;
            m_strLoginUserRole = oLoginForm.m_strRole;
            this.Text = string.Format("後臺系統 - {0}({1})", m_strLoginUserName, m_strLoginUserRole);
        }
    }
}
