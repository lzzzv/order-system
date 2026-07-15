using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RestaurantOrderSystem
{
    public class ModernButton : Button
    {
        public Color HoverColor { get; set; } = Color.FromArgb(255, 102, 89);
        public Color PressColor { get; set; } = Color.FromArgb(154, 0, 7);
        public int CornerRadius { get; set; } = 20;
        public bool UseShadow { get; set; } = true;
        public Color ShadowColor { get; set; } = Color.FromArgb(100, 0, 0, 0);
        public int ShadowDepth { get; set; } = 5;

        private bool isHovered = false;
        private bool isPressed = false;

        public ModernButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Microsoft JhengHei", 10, FontStyle.Bold);
            this.ForeColor = Color.White;

            this.MouseEnter += (s, e) => { isHovered = true; this.Invalidate(); };
            this.MouseLeave += (s, e) => { isHovered = false; this.Invalidate(); };
            this.MouseDown += (s, e) => { isPressed = true; this.Invalidate(); };
            this.MouseUp += (s, e) => { isPressed = false; this.Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            // 繪製陰影
            if (UseShadow && !isPressed)
            {
                Rectangle shadowRect = new Rectangle(
                    ShadowDepth, ShadowDepth,
                    rect.Width - ShadowDepth, rect.Height - ShadowDepth);
                using (GraphicsPath shadowPath = GetRoundedRectPath(shadowRect, CornerRadius))
                {
                    using (SolidBrush shadowBrush = new SolidBrush(ShadowColor))
                    {
                        g.FillPath(shadowBrush, shadowPath);
                    }
                }
            }

            // 繪製按鈕背景
            Color backColor = this.BackColor;
            if (isPressed)
                backColor = PressColor;
            else if (isHovered)
                backColor = HoverColor;

            using (GraphicsPath path = GetRoundedRectPath(rect, CornerRadius))
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    g.FillPath(brush, path);
                }
            }

            // 繪製文字
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor),
                        rect, format);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            return path;
        }
    }

    public class ModernPanel : Panel
    {
        public int CornerRadius { get; set; } = 10;
        public Color BorderColor { get; set; } = Color.FromArgb(200, 200, 200);
        public int BorderWidth { get; set; } = 1;
        public bool UseShadow { get; set; } = true;
        public int ShadowDepth { get; set; } = 5;
        public Color ShadowColor { get; set; } = Color.FromArgb(30, 0, 0, 0);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            // 繪製陰影
            if (UseShadow)
            {
                Rectangle shadowRect = new Rectangle(
                    ShadowDepth, ShadowDepth,
                    rect.Width - ShadowDepth, rect.Height - ShadowDepth);
                using (GraphicsPath shadowPath = GetRoundedRectPath(shadowRect, CornerRadius))
                {
                    using (SolidBrush shadowBrush = new SolidBrush(ShadowColor))
                    {
                        g.FillPath(shadowBrush, shadowPath);
                    }
                }
            }

            // 繪製邊框
            using (GraphicsPath path = GetRoundedRectPath(rect, CornerRadius))
            {
                using (Pen pen = new Pen(BorderColor, BorderWidth))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            return path;
        }
    }

    public class GradientPanel : Panel
    {
        public Color StartColor { get; set; } = Color.FromArgb(255, 255, 255);
        public Color EndColor { get; set; } = Color.FromArgb(240, 240, 240);
        public float Angle { get; set; } = 90f;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect, StartColor, EndColor, Angle))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }
    }
}