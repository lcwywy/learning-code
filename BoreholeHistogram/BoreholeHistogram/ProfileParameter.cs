using System;
using System.Collections.Generic;
using System.Text;

namespace SmartGeoLogical.Render
{
    public class ProfileParameter
    { 
        /// <summary>
        /// 细分次数
        /// </summary>
        public int SubdivideCount { get; set; }
        /// <summary>
        /// 尖灭
        /// </summary>
        public int pinchout { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// X轴缩放比例
        /// </summary>
        public double ScaleX { get; set; }
        /// <summary>
        /// Y轴缩放比例
        /// </summary>
        public double ScaleY { get; set; }
        /// <summary>
        /// 输出文件类型
        /// </summary>
        public OutputFormart Formart { get; set; } = OutputFormart.Png;

        /// <summary>
        /// 钻孔间的高程
        /// </summary>
        public Dictionary<string, List<double>> DrillLineElevation { get; set; }

        public double ScaleXY
        {
            get
            {
                return  ScaleX / ScaleY;
            }
        }

        /// <summary>
        /// 使用的纹理
        /// </summary>
        /// <returns></returns>
        public string Texture { get; set; }

    }
}
