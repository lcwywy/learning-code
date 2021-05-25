namespace SmartGeoLogical.Render.Bore.Column
{
    public class Parameter
    {
        public string key;
        public object value;

        public Parameter()
        {
        }

        public Parameter(string key, object obj)
        {
            this.key = key;
            this.value = obj;
        }
    }
}
