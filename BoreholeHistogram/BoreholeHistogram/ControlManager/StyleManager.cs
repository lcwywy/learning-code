using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BoreholeHistogram;

namespace BoreholeHistogram.ControlManager
{
    class StyleManager
    {

        public void CreateStyle() //新建一个样式
        { }

        public XmlDocument OpenStyle(string DataFile)  //打开一个样式,目前为XML文件
        {
            //加载文档
            XmlImplementation xml = new XmlImplementation();
            XmlDocument document = xml.CreateDocument();
            document.Load(DataFile);

            return document;
        }

        public void SaveStyle()  //保存样式
        { }
    }
}
