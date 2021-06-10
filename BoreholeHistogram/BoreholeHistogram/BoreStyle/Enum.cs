namespace BoreholeHistogram.BoreStyle
{
    /// <summary>
    /// 文本文字绘制位置
    /// </summary>
    public enum AlignStyle
    {
        #region 相对矩阵绘制

        NearCenter = 1,   //中左
        CenterCenter = 2, //正中        
        FarCenter = 3,    //中右

        NearNear = 4,     //左上
        CenterNear = 5,   //中上
        FarNear = 6,      //右上

        NearFar = 7,      //左下
        CenterFar = 8,    //中下
        FarFar = 9,       //右下

        #endregion

        Point = 0,        //点位绘制
    }

    /// <summary>
    /// 文字角度，水平/垂直/倒垂直
    /// </summary>
    public enum AngleStyle
    {
        Horizontal = 1,
        Vertical = 2,
        ReverseHorizontal = 3
    }

    /// <summary>
    /// 标尺位置，左/右/两边
    /// </summary>
    public enum RulePosition
    {
        Left = 1,
        Right = 2,
        BothSides = 3
    }

    /// <summary>
    /// 填充模式，单填充/基于值的填充
    /// </summary>
    public enum FillMode
    {
        Single = 1,
        ValueBased = 2
    }

    

    /// <summary>
    /// 曲线标记样式，圆/三角形/矩形/六边形
    /// </summary>
    public enum SymbolStyle
    {
        Ellipse=1,
        Triangle=2,
        Rectangle=3,
        Hexangular=4
    }

    /// <summary>
    /// 曲线图样式，曲线，点线，填充，点图
    /// </summary>
    public enum CurveCatalog
    {
        Line=1,
        Point=2,
        LinePoint=3,
        Bar=4,
        Fill=5,
        FillPoint=6
    }
    public enum DirectionType
    {
        正东=1,
        东南= 2,
        正南 = 3,
        西南 = 4,
        正西 = 5,
        西北 = 6,
        正北 = 7,
        东北 = 8,
    }

}
