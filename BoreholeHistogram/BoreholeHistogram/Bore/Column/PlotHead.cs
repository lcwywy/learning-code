using System.Drawing;
using System.Threading.Tasks;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 图片
    /// </summary>
    public class PlotHead : Control
    {
        private float max, min;
        private string curveBarName, secondaryName;
        private Font primaryFont, secondaryFont;
        private Color primaryColor, secondaryColor;
        private bool ifDrawSecondaryName;
        private float lineWidth;
        private Color lineColor;
        /// <summary>
        /// 图片构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public PlotHead(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner, name)
        {
            max = 3;
            min = 0;
            ifDrawSecondaryName = false;
            curveBarName = "pri";
            primaryFont = new Font("宋体", 14);
            secondaryFont = new Font("宋体", 10);
            primaryColor = Color.Black;
            secondaryColor = Color.Black;
            lineWidth = 3;
            lineColor = Color.Red;
        }

        public float Max
        {
            get { return max; }
            set { max = value; }
        }

        public float Min
        {
            get { return min; }
            set { min = value; }
        }

        public string CurveBarName
        {
            get { return curveBarName; }
            set { curveBarName = value; }
        }

        public string SecondaryName
        {
            get { return secondaryName; }
            set { secondaryName = value; }
        }

        public Font PrimaryFont
        {
            get { return primaryFont; }
            set { primaryFont = value; }
        }

        public Font SecondaryFont
        {
            get { return secondaryFont; }
            set { secondaryFont = value; }
        }

        public float LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        public Color PrimaryColor
        {
            get { return primaryColor; }
            set { primaryColor = value; }
        }

        public Color SecondaryColor
        {
            get { return secondaryColor; }
            set { secondaryColor = value; }
        }

        public bool IfDrawSecondaryName
        {
            get { return ifDrawSecondaryName; }
            set { ifDrawSecondaryName = value; }
        }

        public override void Draw(Graphics g, BoreData data)
        {
            base.Draw(g, data);
            DrawLine(g);
            DrawMaxMinString(g);
            DrawTitle(g);

        }

        private void DrawTitle(Graphics g)
        {

            string primaryString = this.CtrName;
            
            SizeF size1 = g.MeasureString(primaryString, this.primaryFont);
            float primaryHeight = this.Top + this.Height / 20; //主标题高度
            PointF pointMax = new PointF(this.Left+(this.Width-size1.Width)/2, primaryHeight);
            
            g.DrawString(primaryString, this.primaryFont, new SolidBrush(primaryColor), pointMax);
            if (ifDrawSecondaryName)
            {
                string secondaryString = secondaryName.ToString();
                SizeF size2 = g.MeasureString(secondaryString, this.secondaryFont);
                PointF pointMin = new PointF(this.Left + (this.Width - size2.Width) / 2, primaryHeight+size1.Height+this.Height/10);
                g.DrawString(secondaryString, this.secondaryFont, new SolidBrush(secondaryColor), pointMin);
            }
        }

        private void DrawMaxMinString(Graphics g)
        {
            string maxString = max.ToString();
            string minString = min.ToString();

            SizeF size2 = g.MeasureString(minString, new Font("宋体", 8));

            g.DrawString(maxString, new Font("宋体", 8), new SolidBrush(Color.Black), new PointF(this.Left, this.Top + this.Height * 3 / 4 - size2.Height));
            g.DrawString(minString,new Font("宋体",8),new SolidBrush(Color.Black),new PointF(this.Left+this.Width-size2.Width,this.Top+this.Height*3/4 - size2.Height));
        }

        private void DrawLine(Graphics g)
        {
            float begin = this.Left + this.Width / 20;
            float end = this.Left + this.Width * 19 / 20;
            float verticalPosition = this.Top + this.Height * 3 / 4;
            Pen pen1 = new Pen(this.lineColor, this.lineWidth);

            g.DrawLine(pen1, new PointF(begin, verticalPosition), new PointF(end, verticalPosition));
        }


    }
}
