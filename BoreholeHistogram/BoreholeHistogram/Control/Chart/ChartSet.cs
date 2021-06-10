using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BoreholeHistogram.Control
{
    enum ChartType  //图表类型
    {
        Area,  //面积图     
        Histogram,  //直方图          
        Box,  //箱图        
        Curve,  //曲线图
        BrokenLine,  //折线图
        Oscillogram,  //波形图
        Envelope,  //包线图
    }

    class ChartSet  //图表设置器
    {

        bool ChartDirection;  //图像摆放方向，默认为纵向false、横向true



        #region 图表设置器内容
        //图表控件实现绘制功能
        public void Draw(Graphics g) //实现绘制功能
        { }

        public void SetData()  //设置数据来源
        { }

        public void SetXY()  //设置XY轴
        { }

        public void SetMajorGrid()  //设置网格
        { }

        public void SetLegend()  //设置图例
        { }

        public void DrawChart(ChartType type)  //绘画图像
        { }
        #endregion
    }

    class Curve  //曲线图
    { 
    
    }
}
