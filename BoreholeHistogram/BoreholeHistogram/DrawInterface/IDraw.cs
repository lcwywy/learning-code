using System.Collections;
using System.Drawing;
using System.Threading.Tasks;

namespace SmartGeoLogical.Render.DrawInterface
{
    /// <summary>
    /// 绘制接口
    /// </summary>
    public interface IDraw
    {
        void Draw( Graphics g,BoreData.BoreData data);
        void SetParameters(IList list);
    }
}
