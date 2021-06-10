using System.Collections;
using System.Drawing;
using System.Threading.Tasks;

namespace BoreholeHistogram.Control
{
    /// <summary>
    /// 绘制接口
    /// </summary>
    public interface IDraw
    {
        /// <summary>
        /// 实现绘制功能
        /// </summary>
        /// <param name="g"></param>
        /// <param name="data"></param>
        void Draw( Graphics g,BoreData.BoreData data);

        /// <summary>
        /// 未知
        /// </summary>
        /// <param name="list"></param>
        void SetParameters(IList list);
    }
}
