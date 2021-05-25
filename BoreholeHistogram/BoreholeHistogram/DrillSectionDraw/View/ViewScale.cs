using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using SmartGeoLogical.Core.DrillSection;

namespace SmartGeoLogical.Render.DrillSectionDraw.Draw.View
{
    /// <summary>
    /// 刻度尺
    /// </summary>
    class ViewScale
    {

        //剖面比例尺的逻辑模型
        private LogicScale mlogicleftscale;//左
        /// <summary>
        /// 刻度尺名字
        /// </summary>
        public string scalename;
        /// <summary>
        /// 判断刻度尺是否为左刻度尺，在计算和显示左右刻度尺时方法略有不同，需要区分
        /// </summary>
        public bool isLeft;
        //刻度
        public List<VLine> mGraduations;

        //主刻度尺
        public VLine mMainGreaduation;

        //刻度线的长度
        int mMarkLen = 30;

        int mxscale, myscale;

        List<LogicBore> bores;
        DrawScaling drawScaling;
        public ViewScale(DrawScaling drawScaling, string scalename, LogicScale logicscale, List<LogicBore> bores, int xscale, int yscale, bool isleft)
        {
            this.scalename = scalename;
            this.mlogicleftscale = logicscale;
            this.mxscale = xscale;
            this.myscale = yscale;
            this.isLeft = isleft;
            this.bores = bores;
            this.drawScaling = drawScaling;
        }
        public void GetGraduations(bool isLeft)
        {
            ////清空刻度尺
            mGraduations = new List<VLine>();

            //起始刻度
            int max = (int)(5 - ScaleSection.PolygonRectangle.Top % 5 + ScaleSection.PolygonRectangle.Top);
            //终止刻度
            int min = (int)( ScaleSection.PolygonRectangle.Bottom % 5-5 + ScaleSection.PolygonRectangle.Bottom);

            if (ScaleSection.PolygonRectangle.Bottom >= 0)
            {

            }
            else
            { 
                min = (int)(-5 - ScaleSection.PolygonRectangle.Bottom % 5 + ScaleSection.PolygonRectangle.Bottom);
            }
            double endX;
            if (isLeft)
            {
                endX = -100;//( mlogicleftscale.Abscissa-20)* DrawScaling.compensateX;
            }
            else
            {
                endX = -(drawScaling.Width - 100);// mlogicleftscale.Abscissa + ( mMarkLen)*DrawScaling.compensateX;
            }
            for (int y = min; y <= max; y = y + 1)
            {

                var line = GetLine(endX, y, isLeft);

                mGraduations.Add(line);

            }
        }
        /// <summary>
        /// 刻度 每间隔5显示刻度，刻度线加长
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isLeft"></param>
        /// <returns></returns>
        private VLine GetLine(double x, double y, bool isLeft)
        {
            VLine line = new VLine();
            int num = 3;
            if (y % 5 == 0)
            {
                line.attribute = y.ToString();
                num = 6;
            }
            else
            {
                line.attribute = "";
            }

            if (isLeft)
            {
                line.StartP = new VPoint() { X = x, Y = (int)y };
                line.EndP = new VPoint() { X = x - num, Y = (int)y };
            }
            else
            {

                line.StartP = new VPoint() { X = x - 6, Y = (int)y };
                if (num == 6)
                {
                    line.EndP = new VPoint() { X = x, Y = (int)y };
                }
                else
                {
                    line.EndP = new VPoint() { X = x - 3, Y = (int)y };
                }
            }
            return line;
        }
        ///// <summary>
        ///// 根据比例尺缩放标尺，包括刻度线集和主刻度尺
        ///// </summary>
        ///// <param name="islr">确定是左标尺还是右标尺</param>
        //public void GetGraduations(bool islr)
        //{
        //    ////清空刻度尺
        //    //mGraduations.RemoveAll();
        //    //最小、最大整数
        //    int min, max;
        //    //取个位数
        //    double minHeigth = 0;
        //    if (islr)
        //    {
        //        minHeigth = this.bores[0].Borelinepoints.mpoints[this.bores[0].Borelinepoints.mpoints.Count - 1].Y;
        //    }
        //    else
        //    {
        //        minHeigth = this.bores[bores.Count-1].Borelinepoints.mpoints[this.bores[bores.Count - 1].Borelinepoints.mpoints.Count - 1].Y;
        //    }

