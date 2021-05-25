using SmartGeoLogical.Core.DrillSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SmartGeoLogical.Render.DrillSectionDraw.Draw.View
{
    /// <summary>
    /// 剖面地层显示的范围
    /// </summary>
    struct Rectangle
    {
        public double Top;
        public double Bottom;
        public double Left;
        public double Right;
    }

    /// <summary>
    /// 按比例尺将剖面逻辑模型转换到剖面表达模型
    /// 原始数据坐标值根据比例尺缩放,
    /// </summary>
    class ScaleSection
    {
        public int mXScale = 10;
        public int mYScale = 1;
        public static Rectangle PolygonRectangle = new Rectangle();

        Rectangle sectionrectangle = new Rectangle();

        public void GetSectionRectangle(List<LogicBore> Bores)
        {
            double Max = 0, Min = 0;
            if (Bores != null)
            {
                for (int n = 0; n < Bores.Count; n++)
                {
                    Boreline linepoints = Bores[n].Borelinepoints;

                    int m = linepoints.mpoints.Count;
                    SPoint pointup = linepoints.mpoints[0];
                    SPoint pointdown = linepoints.mpoints[m - 1];
                    if (n == 0)
                    {
                        Max = pointup.Y;
                        Min = pointdown.Y;
                    }
                    else
                    {
                        //求Y最小值
                        if (Min > pointdown.Y)
                        {
                            Min = pointdown.Y;
                        }
                        //求Y最大值
                        if (Max < pointup.Y)
                        {
                            Max = pointup.Y;
                        }
                    }
                }
                sectionrectangle.Bottom = Min;
                sectionrectangle.Top = Max;
                //最左边钻孔的横坐标
                sectionrectangle.Left = Bores[0].Borelinepoints.mpoints[0].X;
                //最右边钻孔的横坐标
                sectionrectangle.Right = Bores[Bores.Count - 1].Borelinepoints.mpoints[0].X;
            }
        }
    }
}
