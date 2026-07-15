using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 點餐頁面
{
    public class HomeForm : Form
    {
        // ======背景======
        private readonly Color Bg = Color.FromArgb(248, 242, 232);
        private readonly Color Card = Color.White;
        private readonly Color TextMain = Color.FromArgb(33, 33, 33);
        private readonly Color TextSub = Color.FromArgb(115, 115, 115);
        private readonly Color Border = Color.FromArgb(235, 235, 235);

        private readonly Color Primary = Color.FromArgb(211, 47, 47);
        private readonly Color PrimaryDark = Color.FromArgb(154, 0, 7);
        private readonly Color SoftRed = Color.FromArgb(255, 238, 238);

        private readonly string HeroImagePath = @"E:\系統分析期末報告\點餐系統\好爽漢堡LOGO.jpg";
        private readonly string ImagesFolder = @"E:\系統分析期末報告\點餐系統\餐點圖片";

        private Panel _root;
        private Panel _container;
        private FlowLayoutPanel _stack;
        private Panel _bestScroller;
        private FlowLayoutPanel _bestWrap;

        private readonly List<Control> _fullWidth = new List<Control>();
        private bool _layoutHooked = false;

        private OrderForm _orderForm;

        public HomeForm()
        {
            this.Text = "歡迎使用好爽餐廳點餐系統!!!!!";
            this.BackColor = Bg;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Font = new Font("Microsoft JhengHei UI", 10.5f, FontStyle.Regular);
            this.DoubleBuffered = true;

            Build();
        }

        private void OpenOrderAndAddToCart(ProductSale p, int qty = 1)
        {
            if (_orderForm == null || _orderForm.IsDisposed)
                _orderForm = new OrderForm();

            if (!_orderForm.Visible)
                _orderForm.Show();

            _orderForm.BringToFront();
            _orderForm.Activate();

            _orderForm.AddToCartAndOpenDrawer(
                productId: p.ProductId,
                name: p.Name,
                price: p.Price,
                catId: p.CatId,
                imgFile: p.ImgUrl,
                qty: qty
            );

        }

        private void Build()
        {
            // 視窗
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1400, 750);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.ShowIcon = false;

            this.Controls.Clear();
            _fullWidth.Clear();

            _root = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Bg,
                Padding = new Padding(26, 20, 26, 20)
            };
            this.Controls.Add(_root);

            // =====Container=====
            _container = new Panel
            {
                Width = 1120,
                Height = 10,
                AutoSize = false,
                BackColor = Color.Transparent
            };
            _root.Controls.Add(_container);

            _stack = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };
            _container.Controls.Add(_stack);

            var topBar = CreateTopBar();
            AddFullWidth(topBar);
            _stack.Controls.Add(topBar);

            var hero = CreateHero();
            AddFullWidth(hero);
            _stack.Controls.Add(hero);

            var s1 = SectionTitle("🔥 熱銷商品", "大家都在點，點了不會尷尬");
            AddFullWidth(s1);
            _stack.Controls.Add(s1);

            var best = CreateBestSellers(topN: 5);
            AddFullWidth(best);
            _stack.Controls.Add(best);

            // 餐廳介紹
            var s2 = SectionTitle("🏠 餐廳介紹", "我們把「好吃」當成最低標準");
            AddFullWidth(s2);
            _stack.Controls.Add(s2);

            var about = CreateAbout();
            AddFullWidth(about);
            _stack.Controls.Add(about);

            var footer = CreateFooter();
            AddFullWidth(footer);
            _stack.Controls.Add(footer);

            if (!_layoutHooked)
            {
                _layoutHooked = true;
                _root.Resize += (s, e) => RefreshLayout();
                this.Shown += (s, e) => RefreshLayout();
            }

            _stack.SizeChanged += (s, e) => SyncHeights();
            RefreshLayout();
        }

        private Control CreateTopBar()
        {
            var bar = new Panel
            {
                Height = 56,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 14)
            };

            var brand = new Label
            {
                Text = "好爽餐廳",
                AutoSize = true,
                Font = new Font("Microsoft JhengHei UI", 13.5f, FontStyle.Bold),
                ForeColor = TextMain,
                Location = new Point(0, 14)
            };
            bar.Controls.Add(brand);

            var btnExit = Ui.PrimaryButton("離開", 96, 40, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            btnExit.Top = 8;
            btnExit.Click += (s, e) => this.Close();
            bar.Controls.Add(btnExit);

            bar.Resize += (s, e) => { btnExit.Left = bar.Width - btnExit.Width; };

            return bar;
        }

        private void AddFullWidth(Control c) => _fullWidth.Add(c);

        private void RefreshLayout()
        {
            CenterContainer();

            for (int i = 0; i < _fullWidth.Count; i++)
                _fullWidth[i].Width = _container.Width;

            if (_stack != null) _stack.Width = _container.Width;
            SyncHeights();
        }

        private void SyncHeights()
        {
            if (_stack == null || _container == null) return;
            _container.Height = _stack.Height;
        }

        private void CenterContainer()
        {
            if (_root == null || _container == null) return;

            int viewport = _root.ClientSize.Width - _root.Padding.Left - _root.Padding.Right;
            if (viewport <= 0) return;

            int w = Math.Min(1120, viewport);
            _container.Width = w;

            int x = _root.Padding.Left + Math.Max(0, (viewport - w) / 2);
            _container.Left = x;
            _container.Top = _root.Padding.Top;
        }

        // UI blocks
        private Control SectionTitle(string title, string subtitle)
        {
            var wrap = new Panel
            {
                Height = 76,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 10)
            };

            var t = new Label
            {
                Text = title,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Microsoft JhengHei UI", 16f, FontStyle.Bold),
                ForeColor = TextMain
            };
            wrap.Controls.Add(t);

            var s = new Label
            {
                Text = subtitle,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Microsoft JhengHei UI", 10.5f, FontStyle.Regular),
                ForeColor = TextSub
            };
            wrap.Controls.Add(s);

            var line = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 3,
                BackColor = SoftRed
            };
            Ui.ApplyRound(line, 8);
            wrap.Controls.Add(line);

            return wrap;
        }

        private Control CreateHero()
        {
            var hero = new Panel
            {
                Height = 260,
                BackColor = Card,
                Margin = new Padding(0, 0, 0, 18),
                Padding = new Padding(22)
            };
            Ui.ApplyRound(hero, 20);

            hero.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = hero.ClientRectangle;
                rect.Inflate(-1, -1);

                using (GraphicsPath path = Ui.RoundRectPath(rect, 20))
                using (LinearGradientBrush br = new LinearGradientBrush(
                    rect,
                    Color.FromArgb(255, 252, 250),
                    Color.FromArgb(246, 241, 235),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillPath(br, path);
                }

                using (Pen pen = new Pen(Border, 1))
                using (GraphicsPath pth = Ui.RoundRectPath(rect, 20))
                {
                    e.Graphics.DrawPath(pen, pth);
                }
            };

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            hero.Controls.Add(grid);

            var left = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var right = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(10, 0, 0, 0) };
            grid.Controls.Add(left, 0, 0);
            grid.Controls.Add(right, 1, 0);

            var h1 = new Label
            {
                Text = "好爽餐廳\n今天也要吃得很爽!!!!",
                Dock = DockStyle.Top,
                Height = 106,
                Font = new Font("Microsoft JhengHei UI", 26f, FontStyle.Bold),
                ForeColor = TextMain
            };
            left.Controls.Add(h1);

            var p = new Label
            {
                Text = "現點現做｜熱銷套餐｜3 分鐘急速上桌\n用料不手軟，回味無窮。",
                Dock = DockStyle.Top,
                Height = 56,
                Font = new Font("Microsoft JhengHei UI", 11f, FontStyle.Regular),
                ForeColor = TextSub
            };
            left.Controls.Add(p);

            var btnRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 52,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 14, 0, 0)
            };
            left.Controls.Add(btnRow);

            var btnOrder = Ui.PrimaryButton("開始點餐", 140, 42, Primary, Color.White);
            btnOrder.Click += (s, e) =>
            {

                if (_orderForm == null || _orderForm.IsDisposed)
                    _orderForm = new OrderForm();

                if (!_orderForm.Visible) _orderForm.Show();
                _orderForm.BringToFront();
                _orderForm.Activate();

            };
            btnRow.Controls.Add(btnOrder);

            var btnPromo = Ui.PrimaryButton("查看優惠", 140, 42, Color.FromArgb(245, 245, 245), PrimaryDark);
            btnRow.Controls.Add(btnPromo);

            var preview = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(120, 165, 178),
                Padding = new Padding(14)
            };
            Ui.ApplyRound(preview, 18);
            right.Controls.Add(preview);

            var heroPic = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(0)
            };
            preview.Controls.Add(heroPic);

            Ui.LoadLocalImage(heroPic, HeroImagePath);

            var badge = new Label
            {
                Text = "TOP 1  招牌牛肉堡",
                AutoSize = true,
                BackColor = SoftRed,
                ForeColor = PrimaryDark,
                Font = new Font("Microsoft JhengHei UI", 10f, FontStyle.Bold),
                Padding = new Padding(10, 6, 10, 6)
            };
            Ui.ApplyRound(badge, 12);
            preview.Controls.Add(badge);
            badge.BringToFront();

            preview.Resize += (s, e) =>
            {
                badge.Left = 14;
                badge.Top = 14;
            };

            return hero;
        }

        private Control CreateBestSellers(int topN)
        {
            _bestScroller = new Panel
            {
                Height = 245,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 18),
                AutoScroll = true,
                TabStop = true
            };

            _bestScroller.HorizontalScroll.Enabled = true;
            _bestScroller.HorizontalScroll.Visible = true;
            _bestScroller.VerticalScroll.Enabled = false;

            _bestWrap = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            _bestScroller.Controls.Add(_bestWrap);
            _bestScroller.MouseEnter += (s, e) => _bestScroller.Focus();

            List<ProductSale> tops;
            try
            {
                tops = Db.GetTopSellers(topN);
            }
            catch (Exception ex)
            {
                _bestWrap.Controls.Add(new Label
                {
                    Text = "熱銷查詢失敗：\n" + ex.Message,
                    AutoSize = true,
                    ForeColor = TextSub,
                    Margin = new Padding(0, 12, 0, 0)
                });
                return _bestScroller;
            }

            if (tops.Count == 0)
            {
                _bestWrap.Controls.Add(new Label
                {
                    Text = "目前還沒有銷售資料（訂單明細或狀態可能沒有 PAID）",
                    AutoSize = true,
                    ForeColor = TextSub,
                    Margin = new Padding(0, 12, 0, 0)
                });
                return _bestScroller;
            }

            _bestWrap.Controls.Clear();

            foreach (var p in tops)
            {
                string localPath = BuildLocalImagePath(p.ImgUrl, p.CatId);
                _bestWrap.Controls.Add(CreateFoodCardWrap(p, localPath));
            }

            _bestWrap.SizeChanged += (s, e) =>
            {
                _bestScroller.AutoScrollMinSize = new Size(_bestWrap.Width + 10, 0);
            };

            return _bestScroller;
        }

        private string BuildLocalImagePath(string fileNameFromDb, int catId)
        {
            string fileName = fileNameFromDb;
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = $"cat_{catId}.jpg";

            return Path.Combine(ImagesFolder, fileName);
        }

        private Control CreateFoodCardWrap(ProductSale p, string localImagePath)
        {
            var wrap = new Panel
            {
                Width = 260,
                Height = 220,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 14, 0)
            };

            var card = CreateFoodCard(
                p: p,
                desc: $"已售 {p.SoldQty} 份",
                priceText: $"NT$ {p.Price:0}",
                emoji: EmojiByCat(p.CatId),
                localImagePath: localImagePath
            );

            int baseW = 254;
            int baseH = 210;
            int grow = 10;

            card.Size = new Size(baseW, baseH);
            CenterInWrap(wrap, card);
            wrap.Controls.Add(card);

            bool hovering = false;
            var leaveTimer = new Timer { Interval = 50 };

            void ApplyGrow()
            {
                if (hovering) return;
                hovering = true;

                wrap.SuspendLayout();
                card.SuspendLayout();

                card.Size = new Size(baseW + grow, baseH + grow);
                CenterInWrap(wrap, card);

                card.ResumeLayout(true);
                wrap.ResumeLayout(true);
            }

            void ApplyShrink()
            {
                if (!hovering) return;
                hovering = false;

                wrap.SuspendLayout();
                card.SuspendLayout();

                card.Size = new Size(baseW, baseH);
                CenterInWrap(wrap, card);

                card.ResumeLayout(true);
                wrap.ResumeLayout(true);
            }

            void HookEnter(Control c)
            {
                c.MouseEnter += (s, e) =>
                {
                    leaveTimer.Stop();
                    ApplyGrow();
                };
            }

            void HookLeave(Control c)
            {
                c.MouseLeave += (s, e) =>
                {
                    leaveTimer.Stop();
                    leaveTimer.Start();
                };
            }

            leaveTimer.Tick += (s, e) =>
            {
                leaveTimer.Stop();
                if (wrap.IsDisposed) return;

                Rectangle screenRect = wrap.RectangleToScreen(wrap.ClientRectangle);
                if (screenRect.Contains(Cursor.Position)) return;

                ApplyShrink();
            };

            HookEnter(wrap);
            HookLeave(wrap);

            HookEnter(card);
            HookLeave(card);

            foreach (Control child in card.Controls)
            {
                HookEnter(child);
                HookLeave(child);
            }

            return wrap;
        }

        private void CenterInWrap(Control wrap, Control inner)
        {
            inner.Left = (wrap.Width - inner.Width) / 2;
            inner.Top = (wrap.Height - inner.Height) / 2;
        }

        private Panel CreateFoodCard(ProductSale p, string desc, string priceText, string emoji, string localImagePath)
        {
            var card = new Panel
            {
                BackColor = Card,
                Padding = new Padding(14)
            };
            Ui.ApplyRound(card, 16);

            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = card.ClientRectangle;
                r.Inflate(-1, -1);
                using (var pen = new Pen(Border, 1))
                using (var path = Ui.RoundRectPath(r, 16))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };

            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 44, BackColor = Color.Transparent };

            var lblPrice = new Label
            {
                Text = priceText,
                AutoSize = true,
                Location = new Point(0, 12),
                Font = new Font("Microsoft JhengHei UI", 11.2f, FontStyle.Bold),
                ForeColor = TextMain
            };
            bottom.Controls.Add(lblPrice);

            var btn = Ui.PrimaryButton("加入", 80, 30, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            btn.Top = 7;
            bottom.Controls.Add(btn);
            bottom.Resize += (s, e) => { btn.Left = bottom.Width - btn.Width; };

            btn.Click += (s, e) => OpenOrderAndAddToCart(p, qty: 1);

            var pic = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 95,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(0, 4, 0, 8)
            };

            var title = new Label
            {
                Text = p.Name,
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Microsoft JhengHei UI", 12.2f, FontStyle.Bold),
                ForeColor = TextMain
            };

            var sub = new Label
            {
                Text = desc,
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Microsoft JhengHei UI", 10.3f, FontStyle.Regular),
                ForeColor = TextSub
            };

            var badge = new Label
            {
                Text = emoji,
                AutoSize = true,
                BackColor = SoftRed,
                ForeColor = PrimaryDark,
                Font = new Font("Segoe UI Emoji", 16f, FontStyle.Regular),
                Padding = new Padding(8, 6, 8, 6)
            };
            Ui.ApplyRound(badge, 14);

            card.Controls.Add(bottom);
            card.Controls.Add(pic);
            card.Controls.Add(sub);
            card.Controls.Add(title);
            card.Controls.Add(badge);
            badge.BringToFront();

            card.Resize += (s, e) =>
            {
                badge.Left = card.Width - badge.Width - 12;
                badge.Top = 10;
            };

            Ui.LoadLocalImage(pic, localImagePath);
            return card;
        }

        private string EmojiByCat(int catId)
        {
            switch (catId)
            {
                case 2: return "🧋";
                case 3: return "🍔";
                case 4: return "🍕";
                case 5: return "🍝";
                case 6: return "🍟";
                case 7: return "🍱";
                case 8: return "🍰";
                default: return "🍽️";
            }
        }

        private Control CreateAbout()
        {
            var card = new Panel
            {
                Height = 220,
                BackColor = Card,
                Margin = new Padding(0, 0, 0, 18),
                Padding = new Padding(18)
            };
            Ui.ApplyRound(card, 16);

            var title = new Label
            {
                Text = "關於我們",
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Microsoft JhengHei UI", 13.5f, FontStyle.Bold),
                ForeColor = TextMain
            };
            card.Controls.Add(title);

            var body = new Label
            {
                Text = "我們主打：吃得爽、速度快、品質穩。\n\n✔ 現點現做\n✔ 每日限量熱銷套餐\n✔ 可客製化加點\n✔ 價格透明、明細清楚",
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft JhengHei UI", 10.6f, FontStyle.Regular),
                ForeColor = TextSub
            };
            card.Controls.Add(body);

            return card;
        }

        private Control CreateFooter()
        {
            var footer = new Panel
            {
                Height = 54,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };

            var label = new Label
            {
                Dock = DockStyle.Fill,
                Text = "© 2025 爽餐廳｜營業 11:00–21:00｜電話 02-xxxx-xxxx｜地址 台北市XX區XX路",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(140, 140, 140),
                Font = new Font("Microsoft JhengHei UI", 9.8f, FontStyle.Regular)
            };
            footer.Controls.Add(label);

            return footer;
        }
    }

    internal static class Ui
    {
        public static Button PrimaryButton(string text, int w, int h, Color back, Color fore)
        {
            var b = new Button
            {
                Text = text,
                Width = w,
                Height = h,
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 11f, FontStyle.Bold)
            };
            b.FlatAppearance.BorderSize = 0;
            ApplyRound(b, 12);
            return b;
        }

        public static void ApplyRound(Control c, int radius)
        {
            void apply()
            {
                if (c.Width <= 0 || c.Height <= 0) return;

                Rectangle r = new Rectangle(0, 0, c.Width, c.Height);
                using (GraphicsPath path = RoundRectPath(r, radius))
                {
                    Region old = c.Region;
                    c.Region = new Region(path);
                    old?.Dispose();
                }
            }

            c.Resize += (s, e) => apply();
            apply();
        }

        public static GraphicsPath RoundRectPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            Rectangle rr = new Rectangle(r.X, r.Y, Math.Max(1, r.Width - 1), Math.Max(1, r.Height - 1));

            if (radius <= 0)
            {
                path.AddRectangle(rr);
                path.CloseFigure();
                return path;
            }

            path.AddArc(rr.X, rr.Y, d, d, 180, 90);
            path.AddArc(rr.Right - d, rr.Y, d, d, 270, 90);
            path.AddArc(rr.Right - d, rr.Bottom - d, d, d, 0, 90);
            path.AddArc(rr.X, rr.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void LoadLocalImage(PictureBox pb, string path)
        {
            if (pb == null) return;
            if (string.IsNullOrWhiteSpace(path)) return;

            try
            {
                if (!File.Exists(path)) return;

                byte[] bytes = File.ReadAllBytes(path);
                using (var ms = new MemoryStream(bytes))
                using (var tmp = Image.FromStream(ms))
                {
                    pb.Image = new Bitmap(tmp);
                }
            }
            catch
            {
            }
        }
    }
}

