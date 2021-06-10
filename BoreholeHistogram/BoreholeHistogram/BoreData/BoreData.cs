using System.Collections.Generic;
using System.IO;

namespace BoreholeHistogram.BoreData
{
    /// <summary>
    /// 数据模型
    /// </summary>
    public class BoreData
    {
        public BasicInfo BasicBoreInfo;
        public UserDefinedInfo UserDefinedBoreInfo;
        public List<LayerInfo> LayerInfoList;
        public List<SamplingInfo> SamplingInfoList;
        public string LegendPath;
        public BoreData(BasicInfo basicBoreInfo, UserDefinedInfo userDefinedBoreInfo, List<LayerInfo> layerInfoList, List<SamplingInfo> samplingInfoList)
        {
            this.BasicBoreInfo = basicBoreInfo;
            this.UserDefinedBoreInfo = userDefinedBoreInfo;
            this.LayerInfoList = layerInfoList;
            this.SamplingInfoList = samplingInfoList;
            this.BasicBoreInfo.totalDepth = layerInfoList[layerInfoList.Count - 1].LayerBottom;//同步完井深度与地层底界
        }

        public BoreData()
        {
            this.BasicBoreInfo = new BasicInfo();
            this.UserDefinedBoreInfo = new UserDefinedInfo();
            this.LayerInfoList = new List<LayerInfo>();
            this.SamplingInfoList = new List<SamplingInfo>();
        }

        public List<SamplingInfo> GetSamplingDataByName(string name)
        {
            List<SamplingInfo> sampleInfos=new List<SamplingInfo>();
            foreach (SamplingInfo sampling in SamplingInfoList)
            {
                if(sampling is PointSamplingInfo) //Text,Curve只针对点采样信息
                {
                    foreach(SampleValue sValue in ((PointSamplingInfo)sampling).SampleValueList)
                    {
                        if(sValue.SamplingValueName==name)
                        {
                            List<SampleValue> valueList=new List<SampleValue>();
                            valueList.Add(sValue);
                            sampleInfos.Add(new PointSamplingInfo(sampling.SampleName, ((PointSamplingInfo)sampling).Depth, valueList));//重新构造一个SamplingInfo
                        }
                    }
                }
                if (sampling is AreaSamplingInfo) //
                {
                    foreach (SampleValue sValue in ((AreaSamplingInfo)sampling).SampleValueList)
                    {
                        if (sValue.SamplingValueName == name)
                        {
                            List<SampleValue> valueList = new List<SampleValue>();
                            valueList.Add(sValue);
                            sampleInfos.Add(new AreaSamplingInfo(sampling.SampleName, ((AreaSamplingInfo)sampling).BeginDepth, ((AreaSamplingInfo)sampling).EndDepth, valueList));//重新构造一个SamplingInfo
                        }
                    }
                }
            }
            return sampleInfos;
        }
    }
}
