using System.Drawing;
using System.Drawing.Drawing2D;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 格网样式， 为Bar和Curve的聚合类
    /// </summary>
    public class GridStyle
    {
        private bool drawHorizontalGrid, drawVerticalGrid;
        private GridControl horizontalControl, verticalControl;
        private bool isLogCord;
        private int logBase;
        

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GridStyle()
        {
            this.drawHorizontalGrid = true;
            this.drawVerticalGrid = true;
            this.horizontalControl = new GridControl();
            this.verticalControl = new GridControl();
            this.isLogCord = false;
        }
        /// <summary>
        /// 格网岩石构造函数
        /// </summary>
        /// <param name="drawHorizontalGrid">是否绘制水平格网</param>
        /// <param name="drawVerticalGrid">是否绘制垂直格网</param>
        /// <param name="horizontalControl">水平格网样式</param>
        /// <param name="verticalControl">垂直格网样式</param>
        public GridStyle(bool drawHorizontalGrid, bool drawVerticalGrid, GridControl horizontalControl, GridControl verticalControl)
        {
            this.drawHorizontalGrid = drawHorizontalGrid;
            this.drawVerticalGrid = drawVerticalGrid;
            this.horizontalControl = horizontalControl;
            this.verticalControl = verticalControl;
            this.isLogCord = false;
        }
        
        /// <summary>
        /// 格网样式构造函数
        /// </summary>
        /// <param name="drawHorizontalGrid">是否绘制水平格网</param>
        /// <param name="drawVerticalGrid">是否绘制垂直格网</param>
        /// <param name="horizontalControl">水平格网样式</param>
        /// <param name="verticalControl">垂直格网样式</param>
        /// <param name="isLogCord">是否对数坐标系</param>
        /// <param name="logBase">对数底</param>
        public GridStyle(bool drawHorizontalGrid, bool drawVerticalGrid, GridControl horizontalControl, GridControl verticalControl,bool isLogCord,int logBase)
        {
            this.drawHorizontalGrid = drawHorizontalGrid;
            this.drawVerticalGrid = drawVerticalGrid;
            this.horizontalControl = horizontalControl;
            this.verticalControl = verticalControl;
            this.isLogCord = isLogCord;
            this.logBase = logBase;
        }
        /// <summary>
        /// 是否绘制水平格网
        /// </summary>
        public bool DrawHorizontalGrid
        {
            get { return drawHorizontalGrid; }
            set { drawHorizontalGrid = value; }
        }

        /// <summary>
        /// 是否绘制垂直格网
        /// </summary>
        public bool DrawVerticalGrid
        {
            get { return drawVerticalGrid; }
            set { drawVerticalGrid = value; }
        }

        /// <summary>
        /// 水平格网样式
        /// </summary>
        public GridControl HorizontalControl
        {
            get { return horizontalControl; }
            set { horizontalControl = value; }
        }

        /// <summary>
        /// 垂直格网样式
        /// </summary>
        public GridControl VerticalControl
        {
            get { return verticalControl; }
            set { verticalControl = value; }
        }


        /// <summary>
        /// 是否对数坐标系
        /// </summary>
        public bool IsLogCord
        {
            get { return isLogCord; }
            set { isLogCord = value; }
        }

        /// <summary>
        /// 对数底
        /// </summary>
        public int LogBase
        {
            get { return logBase; }
            set { logBase = value; }
        }

        
        public void DrawGrid(System.Drawing.Graphics g, System.Drawing.RectangleF rect)
        {
            if (drawHorizontalGrid)
                this.horizontalControl.DrawHorizontalGrid(g, rect);
            if (drawVerticalGrid)
                this.verticalControl.DrawVerticalGrid(g, rect);
        }
    }

    /// <summary>
    /// 格网控制
    /// </summary>
    public class GridControl
    {
        private bool drawLargeScaleGrid, drawSmallScaleGrid;
        private int largeScaleValue, smallScaleValue;        //大尺度值表示将总宽分成几份，小值表示将大尺度分成几份
        private Color largeLineColor,smallLineColor; //大尺度线色
        private DashStyle largeDashStyle, smallDashStyle; //大尺度线型，小尺度线型
        private float largePenWidth, smallPenWidth;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GridControl()
        {
            this.drawLargeScaleGrid = true;
            this.drawSmallScaleGrid = true;
            this.largeScaleValue = 5;
            this.smallScaleValue = 2;
            this.largeLineColor = Color.Black;
            this.smallLineColor = Color.Black;
            largeDashStyle = 0;
            smallDashStyle = 0;
            this.largePenWidth = 1;
            this.smallPenWidth = 1;
           
        }
        /// <summary>
        /// 格网控制构造函数
        /// </summary>
        /// <param name="drawLargeScaleGrid">是否绘制大尺度格网</param>
        /// <param name="drawSmallScaleGrid">是否绘制小尺度格网</param>
        /// <param name="largeScaleValue">大尺度格网值</param>
        /// <param name="smallScaleValue">小尺度格网值</param>
       
        public GridControl(bool drawLargeScaleGrid, bool drawSmallScaleGrid, int largeScaleValue, int smallScaleValue)
        {
            this.drawLargeScaleGrid = drawLargeScaleGrid;
            this.drawSmallScaleGrid = drawSmallScaleGrid;
            this.largeScaleValue = largeScaleValue;
            this.smallScaleValue = smallScaleValue;
            this.largeLineColor = Color.Black;
            this.smallLineColor = Color.Black;
            largeDashStyle = 0;
            smallDashStyle = 0;
            this.largePenWidth =1;
            this.smallPenWidth = 1;
        }

        /// <summary>
        /// 大尺度线宽
        /// </summary>
        public float LargePenWidth
        {
            get { return this.largePenWidth; }
            set { this.largePenWidth = value; }
        }

        /// <summary>
        /// 小尺度线宽
        /// </summary>
        public float SmallPenWidth
        {
            get { return this.smallPenWidth; }
            set { this.smallPenWidth = value; }
        }
        /// <summary>
        /// 大尺度线型
        /// </summary>
        public DashStyle LargeDashStyle
        {
            get {return this.largeDashStyle;}
            set { largeDashStyle = value; }
        }

        /// <summary>
        /// 小尺度线型
        /// </summary>
        public DashStyle SmallDashStyle
        {
            get { return this.smallDashStyle; }
            set { smallDashStyle = value; }
        }

        /// <summary>
        /// 是否绘制大尺度格网
        /// </summary>
        public bool DrawLargeScaleGrid
        {
            get { return drawLargeScaleGrid; }
            set { drawLargeScaleGrid = value; }
        }

        /// <summary>
        /// 是否绘制小尺度格网
        /// </summary>
        public bool DrawSmallScaleGrid
        {
            get { return drawSmallScaleGrid; }
            set { drawSmallScaleGrid = value; }
        }

        /// <summary>
        /// 大尺度格网值
        /// </summary>
        public int LargeScaleValue
        {
            get { return largeScaleValue; }
            set { largeScaleValue = value; }
        }

        /// <summary>
        /// 小尺度格网值
        /// </summary>
        public int SmallScaleValue
        {
            get { return smallScaleValue; }
            set { smallScaleValue = value; }
        }

        /// <summary>
        /// 大尺度线色
        /// </summary>
        public Color LargeLineColor
        {
            get { return this.largeLineColor; }
            set { this.largeLineColor = value; }
        }

        /// <summary>
        /// 小尺度线色
        /// </summary>
        public Color SmallLineColor
        {
            get { return this.smallLineColor; }
            set { this.smallLineColor = value; }
        }

        public void DrawHorizontalGrid(System.Drawing.Graphics g, System.Drawing.RectangleF rect)
        {
            float lastHeight=0;
            if(this.drawLargeScaleGrid)
                for (int i = 1; i < this.largeScaleValue; i++)
                {
                    if (i == this.largeScaleValue - 1)
                        lastHeight = rect.Height * i/this.largeScaleValue + rect.Top; //记录最后一行的高度
                    float priHeight = rect.Height * (i - 1) / this.largeScaleValue;
                    float height = rect.Height * i / this.largeScaleValue;
                    PointF p1 = new PointF(rect.Left, rect.Top + height);
                    PointF p2 = new PointF(rect.Left + rect.Width, rect.Top + height);
                    Pen pen1 = this.SetLargePen();
                    g.DrawLine(pen1, p1, p2);
                    if (this.drawSmallScaleGrid)
                        for (int j = 1; j < this.smallScaleValue; j++)
                        {
                            PointF p3 = new PointF(rect.Left, rect.Top + priHeight + (height - priHeight) * j / this.smallScaleValue);
                            PointF p4 = new PointF(rect.Left+rect.Width,rect.Top+priHeight+(height - priHeight) * j / this.smallScaleValue);
                            Pen pen2 = SetSmallPen();
                            g.DrawLine(pen2, p3, p4);
                        }
                }
            //绘制最后一行
            if(this.drawSmallScaleGrid)
                for (int j = 1; j < this.smallScaleValue; j++)
                {
                    float lastSmallHeight = lastHeight + (rect.Top + rect.Height - lastHeight) * j / smallScaleValue;
                    PointF p3 = new PointF(rect.Left, lastSmallHeight);
                    PointF p4 = new PointF(rect.Left + rect.Width, lastSmallHeight);
                    Pen pen2 = SetSmallPen();
                    g.DrawLine(pen2, p3, p4);
                }
           
        }

        public void DrawVerticalGrid(System.Drawing.Graphics g, System.Drawing.RectangleF rect)
        {
            float lastWidth=0;
            if (this.drawLargeScaleGrid)
                for (int i = 1; i < this.largeScaleValue; i++)
                {
                    if (i == this.largeScaleValue - 1)
                        lastWidth = rect.Width * i/this.largeScaleValue + rect.Left; //记录最后一列的水平位置
                    float priWidth=rect.Width*(i-1)/this.largeScaleValue;
                    float width=rect.Width*i/this.largeScaleValue;
                    PointF p1 = new PointF(rect.Left + width, rect.Top);
                    PointF p2 = new PointF(rect.Left + width, rect.Top + rect.Height);
                    Pen pen1 = this.SetLargePen();
                    g.DrawLine(pen1, p1, p2);
                    if (this.drawSmallScaleGrid)
                        for (int j = 1; j < this.smallScaleValue; j++)
                        {
                            PointF p3 = new PointF(rect.Left + priWidth + (width - priWidth) * j / this.smallScaleValue, rect.Top);
                            PointF p4 = new PointF(rect.Left + priWidth + (width - priWidth) * j / this.smallScaleValue, rect.Top + rect.Height);
                            Pen pen2 = SetSmallPen();
                            g.DrawLine(pen2, p3, p4);
                        }
                }
            if (this.drawSmallScaleGrid)
                for (int j = 1; j < this.smallScaleValue; j++)
                {
                    float lastSmallWidth = lastWidth + (rect.Left + rect.Width-lastWidth) * j / smallScaleValue;
                    PointF p3 = new PointF(lastSmallWidth, rect.Top);
                    PointF p4 = new PointF(lastSmallWidth, rect.Top+rect.Height);
                    Pen pen2 = SetSmallPen();
                    g.DrawLine(pen2, p3, p4);
                }

           
           
        }

        /// <summary>
        /// 设置大尺度线型
        /// </summary>
        /// <returns></returns>
        private Pen SetLargePen()
        {
            Pen pen1 = new Pen(this.largeLineColor);
            pen1.DashStyle = this.largeDashStyle;
            pen1.Width = this.largePenWidth;
            return pen1;
        }

        /// <summary>
        /// 设置小尺度线型
        /// </summary>
        /// <returns></returns>
        private Pen SetSmallPen()
        {
            Pen pen1 = new Pen(this.smallLineColor);
            pen1.DashStyle = this.smallDashStyle;
            pen1.Width = this.smallPenWidth;
            return pen1;
        }
    }
}
