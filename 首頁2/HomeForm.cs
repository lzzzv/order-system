using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 首頁2
{
    public partial class HomeForm: Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }
        private void InitializeHomeForm()
        {
            this.Size = new Size(980, 730);
            this.BackColor = Color.White;

            // 創建滾動面板
            Panel scrollPanel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill
            };

            // 餐廳介紹區塊
            Panel introPanel = CreateIntroPanel();
            scrollPanel.Controls.Add(introPanel);

            // 熱銷商品區塊
            Panel popularPanel = CreatePopularItemsPanel();
            popularPanel.Location = new Point(0, 350);
            scrollPanel.Controls.Add(popularPanel);

            // 特色區塊
            Panel featuresPanel = CreateFeaturesPanel();
            featuresPanel.Location = new Point(0, 800);
            scrollPanel.Controls.Add(featuresPanel);

            this.Controls.Add(scrollPanel);
        }

        private Panel CreateIntroPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(940, 320),
                Location = new Point(20, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // 標題
            Label titleLabel = new Label
            {
                Text = "🏮 歡迎光臨美味餐廳",
                Font = new Font("Microsoft YaHei", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(0, 0),
                AutoSize = true
            };

            // 分隔線
            Panel divider = new Panel
            {
                Size = new Size(100, 3),
                Location = new Point(0, 50),
                BackColor = Color.FromArgb(231, 76, 60)
            };

            // 介紹文字
            Label introLabel = new Label
            {
                Text = @"成立於2010年，我們堅持使用最新鮮的食材和最用心的烹飪技術，
為您帶來最道地的美食體驗。從經典菜餚到創意料理，
每一道菜都是我們對美食熱情的體現。

營業時間：
週一至週五：11:00 AM - 10:00 PM
週六至週日：10:00 AM - 11:00 PM

📍 地址：台北市信義區美食大道123號
📞 電話：(02) 1234-5678",
                Font = new Font("Microsoft YaHei", 11),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(0, 70),
                Size = new Size(450, 200),
                Padding = new Padding(10)
            };

            // 餐廳圖片（模擬）
            Panel imagePanel = new Panel
            {
                Size = new Size(450, 250),
                Location = new Point(470, 30),
                BackColor = Color.FromArgb(230, 230, 230),
                BorderRadius = 15
            };

            // 在圖片面板中添加標籤模擬圖片
            Label imageLabel = new Label
            {
                Text = "🍳\n餐廳環境",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft YaHei", 20),
                Dock = DockStyle.Fill
            };

            imagePanel.Controls.Add(imageLabel);

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(divider);
            panel.Controls.Add(introLabel);
            panel.Controls.Add(imagePanel);

            return panel;
        }

        private Panel CreatePopularItemsPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(940, 420),
                BackColor = Color.White
            };

            // 區塊標題
            Label titleLabel = new Label
            {
                Text = "🔥 熱銷商品",
                Font = new Font("Microsoft YaHei", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(0, 0),
                AutoSize = true
            };

            // 商品卡片容器
            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 50),
                Size = new Size(940, 360),
                WrapContents = true,
                AutoScroll = false
            };

            // 添加熱銷商品卡片
            AddProductCard(flowPanel, "招牌牛肉麵", "NT$ 180", "🍜", Color.FromArgb(52, 152, 219));
            AddProductCard(flowPanel, "香辣雞腿飯", "NT$ 150", "🍗", Color.FromArgb(231, 76, 60));
            AddProductCard(flowPanel, "海鮮披薩", "NT$ 280", "🍕", Color.FromArgb(26, 188, 156));
            AddProductCard(flowPanel, "水果沙拉", "NT$ 120", "🥗", Color.FromArgb(241, 196, 15));
            AddProductCard(flowPanel, "珍珠奶茶", "NT$ 70", "🧋", Color.FromArgb(155, 89, 182));
            AddProductCard(flowPanel, "提拉米蘇", "NT$ 90", "🍰", Color.FromArgb(230, 126, 34));

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(flowPanel);

            return panel;
        }

        private void AddProductCard(FlowLayoutPanel parent, string name, string price, string icon, Color color)
        {
            Panel card = new Panel
            {
                Size = new Size(280, 160),
                BackColor = Color.White,
                Margin = new Padding(10),
                BorderRadius = 10,
                Cursor = Cursors.Hand
            };

            // 陰影效果
            card.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.Transparent, 0, ButtonBorderStyle.None,
                    Color.Transparent, 0, ButtonBorderStyle.None,
                    Color.Transparent, 0, ButtonBorderStyle.None,
                    Color.FromArgb(200, 200, 200), 1, ButtonBorderStyle.Solid);

                // 圓角
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 10;
                    path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                    path.AddArc(card.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                    path.AddArc(card.Width - radius * 2, card.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(0, card.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    card.Region = new Region(path);
                }
            };

            // 圖示區域
            Panel iconPanel = new Panel
            {
                Size = new Size(60, 60),
                Location = new Point(20, 20),
                BackColor = color,
                BorderRadius = 30
            };

            Label iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 24),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 商品名稱
            Label nameLabel = new Label
            {
                Text = name,
                Font = new Font("Microsoft YaHei", 14, FontStyle.Bold),
                Location = new Point(100, 25),
                AutoSize = true
            };

            // 價格
            Label priceLabel = new Label
            {
                Text = price,
                Font = new Font("Microsoft YaHei", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 76, 60),
                Location = new Point(100, 60),
                AutoSize = true
            };

            // 加入購物車按鈕
            Button addButton = new Button
            {
                Text = "加入購物車",
                Size = new Size(100, 35),
                Location = new Point(160, 100),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei", 9)
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.BorderRadius = 5;

            iconPanel.Controls.Add(iconLabel);
            card.Controls.Add(iconPanel);
            card.Controls.Add(nameLabel);
            card.Controls.Add(priceLabel);
            card.Controls.Add(addButton);

            parent.Controls.Add(card);
        }

        private Panel CreateFeaturesPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(940, 200),
                BackColor = Color.FromArgb(52, 73, 94)
            };

            // 圓角效果
            panel.Paint += (sender, e) =>
            {
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 15;
                    path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                    path.AddArc(panel.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                    path.AddArc(panel.Width - radius * 2, panel.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(0, panel.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();
                    panel.Region = new Region(path);
                }
            };

            Label featuresLabel = new Label
            {
                Text = @"✨ 我們的特色 ✨
• 嚴選新鮮食材，每日現做
• 專業廚師團隊，品質保證
• 舒適用餐環境，免費WiFi
• 提供外送服務，滿$500免運費
• 會員積分制度，消費享折扣",
                Font = new Font("Microsoft YaHei", 12),
                ForeColor = Color.White,
                Location = new Point(50, 30),
                Size = new Size(800, 150)
            };

            panel.Controls.Add(featuresLabel);

            return panel;
        }
    }
}
    

