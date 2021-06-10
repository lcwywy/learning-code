namespace BoreholeHistogram.BoreStyle
{
    /// <summary>
    /// 对齐方式
    /// </summary>
    public enum AlignStyle
    {
        Left = 1,
        Center = 2,
        Right = 3,
        
        NearNear=4,
        NearCenter=5,
        NearFar = 6,
        CenterNear=7,
        CenterCenter = 8,
        CenterFar =9,

        FarNear =10,
        FarCenter = 11,
        FarFar = 12,


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
