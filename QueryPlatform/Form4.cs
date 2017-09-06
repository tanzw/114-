using QueryPlatform.Code.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            this.Name = "GlassPanelForm";
            this.Text = "GlassPanelForm";

            ClientSize = new System.Drawing.Size(0, 0);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.Manual;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;

            SetStyle(ControlStyles.Selectable, false);
            //fmMain2 fm = Application.OpenForms["fmMain2"] as fmMain2;
             
            this.Opacity = 1f;
            this.BackColor = Color.FromArgb(255, 254, 254, 254);
            this.TransparencyKey = this.BackColor;
            //this.HideGlass();
            // this.Bounds = new Rectangle(-10000, -10000, 1, 1);
          

            HideGlass();
            Win32.ShowWindow(this.Handle, 8);
        }

        public void BindControls(Control ctr, Form owner)
        {
            this.Owner = owner;
            bindControl = ctr;
        }

        private Control bindControl;


        public void HideGlass()
        {
            
            this.Bounds = new Rectangle(-10000, -10000, 1, 1);
        }

        protected void ShowGlass()
        {
            if (CurrentVisible)
            {
                HideGlass();
            }
            
            Rectangle rect = this.bindControl.ClientRectangle;
            rect.X = 0;
            rect.Y = 0;
            this.Bounds = this.bindControl.RectangleToScreen(rect);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                cp.ExStyle |= 0x80; // WS_EX_TOOLWINDOW 
                return cp;
            }
        }

        private LeadModel DisplayModel;
        private int _ParentWidth;
        private Rectangle WorkRect;
        int lX = 0;
        int lY = 0;

        private bool _CurrentVisible;

        public bool CurrentVisible
        {
            get
            {
                if (this.Bounds.X < 0 || this.Bounds.Y < 0)
                { _CurrentVisible = false; }
                else { _CurrentVisible = true; }
                return _CurrentVisible;
            }

        }

        public void ShowModel(LeadModel model, int x, int y)
        {


            lX = x;
            lY = y;
            //model.OfficeAddress = "阿斯蒂芬三等奖分开了就是打开房间失控的风景独守空房就算到了疯狂就算到了看福建省电力防水堵漏分散到了看分就是打开浪费加速度发生的发生的发生的";
            //model.DormitoryAddress = "阿斯蒂芬三等奖分开了就是打开房间失控的风景独守空房就算到了疯狂就算到了看福建省电力防水堵漏分散到了看分就是打开浪费加速度发生的发生的发生的";
            //model.Secretary = "阿斯蒂芬三等奖分开了就是打开房间失控的风景独守空房就算到了疯狂就算到了看福建省电力防水堵漏分散到了看分就是打开浪费加速度发生的发生的发生的";

            if (DisplayModel == null || DisplayModel.ID == model.ID)
            {
                DisplayModel = model;
                if (CurrentVisible)
                {
                    HideGlass();
                }
                else
                {
                    ShowGlass();
                }
            }
            else
            {
                DisplayModel = model;
                ShowGlass();
            }

        }



        Brush ContentTextBrush = Brushes.Black;

 

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 132;
            const int HTTRANSPARENT = -1;
            switch (m.Msg)
            {
                // Ignore all mouse interactions
                case WM_NCHITTEST:
                    m.Result = (IntPtr)HTTRANSPARENT;
                    break;
            }
            base.WndProc(ref m);
        }

        private GraphicsPath GetRoundedRect(RectangleF rect, float diameter)
        {
            GraphicsPath path = new GraphicsPath();

            RectangleF arc = new RectangleF(rect.X, rect.Y, diameter, diameter);
            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            //g.TxtRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // g.SmoothingMode = SmoothingMode.AntiAlias;


            Font font = Code.Common.Config.ToolTipFont;

            if (DisplayModel == null)
            {
                return;
            }

            StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap);
            fmt.Trimming = StringTrimming.None;
            fmt.Alignment = StringAlignment.Near;
            fmt.LineAlignment = StringAlignment.Near;

            string str1 = DisplayModel.DepartmentName + "    " + DisplayModel.AName + "    " + DisplayModel.Duty;
            string str2 = DisplayModel.OfficeAddress + "    " + DisplayModel.OfficeRedPhone + "    " + DisplayModel.Phone;
            string str3 = DisplayModel.DormitoryAddress + "    " + DisplayModel.DormitoryRedPhone + "    " + DisplayModel.DormitoryPhone;
            string str4 = DisplayModel.Secretary + "    " + DisplayModel.SecretaryRedPhone + "    " + DisplayModel.SecretaryDormitoryRedPhone + "    " + DisplayModel.SecretaryAnyCall;
            SizeF size1 = g.MeasureString(str1, font, this.Width - 200, fmt);
            SizeF size2 = g.MeasureString(str2, font, this.Width - 200, fmt);
            SizeF size3 = g.MeasureString(str3, font, this.Width - 200, fmt);
            SizeF size4 = g.MeasureString(str4, font, this.Width - 200, fmt);

            List<float> l = new List<float>();
            l.Add(size1.Width);
            l.Add(size2.Width);
            l.Add(size3.Width);
            l.Add(size4.Width);

            const int spacing = 8;
            float Maxwidth = l.OrderByDescending(x => x).Take(1).ToList()[0];
            int height = Convert.ToInt32(size1.Height + size2.Height + size3.Height + size4.Height) + 4 * spacing;

            Size cardSize = new Size(Convert.ToInt32(Maxwidth) + 20, height + 20);
            var s11 = ClientSize;
            var s22 = ClientRectangle;
            //this.Width = cardSize.Width;
            //  this.Height = cardSize.Height;


            // this.Location = new Point(WorkRect.Right - this.Width, WorkRect.Bottom - this.Height);
            int tx = lX + 10;
            int ty = lY + 10;
            var xx = (tx + cardSize.Width + 4) > this.Width ? (this.Width - cardSize.Width - 2) : tx;

            var yy = (ty + cardSize.Height + 4) > this.Height ? (this.Height - cardSize.Height - 2) : ty;
            // this.Location = new Point(xx, yy);
            Rectangle itemBounds = new Rectangle(
              xx, yy, cardSize.Width, cardSize.Height);



            const int rounding = 20;
            GraphicsPath path = this.GetRoundedRect(itemBounds, rounding);
            HatchBrush ds = new HatchBrush(HatchStyle.Percent05, Color.FromArgb(205, 219, 233));
          //  PathGradientBrush  ds = new PathGradientBrush(
             
            //Brushes.BlueViolet
            SolidBrush redBrush = new SolidBrush(Color.FromArgb(205, 219, 233));
            g.FillPath(redBrush, path);
            g.DrawPath(new Pen(Color.FromArgb(100, 100, 100), 1), path);
            g.Clip = new Region(itemBounds);


            #region 第一行
            RectangleF textBoxRect1 = itemBounds;

            textBoxRect1.X = itemBounds.X;
            textBoxRect1.Y = itemBounds.Y + spacing;
            textBoxRect1.Width = itemBounds.Right - textBoxRect1.X - spacing;

            RectangleF r1 = textBoxRect1;

            //path = this.GetRoundedRect(r3, 15);
            //g.FillPath(this.HeaderBackBrush, path);
            g.DrawString(str1, font, this.ContentTextBrush, textBoxRect1, fmt);

            textBoxRect1.Y += size1.Height + spacing;
            #endregion

            #region 第二行
            RectangleF textBoxRect2 = itemBounds;

            textBoxRect2.X = itemBounds.X;
            textBoxRect2.Y = textBoxRect1.Y;
            textBoxRect2.Width = itemBounds.Right - textBoxRect2.X - spacing;

            RectangleF r2 = textBoxRect2;

            //path = this.GetRoundedRect(r3, 15);
            //g.FillPath(this.HeaderBackBrush, path);
            g.DrawString(str2, font, this.ContentTextBrush, textBoxRect2, fmt);
            textBoxRect2.Y += size2.Height + spacing;

            #endregion

            #region 第三行
            RectangleF textBoxRect3 = itemBounds;
            textBoxRect3.X = itemBounds.X;
            textBoxRect3.Y = textBoxRect2.Y;
            textBoxRect3.Width = itemBounds.Right - textBoxRect3.X - spacing;

            RectangleF r3 = textBoxRect3;

            //path = this.GetRoundedRect(r3, 15);
            //g.FillPath(this.HeaderBackBrush, path);
            g.DrawString(str3, font, this.ContentTextBrush, textBoxRect3, fmt);
            textBoxRect3.Y += size3.Height + spacing;

            #endregion

            #region 第四行
            RectangleF textBoxRect4 = itemBounds;
            textBoxRect4.X = itemBounds.X;
            textBoxRect4.Y = textBoxRect3.Y;
            textBoxRect4.Width = itemBounds.Right - textBoxRect4.X - spacing;

            RectangleF r4 = textBoxRect4;

            //path = this.GetRoundedRect(r3, 15);
            //g.FillPath(this.HeaderBackBrush, path);
            g.DrawString(str4, font, this.ContentTextBrush, textBoxRect4, fmt);
            textBoxRect4.Y += size4.Height + spacing;

            #endregion
        }
    }

    public class Win32
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
