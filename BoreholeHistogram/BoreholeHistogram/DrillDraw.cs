using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SmartGeoLogical.Core.DrillSection;
using SmartGeoLogical.Core.Entities;
using SmartGeoLogical.Core.ViewModels;
using SmartGeoLogical.Render.Bore.Sys;
using SmartGeoLogical.Render.DrillSectionDraw;
using SmartGeoLogical.Render.DrillSectionDraw.Draw;

namespace SmartGeoLogical.Render {
    public class DrillDraw : IDrillDraw {
        /// <summary>
        /// 绘制钻孔柱状图
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<byte[]> Draw(DrillModel model, DrawParameter parameter) {
            // 创建绘制区域
            try
            {
                var width = parameter.Width;
                var height = parameter.Height;
                var rect = new RectangleF(1, 1, width - 2, height - 2);
                using Bitmap bmp = new Bitmap(width, height);
                using Graphics graphics = Graphics.FromImage(bmp);
                MainControl mainControl = new MainControl();
                graphics.FillRectangle(new SolidBrush(Color.White), rect);

                
               await mainControl.Excute(model, parameter.Style, graphics, rect);

                using var ms = new MemoryStream();
                switch (parameter.Formart)
                {
                    case OutputFormart.Png:
                        bmp.Save(ms, ImageFormat.Png);
                        break;
                    case OutputFormart.Bmp:
                        bmp.Save(ms, ImageFormat.Bmp);
                        break;
                    case OutputFormart.Jpeg:
                        bmp.Save(ms, ImageFormat.Jpeg);
                        break;
                }
                var content = ms.ToArray();
                return content;
            }
            catch (Exception ex)
            {
                throw ex ;
            }
          
        }
        /// <summary>
        /// 绘制钻孔剖面
        /// </summary>
        /// <param name="models"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<byte[]> DrawDrillSection(IEnumerable<DrillModel> models, ProfileParameter parameter)
        {
            ProfileControl profileControl = GetProfileControl(models, parameter);

            using Bitmap bmp = new Bitmap((int)profileControl.drawScaling.Width, parameter.Height);
            using Graphics graphics = Graphics.FromImage(bmp);
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, (int)profileControl.drawScaling.Width, parameter.Height);

            await profileControl.Draw(graphics);
            using var ms = new MemoryStream();
            switch (parameter.Formart)
            {
                case OutputFormart.Png:
                    bmp.Save(ms, ImageFormat.Png);
                    break;
                case OutputFormart.Bmp:
                    bmp.Save(ms, ImageFormat.Bmp);
                    break;
                case OutputFormart.Jpeg:
                    bmp.Save(ms, ImageFormat.Jpeg);
                    break;
            }
            var content = ms.ToArray();
            return content;
        }
        /// <summary>
        /// 按照对比剖面生成虚拟钻孔
        /// </summary>
        /// <param name="models">钻孔信息</param>
        /// <param name="parameter">剖面参数</param>
        /// <param name="distance">垂点距离第一个钻孔点的距离</param>
        /// <returns></returns>
        public IList<StratumModel> VirtualDrillStratum(IEnumerable<DrillModel> models,  ProfileParameter parameter,double distance)
        {
            ProfileControl profileControl = GetProfileControl(models, parameter);
            VirtualDrill virtualDrill = new VirtualDrill(profileControl.logicSection.SectionPloygons, profileControl.drawScaling, profileControl.legends);
            var stratums = virtualDrill.GetVirtualDrill(distance);
            return stratums;
        }


        private ProfileControl GetProfileControl(IEnumerable<DrillModel> models, ProfileParameter parameter)
        {
            try
            {
                Dictionary<int, Lithology> lithList = new Dictionary<int, Lithology>();
                var drillList = models.ToList();
                var drillSectionGenner = new DrillSectionGen(drillList, lithList);

                LogicSection logicSection = drillSectionGenner.Run(parameter.SubdivideCount, parameter.pinchout, parameter.Texture);
                DrawScaling drawScaling = new DrawScaling();
                drawScaling.CalculateParm(parameter.Height, logicSection.SectionPloygons, parameter.ScaleXY, parameter.Texture, logicSection.AllList.Count);
                ProfileControl profileControl = new ProfileControl(logicSection, drillList, parameter, drawScaling);

               


                return profileControl;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
    }
}