using System.Collections.Generic;
using System.Threading.Tasks;
using SmartGeoLogical.Core.ViewModels;

namespace SmartGeoLogical.Render {
    public interface IDrillDraw {
        /// <summary>
        /// 根据钻孔信息绘制图片
        /// </summary>
         Task<byte[]> Draw(DrillModel model, DrawParameter parameter);

        /// <summary>
        /// 绘制钻孔对比剖面图
        /// </summary>
        /// <param name="models"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<byte[]> DrawDrillSection(IEnumerable<DrillModel> models, ProfileParameter parameter);

        /// <summary>
        /// 根据钻孔对比剖面图生成虚拟钻孔的地层信息
        /// </summary>
        /// <param name="models"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        IList<StratumModel> VirtualDrillStratum(IEnumerable<DrillModel> models, ProfileParameter parameter, double distance);

    }
}