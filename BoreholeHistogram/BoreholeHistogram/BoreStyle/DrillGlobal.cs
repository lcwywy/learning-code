namespace BoreholeHistogram.BoreStyle
{
    /// <summary>
    /// 全局变量，绘制的状态如页深、当前页等
    /// </summary>
    public class DrillGlobal
    {
        /// <summary>
        /// 每页的深度
        /// </summary>
        public  float depthPerPage=0;         
        /// <summary>
       /// 当前页
       /// </summary>
        public  int currentPage=0;            
        /// <summary>
        /// 总页数
        /// </summary>
        public  int totalPage=1;            //总页数      
        /// <summary>
        /// 总深度
        /// </summary>
        public  float totalDepth=10;        // 总深度  
        /// <summary>
        /// 控件开始位置，初设为1，为鼠标移动事件的位置显示做基础
        /// </summary>
        public  float columnBeginDepth = 0;    
        /// <summary>
        /// 控件高度，初始设为0，为鼠标移动事件的位置显示做基础
        /// </summary>
        public float columnHeight = 0;        
    }
}
