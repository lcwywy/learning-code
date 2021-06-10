namespace BoreholeHistogram.BoreData
{
    public class BasicInfo
    {
        //钻孔编号
        public string 钻孔编号;
        //横坐标，纵坐标，高程，完井深度
        public float DrillX;
        public float DrillY;
        public float DrillZ;
        public float totalDepth;
        public double WaterLevel;

        public BasicInfo(string boreId, float x, float y, float elevation, float totalDepth, double waterLevel)
        {
            this.钻孔编号 = boreId;
            this.DrillX = x;
            this.DrillY = y;
            this.DrillZ = elevation;
            this.totalDepth = totalDepth;
            this.WaterLevel = waterLevel;
        }

        public BasicInfo()
        {

        }
    }
}