        //    int geshu = mlogicleftscale.Min % 10;

        //    min = (int)(minHeigth - geshu);
        //    geshu = mlogicleftscale.Max % 10;
        //    if (geshu>0)
        //    {
        //        max = mlogicleftscale.Max +10- geshu;
        //    }
        //    else
        //    {
        //        max = mlogicleftscale.Max;
        //    }

        //    double sx = mlogicleftscale.Abscissa;//刻度起始点的横坐标，一个比例尺是一定的
        //    double ex;
        //    if (islr == true)
        //    {
        //        ex = mlogicleftscale.Abscissa - mMarkLen;//刻度终止点的横坐标
        //    }
        //    else
        //        ex = mlogicleftscale.Abscissa + mMarkLen;

        //    VPoint vp = new VPoint();
        //    vp.X = sx;
        //    vp.Y = min - mlogicleftscale.Mark;
        //    VPoint vp2 = new VPoint();
        //    vp2.X = ex;
        //    vp2.Y = vp.Y;
        //    //值最小的刻度线
        //    VLine linemin = new VLine();
        //    linemin.StartP = vp;
        //    linemin.EndP = vp2;
        //    linemin.attribute = vp.Y.ToString();//刻度线的真实值
        //    //添加刻度线
        //    mGraduations.Add(linemin);

        //    for (int n = min; n <= max + mlogicleftscale.Mark; n = n + mlogicleftscale.Mark)
        //    {
        //        VPoint vp0 = new VPoint();
        //        vp0.X = sx;
        //        vp0.Y = n;
        //        VPoint vp1 = new VPoint();
        //        vp1.X = ex;
        //        vp1.Y = n;
        //        VLine lineup = new VLine();
        //        lineup.StartP = vp0;
        //        lineup.EndP = vp1;
        //        lineup.attribute = n.ToString();//刻度线的真实值
        //        mGraduations.Add(lineup);
        //    }
        //    int kedu = Convert.ToInt32(mGraduations[0].attribute);
        //    if (kedu > mlogicleftscale.Min)
        //    {
        //        VPoint vp0 = new VPoint();
        //        vp0.X = sx;
        //        vp0.Y = kedu - mlogicleftscale.Mark;
        //        VPoint vp1 = new VPoint();
        //        vp1.X = ex;
        //        vp1.Y = vp0.Y;
        //        VLine lineup = new VLine();
        //        lineup.StartP = vp0;
        //        lineup.EndP = vp1;
        //        lineup.attribute = vp0.Y.ToString();//刻度线的真实值
        //        mGraduations.Insert(0, lineup);
        //    }
        //    kedu = Convert.ToInt32(mGraduations[mGraduations.Count - 1].attribute);
        //    if (kedu < mlogicleftscale.Max)
        //    {
        //        VPoint vp0 = new VPoint();
        //        vp0.X = sx;
        //        vp0.Y = kedu + mlogicleftscale.Mark;
        //        VPoint vp1 = new VPoint();
        //        vp1.X = ex;
        //        vp1.Y = vp0.Y;
        //        VLine lineup = new VLine();
        //        lineup.StartP = vp0;
        //        lineup.EndP = vp1;
        //        lineup.attribute = vp0.Y.ToString();//刻度线的真实值
        //        mGraduations.Add(lineup);
        //    }

        //    VLine mainline = new VLine();
        //    if (mGraduations.Count > 0)
        //    {
        //        //主刻度尺的起始点即最低点
        //        mainline.StartP = mGraduations[0].StartP;
        //        //主刻度尺的终止点即最高点
        //        mainline.EndP = mGraduations[mGraduations.Count - 1].StartP;
        //        mMainGreaduation = mainline;
        //    }
        //    //if (islr)
        //    //{
        //    //    ScaleSection.PolygonRectangle.Bottom = mGraduations[0].StartP.Y;
        //    //    ScaleSection.PolygonRectangle.Top = mGraduations[mGraduations.Count - 1].StartP.Y;
        //    //    ScaleSection.PolygonRectangle.Left = mGraduations[0].EndP.X;
        //    //}
        //    //else
        //    //{
        //    //    ScaleSection.PolygonRectangle.Right = mGraduations[0].EndP.X;
        //    //}

        //}

    }

}
