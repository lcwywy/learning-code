using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BoreholeHistogram.Control.Chart
{
    class Histogram:Control
    {
        public Histogram(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
             : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {

        }
    }
}
