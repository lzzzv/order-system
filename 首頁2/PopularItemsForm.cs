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
    public partial class PopularItemsForm: Form
    {
        public PopularItemsForm()
        {
            InitializeComponent();
        }
        private void InitializePopularItemsForm()
        {
            this.Size = new Size(980, 730);
            this.BackColor = Color.White;

            Label title = new Label
            {
                Text = "🏆 本週熱銷排行榜",
                Font = new Font("Microsoft YaHei", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(50, 30),
                AutoSize = true
            };

            this.Controls.Add(title);

            // 添加熱銷商品列表
            CreatePopularList();
        }

        private void CreatePopularList()
        {
            string[] items = {
                "1. 招牌牛肉麵 - 銷售量: 385份",
                "2. 香辣雞腿飯 - 銷售量: 312份",
                "3. 海鮮披薩 - 銷售量: 298份",
                "4. 珍珠奶茶 - 銷售量: 456杯",
                "5. 水果沙拉 - 銷售量: 267份",
                "6. 提拉米蘇 - 銷售量: 189份",
                "7. 麻辣火鍋 - 銷售量: 234份",
                "8. 烤鮭魚排 - 銷售量: 178份"
            };

            int y = 100;
            foreach (string item in items)
            {
                Label itemLabel = new Label
                {
                    Text = "★ " + item,
                    Font = new Font("Microsoft YaHei", 14),
                    ForeColor = Color.FromArgb(80, 80, 80),
                    Location = new Point(80, y),
                    AutoSize = true
                };

                this.Controls.Add(itemLabel);
                y += 50;
            }
        }
    }
}
    

