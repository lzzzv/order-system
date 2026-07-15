using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 後台統計
{
    public partial class ChangeStatusForm : Form
    {
        public string m_strTotal;
        public string m_strStatus;
        public string m_strPayWay;

        public string m_strNewStatus;
        public string m_strNewPayWay;

        public ChangeStatusForm()
        {
            InitializeComponent();
        }

        private void ChangeStatusForm_Load(object sender, EventArgs e)
        {
            // Total;
            txtTotal.Text = m_strTotal;
            // Status
            if (m_strStatus == "PAID")
                radioButton2.Checked = true;
            else if (m_strStatus == "CANCEL")
                radioButton3.Checked = true;
            else
                radioButton1.Checked = true;
            int nIndex = cbPayChannel.FindStringExact(m_strPayWay);
            if (nIndex < 0)
                nIndex = 1;
            cbPayChannel.SelectedIndex = nIndex;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked && cbPayChannel.SelectedIndex < 0)
            {
                MessageBox.Show("付款方式不可為空，請重新輸入！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (radioButton1.Checked)
                    m_strNewStatus = "UNPAID";
                else if (radioButton2.Checked)
                    m_strNewStatus = "PAID";
                else
                    m_strNewStatus = "CANCEL";
                m_strNewPayWay = cbPayChannel.SelectedText;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
