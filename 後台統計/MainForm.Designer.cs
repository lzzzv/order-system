namespace 後台統計
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.員工管理 = new System.Windows.Forms.TabPage();
            this.editEmployeeControl1 = new 後台統計.EditEmployeeControl();
            this.菜單類別管理 = new System.Windows.Forms.TabPage();
            this.editCategoryControl1 = new 後台統計.EditCategoryControl();
            this.菜單管理 = new System.Windows.Forms.TabPage();
            this.editProductControl1 = new 後台統計.EditProductControl();
            this.收銀管理 = new System.Windows.Forms.TabPage();
            this.paymentControl1 = new 後台統計.PaymentControl();
            this.tabControl1.SuspendLayout();
            this.員工管理.SuspendLayout();
            this.菜單類別管理.SuspendLayout();
            this.菜單管理.SuspendLayout();
            this.收銀管理.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.員工管理);
            this.tabControl1.Controls.Add(this.菜單類別管理);
            this.tabControl1.Controls.Add(this.菜單管理);
            this.tabControl1.Controls.Add(this.收銀管理);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(888, 626);
            this.tabControl1.TabIndex = 0;
            // 
            // 員工管理
            // 
            this.員工管理.Controls.Add(this.editEmployeeControl1);
            this.員工管理.Location = new System.Drawing.Point(4, 25);
            this.員工管理.Name = "員工管理";
            this.員工管理.Padding = new System.Windows.Forms.Padding(3);
            this.員工管理.Size = new System.Drawing.Size(925, 571);
            this.員工管理.TabIndex = 0;
            this.員工管理.Text = "員工管理";
            this.員工管理.UseVisualStyleBackColor = true;
            // 
            // editEmployeeControl1
            // 
            this.editEmployeeControl1.AutoSize = true;
            this.editEmployeeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editEmployeeControl1.Location = new System.Drawing.Point(3, 3);
            this.editEmployeeControl1.Margin = new System.Windows.Forms.Padding(4);
            this.editEmployeeControl1.Name = "editEmployeeControl1";
            this.editEmployeeControl1.Size = new System.Drawing.Size(919, 565);
            this.editEmployeeControl1.TabIndex = 0;
            // 
            // 菜單類別管理
            // 
            this.菜單類別管理.Controls.Add(this.editCategoryControl1);
            this.菜單類別管理.Location = new System.Drawing.Point(4, 25);
            this.菜單類別管理.Name = "菜單類別管理";
            this.菜單類別管理.Padding = new System.Windows.Forms.Padding(3);
            this.菜單類別管理.Size = new System.Drawing.Size(925, 571);
            this.菜單類別管理.TabIndex = 1;
            this.菜單類別管理.Text = "菜單類別管理";
            this.菜單類別管理.UseVisualStyleBackColor = true;
            // 
            // editCategoryControl1
            // 
            this.editCategoryControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editCategoryControl1.Location = new System.Drawing.Point(3, 3);
            this.editCategoryControl1.Margin = new System.Windows.Forms.Padding(4);
            this.editCategoryControl1.Name = "editCategoryControl1";
            this.editCategoryControl1.Size = new System.Drawing.Size(919, 565);
            this.editCategoryControl1.TabIndex = 0;
            // 
            // 菜單管理
            // 
            this.菜單管理.Controls.Add(this.editProductControl1);
            this.菜單管理.Location = new System.Drawing.Point(4, 25);
            this.菜單管理.Name = "菜單管理";
            this.菜單管理.Size = new System.Drawing.Size(925, 571);
            this.菜單管理.TabIndex = 2;
            this.菜單管理.Text = "菜單管理";
            this.菜單管理.UseVisualStyleBackColor = true;
            // 
            // editProductControl1
            // 
            this.editProductControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editProductControl1.Location = new System.Drawing.Point(0, 0);
            this.editProductControl1.Margin = new System.Windows.Forms.Padding(4);
            this.editProductControl1.Name = "editProductControl1";
            this.editProductControl1.Size = new System.Drawing.Size(925, 571);
            this.editProductControl1.TabIndex = 0;
            // 
            // 收銀管理
            // 
            this.收銀管理.Controls.Add(this.paymentControl1);
            this.收銀管理.Location = new System.Drawing.Point(4, 25);
            this.收銀管理.Name = "收銀管理";
            this.收銀管理.Padding = new System.Windows.Forms.Padding(3);
            this.收銀管理.Size = new System.Drawing.Size(880, 597);
            this.收銀管理.TabIndex = 3;
            this.收銀管理.Text = "收銀管理";
            this.收銀管理.UseVisualStyleBackColor = true;
            // 
            // paymentControl1
            // 
            this.paymentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentControl1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.paymentControl1.Location = new System.Drawing.Point(3, 3);
            this.paymentControl1.Margin = new System.Windows.Forms.Padding(4);
            this.paymentControl1.Name = "paymentControl1";
            this.paymentControl1.Size = new System.Drawing.Size(874, 591);
            this.paymentControl1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 626);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "後臺系統";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.員工管理.ResumeLayout(false);
            this.員工管理.PerformLayout();
            this.菜單類別管理.ResumeLayout(false);
            this.菜單管理.ResumeLayout(false);
            this.收銀管理.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 員工管理;
        private System.Windows.Forms.TabPage 菜單類別管理;
        private System.Windows.Forms.TabPage 菜單管理;
        private EditEmployeeControl editEmployeeControl1;
        private EditCategoryControl editCategoryControl1;
        private EditProductControl editProductControl1;
        private System.Windows.Forms.TabPage 收銀管理;
        private PaymentControl paymentControl1;
    }
}