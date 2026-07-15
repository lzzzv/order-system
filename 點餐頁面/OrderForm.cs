using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 點餐頁面
{

    public partial class OrderForm : Form
    {
       
        private Panel _cartListHost;
        private Panel _drawerContent;
        // ====== 色系 ======
        private readonly Color Bg = Color.FromArgb(248, 242, 232);
        private readonly Color Card = Color.White;
        private readonly Color TextMain = Color.FromArgb(33, 33, 33);
        private readonly Color TextSub = Color.FromArgb(115, 115, 115);
        private readonly Color Border = Color.FromArgb(235, 235, 235);
        private readonly Color Primary = Color.FromArgb(211, 47, 47);
        private readonly Color PrimaryDark = Color.FromArgb(154, 0, 7);
        private readonly Color SoftRed = Color.FromArgb(255, 238, 238);

        private readonly string ImagesFolder = @"E:\系統分析期末報告\點餐系統\餐點圖片";

        // ===== UI =====
        private TableLayoutPanel _layout;
        private FlowLayoutPanel _catList;

        private Panel _right;
        private Panel _productsHost;
        private FlowLayoutPanel _productsWrap;

        private TableLayoutPanel _bottomBar;
        private Button _btnBack;

        private int? _currentCatId = null;

        // ===== Cart Model =====
        private class CartItem
        {
            public int ProductId;
            public string Name;
            public decimal Price;
            public int Qty;
            public int CatId;
            public string ImgFile;
        }

        private readonly Dictionary<int, CartItem> _cart = new Dictionary<int, CartItem>();

        // ===== Drawer UI =====
        private Panel _cartDrawer;
        private bool _drawerOpen = false;
        private Timer _drawerAnim;
        private int _drawerTargetX;
        private const int DrawerWidth = 420;

        private FlowLayoutPanel _cartList;
        private Label _lblTotal;
        private Button _btnSubmit;
        private Button _btnClear;

        // ===== Floating Ball =====
        private FloatingBall _ball;
        private Timer _ballIdleTimer;

        private Timer _ballSnapTimer;
        private int _ballSnapTargetX;

        // ===== 這三個你之後可接登入/選擇 =====
        private int _customerId = 2;        
        private int _employeeId = 1;         
        private string _payWay = "現金";     

        public OrderForm()
        {
            BuildUI();
            LoadCategories();
            LoadProducts(null);
            UpdateCartUi();
        }

        private void BuildUI()
        {
            Text = "點餐介面";
            BackColor = Bg;
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Microsoft JhengHei UI", 10.5f, FontStyle.Regular);
            DoubleBuffered = true;

            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1200, 750);

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;

            Controls.Clear();

            _layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(28, 22, 28, 22)
            };
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            Controls.Add(_layout);

            _catList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 18, 0)
            };
            _layout.Controls.Add(_catList, 0, 0);

            _right = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 0, 0, 0)
            };
            _layout.Controls.Add(_right, 1, 0);

            _productsHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                Padding = new Padding(0, 18, 8, 22)
            };
            _right.Controls.Add(_productsHost);

            _productsHost.HorizontalScroll.Enabled = false;
            _productsHost.HorizontalScroll.Visible = false;

            _productsWrap = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            _productsHost.Controls.Add(_productsWrap);

            _productsHost.Resize += (s, e) =>
            {
                _productsWrap.MaximumSize = new Size(_productsHost.ClientSize.Width - 6, 0);
            };

            _bottomBar = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0, 10, 0, 10)
            };
            _bottomBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _bottomBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _layout.Controls.Add(_bottomBar, 0, 1);
            _layout.SetColumnSpan(_bottomBar, 2);

            _btnBack = OrderUi.PrimaryButton("返回", 220, 64, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            _btnBack.Anchor = AnchorStyles.Left;
            _btnBack.Click += (s, e) => Close();
            _bottomBar.Controls.Add(_btnBack, 0, 0);

            BuildCartDrawer();
            BuildFloatingBall();

            this.FormClosing += OrderForm_FormClosing;

            this.Resize += (s, e) =>
            {
                if (_cartDrawer != null)
                {
                    _cartDrawer.Height = ClientSize.Height - 40;
                    _cartDrawer.Top = 20;
                    _cartDrawer.Left = _drawerOpen ? ClientSize.Width - DrawerWidth - 16 : ClientSize.Width;
                }
                ClampBallInside();
                SnapBallToNearestEdge(animated: false);
            };
        }

        private void OrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cart.Count == 0) return;

            var result = MessageBox.Show(
                "購物車內還有商品。\n如果返回，購物車會清空。\n\n確定要返回嗎？",
                "提醒",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                _cart.Clear();
                RefreshCartDrawer();
                UpdateCartUi();
                return;
            }

            e.Cancel = true;
        }

        // =========================
        // Floating Ball
        // =========================
        private void BuildFloatingBall()
        {
            _ball = new FloatingBall
            {
                Size = new Size(76, 76),
                BallColor = Primary,
                BorderColor = Color.FromArgb(220, 255, 255, 255),
                AlphaActive = 255,
                AlphaIdle = 130,
                IdleMs = 1600
            };
            _ball.SetCount(0);

            Controls.Add(_ball);
            _ball.BringToFront();

            _ball.Left = ClientSize.Width - _ball.Width - 26;
            _ball.Top = ClientSize.Height - _ball.Height - 170;

            ClampBallInside();
            SnapBallToNearestEdge(animated: false);

            _ball.Click += (s, e) =>
            {
                _ball.SetActiveNow();
                ToggleDrawer();
            };

            _ball.MouseEnter += (s, e) => _ball.SetActiveNow();
            _ball.MouseMove += (s, e) => _ball.SetActiveNow();

            _ball.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                _ball.SetActiveNow();
                _ball.BeginDrag(e.Location);
            };
            _ball.MouseMove += (s, e) =>
            {
                if (!_ball.Dragging) return;
                _ball.DragTo(e.Location);
                ClampBallInside();
            };
            _ball.MouseUp += (s, e) =>
            {
                if (!_ball.Dragging) return;
                _ball.EndDrag();
                ClampBallInside();
                SnapBallToNearestEdge(animated: true);
            };

            _ballIdleTimer = new Timer { Interval = 120 };
            _ballIdleTimer.Tick += (s, e) => _ball.TickIdle();
            _ballIdleTimer.Start();

            _ballSnapTimer = new Timer { Interval = 15 };
            _ballSnapTimer.Tick += (s, e) =>
            {
                int speed = 22;
                int dx = _ballSnapTargetX - _ball.Left;
                if (Math.Abs(dx) <= speed)
                {
                    _ball.Left = _ballSnapTargetX;
                    _ballSnapTimer.Stop();
                }
                else
                {
                    _ball.Left += Math.Sign(dx) * speed;
                }
            };
        }

        private void ClampBallInside()
        {
            if (_ball == null) return;

            int margin = 10;
            int minX = margin;
            int maxX = ClientSize.Width - _ball.Width - margin;
            int minY = margin;
            int maxY = ClientSize.Height - _ball.Height - margin;

            _ball.Left = Math.Max(minX, Math.Min(maxX, _ball.Left));
            _ball.Top = Math.Max(minY, Math.Min(maxY, _ball.Top));
        }

        private void SnapBallToNearestEdge(bool animated)
        {
            if (_ball == null) return;

            int margin = 10;
            int leftEdge = margin;
            int rightEdge = ClientSize.Width - _ball.Width - margin;

            int centerX = _ball.Left + _ball.Width / 2;
            _ballSnapTargetX = (centerX < ClientSize.Width / 2) ? leftEdge : rightEdge;

            if (!animated)
            {
                _ball.Left = _ballSnapTargetX;
                _ballSnapTimer?.Stop();
                return;
            }

            _ballSnapTimer?.Stop();
            _ballSnapTimer?.Start();
        }

        // =========================
        // Drawer
        // =========================
        private void BuildCartDrawer()
        {
       
            _cartDrawer = new Panel
            {
                Width = DrawerWidth,
                Height = ClientSize.Height - 40,
                Top = 20,
                Left = ClientSize.Width,
                BackColor = Card,
                Padding = new Padding(0) 
            };
        
            OrderUi.ApplyRound(_cartDrawer, 18);

            _cartDrawer.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = _cartDrawer.ClientRectangle;
                r.Inflate(-1, -1);
                using (var pen = new Pen(Border, 1))
                using (var path = OrderUi.RoundRectPath(r, 18))
                    e.Graphics.DrawPath(pen, path);
            };

            _drawerContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(16, 16, 46, 16) 
            };
            _cartDrawer.Controls.Add(_drawerContent);

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 8)
            };
            _drawerContent.Controls.Add(header);

            var btnClose = OrderUi.PrimaryButton("✕", 40, 36, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            btnClose.Click += (s, e) => ToggleDrawer();
            header.Controls.Add(btnClose);

            header.Resize += (s, e) =>
            {
                btnClose.Left = header.Width - btnClose.Width;
                btnClose.Top = 0;
            };

            _cartListHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 12, 0, 8)
            };
            _drawerContent.Controls.Add(_cartListHost);

            _cartList = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            _cartListHost.Controls.Add(_cartList);

            var bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 150,
                BackColor = Color.Transparent
            };
            _drawerContent.Controls.Add(bottom);

            _lblTotal = new Label
            {
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Microsoft JhengHei UI", 13f, FontStyle.Bold),
                ForeColor = TextMain,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "總金額：NT$ 0"
            };
            bottom.Controls.Add(_lblTotal);

            var btnRow = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 100,
                ColumnCount = 2,
                RowCount = 1
            };
            btnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            btnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            bottom.Controls.Add(btnRow);

            _btnClear = OrderUi.PrimaryButton("清空", 160, 56, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            _btnClear.Click += (s, e) =>
            {
                if (_cart.Count == 0) return;
                if (MessageBox.Show("確定要清空購物車？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _cart.Clear();
                    RefreshCartDrawer();
                    UpdateCartUi();
                }
            };

            _btnSubmit = OrderUi.PrimaryButton("送出訂單", 220, 56, Primary, Color.White);
            _btnSubmit.Click += async (s, e) => await SubmitOrderAsync();

            btnRow.Controls.Add(_btnClear, 0, 0);
            btnRow.Controls.Add(_btnSubmit, 1, 0);

            Controls.Add(_cartDrawer);

            _drawerAnim = new Timer { Interval = 15 };
            _drawerAnim.Tick += (s, e) =>
            {
                int speed = 32;
                int dx = _drawerTargetX - _cartDrawer.Left;

                if (Math.Abs(dx) <= speed)
                {
                    _cartDrawer.Left = _drawerTargetX;
                    _drawerAnim.Stop();
                }
                else
                {
                    _cartDrawer.Left += Math.Sign(dx) * speed;
                }

                _cartDrawer.BringToFront();
                _ball?.BringToFront();
            };

            RefreshCartDrawer();
        }

        private void ToggleDrawer()
        {
            RefreshCartDrawer();

            _drawerOpen = !_drawerOpen;
            _drawerTargetX = _drawerOpen ? ClientSize.Width - DrawerWidth - 16 : ClientSize.Width;

            _drawerAnim.Stop();
            _drawerAnim.Start();

            _cartDrawer.BringToFront();
            _ball?.BringToFront();
        }

        private void RefreshCartDrawer()
        {
            if (_cartList == null) return;

            _cartList.SuspendLayout();
            _cartList.Controls.Clear();

            if (_cart.Count == 0)
            {
                _cartList.Controls.Add(new Label
                {
                    Text = "購物車是空的",
                    AutoSize = true,
                    ForeColor = TextSub,
                    Font = new Font("Microsoft JhengHei UI", 11f),
                    Padding = new Padding(4, 10, 4, 10)
                });

                _lblTotal.Text = "總金額：NT$ 0";
                _btnSubmit.Enabled = false;
                _btnClear.Enabled = false;

                _cartList.ResumeLayout();
                return;
            }

            _btnSubmit.Enabled = true;
            _btnClear.Enabled = true;

            foreach (var item in _cart.Values.OrderBy(x => x.ProductId))
                _cartList.Controls.Add(BuildCartRow(item));

            _lblTotal.Text = $"總金額：NT$ {CalcTotal():0}";
            _cartList.ResumeLayout();
        }

        private Control BuildCartRow(CartItem item)
        {
            var row = new Panel
            {
                Width = _cartDrawer.ClientSize.Width - 32,
                Height = 84,
                BackColor = Color.White,
                Margin = new Padding(0, 45, 0, 5),
                Padding = new Padding(12, 10, 28, 10)
            };

            row.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = row.ClientRectangle;
                r.Inflate(-1, -1);
                using (var pen = new Pen(Border, 1))
                using (var path = OrderUi.RoundRectPath(r, 14))
                    e.Graphics.DrawPath(pen, path);
            };

            var left = new Panel { Dock = DockStyle.Fill };
            row.Controls.Add(left);

            left.Controls.Add(new Label
            {
                Text = item.Name,
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Microsoft JhengHei UI", 11.8f, FontStyle.Bold),
                ForeColor = TextMain,
                AutoEllipsis = true
            });

            left.Controls.Add(new Label
            {
                Text = $"NT$ {item.Price:0}",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Microsoft JhengHei UI", 10.5f),
                ForeColor = TextSub
            });

            var right = new Panel { Dock = DockStyle.Right, Width = 135 };
            row.Controls.Add(right);

            int btnW = 34, btnH = 32;
            int top = (row.Height - btnH) / 2;

            var btnMinus = OrderUi.PrimaryButton("－", btnW, btnH, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            btnMinus.Left = 0;
            btnMinus.Top = top;

            var lblQty = new Label
            {
                Text = item.Qty.ToString(),
                Width = 40,
                Height = btnH,
                Left = btnW + 6,
                Top = top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft JhengHei UI", 11f, FontStyle.Bold),
                ForeColor = TextMain
            };

            var btnPlus = OrderUi.PrimaryButton("＋", btnW, btnH, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            btnPlus.Left = btnW + 6 + 40 + 6;
            btnPlus.Top = top;

            btnMinus.Click += (s, e) =>
            {
                if (!_cart.ContainsKey(item.ProductId)) return;
                _cart[item.ProductId].Qty--;
                if (_cart[item.ProductId].Qty <= 0) _cart.Remove(item.ProductId);
                RefreshCartDrawer();
                UpdateCartUi();
            };

            btnPlus.Click += (s, e) =>
            {
                if (!_cart.ContainsKey(item.ProductId)) return;
                _cart[item.ProductId].Qty++;
                RefreshCartDrawer();
                UpdateCartUi();
            };

            right.Controls.Add(btnMinus);
            right.Controls.Add(lblQty);
            right.Controls.Add(btnPlus);

            return row;
        }

        private decimal CalcTotal() => _cart.Values.Sum(x => x.Price * x.Qty);

        private void UpdateCartUi()
        {
            int totalQty = _cart.Values.Sum(x => x.Qty);
            _ball?.SetCount(totalQty);

            if (_lblTotal != null)
                _lblTotal.Text = $"總金額：NT$ {CalcTotal():0}";

            if (_btnSubmit != null)
                _btnSubmit.Enabled = _cart.Count > 0;
            if (_btnClear != null)
                _btnClear.Enabled = _cart.Count > 0;
        }

        public void AddToCartAndOpenDrawer(int productId, string name, decimal price, int catId, string imgFile, int qty = 1)
        {
            if (qty < 1) qty = 1;

            if (_cart.TryGetValue(productId, out var it))
            {
                it.Qty += qty;
            }
            else
            {
                _cart[productId] = new CartItem
                {
                    ProductId = productId,
                    Name = name,
                    Price = price,
                    Qty = qty,
                    CatId = catId,
                    ImgFile = imgFile
                };
            }

            UpdateCartUi();
            OpenDrawer();
        }

        public void OpenDrawer()
        {
            if (_cartDrawer == null) return;

            RefreshCartDrawer();

            if (!_drawerOpen)
            {
                _drawerOpen = true;
                _drawerTargetX = ClientSize.Width - DrawerWidth - 16;
                _drawerAnim.Stop();
                _drawerAnim.Start();
            }
            else
            {
                RefreshCartDrawer();
            }

            _cartDrawer.BringToFront();
            _ball?.BringToFront();
        }

        private async Task SubmitOrderAsync()
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("購物車是空的，不能送出。");
                return;
            }

            _btnSubmit.Enabled = false;

            try
            {
                var items = _cart.ToDictionary(
                    kv => kv.Key,
                    kv => (qty: kv.Value.Qty, price: kv.Value.Price)
                );

                decimal total = CalcTotal();
                DateTime now = DateTime.Now;

                int orderId = await Task.Run(() =>
                    Db.CreateOrderWithPayment(
                        items,
                        total,
                        customerId: _customerId,
                        employeeId: _employeeId,
                        payWay: _payWay,
                        payTime: now,
                        orderStatus: "PAID"
                    )
                );

                MessageBox.Show($"訂單送出成功！\n訂單編號：{orderId}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _cart.Clear();
                RefreshCartDrawer();
                UpdateCartUi();

                if (_drawerOpen) ToggleDrawer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("送出訂單失敗：\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnSubmit.Enabled = _cart.Count > 0;
            }
        }

        // =========================
        // Categories / Products
        // =========================
        private void LoadCategories()
        {
            _catList.Controls.Clear();
            _catList.Controls.Add(MakeCatButton("全部", null, true));

            List<Db.CatRow> cats;
            try { cats = Db.GetCategories(); }
            catch (Exception ex)
            {
                _catList.Controls.Add(new Label { Text = "分類載入失敗：\n" + ex.Message, AutoSize = true, ForeColor = TextSub });
                return;
            }

            foreach (var c in cats)
                _catList.Controls.Add(MakeCatButton(c.CatName, c.CatId, false));
        }

        private Button MakeCatButton(string text, int? catId, bool isActive)
        {
            var b = new Button
            {
                Text = text,
                Width = 290,
                Height = 56,
                Margin = new Padding(0, 0, 0, 12),
                BackColor = isActive ? SoftRed : Card,
                ForeColor = isActive ? PrimaryDark : TextMain,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 12f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 10, 0)
            };
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Border;
            OrderUi.ApplyRound(b, 16);

            b.Click += (s, e) =>
            {
                foreach (var bb in _catList.Controls.OfType<Button>())
                {
                    bb.BackColor = Card;
                    bb.ForeColor = TextMain;
                }
                b.BackColor = SoftRed;
                b.ForeColor = PrimaryDark;

                _currentCatId = catId;
                LoadProducts(_currentCatId);
            };

            return b;
        }

        private void LoadProducts(int? catId)
        {
            _productsWrap.Controls.Clear();

            List<Db.ProductRow> items;
            try { items = Db.GetProducts(catId); }
            catch (Exception ex)
            {
                _productsWrap.Controls.Add(new Label { Text = "商品載入失敗：\n" + ex.Message, AutoSize = true, ForeColor = TextSub });
                return;
            }

            if (items.Count == 0)
            {
                _productsWrap.Controls.Add(new Label { Text = "此分類沒有商品", AutoSize = true, ForeColor = TextSub });
                return;
            }

            foreach (var p in items)
            {
                string imgPath = BuildLocalImagePath(p.ImgFile, p.CatId);
                _productsWrap.Controls.Add(CreateProductCard(p, imgPath));
            }
        }

        private string BuildLocalImagePath(string fileNameFromDb, int catId)
        {
            string fileName = string.IsNullOrWhiteSpace(fileNameFromDb) ? $"cat_{catId}.jpg" : fileNameFromDb;
            return Path.Combine(ImagesFolder, fileName);
        }

        private Control CreateProductCard(Db.ProductRow p, string imgPath)
        {
            var card = new Panel
            {
                Width = 360,
                Height = 270,
                BackColor = Card,
                Margin = new Padding(0, 0, 18, 18),
                Padding = new Padding(16)
            };
            OrderUi.ApplyRound(card, 18);

            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = card.ClientRectangle;
                r.Inflate(-1, -1);
                using (var pen = new Pen(Border, 1))
                using (var path = OrderUi.RoundRectPath(r, 18))
                    e.Graphics.DrawPath(pen, path);
            };

            var header = new Panel { Dock = DockStyle.Top, Height = 48 };
            card.Controls.Add(header);

            header.Controls.Add(new Label
            {
                Text = p.Name,
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft JhengHei UI", 13f, FontStyle.Bold),
                ForeColor = TextMain,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Padding = new Padding(2, 0, 100, 0)
            });

            var badge = new Label
            {
                Text = EmojiByCat(p.CatId),
                AutoSize = true,
                BackColor = SoftRed,
                ForeColor = PrimaryDark,
                Font = new Font("Segoe UI Emoji", 15f),
                Padding = new Padding(10, 7, 10, 7)
            };
            OrderUi.ApplyRound(badge, 16);
            header.Controls.Add(badge);

            header.Resize += (s, e) =>
            {
                badge.Left = header.Width - badge.Width - 4;
                badge.Top = (header.Height - badge.Height) / 2;
            };

            var imgHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8, 6, 8, 12) };
            card.Controls.Add(imgHost);

            var pic = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom };
            imgHost.Controls.Add(pic);
            OrderUi.LoadLocalImage(pic, imgPath);

            var bottom = new TableLayoutPanel { Dock = DockStyle.Bottom, Height = 70, ColumnCount = 2, RowCount = 1 };
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            card.Controls.Add(bottom);

            bottom.Controls.Add(new Label
            {
                Text = $"NT$ {p.Price:0}",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft JhengHei UI", 12.5f, FontStyle.Bold),
                ForeColor = TextMain
            }, 0, 0);

            var btnAdd = OrderUi.PrimaryButton("加入", 140, 46, Color.FromArgb(245, 245, 245), Color.FromArgb(60, 60, 60));
            btnAdd.Anchor = AnchorStyles.Right;
            btnAdd.Margin = new Padding(0, 12, 0, 12);

            btnAdd.Click += (s, e) =>
            {
                if (_cart.TryGetValue(p.ProductId, out var it))
                {
                    it.Qty++;
                }
                else
                {
                    _cart[p.ProductId] = new CartItem
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Price = p.Price,
                        Qty = 1,
                        CatId = p.CatId,
                        ImgFile = p.ImgFile
                    };
                }

                UpdateCartUi();
                if (_drawerOpen) RefreshCartDrawer();
            };

            bottom.Controls.Add(btnAdd, 1, 0);

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
    }

    // ===== 懸浮球控制項 =====
    internal class FloatingBall : Control
    {
        public Color BallColor { get; set; } = Color.Red;
        public Color BorderColor { get; set; } = Color.White;
        public int AlphaActive { get; set; } = 255;
        public int AlphaIdle { get; set; } = 140;
        public int IdleMs { get; set; } = 1600;

        private int _alpha;
        private DateTime _lastActive = DateTime.Now;
        private bool _dragging = false;
        private Point _dragOffset;
        private int _count = 0;

        public bool Dragging => _dragging;

        public FloatingBall()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);

            _alpha = AlphaActive;
            Font = new Font("Microsoft JhengHei UI", 12.5f, FontStyle.Bold);
            ForeColor = Color.White;
        }

        public void SetCount(int c) { _count = c; Invalidate(); }

        public void SetActiveNow()
        {
            _lastActive = DateTime.Now;
            _alpha = AlphaActive;
            Invalidate();
        }

        public void TickIdle()
        {
            var idle = (DateTime.Now - _lastActive).TotalMilliseconds >= IdleMs;
            int target = idle ? AlphaIdle : AlphaActive;

            if (_alpha != target)
            {
                int step = 12;
                if (_alpha < target) _alpha = Math.Min(target, _alpha + step);
                else _alpha = Math.Max(target, _alpha - step);
                Invalidate();
            }
        }

        public void BeginDrag(Point mouseLocation)
        {
            _dragging = true;
            _dragOffset = mouseLocation;
        }

        public void DragTo(Point mouseLocationInBall)
        {
            if (!_dragging) return;
            Left = Left + (mouseLocationInBall.X - _dragOffset.X);
            Top = Top + (mouseLocationInBall.Y - _dragOffset.Y);
            SetActiveNow();
        }

        public void EndDrag()
        {
            _dragging = false;
            SetActiveNow();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = ClientRectangle;
            rect.Inflate(-2, -2);

            using (var brush = new SolidBrush(Color.FromArgb(_alpha, BallColor)))
                e.Graphics.FillEllipse(brush, rect);

            using (var pen = new Pen(BorderColor, 2))
                e.Graphics.DrawEllipse(pen, rect);

            string text = $"🛒{_count}";
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            using (var textBrush = new SolidBrush(Color.FromArgb(Math.Min(255, _alpha + 40), ForeColor)))
                e.Graphics.DrawString(text, Font, textBrush, ClientRectangle, sf);
        }
    }

    internal static class OrderUi
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
                Font = new Font("Microsoft JhengHei UI", 12f, FontStyle.Bold)
            };
            b.FlatAppearance.BorderSize = 0;
            ApplyRound(b, 16);
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
            catch { }
        }

    }
}