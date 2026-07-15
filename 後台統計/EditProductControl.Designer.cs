namespace 後台統計
{
    partial class EditProductControl
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.P_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.P_PRICE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CAT_NAME = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.P_ENABLE = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CAT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.P_ID,
            this.P_NAME,
            this.P_PRICE,
            this.CAT_NAME,
            this.P_ENABLE,
            this.CAT_ID});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(464, 478);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowValidated);
            this.dataGridView1.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_RowValidating);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            this.dataGridView1.VisibleChanged += new System.EventHandler(this.dataGridView1_VisibleChanged);
            // 
            // P_ID
            // 
            this.P_ID.DataPropertyName = "P_ID";
            this.P_ID.HeaderText = "ID";
            this.P_ID.Name = "P_ID";
            this.P_ID.ReadOnly = true;
            // 
            // P_NAME
            // 
            this.P_NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.P_NAME.DataPropertyName = "P_NAME";
            this.P_NAME.HeaderText = "品項";
            this.P_NAME.Name = "P_NAME";
            // 
            // P_PRICE
            // 
            this.P_PRICE.DataPropertyName = "P_PRICE";
            this.P_PRICE.HeaderText = "單價";
            this.P_PRICE.Name = "P_PRICE";
            // 
            // CAT_NAME
            // 
            this.CAT_NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CAT_NAME.DataPropertyName = "CAT_NAME";
            this.CAT_NAME.HeaderText = "類別";
            this.CAT_NAME.Name = "CAT_NAME";
            // 
            // P_ENABLE
            // 
            this.P_ENABLE.DataPropertyName = "P_ENABLE";
            this.P_ENABLE.FalseValue = "false";
            this.P_ENABLE.HeaderText = "可用";
            this.P_ENABLE.Name = "P_ENABLE";
            this.P_ENABLE.TrueValue = "true";
            // 
            // CAT_ID
            // 
            this.CAT_ID.DataPropertyName = "CAT_ID";
            this.CAT_ID.HeaderText = "CAT_ID";
            this.CAT_ID.Name = "CAT_ID";
            this.CAT_ID.Visible = false;
            // 
            // EditProductControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "EditProductControl";
            this.Size = new System.Drawing.Size(464, 478);
            this.Load += new System.EventHandler(this.EditProductControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn P_PRICE;
        private System.Windows.Forms.DataGridViewComboBoxColumn CAT_NAME;
        private System.Windows.Forms.DataGridViewCheckBoxColumn P_ENABLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CAT_ID;
    }
}
