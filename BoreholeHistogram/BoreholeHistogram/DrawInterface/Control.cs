using System.Drawing;
using System.Threading.Tasks;

namespace SmartGeoLogical.Render.DrawInterface
{
    /// <summary>
    /// 控件类，各钻孔元素基类
    /// </summary>
    public class Control:IDraw
    {
        //位置属性 左，顶，宽，高
        private float left, top, width, height;       
        //外包框颜色，内部填充颜色
        private Color outLineColor, fillColor;
        //是否绘制外包框、内填充
        private bool drawOutLine,fillInner;
       //控件名称
        string ctrName;
        /// <summary>
        /// 该页的起始深度、终止深度与间隔
        /// </summary>
        protected float beginDepth, endDepth,interval;
        /// <summary>
        /// 全局变量
        /// </summary>
        public DrillGlobal drillGlobal;

        /// <summary>
        /// 赋值控件，构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        /// <param name="name"></param>
        public Control(RectangleF rect,Color outLineColor,Color fillColor,bool drawOutLine,bool fillInner,string name)
        {
            this.left = rect.Left;
            this.top = rect.Top;
            this.width = rect.Width;
            this.height = rect.Height;
            this.outLineColor = outLineColor;
            this.fillColor = fillColor;
            this.drawOutLine = drawOutLine;
            this.fillInner = fillInner;
            this.ctrName = name;
        }

        /// <summary>
        /// 初始化控件，构造函数
        /// </summary>
        public Control()
        {
            left = 0;
            top = 0;
            width = 0;
            height = 0;
            outLineColor = Color.Blue;
            fillColor = Color.White;
            drawOutLine = false;
            fillInner = false;
        }
        #region IDraw 成员

        /// <summary>
        /// 实现IDraw接口绘制功能,得到起始、终止深度及间隔，可绘制、填充矩形框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="data"></param>
        public virtual  void Draw(Graphics g,BoreData.BoreData data)
        {
            try
            {
               
                //根据总深度与当前页计算该页起始深度与终止深度
                //GetBeginAndEndDpth(data.BasicBoreInfo.完井深度-data.BasicBoreInfo.孔口高程);
                GetBeginAndEndDpth(data.BasicBoreInfo.totalDepth);
                using Pen linePen = new Pen(outLineColor);

                using Brush fillBrush = new SolidBrush(fillColor);
                //必须先填充再绘制，否则外框出不来
                if (fillInner)
                    g.FillRectangle(fillBrush, left, top, width, height);
                if (drawOutLine)
                    g.DrawRectangle(linePen, left, top, width, height);
            
            //g.DrawString(this.ctrName, new Font("宋体", 8), new SolidBrush(Color.Red), new PointF(left + width / 2, top + height / 2));
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.ToString());
            }


        }

        /// <summary>
        /// 计算起始深度与终止深度、间隔
        /// </summary>
        /// <param name="td">总深度</param>
        private void GetBeginAndEndDpth(float td)
        {
            //beginDepth = td * (BGlobal.currentPage - 1) / BGlobal.totalPage;
            //if (BGlobal.currentPage != BGlobal.totalPage)
            //    endDepth = td * (BGlobal.currentPage) / BGlobal.totalPage;
            //else
            //    endDepth = td;
            beginDepth = -(drillGlobal.currentPage - 1) * drillGlobal.depthPerPage; //如不为负值后面计算深度会出错
            if (drillGlobal.currentPage != drillGlobal.totalPage)
                endDepth = -drillGlobal.currentPage * drillGlobal.depthPerPage;
            else
                endDepth =td;
            this.interval = endDepth - beginDepth;
        }
        
      
        public void SetParameters(System.Collections.IList list)
        {
            
        }

        /// <summary>
        /// 设置文字格式，位置格式
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public StringFormat GetFormat(AlignStyle align)
        {
            StringFormat stringFormat = new StringFormat();

            switch (align)
            {
                case AlignStyle.Left:
                    break;
                case AlignStyle.Center:
                    break;
                case AlignStyle.Right:
                    break;
                case AlignStyle.NearNear:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case AlignStyle.NearCenter:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case AlignStyle.NearFar:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case AlignStyle.CenterNear:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case AlignStyle.CenterCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case AlignStyle.CenterFar:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case AlignStyle.FarNear:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case AlignStyle.FarCenter:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case AlignStyle.FarFar:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                default:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
            }
            return stringFormat;
        }
        #endregion

        /// <summary>
        /// 控件关联矩形框的位置信息，左部
        /// </summary>
        public float Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// 控件关联矩形框的位置信息，顶部
        /// </summary>
        public float Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// 控件关联矩形框的位置信息,宽
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// 控件关联矩形框的位置信息,高
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 外包框色
        /// </summary>
        public Color OutLineColor
        {
            get { return outLineColor; }
            set { outLineColor = value; }
        }

        /// <summary>
        /// 填充色
        /// </summary>
        public Color FillColor
        {
            get { return fillColor; }
            set { fillColor = value; }
        }

        /// <summary>
        /// 是否绘制外包框
        /// </summary>
        public bool DrawOutLine
        {
            get { return drawOutLine; }
            set { drawOutLine = value; }
        }

        /// <summary>
        /// 是否填充矩形框
        /// </summary>
        public bool FillInner
        {
            get { return fillInner; }
            set { fillInner = value; }
        }

        /// <summary>
        /// 控件名称
        /// </summary>
        public string CtrName
        {
            get { return ctrName; }
            set { ctrName = value; }
        }
    }
}
