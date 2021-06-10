using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BoreholeHistogram;

namespace BoreholeHistogram.ControlManager
{
    class StyleManager
    {
        private Bore.BoreStyle.BoreStyle borestyle;

        public void CreateStyle() //新建一个样式
        { }

        public void OpenStyle(string DataFile)  //打开一个样式,目前为XML文件
        {
            //加载文档获取根节点
            XmlImplementation xml = new XmlImplementation();
            XmlDocument document = xml.CreateDocument();
            document.Load(DataFile);
            XmlNode root = document.FirstChild;

            //新建样式类并开始读取XML文件样式
            BoreStyle.BoreStyle style = new BoreStyle.BoreStyle();

        }

        public void SaveStyle()  //保存样式
        { }
    }
}
