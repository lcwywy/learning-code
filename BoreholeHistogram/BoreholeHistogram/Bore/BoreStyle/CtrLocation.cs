namespace SmartGeoLogical.Render.Bore.BoreStyle
{
    /// <summary>
    ///  控件相对于布局的位置信息
    /// </summary>
    public class CtrLocation
    {
        //行，列，行跨度，列跨度
        public int Row, Col, ColSpan, RowSpan;

        public CtrLocation(int row, int col, int colSpan, int rowSpan)
        {
            this.Row = row;
            this.Col = col;
            this.ColSpan = colSpan;
            this.RowSpan = rowSpan;
        }
    }
}
