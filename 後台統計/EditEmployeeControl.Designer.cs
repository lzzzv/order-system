namespace 後台統計
{
    partial class EditEmployeeControl
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.eMPLOYEEBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.點餐系統資料庫DataSet = new 後台統計.點餐系統資料庫DataSet();
            this.eMPLOYEETableAdapter = new 後台統計.點餐系統資料庫DataSetTableAdapters.EMPLOYEETableAdapter();
            this.E_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.E_ACCOUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.E_ROLE = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eMPLOYEEBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.點餐系統資料庫DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.E_ID,
            this.E_ACCOUNT,
            this.E_ROLE});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(489, 369);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowValidated);
            this.dataGridView1.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_RowValidating);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            // 
            // eMPLOYEEBindingSource
            // 
            this.eMPLOYEEBindingSource.DataMember = "EMPLOYEE";
            this.eMPLOYEEBindingSource.DataSource = this.點餐系統資料庫DataSet;
            // 
            // 點餐系統資料庫DataSet
            // 
            this.點餐系統資料庫DataSet.DataSetName = "點餐系統資料庫DataSet";
            this.點餐系統資料庫DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // eMPLOYEETableAdapter
            // 
            this.eMPLOYEETableAdapter.ClearBeforeFill = true;
            // 
            // E_ID
            // 
            this.E_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.E_ID.DataPropertyName = "E_ID";
            this.E_ID.HeaderText = "ID";
            this.E_ID.Name = "E_ID";
            this.E_ID.ReadOnly = true;
            // 
            // E_ACCOUNT
            // 
            this.E_ACCOUNT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.E_ACCOUNT.DataPropertyName = "E_ACCOUNT";
            this.E_ACCOUNT.HeaderText = "使用者帳號";
            this.E_ACCOUNT.Name = "E_ACCOUNT";
            // 
            // E_ROLE
            // 
            this.E_ROLE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.E_ROLE.DataPropertyName = "E_ROLE";
            this.E_ROLE.HeaderText = "角色";
            this.E_ROLE.Items.AddRange(new object[] {
            "系統管理員",
            "服務員",
            "收銀員",
            "會計",
            "廚師"});
            this.E_ROLE.Name = "E_ROLE";
            // 
            // EditEmployeeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "EditEmployeeControl";
            this.Size = new System.Drawing.Size(489, 369);
            this.Load += new System.EventHandler(this.EditEmployeeControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eMPLOYEEBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.點餐系統資料庫DataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource eMPLOYEEBindingSource;
        private 點餐系統資料庫DataSet 點餐系統資料庫DataSet;
        private 點餐系統資料庫DataSetTableAdapters.EMPLOYEETableAdapter eMPLOYEETableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn E_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn E_ACCOUNT;
        private System.Windows.Forms.DataGridViewComboBoxColumn E_ROLE;
    }
}
