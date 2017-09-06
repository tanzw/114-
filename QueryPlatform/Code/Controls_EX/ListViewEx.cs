using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform.Code.Controls_EX
{
    public class ListViewEx : ListView
    {
         
        public Color _headColor = Color.Red;

        public Color _selectedColor;
        public Color _rowBackColor1;
        public Color _rowBackColor2;

        public delegate void SelectedIndexChange(int itemIndex);//声明委托
        public event SelectedIndexChange OnSelectedIndexChange = null;//定义委托事件


        private int SelectedIndex = -1;

        public void SetSelectedIndex(int index)
        {
            SelectedIndex = index;
        }

        public ListViewEx()
            : base()
        {
            base.OwnerDraw = true
;
            // 开启双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }
        

        //protected  override o
        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }

        }


        //private Image sd()
        //{
        //    ImageList imgList = new ImageList();
        //    imgList.ImageSize = new Size(1, 20);

              
        //}



        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            base.OnDrawColumnHeader(e);

            Image imgTemp = new Bitmap(1, 40, e.Graphics);
            using (StringFormat sf = new StringFormat())
            {
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Left:
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                }
            

                var g = e.Graphics;
                TextFormatFlags flags = GetFormatFlags(e.Header.TextAlign);

 

                e.DrawBackground();
                using (Font headerFont =
                            new Font("宋体", 12, FontStyle.Bold))
                {
                    TextRenderer.DrawText(
                 g,
                e.Header.Text,
                headerFont,
              e.Bounds,
              Color.Black,
                 flags | TextFormatFlags.GlyphOverhangPadding);

                    //e.Graphics.DrawString(e.Header.Text, headerFont,
                    //      Brushes.Black, e.Bounds, sf);
                }
               
            }
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (View != View.Details)
            {
                e.DrawDefault = true;
            }

        }
        string str = "";
    
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            //base.OnDrawSubItem(e);
            if (View != View.Details)
            {
                return;
            }
            if (e.ItemIndex == -1)
            {
                return;
            }
            Graphics g = e.Graphics;
            Rectangle textRect = e.SubItem.Bounds;
            if (e.ColumnIndex == 0)
            {
                textRect.Width = e.Header.Width - 10;
            }
            Color selecedForeColor = e.SubItem.ForeColor;
           
            //if ((e.ItemState & ListViewItemStates.Selected) == ListViewItemStates.Selected)
            //{
            //    SolidBrush brush = new SolidBrush(Color.FromArgb(51, 153, 255));
            //    g.FillRectangle(brush, e.Bounds);
                
            //    selecedForeColor = Color.White;
            //    string str = "X:" + e.Bounds.X + " Y:" + e.Bounds.Y + " W:" + e.Bounds.Width + " H:" + e.Bounds.Height;
            //   // MessageBox.Show(str);
            //}
            //else
            //{
            //if (e.ItemState.HasFlag(ListViewItemStates.Selected))
            //{
            //    selecedIndex = e.ItemIndex;
            //    SolidBrush brush = new SolidBrush(Color.FromArgb(51, 153, 255));
            //    g.FillRectangle(brush, e.Bounds);
            //    selecedForeColor = Color.White;

            //    str += "[" + selecedIndex + "]1,";
            //}
            //else
            //{
            //    if (e.ItemIndex != selecedIndex)
            //    {
            //        Color backColor = e.ItemIndex % 2 == 0 ?
            // Color.FromArgb(255, 217, 217) : Color.FromArgb(201, 201, 201);
            //        SolidBrush brush = new SolidBrush(backColor);
            //        g.FillRectangle(brush, e.Bounds);
            //    }
            //    else {
            //        str += "[" + selecedIndex + "]0,";
            //    }
             
            //}
            //lb.Text = str;
            //}
           
            if (SelectedIndex == e.ItemIndex)
            {
                e.SubItem.BackColor = Color.FromArgb(51, 153, 255);
                e.SubItem.ForeColor = Color.White;
                e.DrawBackground();
            }
            else {
                 Color backColor = e.ItemIndex % 2 == 0 ?
             Color.FromArgb(255, 217, 217) : Color.FromArgb(201, 201, 201);
                 e.SubItem.BackColor = backColor;
                 if (e.ColumnIndex == 2)
                 {
                     e.SubItem.ForeColor = Color.FromArgb(128, 0, 54);  
                 }
                 else if (e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6)
                 {
                     if (e.SubItem.Text.Contains("*"))
                     {
                         e.SubItem.ForeColor = Color.FromArgb(0, 255, 0);
                     }
                     else
                     {
                         e.SubItem.ForeColor = Color.FromArgb(38, 38, 248);
                     }
                 }
                 else
                 {
                     e.SubItem.ForeColor = Color.Black;
                 }
                //e.SubItem.ForeColor = Color.White;
                e.DrawBackground();
            }
            using (StringFormat sf = new StringFormat())
            {
                TextFormatFlags flags = GetFormatFlags(e.Header.TextAlign);
                 
                TextRenderer.DrawText(
                  g,
                  e.SubItem.Text,
                  e.SubItem.Font,
                  textRect,
                  selecedForeColor,
                  flags);
            }
        }


        public void RenderBackgroundInternal(Graphics g, Rectangle rect,
            Color baseColor, Color borderColor, Color innerBorderColor,
            float basePosition, bool drawBorder, LinearGradientMode mode)
        {
            if (drawBorder)
            {
                rect.Width--;
                rect.Height--;
            }
            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 68, 69, 54);

                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, rect);
            }
            if (baseColor.A > 80)
            {
                Rectangle rectTop = rect;
                if (mode == LinearGradientMode.Vertical)
                {
                    rectTop.Height = (int)(rectTop.Height * basePosition);
                }
                else
                {
                    rectTop.Width = (int)(rect.Width * basePosition);
                }
                using (SolidBrush brushAlpha =
                    new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                {
                    g.FillRectangle(brushAlpha, rectTop);
                }
            }
            if (drawBorder)
            {
                using (Pen pen = new Pen(borderColor))
                {
                    g.DrawRectangle(pen, rect);
                }

                rect.Inflate(-1, -1);
                using (Pen pen = new Pen(innerBorderColor))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        private Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;

            if (a + a0 > 255) { a = 255; } else { a = Math.Max(a + a0, 0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(r + r0, 0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(g + g0, 0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(b + b0, 0); }

            return Color.FromArgb(a, r, g, b);
        }

        private TextFormatFlags GetFormatFlags(HorizontalAlignment v)
        {
            var f = TextFormatFlags.Default;
            switch (v)
            {
                case HorizontalAlignment.Left:
                    f = TextFormatFlags.Default | TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;
                    break;
                case HorizontalAlignment.Center:
                    f = TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;
                    break;
                case HorizontalAlignment.Right:
                    f = TextFormatFlags.Right | TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;
                    break;
                default:
                    f = TextFormatFlags.Default;
                    break;
            }
            return f;
        }

    }
}
