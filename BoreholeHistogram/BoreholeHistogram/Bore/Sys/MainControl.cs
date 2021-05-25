using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SmartGeoLogical.Render.DrawInterface;
using SmartGeoLogical.Render.Bore.Column;
using SmartGeoLogical.Core.Entities;
using SmartGeoLogical.Core.ViewModels;
using SmartGeoLogical.Render.DrawInterface.BoreData;
using System.Threading.Tasks;
using System.IO;

namespace SmartGeoLogical.Render.Bore.Sys
{
    public class MainControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parentRect"></param>
        public MainControl( )
        {
        }

        /// <summary>
        /// 加载样式,返回一个StyleLoader对象
        /// </summary>
        /// <param name="styleFile"></param>
        public   Task<StyleLoader> LoadStyle(string styleFile)
        {
            return Task.Run(() =>
            {
                var styleLoad = new StyleLoader();
                styleLoad.Load(styleFile);
                return styleLoad;
            });
          
        }
        /// <summary>
        /// 执行绘制钻孔柱状图，把绘制图包装一下用以减少输入参数并采用异步为了加快执行速度
        /// </summary>
        /// <param name="drill">钻孔数据</param>
        /// <param name="style">样式文件名称</param>
        /// <param name="graphics">图片画布</param>
        /// <param name="parentRect">区域范围</param>
        /// <returns></returns>
        public async Task Excute(DrillModel drill, string style, Graphics  graphics, RectangleF parentRect)
        {

            //Linux和Windows的分隔符是不一样的
            var stylePath = AppDomain.CurrentDomain.BaseDirectory +
                                $"DrillSet{Path.DirectorySeparatorChar}style{Path.DirectorySeparatorChar}{style}.xml";
            var dataTask = ConvertToBoreData(drill);
            var styleLoaderTask = LoadStyle(stylePath);
            var tasks = new List<Task>() { dataTask, styleLoaderTask };
            await Task.WhenAll(tasks).ConfigureAwait(false);

            BoreData data = dataTask.Result;
            data.LegendPath = style;
            StyleLoader styleLoad = styleLoaderTask.Result;

            var drillGlobalTask = SetGlobalPage(data);
            DrillGlobal drillGlobal = await drillGlobalTask;
           await Draw(graphics, data, styleLoad, drillGlobal, parentRect);

        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="g"></param>
        public async Task Draw(Graphics g, BoreData data, StyleLoader styleLoad, DrillGlobal drillGlobal, RectangleF parentRect)
        {
            try
            {
                List<Control> ctrList = await GetControls(styleLoad, drillGlobal, parentRect);

                foreach (Control ctr1 in ctrList)
                {
                    try
                    {
                        ctr1.drillGlobal = drillGlobal;
                        ctr1.Draw(g, data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
           
        }

        /// <summary>
        /// 根据样式加载器，得到钻孔元素
        /// </summary>
        /// <param name="styleLoad"></param>
        /// <param name="drillGlobal"></param>
        /// <param name="parentRect"></param>
        /// <returns></returns>
        private Task<List<Control>> GetControls(StyleLoader styleLoad, DrillGlobal drillGlobal, RectangleF parentRect)
        {
            return Task.Run(() =>
            {
                var ctrList = new List<Control>();
                foreach (BoreStyle.Item item in styleLoad.Style.Items)
                    LoadControl(drillGlobal, ctrList, item, styleLoad.Style.BoreLayout, parentRect);
                return ctrList;
            });
           
        }

        /// <summary>
        /// 根据样式加载控件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parentLayout"></param>
        /// <param name="rect"></param>
        private void LoadControl(DrillGlobal drillGlobal, List<Control> ctrList, Bore.BoreStyle.Item item,BoreStyle.Layout parentLayout,RectangleF rect)
        {
            Control ctr = ControlFactory.CreateControl(drillGlobal,item, parentLayout, rect);
            ctrList.Add(ctr);
            foreach (BoreStyle.Item childItem in item.Items)
                LoadControl(drillGlobal, ctrList, childItem, item.ItemLayout,new RectangleF(ctr.Left,ctr.Top,ctr.Width,ctr.Height));
        }
        
        /// <summary>
        /// 转换钻孔视图模型为钻孔数据模型，返回一个BoreData对象
        /// </summary>
        /// <param name="drill"></param>
        /// <returns></returns>
        private  Task<BoreData>  ConvertToBoreData(DrillModel drill)
        {
            return Task.Run(() =>
            {
                BoreData boreData = new BoreData();
                boreData.BasicBoreInfo = GetBasicInfo(drill);
                boreData.UserDefinedBoreInfo = GetUserDefinedInfo(drill);
                boreData.LayerInfoList = GetLayerInfos(drill);
                boreData.SamplingInfoList = GetSamplingInfos(drill);
                return boreData;
            });
          
        }

        /// <summary>
        /// 地层信息读取
        /// </summary>
        /// <param name="drill"></param>
        /// <returns></returns>
        private List<LayerInfo> GetLayerInfos(DrillModel drill)
        {
            List<LayerInfo> layerInfos = new List<LayerInfo>();
            foreach (var item in drill.StratumList)
            {
                LayerInfo layerInfo = new LayerInfo();
                layerInfo.LithName = item.LithCode;
                layerInfo.LayerDescription = item.LithDescribe;
                layerInfo.LayerBottom = Convert.ToSingle(item.Height);
                layerInfo.ZUp = Convert.ToSingle(item.ZUp);
                layerInfo.ZDown = Convert.ToSingle(item.ZDown);
                layerInfos.Add(layerInfo);
            }
            return layerInfos;
        }
        /// <summary>
        /// 采样点信息读取
        /// </summary>
        /// <param name="drill"></param>
        /// <returns></returns>
        private List<SamplingInfo> GetSamplingInfos(DrillModel drill)
        {
            List<SamplingInfo> samplingInfos = new List<SamplingInfo>();
            foreach (SampleModel sample in drill.SampleList)
            {
                SamplingInfo samplingInfo = null;
                if (sample.Type == Core.SampleType.Point)
                {
                    PointSamplingInfo pointSamplingInfo = new PointSamplingInfo();
                    samplingInfo = pointSamplingInfo;
                    pointSamplingInfo.Depth = Convert.ToSingle(sample.HeightDown);
                    
                }
                else
                {
                    AreaSamplingInfo areaSamplingInfo = new AreaSamplingInfo();
                    samplingInfo = areaSamplingInfo;
                    areaSamplingInfo.BeginDepth = Convert.ToSingle(sample.HeightUp);
                    areaSamplingInfo.EndDepth = Convert.ToSingle(sample.HeightDown);
                }

                samplingInfo.SampleName = sample.SampleId;
                samplingInfo.SampleValueList = new List<SampleValue>();
                sample.Properties.ToList().ForEach(delegate (PropertyModel property)
                {
                    SampleValue sampleValue = new SampleValue();
                    sampleValue.SamplingValueName = property.PropertyName;
                    sampleValue.SamplingValue = property.PropertyValue;
                    sampleValue.SamplingType = property.PropertyType;
                    samplingInfo.SampleValueList.Add(sampleValue);
                });
                samplingInfos.Add(samplingInfo);

            }
            return samplingInfos;
        }

        /// <summary>
        /// BasicInfo信息读取
        /// </summary>
        /// <param name="drill"></param>
        /// <returns></returns>
        private BasicInfo GetBasicInfo(DrillModel drill)
        {
            BasicInfo basicInfo = new BasicInfo();
            basicInfo.钻孔编号 = drill.Id;
            basicInfo.DrillX = Convert.ToSingle(drill.X);
            basicInfo.DrillY = Convert.ToSingle(drill.Y);
            basicInfo.DrillZ = Convert.ToSingle(drill.H);
            if (drill.StratumList!=null&& drill.StratumList.Count>0)
            {
                basicInfo.totalDepth = Convert.ToSingle(drill.StratumList[drill.StratumList.Count - 1].Height);
            }
            else
            {
                basicInfo.totalDepth = Convert.ToSingle(drill.Depth);
            }
            basicInfo.WaterLevel = drill.WaterLevel;
            return basicInfo;
        }
        /// <summary>
        /// 用户自定义信息
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        private UserDefinedInfo GetUserDefinedInfo(DrillModel drill)
        {
            
            UserDefinedInfo userDefinedInfo = new UserDefinedInfo();
            userDefinedInfo.UserDefinedList = new List<UserDefined>();
            foreach (var kv in drill.Extensions)
            {
                UserDefined userDefined = new UserDefined();
                userDefined.UName = kv.Key;
                userDefined.Value = kv.Value;
                userDefinedInfo.UserDefinedList.Add(userDefined);
            }
            return userDefinedInfo;
        }
        /// <summary>
        /// 设置默认页面设置：总页数1,返回一个DrillGlobal对象
        /// </summary>
        private  async  Task<DrillGlobal> SetGlobalPage(BoreData data)
        {
            return  await Task.Run(() =>
            {
                DrillGlobal global = new DrillGlobal();

                if (global.depthPerPage == 0) //没设置页面深度，设置为总深度
                global.depthPerPage = data.BasicBoreInfo.totalDepth;


                if (global.currentPage == 0) //未设置页数，从1开始
                global.currentPage = 1;
                global.totalDepth = data.BasicBoreInfo.totalDepth;
                global.totalPage = Math.Abs((int)global.totalDepth / (int)global.depthPerPage);
                if ((int)global.totalDepth % (int)global.depthPerPage != 0)
                    global.totalPage += 1;
                return global;
            });
            
        }

    }
}
