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
 
        public static class ControlExtensions
        {
            public static void BorderRadius(this Control control, int radius)
            {
                control.Paint += (sender, e) =>
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                        path.AddArc(control.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                        path.AddArc(control.Width - radius * 2, control.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                        path.AddArc(0, control.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                        path.CloseFigure();

                        control.Region = new Region(path);
                    }
                };
            }
        }
    }
