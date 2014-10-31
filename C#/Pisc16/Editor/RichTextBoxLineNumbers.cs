using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Pisc16
{
    [DefaultProperty("ParentRichTextBox")]
    public class RichTextBoxLineNumbers : Control
    {
        // Fields
        private static List<WeakReference> __ENCList = new List<WeakReference>();
        [AccessedThroughProperty("zParent")]
        private RichTextBox _zParent;
        [AccessedThroughProperty("zTimer")]
        private Timer _zTimer;
        private bool zAutoSizing;
        private Size zAutoSizing_Size;
        private Color zBorderLines_Color;
        private bool zBorderLines_Show;
        private DashStyle zBorderLines_Style;
        private float zBorderLines_Thickness;
        private Rectangle zContentRectangle;
        private LineNumberDockSide zDockSide;
        private LinearGradientMode zGradient_Direction;
        private Color zGradient_EndColor;
        private bool zGradient_Show;
        private Color zGradient_StartColor;
        private Color zGridLines_Color;
        private bool zGridLines_Show;
        private DashStyle zGridLines_Style;
        private float zGridLines_Thickness;
        private ContentAlignment zLineNumbers_Alignment;
        private bool zLineNumbers_AntiAlias;
        private bool zLineNumbers_ClipByItemRectangle;
        private string zLineNumbers_Format;
        private Size zLineNumbers_Offset;
        private bool zLineNumbers_Show;
        private bool zLineNumbers_ShowAsHexadecimal;
        private bool zLineNumbers_ShowLeadingZeroes;
        private List<LineNumberItem> zLNIs;
        private Color zMarginLines_Color;
        private bool zMarginLines_Show;
        private LineNumberDockSide zMarginLines_Side;
        private DashStyle zMarginLines_Style;
        private float zMarginLines_Thickness;
        private int zParentInMe;
        private bool zParentIsScrolling;
        private Point zPointInMe;
        private Point zPointInParent;
        private bool zSeeThroughMode;

        // Methods
        public RichTextBoxLineNumbers()
        {
            List<WeakReference> list = __ENCList;
            lock (list)
            {
                __ENCList.Add(new WeakReference(this));
            }
            this.zParent = null;
            this.zTimer = new Timer();
            this.zAutoSizing = true;
            this.zAutoSizing_Size = new Size(0, 0);
            this.zContentRectangle = new Rectangle();
            this.zDockSide = LineNumberDockSide.Left;
            this.zParentIsScrolling = false;
            this.zSeeThroughMode = false;
            this.zGradient_Show = true;
            this.zGradient_Direction = LinearGradientMode.Horizontal;
            this.zGradient_StartColor = Color.FromArgb(0, 0, 0, 0);
            this.zGradient_EndColor = Color.LightSteelBlue;
            this.zGridLines_Show = true;
            this.zGridLines_Thickness = 1f;
            this.zGridLines_Style = DashStyle.Dot;
            this.zGridLines_Color = Color.SlateGray;
            this.zBorderLines_Show = true;
            this.zBorderLines_Thickness = 1f;
            this.zBorderLines_Style = DashStyle.Dot;
            this.zBorderLines_Color = Color.SlateGray;
            this.zMarginLines_Show = true;
            this.zMarginLines_Side = LineNumberDockSide.Right;
            this.zMarginLines_Thickness = 1f;
            this.zMarginLines_Style = DashStyle.Solid;
            this.zMarginLines_Color = Color.SlateGray;
            this.zLineNumbers_Show = true;
            this.zLineNumbers_ShowLeadingZeroes = true;
            this.zLineNumbers_ShowAsHexadecimal = false;
            this.zLineNumbers_ClipByItemRectangle = true;
            this.zLineNumbers_Offset = new Size(0, 0);
            this.zLineNumbers_Format = "0";
            this.zLineNumbers_Alignment = ContentAlignment.TopRight;
            this.zLineNumbers_AntiAlias = true;
            this.zLNIs = new List<LineNumberItem>();
            this.zPointInParent = new Point(0, 0);
            this.zPointInMe = new Point(0, 0);
            this.zParentInMe = 0;
            RichTextBoxLineNumbers box = this;
            box.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            box.SetStyle(ControlStyles.ResizeRedraw, true);
            box.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            box.SetStyle(ControlStyles.UserPaint, true);
            box.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            Padding padding = new Padding(0);
            box.Margin = padding;
            padding = new Padding(0, 0, 2, 0);
            box.Padding = padding;
            box = null;
            Timer zTimer = this.zTimer;
            zTimer.Enabled = true;
            zTimer.Interval = 200;
            zTimer.Stop();
            zTimer = null;
            this.Update_SizeAndPosition();
            this.Invalidate();
        }

        private void FindStartIndex(ref int zMin, ref int zMax, ref int zTarget)
        {
            if (!((zMax == (zMin + 1)) | (zMin == ((zMax + zMin) / 2))))
            {
                int y = this.zParent.GetPositionFromCharIndex((zMax + zMin) / 2).Y;
                if (y == zTarget)
                {
                    zMin = (zMax + zMin) / 2;
                }
                else if (y > zTarget)
                {
                    zMax = (zMax + zMin) / 2;
                    this.FindStartIndex(ref zMin, ref zMax, ref zTarget);
                }
                else if (y < 0)
                {
                    zMin = (zMax + zMin) / 2;
                    this.FindStartIndex(ref zMin, ref zMax, ref zTarget);
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AutoSize = false;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            if (this.DesignMode)
            {
                this.Refresh();
            }
            base.OnLocationChanged(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SizeF ef;
            Point point4;
            Point point5;
            Rectangle rectangle3;
            this.Update_VisibleLineNumberItems();
            base.OnPaint(e);
            if (this.zLineNumbers_AntiAlias)
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            }
            string text = "";
            string str = "";
            StringFormat stringFormat = new StringFormat();
            Pen pen = new Pen(this.ForeColor);
            SolidBrush brush = new SolidBrush(this.ForeColor);
            Point location = new Point(0, 0);
            Rectangle rect = new Rectangle(0, 0, 0, 0);
            GraphicsPath path2 = new GraphicsPath(FillMode.Winding);
            GraphicsPath path = new GraphicsPath(FillMode.Winding);
            GraphicsPath path4 = new GraphicsPath(FillMode.Winding);
            GraphicsPath path3 = new GraphicsPath(FillMode.Winding);
            Region region = new Region(base.ClientRectangle);
            if (this.DesignMode)
            {
                if (this.zParent == null)
                {
                    str = "-!- Set ParentRichTextBox -!-";
                }
                else if (this.zLNIs.Count == 0)
                {
                    str = "LineNrs (  " + this.zParent.Name + "  )";
                }
                if (str.Length > 0)
                {
                    e.Graphics.TranslateTransform((float)(((double)this.Width) / 2.0), (float)(((double)this.Height) / 2.0));
                    e.Graphics.RotateTransform(-90f);
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    ef = e.Graphics.MeasureString(str, this.Font, (PointF)location, stringFormat);
                    e.Graphics.DrawString(str, this.Font, Brushes.WhiteSmoke, 1f, 1f, stringFormat);
                    e.Graphics.DrawString(str, this.Font, Brushes.Firebrick, 0f, 0f, stringFormat);
                    e.Graphics.ResetTransform();
                    Rectangle rectangle2 = new Rectangle((int)Math.Round((double)((((double)this.Width) / 2.0) - (ef.Height / 2f))), (int)Math.Round((double)((((double)this.Height) / 2.0) - (ef.Width / 2f))), (int)Math.Round((double)ef.Height), (int)Math.Round((double)ef.Width));
                    path3.AddRectangle(rectangle2);
                    path3.CloseFigure();
                    if (this.zAutoSizing)
                    {
                        rectangle2.Inflate((int)Math.Round((double)(ef.Height * 0.2)), (int)Math.Round((double)(ef.Width * 0.1)));
                        this.zAutoSizing_Size = new Size(rectangle2.Width, rectangle2.Height);
                    }
                }
            }
            if (this.zLNIs.Count > 0)
            {
                LinearGradientBrush brush2 = null;
                pen = new Pen(this.zGridLines_Color, this.zGridLines_Thickness);
                pen.DashStyle = this.zGridLines_Style;
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
                stringFormat.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;
                int num2 = this.zLNIs.Count - 1;
                for (int i = 0; i <= num2; i++)
                {
                    if (this.zGradient_Show)
                    {
                        brush2 = new LinearGradientBrush(this.zLNIs[i].Rectangle, this.zGradient_StartColor, this.zGradient_EndColor, this.zGradient_Direction);
                        e.Graphics.FillRectangle(brush2, this.zLNIs[i].Rectangle);
                    }
                    if (this.zGridLines_Show)
                    {
                        point4 = new Point(0, this.zLNIs[i].Rectangle.Y);
                        point5 = new Point(this.Width, this.zLNIs[i].Rectangle.Y);
                        e.Graphics.DrawLine(pen, point4, point5);
                        rectangle3 = new Rectangle((int)Math.Round((double)-this.zGridLines_Thickness), this.zLNIs[i].Rectangle.Y, (int)Math.Round((double)(this.Width + (this.zGridLines_Thickness * 2f))), (int)Math.Round((double)((this.Height - this.zLNIs[0].Rectangle.Y) + this.zGridLines_Thickness)));
                        path2.AddRectangle(rectangle3);
                        path2.CloseFigure();
                    }
                    if (this.zLineNumbers_Show)
                    {
                        if (this.zLineNumbers_ShowLeadingZeroes)
                        {
                            text = this.zLineNumbers_ShowAsHexadecimal ? this.zLNIs[i].LineNumber.ToString("X") : this.zLNIs[i].LineNumber.ToString(this.zLineNumbers_Format);
                        }
                        else
                        {
                            text = this.zLineNumbers_ShowAsHexadecimal ? this.zLNIs[i].LineNumber.ToString("X") : this.zLNIs[i].LineNumber.ToString();
                        }
                        ef = e.Graphics.MeasureString(text, this.Font, (PointF)location, stringFormat);
                        switch (this.zLineNumbers_Alignment)
                        {
                            case ContentAlignment.TopLeft:
                                location = new Point((this.zLNIs[i].Rectangle.Left + this.Padding.Left) + this.zLineNumbers_Offset.Width, (this.zLNIs[i].Rectangle.Top + this.Padding.Top) + this.zLineNumbers_Offset.Height);
                                break;

                            case ContentAlignment.MiddleLeft:
                                location = new Point((this.zLNIs[i].Rectangle.Left + this.Padding.Left) + this.zLineNumbers_Offset.Width, (int)Math.Round((double)(((this.zLNIs[i].Rectangle.Top + (((double)this.zLNIs[i].Rectangle.Height) / 2.0)) + this.zLineNumbers_Offset.Height) - (ef.Height / 2f))));
                                break;

                            case ContentAlignment.BottomLeft:
                                location = new Point((this.zLNIs[i].Rectangle.Left + this.Padding.Left) + this.zLineNumbers_Offset.Width, (int)Math.Round((double)((((this.zLNIs[i].Rectangle.Bottom - this.Padding.Bottom) + 1) + this.zLineNumbers_Offset.Height) - ef.Height)));
                                break;

                            case ContentAlignment.TopCenter:
                                location = new Point((int)Math.Round((double)(((((double)this.zLNIs[i].Rectangle.Width) / 2.0) + this.zLineNumbers_Offset.Width) - (ef.Width / 2f))), (this.zLNIs[i].Rectangle.Top + this.Padding.Top) + this.zLineNumbers_Offset.Height);
                                break;

                            case ContentAlignment.MiddleCenter:
                                location = new Point((int)Math.Round((double)(((((double)this.zLNIs[i].Rectangle.Width) / 2.0) + this.zLineNumbers_Offset.Width) - (ef.Width / 2f))), (int)Math.Round((double)(((this.zLNIs[i].Rectangle.Top + (((double)this.zLNIs[i].Rectangle.Height) / 2.0)) + this.zLineNumbers_Offset.Height) - (ef.Height / 2f))));
                                break;

                            case ContentAlignment.BottomCenter:
                                location = new Point((int)Math.Round((double)(((((double)this.zLNIs[i].Rectangle.Width) / 2.0) + this.zLineNumbers_Offset.Width) - (ef.Width / 2f))), (int)Math.Round((double)((((this.zLNIs[i].Rectangle.Bottom - this.Padding.Bottom) + 1) + this.zLineNumbers_Offset.Height) - ef.Height)));
                                break;

                            case ContentAlignment.TopRight:
                                location = new Point((int)Math.Round((double)(((this.zLNIs[i].Rectangle.Right - this.Padding.Right) + this.zLineNumbers_Offset.Width) - ef.Width)), (this.zLNIs[i].Rectangle.Top + this.Padding.Top) + this.zLineNumbers_Offset.Height);
                                break;

                            case ContentAlignment.MiddleRight:
                                location = new Point((int)Math.Round((double)(((this.zLNIs[i].Rectangle.Right - this.Padding.Right) + this.zLineNumbers_Offset.Width) - ef.Width)), (int)Math.Round((double)(((this.zLNIs[i].Rectangle.Top + (((double)this.zLNIs[i].Rectangle.Height) / 2.0)) + this.zLineNumbers_Offset.Height) - (ef.Height / 2f))));
                                break;

                            case ContentAlignment.BottomRight:
                                location = new Point((int)Math.Round((double)(((this.zLNIs[i].Rectangle.Right - this.Padding.Right) + this.zLineNumbers_Offset.Width) - ef.Width)), (int)Math.Round((double)((((this.zLNIs[i].Rectangle.Bottom - this.Padding.Bottom) + 1) + this.zLineNumbers_Offset.Height) - ef.Height)));
                                break;
                        }
                        rect = new Rectangle(location, ef.ToSize());
                        if (this.zLineNumbers_ClipByItemRectangle)
                        {
                            rect.Intersect(this.zLNIs[i].Rectangle);
                            e.Graphics.SetClip(rect);
                        }
                        e.Graphics.DrawString(text, this.Font, brush, (PointF)location, stringFormat);
                        e.Graphics.ResetClip();
                        path3.AddRectangle(rect);
                        path3.CloseFigure();
                    }
                }
                if (this.zGridLines_Show)
                {
                    pen.DashStyle = DashStyle.Solid;
                    path2.Widen(pen);
                }
                if (brush2 != null)
                {
                    brush2.Dispose();
                }
            }
            Point point = new Point((int)Math.Round(Math.Floor((double)(this.zBorderLines_Thickness / 2f))), (int)Math.Round(Math.Floor((double)(this.zBorderLines_Thickness / 2f))));
            Point point2 = new Point((int)Math.Round((double)(this.Width - Math.Ceiling((double)(this.zBorderLines_Thickness / 2f)))), (int)Math.Round((double)(this.Height - Math.Ceiling((double)(this.zBorderLines_Thickness / 2f)))));
            Point[] pointArray2 = new Point[5];
            point5 = new Point(point.X, point.Y);
            pointArray2[0] = point5;
            point4 = new Point(point2.X, point.Y);
            pointArray2[1] = point4;
            Point point6 = new Point(point2.X, point2.Y);
            pointArray2[2] = point6;
            Point point7 = new Point(point.X, point2.Y);
            pointArray2[3] = point7;
            Point point8 = new Point(point.X, point.Y);
            pointArray2[4] = point8;
            Point[] points = pointArray2;
            if (this.zBorderLines_Show)
            {
                pen = new Pen(this.zBorderLines_Color, this.zBorderLines_Thickness);
                pen.DashStyle = this.zBorderLines_Style;
                e.Graphics.DrawLines(pen, points);
                path.AddLines(points);
                path.CloseFigure();
                pen.DashStyle = DashStyle.Solid;
                path.Widen(pen);
            }
            if (((this.zMarginLines_Show && (this.zMarginLines_Side > LineNumberDockSide.None)) ? 1 : 0) != 0)
            {
                point = new Point((int)Math.Round((double)-this.zMarginLines_Thickness), (int)Math.Round((double)-this.zMarginLines_Thickness));
                point2 = new Point((int)Math.Round((double)(this.Width + this.zMarginLines_Thickness)), (int)Math.Round((double)(this.Height + this.zMarginLines_Thickness)));
                pen = new Pen(this.zMarginLines_Color, this.zMarginLines_Thickness);
                pen.DashStyle = this.zMarginLines_Style;
                if ((this.zMarginLines_Side == LineNumberDockSide.Left) | (this.zMarginLines_Side == LineNumberDockSide.Height))
                {
                    point8 = new Point((int)Math.Round(Math.Floor((double)(this.zMarginLines_Thickness / 2f))), 0);
                    point7 = new Point((int)Math.Round(Math.Floor((double)(this.zMarginLines_Thickness / 2f))), this.Height - 1);
                    e.Graphics.DrawLine(pen, point8, point7);
                    point = new Point((int)Math.Round(Math.Ceiling((double)(this.zMarginLines_Thickness / 2f))), (int)Math.Round((double)-this.zMarginLines_Thickness));
                }
                if ((this.zMarginLines_Side == LineNumberDockSide.Right) | (this.zMarginLines_Side == LineNumberDockSide.Height))
                {
                    point8 = new Point((int)Math.Round((double)(this.Width - Math.Ceiling((double)(this.zMarginLines_Thickness / 2f)))), 0);
                    point7 = new Point((int)Math.Round((double)(this.Width - Math.Ceiling((double)(this.zMarginLines_Thickness / 2f)))), this.Height - 1);
                    e.Graphics.DrawLine(pen, point8, point7);
                    point2 = new Point((int)Math.Round((double)(this.Width - Math.Ceiling((double)(this.zMarginLines_Thickness / 2f)))), (int)Math.Round((double)(this.Height + this.zMarginLines_Thickness)));
                }
                Size size = new Size(point2.X - point.X, point2.Y - point.Y);
                rectangle3 = new Rectangle(point, size);
                path4.AddRectangle(rectangle3);
                pen.DashStyle = DashStyle.Solid;
                path4.Widen(pen);
            }
            if (this.zSeeThroughMode)
            {
                region.MakeEmpty();
                region.Union(path);
                region.Union(path4);
                region.Union(path2);
                region.Union(path3);
            }
            if (region.GetBounds(e.Graphics).IsEmpty)
            {
                path.AddLines(points);
                path.CloseFigure();
                pen = new Pen(this.zBorderLines_Color, 1f);
                pen.DashStyle = DashStyle.Solid;
                path.Widen(pen);
                region = new Region(path);
            }
            this.Region = region;
            if (pen != null)
            {
                pen.Dispose();
            }
            if (brush != null)
            {
                pen.Dispose();
            }
            if (region != null)
            {
                region.Dispose();
            }
            if (path2 != null)
            {
                path2.Dispose();
            }
            if (path != null)
            {
                path.Dispose();
            }
            if (path4 != null)
            {
                path4.Dispose();
            }
            if (path3 != null)
            {
                path3.Dispose();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.DesignMode)
            {
                this.Refresh();
            }
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        public override void Refresh()
        {
            base.Refresh();
            this.Update_SizeAndPosition();
        }

        private void Update_SizeAndPosition()
        {
            if (!this.AutoSize && !(((this.Dock == DockStyle.Bottom) | (this.Dock == DockStyle.Fill)) | (this.Dock == DockStyle.Top)))
            {
                Point location = this.Location;
                Size size = this.Size;
                if (this.zAutoSizing)
                {
                    bool flag = true;
                    if (flag == (this.zParent == null))
                    {
                        if (this.zAutoSizing_Size.Width > 0)
                        {
                            size.Width = this.zAutoSizing_Size.Width;
                        }
                        if (this.zAutoSizing_Size.Height > 0)
                        {
                            size.Height = this.zAutoSizing_Size.Height;
                        }
                        this.Size = size;
                    }
                    else if (flag == ((this.Dock == DockStyle.Left) | (this.Dock == DockStyle.Right)))
                    {
                        if (this.zAutoSizing_Size.Width > 0)
                        {
                            size.Width = this.zAutoSizing_Size.Width;
                        }
                        this.Width = size.Width;
                    }
                    else if (flag == (this.zDockSide != LineNumberDockSide.None))
                    {
                        if (this.zAutoSizing_Size.Width > 0)
                        {
                            size.Width = this.zAutoSizing_Size.Width;
                        }
                        size.Height = this.zParent.Height;
                        if (this.zDockSide == LineNumberDockSide.Left)
                        {
                            location.X = (this.zParent.Left - size.Width) - 1;
                        }
                        if (this.zDockSide == LineNumberDockSide.Right)
                        {
                            location.X = this.zParent.Right + 1;
                        }
                        location.Y = this.zParent.Top;
                        this.Location = location;
                        this.Size = size;
                    }
                    else if (flag == (this.zDockSide == LineNumberDockSide.None))
                    {
                        if (this.zAutoSizing_Size.Width > 0)
                        {
                            size.Width = this.zAutoSizing_Size.Width;
                        }
                        this.Size = size;
                    }
                }
                else
                {
                    bool flag2 = true;
                    if (flag2 == (this.zParent == null))
                    {
                        if (this.zAutoSizing_Size.Width > 0)
                        {
                            size.Width = this.zAutoSizing_Size.Width;
                        }
                        if (this.zAutoSizing_Size.Height > 0)
                        {
                            size.Height = this.zAutoSizing_Size.Height;
                        }
                        this.Size = size;
                    }
                    else if (flag2 == (this.zDockSide != LineNumberDockSide.None))
                    {
                        size.Height = this.zParent.Height;
                        if (this.zDockSide == LineNumberDockSide.Left)
                        {
                            location.X = (this.zParent.Left - size.Width) - 1;
                        }
                        if (this.zDockSide == LineNumberDockSide.Right)
                        {
                            location.X = this.zParent.Right + 1;
                        }
                        location.Y = this.zParent.Top;
                        this.Location = location;
                        this.Size = size;
                    }
                }
            }
        }

        private void Update_VisibleLineNumberItems()
        {
            this.zLNIs.Clear();
            this.zAutoSizing_Size = new Size(0, 0);
            this.zLineNumbers_Format = "0";
            if (this.zAutoSizing)
            {
                this.zAutoSizing_Size = new Size(TextRenderer.MeasureText(this.zLineNumbers_Format.Replace(new string("0".ToCharArray()), new string("W".ToCharArray())), this.Font).Width, 0);
            }
            if ((((this.zParent == null) || (this.zParent.Text == "")) ? 1 : 0) == 0)
            {
                Rectangle clientRectangle = this.zParent.ClientRectangle;
                this.zPointInParent = this.zParent.PointToScreen(clientRectangle.Location);
                Point p = new Point(0, 0);
                this.zPointInMe = this.PointToScreen(p);
                this.zParentInMe = (this.zPointInParent.Y - this.zPointInMe.Y) + 1;
                this.zPointInParent = this.zParent.PointToClient(this.zPointInMe);
                string[] strArray = this.zParent.Text.Split("\r\n".ToCharArray());
                if (strArray.Length < 2)
                {
                    Point positionFromCharIndex = this.zParent.GetPositionFromCharIndex(0);
                    p = new Point(0, (positionFromCharIndex.Y - 1) + this.zParentInMe);
                    Size size = new Size(this.Width, this.zContentRectangle.Height - positionFromCharIndex.Y);
                    clientRectangle = new Rectangle(p, size);
                    this.zLNIs.Add(new LineNumberItem(1, clientRectangle));
                }
                else
                {
                    TimeSpan span = new TimeSpan(DateTime.Now.Ticks);
                    Point point2 = new Point(0, 0);
                    int zMin = 0;
                    int zMax = this.zParent.Text.Length - 1;
                    int y = this.zPointInParent.Y;
                    this.FindStartIndex(ref zMin, ref zMax, ref y);
                    this.zPointInParent.Y = y;
                    zMin = Math.Max(0, Math.Min((int)(this.zParent.Text.Length - 1), (int)(this.zParent.Text.Substring(0, zMin).LastIndexOf('\n') + 1)));
                    int num5 = strArray.Length - 1;
                    zMax = Math.Max(0, this.zParent.Text.Substring(0, zMin).Split("\r\n".ToCharArray()).Length - 1);
                    while (zMax <= num5)
                    {
                        point2 = this.zParent.GetPositionFromCharIndex(zMin);
                        zMin += Math.Max(1, strArray[zMax].Length + 1);
                        if ((point2.Y + this.zParentInMe) > this.Height)
                        {
                            break;
                        }
                        clientRectangle = new Rectangle(0, (point2.Y - 1) + this.zParentInMe, this.Width, 1);
                        this.zLNIs.Add(new LineNumberItem(zMax + 1, clientRectangle));
                        if (((this.zParentIsScrolling && (DateTime.Now.Ticks > (span.Ticks + 0x7a120L))) ? 1 : 0) != 0)
                        {
                            if (this.zLNIs.Count == 1)
                            {
                                this.zLNIs[0].Rectangle.Y = 0;
                            }
                            this.zParentIsScrolling = false;
                            this.zTimer.Start();
                            break;
                        }
                        zMax++;
                    }
                    if (this.zLNIs.Count == 0)
                    {
                        return;
                    }
                    if (zMax < strArray.Length)
                    {
                        point2 = this.zParent.GetPositionFromCharIndex(Math.Min(zMin, this.zParent.Text.Length - 1));
                        clientRectangle = new Rectangle(0, (point2.Y - 1) + this.zParentInMe, 0, 0);
                        this.zLNIs.Add(new LineNumberItem(-1, clientRectangle));
                    }
                    else
                    {
                        clientRectangle = new Rectangle(0, this.zContentRectangle.Bottom, 0, 0);
                        this.zLNIs.Add(new LineNumberItem(-1, clientRectangle));
                    }
                    int num6 = this.zLNIs.Count - 2;
                    for (zMax = 0; zMax <= num6; zMax++)
                    {
                        this.zLNIs[zMax].Rectangle.Height = Math.Max(1, this.zLNIs[zMax + 1].Rectangle.Y - this.zLNIs[zMax].Rectangle.Y);
                    }
                    this.zLNIs.RemoveAt(this.zLNIs.Count - 1);
                    if (this.zLineNumbers_ShowAsHexadecimal)
                    {
                        y = strArray.Length;
                        this.zLineNumbers_Format = "".PadRight(y.ToString("X").Length, '0');
                    }
                    else
                    {
                        y = strArray.Length;
                        this.zLineNumbers_Format = "".PadRight(y.ToString().Length, '0');
                    }
                }
                if (this.zAutoSizing)
                {
                    this.zAutoSizing_Size = new Size(TextRenderer.MeasureText(this.zLineNumbers_Format.Replace(new string("0".ToCharArray()), new string("W".ToCharArray())), this.Font).Width, 0);
                }
            }
        }

        private void zParent_Changed(object sender, EventArgs e)
        {
            this.Refresh();
            this.Invalidate();
        }

        private void zParent_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            this.zContentRectangle = e.NewRectangle;
            this.Refresh();
            this.Invalidate();
        }

        private void zParent_Disposed(object sender, EventArgs e)
        {
            this.ParentRichTextBox = null;
            this.Refresh();
            this.Invalidate();
        }

        private void zParent_Scroll(object sender, EventArgs e)
        {
            this.zParentIsScrolling = true;
            this.Invalidate();
        }

        private void zTimer_Tick(object sender, EventArgs e)
        {
            this.zParentIsScrolling = false;
            this.zTimer.Stop();
            this.Invalidate();
        }

        // Properties
        [Category("Additional Behavior"), Description("Use this property to enable the control to act as an overlay ontop of the RichTextBox.")]
        public bool _SeeThroughMode_
        {
            get
            {
                return this.zSeeThroughMode;
            }
            set
            {
                this.zSeeThroughMode = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                this.Invalidate();
            }
        }

        [Description("Use this property to automatically resize the control (and reposition it if needed)."), Category("Additional Behavior")]
        public bool AutoSizing
        {
            get
            {
                return this.zAutoSizing;
            }
            set
            {
                this.zAutoSizing = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public Color BackgroundGradient_AlphaColor
        {
            get
            {
                return this.zGradient_StartColor;
            }
            set
            {
                this.zGradient_StartColor = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public Color BackgroundGradient_BetaColor
        {
            get
            {
                return this.zGradient_EndColor;
            }
            set
            {
                this.zGradient_EndColor = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public LinearGradientMode BackgroundGradient_Direction
        {
            get
            {
                return this.zGradient_Direction;
            }
            set
            {
                this.zGradient_Direction = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public Color BorderLines_Color
        {
            get
            {
                return this.zBorderLines_Color;
            }
            set
            {
                this.zBorderLines_Color = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public DashStyle BorderLines_Style
        {
            get
            {
                return this.zBorderLines_Style;
            }
            set
            {
                if (value == DashStyle.Custom)
                {
                    value = DashStyle.Solid;
                }
                this.zBorderLines_Style = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public float BorderLines_Thickness
        {
            get
            {
                return this.zBorderLines_Thickness;
            }
            set
            {
                this.zBorderLines_Thickness = Math.Max(1f, Math.Min(255f, value));
                this.Invalidate();
            }
        }

        [Description("Use this property to dock the LineNumbers to a chosen side of the chosen RichTextBox."), Category("Additional Behavior")]
        public LineNumberDockSide DockSide
        {
            get
            {
                return this.zDockSide;
            }
            set
            {
                this.zDockSide = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public Color GridLines_Color
        {
            get
            {
                return this.zGridLines_Color;
            }
            set
            {
                this.zGridLines_Color = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public DashStyle GridLines_Style
        {
            get
            {
                return this.zGridLines_Style;
            }
            set
            {
                if (value == DashStyle.Custom)
                {
                    value = DashStyle.Solid;
                }
                this.zGridLines_Style = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public float GridLines_Thickness
        {
            get
            {
                return this.zGridLines_Thickness;
            }
            set
            {
                this.zGridLines_Thickness = Math.Max(1f, Math.Min(255f, value));
                this.Invalidate();
            }
        }

        [Category("Additional Behavior"), Description("Use this to align the LineNumbers to a chosen corner (or center) within their item-area.")]
        public ContentAlignment LineNrs_Alignment
        {
            get
            {
                return this.zLineNumbers_Alignment;
            }
            set
            {
                this.zLineNumbers_Alignment = value;
                this.Invalidate();
            }
        }

        [Description("Use this to apply Anti-Aliasing to the LineNumbers (high quality). Some fonts will look better without it, though."), Category("Additional Behavior")]
        public bool LineNrs_AntiAlias
        {
            get
            {
                return this.zLineNumbers_AntiAlias;
            }
            set
            {
                this.zLineNumbers_AntiAlias = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [Category("Additional Behavior"), Description("Use this to set whether the LineNumbers should be shown as hexadecimal values.")]
        public bool LineNrs_AsHexadecimal
        {
            get
            {
                return this.zLineNumbers_ShowAsHexadecimal;
            }
            set
            {
                this.zLineNumbers_ShowAsHexadecimal = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [Category("Additional Behavior"), Description("Use this to set whether the LineNumbers are allowed to spill out of their item-area, or should be clipped by it.")]
        public bool LineNrs_ClippedByItemRectangle
        {
            get
            {
                return this.zLineNumbers_ClipByItemRectangle;
            }
            set
            {
                this.zLineNumbers_ClipByItemRectangle = value;
                this.Invalidate();
            }
        }

        [Description("Use this to set whether the LineNumbers should have leading zeroes (based on the total amount of textlines)."), Category("Additional Behavior")]
        public bool LineNrs_LeadingZeroes
        {
            get
            {
                return this.zLineNumbers_ShowLeadingZeroes;
            }
            set
            {
                this.zLineNumbers_ShowLeadingZeroes = value;
                this.Refresh();
                this.Invalidate();
            }
        }

        [Category("Additional Behavior"), Description("Use this property to manually reposition the LineNumbers, relative to their current location.")]
        public Size LineNrs_Offset
        {
            get
            {
                return this.zLineNumbers_Offset;
            }
            set
            {
                this.zLineNumbers_Offset = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public Color MarginLines_Color
        {
            get
            {
                return this.zMarginLines_Color;
            }
            set
            {
                this.zMarginLines_Color = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public LineNumberDockSide MarginLines_Side
        {
            get
            {
                return this.zMarginLines_Side;
            }
            set
            {
                this.zMarginLines_Side = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public DashStyle MarginLines_Style
        {
            get
            {
                return this.zMarginLines_Style;
            }
            set
            {
                if (value == DashStyle.Custom)
                {
                    value = DashStyle.Solid;
                }
                this.zMarginLines_Style = value;
                this.Invalidate();
            }
        }

        [Category("Additional Appearance")]
        public float MarginLines_Thickness
        {
            get
            {
                return this.zMarginLines_Thickness;
            }
            set
            {
                this.zMarginLines_Thickness = Math.Max(1f, Math.Min(255f, value));
                this.Invalidate();
            }
        }

        [Description("Use this property to enable LineNumbers for the chosen RichTextBox."), Category("Add LineNumbers to")]
        public RichTextBox ParentRichTextBox
        {
            get
            {
                return this.zParent;
            }
            set
            {
                this.zParent = value;
                if (this.zParent != null)
                {
                    this.Parent = this.zParent.Parent;
                    this.zParent.Refresh();
                }
                this.Text = "";
                this.Refresh();
                this.Invalidate();
            }
        }

        [Description("The BackgroundGradient is a gradual blend of two colors, shown in the back of each LineNumber's item-area."), Category("Additional Behavior")]
        public bool Show_BackgroundGradient
        {
            get
            {
                return this.zGradient_Show;
            }
            set
            {
                this.zGradient_Show = value;
                this.Invalidate();
            }
        }

        [Description("BorderLines are shown on all sides of the LineNumber control."), Category("Additional Behavior")]
        public bool Show_BorderLines
        {
            get
            {
                return this.zBorderLines_Show;
            }
            set
            {
                this.zBorderLines_Show = value;
                this.Invalidate();
            }
        }

        [Category("Additional Behavior"), Description("GridLines are the horizontal divider-lines shown above each LineNumber.")]
        public bool Show_GridLines
        {
            get
            {
                return this.zGridLines_Show;
            }
            set
            {
                this.zGridLines_Show = value;
                this.Invalidate();
            }
        }

        [Category("Additional Behavior")]
        public bool Show_LineNrs
        {
            get
            {
                return this.zLineNumbers_Show;
            }
            set
            {
                this.zLineNumbers_Show = value;
                this.Invalidate();
            }
        }

        [Description("MarginLines are shown on the Left or Right (or both in Height-mode) of the LineNumber control."), Category("Additional Behavior")]
        public bool Show_MarginLines
        {
            get
            {
                return this.zMarginLines_Show;
            }
            set
            {
                this.zMarginLines_Show = value;
                this.Invalidate();
            }
        }

        [AmbientValue(""), Browsable(false), DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = "";
                this.Invalidate();
            }
        }

        private RichTextBox zParent
        {
            [DebuggerNonUserCode]
            get
            {
                return this._zParent;
            }
            [MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode]
            set
            {
                EventHandler handler = new EventHandler(this.zParent_Disposed);
                ContentsResizedEventHandler handler2 = new ContentsResizedEventHandler(this.zParent_ContentsResized);
                EventHandler handler3 = new EventHandler(this.zParent_Scroll);
                EventHandler handler4 = new EventHandler(this.zParent_Scroll);
                EventHandler handler5 = new EventHandler(this.zParent_Changed);
                EventHandler handler6 = new EventHandler(this.zParent_Changed);
                EventHandler handler7 = new EventHandler(this.zParent_Changed);
                EventHandler handler8 = new EventHandler(this.zParent_Changed);
                EventHandler handler9 = new EventHandler(this.zParent_Changed);
                EventHandler handler10 = new EventHandler(this.zParent_Changed);
                if (this._zParent != null)
                {
                    this._zParent.Disposed -= handler;
                    this._zParent.ContentsResized -= handler2;
                    this._zParent.VScroll -= handler3;
                    this._zParent.HScroll -= handler4;
                    this._zParent.MultilineChanged -= handler5;
                    this._zParent.TextChanged -= handler6;
                    this._zParent.DockChanged -= handler7;
                    this._zParent.Resize -= handler8;
                    this._zParent.Move -= handler9;
                    this._zParent.LocationChanged -= handler10;
                }
                this._zParent = value;
                if (this._zParent != null)
                {
                    this._zParent.Disposed += handler;
                    this._zParent.ContentsResized += handler2;
                    this._zParent.VScroll += handler3;
                    this._zParent.HScroll += handler4;
                    this._zParent.MultilineChanged += handler5;
                    this._zParent.TextChanged += handler6;
                    this._zParent.DockChanged += handler7;
                    this._zParent.Resize += handler8;
                    this._zParent.Move += handler9;
                    this._zParent.LocationChanged += handler10;
                }
            }
        }

        private Timer zTimer
        {
            [DebuggerNonUserCode]
            get
            {
                return this._zTimer;
            }
            [MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode]
            set
            {
                EventHandler handler = new EventHandler(this.zTimer_Tick);
                if (this._zTimer != null)
                {
                    this._zTimer.Tick -= handler;
                }
                this._zTimer = value;
                if (this._zTimer != null)
                {
                    this._zTimer.Tick += handler;
                }
            }
        }

        // Nested Types
        public enum LineNumberDockSide : byte
        {
            Height = 4,
            Left = 1,
            None = 0,
            Right = 2
        }

        private class LineNumberItem
        {
            // Fields
            internal int LineNumber;
            internal Rectangle Rectangle;

            // Methods
            internal LineNumberItem(int zLineNumber, Rectangle zRectangle)
            {
                this.LineNumber = zLineNumber;
                this.Rectangle = zRectangle;
            }
        }
    }
}