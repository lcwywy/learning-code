using System.Collections.Generic;

namespace BoreholeHistogram.BoreData
{
    /// <summary>
    /// 地层属性
    /// </summary>
    public class LayerProperty
    {
        public string LayerName;
        public object Value;

        public LayerProperty(string layerName, object value)
        {
            this.LayerName = layerName;
            this.Value = value;
        }

        public LayerProperty()
        {

        }
    }
    /// <summary>
    /// 地层信息
    /// </summary>
    public class LayerInfo
    {
        private string bordId;
        //底板埋深
        private float layerBottom;
        /// <summary>
        ///地层底板绝对高度
        /// </summary>
        private float zDown;
        /// <summary>
        /// 地层顶板绝对高度
        /// </summary>
        private float zUp;

        //岩性名称
        private string lithName;
        //岩性描述
        private string layerDescription;
        //工程地质名称
        private string engName;
        //成因时间
        private string beginTime;
        //地层属性
        public List<LayerProperty> LayerProperties;
        /// <summary>
        /// 地层厚度
        /// </summary>
        public float Thickness
        {
            get {

                return (float)System.Math.Round( System.Math.Abs(ZUp - ZDown),2);
            }
        }

        public string LithName { get => lithName; set => lithName = value; }
        public string LayerDescription { get => layerDescription; set => layerDescription = value; }
        public string EngName { get => engName; set => engName = value; }
        public string BeginTime { get => beginTime; set => beginTime = value; }
        public float ZUp { get => zUp; set => zUp = value; }
        public float ZDown { get => zDown; set => zDown = value; }
        public float LayerBottom { get => layerBottom; set => layerBottom = value; }
        public string BordId { get => bordId; set => bordId = value; }

        public LayerInfo()
        {

        }
        public LayerInfo(float layerBottom, string lithName, string layerDescription, List<LayerProperty> layerProperties)
        {
            this.LayerBottom = layerBottom;
            this.LithName = lithName;
            this.LayerDescription = layerDescription;
            this.LayerProperties = layerProperties;
        }
    }
}
