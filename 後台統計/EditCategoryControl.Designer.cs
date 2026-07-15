namespace 後台統計
{
    partial class EditCategoryControl
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
            this.CAT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CAT_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.點餐系統資料庫DataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.點餐系統資料庫DataSet = new 後台統計.點餐系統資料庫DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.點餐系統資料庫DataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.點餐系統資料庫DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CAT_ID,
            this.CAT_NAME});
            this.dataGridView1.DataSource = this.點餐系統資料庫DataSetBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(455, 355);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowValidated);
            this.dataGridView1.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_RowValidating);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            // 
            // CAT_ID
            // 
            this.CAT_ID.DataPropertyName = "CAT_ID";
            this.CAT_ID.HeaderText = "ID";
            this.CAT_ID.Name = "CAT_ID";
            this.CAT_ID.ReadOnly = true;
            // 
            // CAT_NAME
            // 
            this.CAT_NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CAT_NAME.DataPropertyName = "CAT_NAME";
            this.CAT_NAME.HeaderText = "類別名稱";
            this.CAT_NAME.Name = "CAT_NAME";
            // 
            // 點餐系統資料庫DataSetBindingSource
            // 
            this.點餐系統資料庫DataSetBindingSource.DataSource = this.點餐系統資料庫DataSet;
            this.點餐系統資料庫DataSetBindingSource.Position = 0;
            // 
            // 點餐系統資料庫DataSet
            // 
            this.點餐系統資料庫DataSet.DataSetName = "點餐系統資料庫DataSet";
            this.點餐系統資料庫DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // EditCategoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "EditCategoryControl";
            this.Size = new System.Drawing.Size(455, 355);
            this.Load += new System.EventHandler(this.EditCategoryControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.點餐系統資料庫DataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.點餐系統資料庫DataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource 點餐系統資料庫DataSetBindingSource;
        private 點餐系統資料庫DataSet 點餐系統資料庫DataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn CAT_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CAT_NAME;
    }
}
