using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BoreholeHistogram.Control.Pannel
{
    /// <summary>
    /// 面板类
    /// </summary>
    class Pannel:Control
    {
        public Pannel(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
             : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {

        }

    }
}
