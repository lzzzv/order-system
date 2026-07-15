using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 首頁2
{
    public partial class Form1: Form
    {
        private Panel sidePanel;
        private Button currentButton;
        private Form activeForm;


        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            OpenChildForm(new HomeForm());
        }

        private void InitializeCustomComponents()
        {
            // 設置窗體屬性
            this.Text = "美食餐廳點餐系統";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // 創建側邊欄
            CreateSidePanel();

            // 創建頂部導航欄
            CreateTopPanel();
        }

        private void CreateTopPanel()
        {
            Panel topPanel = new Panel
            {
                BackColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Top,
                Height = 70
            };

            Label titleLabel = new Label
            {
                Text = "🍽️ 美味餐廳",
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // 購物車按鈕
            Button cartButton = new Button
            {
                Text = "🛒 購物車 (0)",
                Size = new Size(120, 40),
                Location = new Point(1050, 15),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei", 10)
            };
            cartButton.FlatAppearance.BorderSize = 0;

            topPanel.Controls.Add(titleLabel);
            topPanel.Controls.Add(cartButton);
            this.Controls.Add(topPanel);
        }

        private void CreateSidePanel()
        {
            sidePanel = new Panel
            {
                BackColor = Color.FromArgb(52, 73, 94),
                Size = new Size(220, 800),
                Location = new Point(0, 70)
            };

            string[] menuItems = { "首頁", "菜單", "熱銷商品", "訂單", "關於我們" };
            Color[] colors = {
                Color.FromArgb(52, 152, 219),
                Color.FromArgb(155, 89, 182),
                Color.FromArgb(26, 188, 156),
                Color.FromArgb(241, 196, 15),
                Color.FromArgb(230, 126, 34)
            };

            int y = 30;
            for (int i = 0; i < menuItems.Length; i++)
            {
                Button btn = CreateMenuButton(menuItems[i], colors[i], y);
                btn.Click += MenuButton_Click;
                sidePanel.Controls.Add(btn);
                y += 60;
            }

            this.Controls.Add(sidePanel);
        }

        private Button CreateMenuButton(string text, Color color, int y)
        {
            return new Button
            {
                Text = "   " + text,
                Size = new Size(220, 50),
                Location = new Point(0, y),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft YaHei", 11, FontStyle.Bold),
                ImageAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ActivateButton(button);

                if (button.Text.Contains("首頁"))
                    OpenChildForm(new HomeForm());
                else if (button.Text.Contains("熱銷商品"))
                    OpenChildForm(new PopularItemsForm());
                // 其他菜單項目的處理
            }
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    currentButton = (Button)btnSender;
                }
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.BackColor = Color.FromArgb(245, 245, 245);

            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Location = new Point(220, 70),
                Size = new Size(980, 730)
            };

            contentPanel.Controls.Add(childForm);
            this.Controls.Add(contentPanel);
            childForm.BringToFront();
            childForm.Show();
        }
    }
}
