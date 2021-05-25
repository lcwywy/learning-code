using System.Drawing.Printing;

namespace SmartGeoLogical.Render {
    /// <summary>
    /// 绘制的参数
    /// </summary>
    public class DrawParameter {
        public int Width { get; set; } = 1000;
        public int Height { get; set; } = 800;
        public OutputFormart Formart { get; set; } = OutputFormart.Png;
        public string Style { get; set; } = "南京";
    }

    public enum OutputFormart {
        Jpeg,
        Png,
        Bmp
    }
}