using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BoreholeHistogram.Control
{
    /// <summary>
    /// 控件基类
    /// </summary>
    class Control:IDraw
    {
        #region 控件属性

        /// <summary>
        /// 控件名称
        /// </summary>
        public string CtrName { get { return ctrName; }  set { ctrName = value; } }

        /// <summary>
        /// 控件所在矩形框
        /// </summary>
        public RectangleF Rect { get { return rect; } set { rect = value; } }

        /// <summary>
        /// 是否进行内部填充
        /// </summary>      
        public bool FillInner { get { return fillInner; } set { fillInner = value; } }
        /// <summary>
        /// 是否绘制外包框
        /// </summary>
        public bool DrawOutLine { get { return drawOutLine; } set { drawOutLine = value; } }

        /// <summary>
        /// 内部填充颜色
        /// </summary>
        public Color FillColor { get { return fillColor; } set { fillColor = value; } }
        /// <summary>
        /// 外边框颜色
        /// </summary>
        public Color OutLineColor { get { return outLineColor; } set { outLineColor = value; } }

        #endregion

        #region 控件字段

        /// <summary>
        /// 控件名称
        /// </summary>
        private string ctrName;
        /// <summary>
        /// 控件所在矩形框
        /// </summary>
        private RectangleF rect;
        /// <summary>
        /// 控件边框颜色、内填充颜色
        /// </summary>
        private Color outLineColor, fillColor;
        /// <summary>
        /// 是否绘制外边框，是否填充边框内部
        /// </summary>
        private bool drawOutLine, fillInner; 
    
        /// <summary>
        /// 该页的起始深度、终止深度与间隔
        /// </summary>
        protected float beginDepth, endDepth, interval;

        /// <summary>
        /// 全局变量
        /// </summary>
        public BoreStyle.DrillGlobal drillGlobal;  //预留绘制分页的问题,还要修改分页内容
              
        #endregion

        #region 构造函数

        ///构造函数，初始化control类
        public Control()
        {
            rect = new RectangleF();
            outLineColor = Color.Blue;
            fillColor = Color.White;
            drawOutLine = false;
            fillInner = false; 
        }

        /// <summary>
        /// 重载构造函数，创建一个已知部分属性的control类
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        /// <param name="name"></param>
        public Control(RectangleF rectf, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
        {
            this.rect = rectf;
            this.outLineColor = outLineColor;
            this.fillColor = fillColor;
            this.drawOutLine = drawOutLine;
            this.fillInner = fillInner;
            this.ctrName = name;
        }

        #endregion

        #region 控件类基类的绘制

        /// <summary>
        /// 控件类绘制矩形边框，并为分页提供参数
        /// </summary>
        /// <param name="g"></param>
        /// <param name="data"></param>
        public virtual void Draw(Graphics g, BoreData.BoreData data)
        {
            try
            {
                //计算起始深度与终止深度、间隔，配合分页
                GetBeginAndEndDpth(data.BasicBoreInfo.totalDepth);

                //填充绘制控件区域,必须先填充再绘制，否则外框出不来              
                Brush fillBrush = new SolidBrush(fillColor);
                Pen linePen = new Pen(outLineColor);
                if (fillInner)
                    g.FillRectangle(fillBrush, rect);
                if (drawOutLine)
                    g.DrawRectangle(linePen, rect.Left, rect.Top, rect.Width, rect.Height);
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 计算起始深度与终止深度、间隔，配合分页
        /// </summary>
        /// <param name="td">总深度</param>
        private void GetBeginAndEndDpth(float td)
        {           
            beginDepth = -(drillGlobal.currentPage - 1) * drillGlobal.depthPerPage; //如不为负值后面计算深度会出错
            if (drillGlobal.currentPage != drillGlobal.totalPage)
                endDepth = -drillGlobal.currentPage * drillGlobal.depthPerPage;
            else
                endDepth = td;
            this.interval = endDepth - beginDepth;
            //这分页问题很多
        }

        /// <summary>
        /// 好像为了设置参数
        /// </summary>
        /// <param name="list"></param>
        public void SetParameters(System.Collections.IList list)    //这个留着未知
        {

        }

        #endregion


    }
}
