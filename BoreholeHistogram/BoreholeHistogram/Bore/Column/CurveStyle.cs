using System.Drawing;
using System.Drawing.Drawing2D;
using SmartGeoLogical.Render.DrawInterface;
using FillMode = SmartGeoLogical.Render.DrawInterface.FillMode;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 格网样式，格网的聚合类
    /// </summary>
    public class CurveStyle
    {
        private CurveCatalog plotStyle;
        private FillType curveFillType;
        private LineStyle curveLineStyle;
        private CurveSymbol curveSymbol;
        private DrillGlobal drillGlobal;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CurveStyle(DrillGlobal drillGlobal)
        {
            plotStyle = CurveCatalog.Line;
            curveFillType = new FillType();
            curveLineStyle = new LineStyle();
            curveSymbol = new CurveSymbol();
            this.drillGlobal = drillGlobal;
        }

        #region 属性

        /// <summary>
        /// 曲线填充类型
        /// </summary>
        public FillType CurveFillType
        {
            
            get { return curveFillType; }
            set { curveFillType = value; }
        }
      
        /// <summary>
        /// 曲线符号
        /// </summary>
        public CurveSymbol CurveSymbol
        {
            get { return curveSymbol; }
            set { curveSymbol = value; }
        }

        /// <summary>
        /// 曲线线样式
        /// </summary>
        public LineStyle CurveLineStyle
        {
            get { return curveLineStyle; }
            set { curveLineStyle = value; }
        }

        /// <summary>
        /// 曲线图样式
        /// </summary>
        public CurveCatalog PlotStyle
        {
            get { return this.plotStyle; }
            set { plotStyle = value; }
        }

        #endregion 属性

        /// <summary>
        /// 根据plotStyle绘制曲线
        /// </summary>
        /// <param name="points"></param>
        /// <param name="g"></param>
        public void DrawCurveByPoints(PointF[] points, Graphics g)
        {
            Pen pen;
            Brush brush;
            switch (plotStyle)
            {
                case CurveCatalog.Line: //绘曲线图
                    pen = new Pen(curveLineStyle.LineColor);
                    pen.DashStyle = curveLineStyle.Style;
                    pen.Width = curveLineStyle.LineWidth;
                    g.DrawCurve(pen,points);	
                    break;
                case CurveCatalog.Point://绘散点图
                    pen = new Pen(curveSymbol.SymColor);
                    brush = new SolidBrush(curveSymbol.SymColor);
                    DrawSinglePoints(points, g, brush, pen);
                    brush.Dispose();
                    break;

                case CurveCatalog.Bar://绘杆状图
                    pen = new Pen(curveLineStyle.LineColor);
                    pen.DashStyle = curveLineStyle.Style;
                    pen.Width = curveLineStyle.LineWidth;
                    DrawPool(g, pen,points);
                    break;
                case CurveCatalog.Fill://填充
                    DrawFill(points, g);
                    break;
                case CurveCatalog.LinePoint://点线图
                    pen = new Pen(curveSymbol.SymColor);
                    pen.DashStyle = curveLineStyle.Style;
                    pen.Width = curveLineStyle.LineWidth;
                    g.DrawCurve(pen, points);
                    brush = new SolidBrush(curveSymbol.SymColor);
                    DrawSinglePoints(points, g, brush, pen);
                    brush.Dispose();
                    break;
                case CurveCatalog.FillPoint://点填充图
                    DrawFill(points, g);
                    pen = new Pen(curveSymbol.SymColor);
                    pen.DashStyle = curveLineStyle.Style;
                    pen.Width = curveLineStyle.LineWidth;
                    brush = new SolidBrush(curveSymbol.SymColor);
                    DrawSinglePoints(points, g, brush, pen);
                    brush.Dispose();
                    break;
            }
        }

        /// <summary>
        ///    画散点图
        /// </summary>
        /// <param name="w"></param>
        /// <param name="pf"></param>
        /// <param name="g"></param>
        /// <param name="mybrush"></param>
        /// <param name="mypen"></param>
        private void DrawSinglePoints(PointF[] pf, Graphics g, Brush mybrush, Pen mypen)
        {
            float symbolSize = curveSymbol.SymSize;
            //第一页则没有头上的认为增加点，最后一页则没有尾上的认为增加点
            int j=1,k=2;
            if (drillGlobal?.currentPage != 1)
                j++;
            if (drillGlobal?.currentPage == drillGlobal?.totalPage)
                k--;

            for (; j < pf.Length-k; j++)                       //首位人为添加的点不绘制
            {
                if (curveSymbol.SymStyle == SymbolStyle.Triangle)		//三角形
                {
                   
                    PointF[] point = new PointF[3] { new PointF(pf[j].X, pf[j].Y - symbolSize), new PointF(pf[j].X - symbolSize, pf[j].Y + symbolSize ), new PointF(pf[j].X + symbolSize , pf[j].Y + symbolSize ) };
                    //以该点为中心的等边三角形
                    g.DrawPolygon(mypen, point);						//画多边形
                    g.FillPolygon(mybrush, point);					//填充多边形内部
                   
                }
                else if (curveSymbol.SymStyle == SymbolStyle.Ellipse)//椭圆
                {
                    g.DrawEllipse(mypen, pf[j].X - symbolSize, pf[j].Y - symbolSize, 2 * symbolSize, 2 * symbolSize);
                    //画椭圆
                    g.FillEllipse(mybrush, pf[j].X - symbolSize , pf[j].Y - symbolSize , 2 * symbolSize , 2 * symbolSize );
                    //填充椭圆
                    
                }
                else if (curveSymbol.SymStyle == SymbolStyle.Rectangle) //矩形
                {
                    PointF[] points = new PointF[4]{ new PointF(pf[j].X - symbolSize, pf[j].Y - symbolSize),new PointF(pf[j].X+symbolSize,pf[j].Y-symbolSize),
                        new PointF(pf[j].X+symbolSize,pf[j].Y+symbolSize),new PointF(pf[j].X-symbolSize,pf[j].Y+symbolSize)};
                    g.DrawPolygon(mypen, points);	                //画多边形 
                    g.FillPolygon(mybrush, points);					//填充多边形内部
                    					                   
                }
            }
        }

        /// <summary>
        ///  画杆状图
        /// </summary>
        /// <param name="w"></param>
        /// <param name="g"></param>
        /// <param name="mypen"></param>
        /// <param name="pf"></param>
        private void DrawPool( System.Drawing.Graphics g, Pen mypen, PointF[] pf)
        {
            for (int j = 1; j < pf.Length-1; j++)
                g.DrawLine(mypen, pf[0].X, pf[j].Y, pf[j].X, pf[j].Y);
        }

        /// <summary>
        ///    画填充图
        /// </summary>
        /// <param name="pf"></param>
        /// <param name="w"></param>
        /// <param name="g"></param>
        private void DrawFill(PointF[] pf, System.Drawing.Graphics g)
        {
            GraphicsPath myGraphicsPath = new GraphicsPath();
            ////互相联系的一系列线
            //PointF pt1 = new PointF(pf[pf.Length-1].X, pf[pf.Length-1].Y);
            ////最后一个点
            //PointF pt2 = new PointF(pf[0].X, pf[pf.Length-1].Y);
            ////边上和最后一个点平行的一个点
            //PointF pt3 = new PointF(pf[0].X, pf[pf.Length - 1].Y);
            ////左下
            //PointF pt4 = new PointF(pf[0].X, pf[0].Y);
            ////左上
            //PointF pt5 = new PointF(pf[0].X, pf[0].Y);
            ////和第一个点平行的顶上的一个点
            //PointF pt6 = new PointF(pf[0].X, pf[0].Y);
            ////第一个点
            //PointF[] pth = new PointF[6] { pt1, pt2, pt3, pt4, pt5, pt6 };
            //myGraphicsPath.AddLines(pth);
            ////增加一些点
            Brush myBrush;
            myGraphicsPath.AddCurve(pf);
            if(curveFillType.FigFillMode==FillMode.Single)
                myBrush = new HatchBrush(HatchStyle.BackwardDiagonal, curveFillType.SingleColor, curveFillType.SingleColor);
            else
            {
                float maxWidth= GetMaxWidth(pf);
                myBrush = new LinearGradientBrush(pf[0], new PointF(maxWidth,pf[0].Y), curveFillType.ValueBasedBegin, curveFillType.ValueBasedEnd);
            }
            g.FillPath(myBrush, myGraphicsPath);
            //填充起来
        }

        /// <summary>
        /// 将最大横向偏移值设为颜色终止位置
        /// </summary>
        /// <param name="pf"></param>
        /// <returns></returns>
        private float GetMaxWidth(PointF[] pf)
        {
            float max = pf[0].X;
            float min = pf[0].X;
            for (int i = 0; i < pf.Length; i++)
            {
                if (pf[i].X > max)
                    max = pf[i].X;
                if (pf[i].X < min)
                    min = pf[i].X;
            }
            if (max == pf[0].X)
                return min;
            else
                return max;
        }
    }

    /// <summary>
    /// 曲线符号
    /// </summary>
    public class CurveSymbol
    {
        private int symSize;
        private Color symColor;
        private SymbolStyle symStyle;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CurveSymbol()
        {
            symStyle = SymbolStyle.Ellipse;
            symSize = 2;
            symColor = Color.DarkBlue;
        }

        public SymbolStyle SymStyle
        {
            get{return symStyle;}
            set{symStyle=value;}
        }

        public int SymSize
        {
            get{return symSize;}
            set{symSize=value;}
        }

        public Color SymColor
        {
            get{return symColor;}
            set{symColor=value;}
        }
    }

    /// <summary>
    /// 线样式
    /// </summary>
    public class LineStyle
    {
        private int lineWidth;
        private Color lineColor;
        private DashStyle style;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LineStyle()
        {
            style = DashStyle.Solid;
            lineColor = Color.Brown;
            lineWidth = 3;
        }
        public DashStyle Style
        {
            get { return style; }
            set { style = value; }
        }

        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        public Color LineColor
        {
            get { return this.lineColor; }
            set { lineColor = value; }
        }
    }

}
