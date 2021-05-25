using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.DrawInterface.BoreData;
using System.Threading.Tasks;

namespace SmartGeoLogical.Render.Bore.Column
{
    /// <summary>
    /// 岩性纹理
    /// </summary>
    public class Lithority:Control
    {
        private string profileCurve;
        private bool hasProfileCurve;
        private float profileMaxValue, profileMinValue;
        private float tempTop=0;//绘制的临时变量，存储顶界
        /// <summary>
        /// 岩性纹理构造函数
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outLineColor"></param>
        /// <param name="fillColor"></param>
        /// <param name="drawOutLine"></param>
        /// <param name="fillInner"></param>
        public Lithority(RectangleF rect, Color outLineColor, Color fillColor, bool drawOutLine, bool fillInner, string name)
            : base(rect, outLineColor, fillColor, drawOutLine, fillInner,name)
        {
            this.profileCurve = null;
            this.hasProfileCurve = false;
            this.profileMaxValue = 0;
            this.profileMinValue = 0;
        }

        /// <summary>
        /// 附属曲线名
        /// </summary>
        public string ProfileCurve
        {
            get { return profileCurve; }
            set { profileCurve = value; }
        }

        /// <summary>
        /// 是否包含附属曲线
        /// </summary>
        public bool HasProfileCurve
        {
            get { return hasProfileCurve; }
            set { hasProfileCurve = value; }
        }

        /// <summary>
        /// 附属曲线最大值
        /// </summary>
        public float ProfileMaxValue
        {
            get { return profileMaxValue; }
            set { profileMaxValue = value; }
        }

        /// <summary>
        /// 附属曲线最小值
        /// </summary>
        public float ProfileMinValue
        {
            get { return profileMinValue; }
            set { profileMinValue = value; }
        }

        public override void Draw(Graphics g, BoreData data)
        {
            try
            {
                base.Draw(g, data);
              
                //tempTop = data.BasicBoreInfo.孔口高程;
                tempTop = beginDepth;

                List<LayerInfo> layers = data.LayerInfoList;
                //float totalDepth = Math.Abs(layers[layers.Count-1].LayerBottom - data.BasicBoreInfo.孔口高程);
                float totalDepth = Math.Abs(endDepth - beginDepth);//总深度：开始深度-结束深度

                foreach (LayerInfo layer in layers)
                {
                    //if(Math.Abs(layer.LayerBottom)>Math.Abs(beginDepth))
                    DrawLayer(layer, totalDepth, data.BasicBoreInfo.DrillZ, g, data.LegendPath);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        /// <summary>
        /// 绘制地层
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="totalDepth"></param>
        /// <param name="holeElevation"></param>
        /// <param name="g"></param>
        private void DrawLayer(LayerInfo layer, float totalDepth,float holeElevation,Graphics g, string legend)
        {
            if (Math.Abs(layer.LayerBottom) < Math.Abs(beginDepth))
                return;                                          //底界必须大于最小深度，否则不绘制
            if (Math.Abs(layer.LayerBottom) > Math.Abs(endDepth))
                layer.LayerBottom = endDepth;                    //底界大于该页最大深度，画到底界为止

            float layerHeight = Math.Abs( layer.LayerBottom - tempTop)*this.Height/totalDepth;
            //float layerTop = this.Top+Math.Abs(tempTop - holeElevation) * this.Height / totalDepth;
            float layerTop = this.Top + Math.Abs(tempTop-beginDepth) * this.Height / totalDepth;

            var legendPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DrillSet", "Legend", legend, layer.LithName);
            if (File.Exists(legendPath+ ".jpg"))
            {
                legendPath += ".jpg";
            }
            else if (File.Exists(legendPath + ".png"))
            {
                legendPath += ".png";
            }

            //AppDomain.CurrentDomain.BaseDirectory + $"DrillSet{Path.DirectorySeparatorChar}Legend{Path.DirectorySeparatorChar}{layer.LithName}.jpg";
            Image img =Bitmap.FromFile(legendPath);
            img= RepeatImag(layerHeight, img);

            g.DrawImage(img, new RectangleF(this.Left, layerTop, this.Width, layerHeight));
            g.DrawRectangle(new Pen(Color.Black), this.Left, layerTop, this.Width, layerHeight);
            this.tempTop = layer.LayerBottom;
        }
        /// <summary>
        /// 图片重复填充的方式
        /// </summary>
        /// <param name="layerHeight"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        private Image RepeatImag(float layerHeight, Image img)
        {
      
            int xCount = (int)Math.Round(this.Width /50);
            int yCount = (int)Math.Round(layerHeight / 30);
            xCount = xCount != 0 ? xCount : 1;
            yCount = yCount != 0 ? yCount : 1;
            if (!(xCount == 1 && yCount == 1))
            {
                int singleWidth = (int)Math.Ceiling(this.Width / xCount);
                int singleHeigth = (int)Math.Ceiling(layerHeight / yCount);
              
                Size size = new Size((int)this.Width, (int)layerHeight);
                Bitmap bmp = new Bitmap((int)this.Width, (int)layerHeight);
                Graphics graphics = Graphics.FromImage(bmp);
                for (int i = 0; i < xCount; i++)
                {
                    for (int j = 0; j < yCount; j++)
                    {
                        graphics.DrawImage(img, singleWidth * i, singleHeigth * j,singleWidth,  singleHeigth);
                    }
                }

                img = bmp;
            }
            return img;
        }
        private  void GetLengendPath()
        {

        }
    }
}
