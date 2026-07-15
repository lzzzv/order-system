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
using System.Security.Cryptography;

namespace 後台統計
{
    public partial class PaymentControl : UserControl
    {
        public PaymentControl()
        {
            InitializeComponent();
        }

        private void PaymentControl_Load(object sender, EventArgs e)
        {
            RebindOrderList();
        }

        void RebindOrderList()
        {
            try
            {
                DataTable oOrderTable = new DataTable();
                OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
                oConnection.Open();
                string strSQL = "SELECT ORDERS.*, EMPLOYEE.E_ACCOUNT, PAYMENT.PAY_WAY FROM (ORDERS " +
                                "LEFT JOIN EMPLOYEE ON ORDERS.E_ID = EMPLOYEE.E_ID) " +
                                "LEFT JOIN PAYMENT ON ORDERS.O_ID = PAYMENT.O_ID";
                List<string> oCondictions = new List<string>(); 
                if (chkUnpaid.Checked)
                    oCondictions.Add("O_STATUS = 'UNPAID'");
                if (radio1Day.Checked)
                    oCondictions.Add("O_DATE >= DateAdd('h', -24, Now())");
                else if (radio3Days.Checked)
                    oCondictions.Add("O_DATE >= DateAdd('h', -72, Now())");
                string strCondition = string.Join(" AND ", oCondictions);
                if (strCondition.Length > 0)
                    strSQL += " WHERE " + strCondition;

                OleDbCommand oCommand = new OleDbCommand(strSQL, oConnection);
                OleDbDataAdapter oAdapter = new OleDbDataAdapter(oCommand);
                oAdapter.Fill(oOrderTable);
                oConnection.Close();
                lstOrder.Items.Clear();
                foreach (DataRow oRow in oOrderTable.Rows)
                {
                    // O_ID, O_DATE, O_TOTAL, E_ACCOUNT, O_STATUS, PAY_WAY
                    string[] oCells = new string[6];
                    oCells[0] = oRow[0].ToString();
                    oCells[1] = oRow[1].ToString();
                    oCells[2] = oRow[2].ToString();
                    oCells[3] = oRow[6].ToString();
                    oCells[4] = oRow[5].ToString();
                    oCells[5] = oRow[7].ToString();
                    ListViewItem oItem = new ListViewItem(oCells);
                    lstOrder.Items.Add(oItem);
                }
            }
            catch { }
        }

        void RebindDetailList()
        {
            lstDetail.Items.Clear();
            if (lstOrder.SelectedItems != null && lstOrder.SelectedItems.Count > 0)
            {
                string strOID = lstOrder.SelectedItems[0].Text;
                try
                {

                    DataTable oDetailTable = new DataTable();
                    OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
                    oConnection.Open();
                    string strSQL = "SELECT A.*, B.P_NAME, C.CAT_NAME " +
                                    "FROM(訂單明細 AS A " +
                                    "LEFT JOIN " +
                                    "    PRODUCT AS B  ON A.P_ID = B.P_ID) " +
                                    "LEFT JOIN " +
                                    "    CATEGORY AS C ON B.CAT_ID = C.CAT_ID " +
                                    "WHERE A.O_ID = " + strOID;

                    OleDbCommand oCommand = new OleDbCommand(strSQL, oConnection);
                    OleDbDataAdapter oAdapter = new OleDbDataAdapter(oCommand);
                    oAdapter.Fill(oDetailTable);
                    oConnection.Close();
                    // Add Group
                    lstDetail.Groups.Clear();
                    Dictionary<string, ListViewGroup> oGroupHash = new Dictionary<string, ListViewGroup>();
                    ListViewGroup oGroup;
                    foreach (DataRow oRow in oDetailTable.Rows)
                    {
                        // D_ID, O_ID, P_ID, QTY, SUBTOTAL, P_NAME, CAT_NAME
                        string[] oCells = new string[3];
                        oCells[0] = oRow[5].ToString();
                        oCells[1] = oRow[3].ToString();
                        oCells[2] = oRow[4].ToString();
                        string strGroupName = oRow[6].ToString();
                        ListViewItem oItem = new ListViewItem(oCells);
                        if (oGroupHash.ContainsKey(strGroupName))
                        {
                            oItem.Group = oGroupHash[strGroupName];
                        }
                        else
                        {
                            oGroup = new ListViewGroup(strGroupName, strGroupName);
                            lstDetail.Groups.Add(oGroup);
                            oGroupHash.Add(strGroupName, oGroup);
                            oItem.Group = oGroup;
                        }
                        lstDetail.Items.Add(oItem);
                    }
                }
                catch { }
            }
        }
        private void radio1Day_CheckedChanged(object sender, EventArgs e)
        {
            RebindOrderList();
        }

        private void lstOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            RebindDetailList();
        }

        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (lstOrder.SelectedItems != null && lstOrder.SelectedItems.Count > 0)
            {
                ChangeStatusForm oForm = new ChangeStatusForm();
                oForm.m_strTotal = lstOrder.SelectedItems[0].SubItems[2].Text;
                oForm.m_strStatus = lstOrder.SelectedItems[0].SubItems[4].Text;
                oForm.m_strPayWay = lstOrder.SelectedItems[0].SubItems[5].Text;
                if (DialogResult.OK == oForm.ShowDialog(this))
                {
                    // 更新Status
                    string strOID = lstOrder.SelectedItems[0].Text;
                    OleDbConnection oConnection = new OleDbConnection(MainForm.m_strConnectionString);
                    oConnection.Open();
                    string strCommand = string.Format("UPDATE ORDERS SET O_STATUS = '{0}' WHERE O_ID = {1};",
                                                       oForm.m_strNewStatus, strOID);
                    OleDbCommand oCommand = new OleDbCommand(strCommand, oConnection);
                    oCommand.ExecuteNonQuery();
                    // 新增Payment
                    try
                    {
                        strCommand = string.Format("INSERT INTO PAYMENT(O_ID, PAY_WAY, PAY_TIME) VALUES ({0}, '{1}', Now());",
                                                           strOID, oForm.m_strNewPayWay);
                        oCommand = new OleDbCommand(strCommand, oConnection);
                        oCommand.ExecuteNonQuery();
                    }
                    catch {}
                    oConnection.Close();
                }
            }
        }
    }
}
