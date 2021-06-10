using System.Collections.Generic;

namespace BoreholeHistogram.BoreData
{
    /// <summary>
    /// 用户自定义信息单元
    /// </summary>
    public class UserDefined
    {
        public string UName;
        public object Value;

        public UserDefined()
        {

        }
        public UserDefined(string uName, object value)
        {
            this.UName = uName;
            this.Value = value;
        }
    }

    /// <summary>
    /// 用户自定义信息
    /// </summary>
    public class UserDefinedInfo
    {
        public List<UserDefined> UserDefinedList;
        public UserDefinedInfo(List<UserDefined> userDefinedList)
        {
            this.UserDefinedList = userDefinedList;
        }

        public UserDefinedInfo()
        {

        }


    }
}
