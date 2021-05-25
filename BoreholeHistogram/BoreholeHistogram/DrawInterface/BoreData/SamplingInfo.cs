using System.Collections.Generic;

namespace SmartGeoLogical.Render.DrawInterface.BoreData
{
    /// <summary>
    /// 采样信息，基类
    /// </summary>
    public class SamplingInfo
    {
        //采样名
        public string SampleName;
        public List<SampleValue> SampleValueList;

        public SamplingInfo(string sampleName)
        {
            this.SampleName = sampleName;
        }

        public SamplingInfo()
        {

        }
    }

    /// <summary>
    /// 点采样信息
    /// </summary>
    public class PointSamplingInfo:SamplingInfo
    {
        //深度
        public float Depth;
        public PointSamplingInfo(string sampleName, float depth, List<SampleValue> sampleValueList):base(sampleName)
        {
            this.Depth = depth;
            this.SampleValueList = sampleValueList;
        }

        public PointSamplingInfo()
        {
            this.SampleValueList = new List<SampleValue>();
        }
    }

    /// <summary>
    /// 区域采样信息
    /// </summary>
    public class AreaSamplingInfo:SamplingInfo
    {
        //起始深度，终止深度
        public float BeginDepth, EndDepth;
        public AreaSamplingInfo(string sampleName, float beginDepth,float endDepth, List<SampleValue> sampleValueList):base(sampleName)
        {
           
            this.SampleValueList = sampleValueList;
            this.BeginDepth = beginDepth;
            this.EndDepth = endDepth;
        }

        public AreaSamplingInfo()
        {
            this.SampleValueList = new List<SampleValue>();
        }
    }

    /// <summary>
    /// 采样值
    /// </summary>
    public class SampleValue
    {
        //采样值名称
        public string SamplingValueName;
        //采样类型
        public string SamplingType;
        //采样值
        public object SamplingValue;

        public SampleValue(string samplingValueName, string samplingType, object samplingValue)
        {
            this.SamplingType = samplingType;
            this.SamplingValueName = samplingValueName;
            this.SamplingValue = samplingValue;
        }

        public SampleValue()
        {

        }
    }
}
