using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SmartGeoLogical.Render
{
    public static class ObjectExpend
    {
        /// <summary>
        /// 读取文本，计算换行
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="s"></param>
        /// <param name="font"></param>
        /// <param name="brush"></param>
        /// <param name="rectangleF"></param>
        /// <param name="format"></param>
        public static void  DrawString_2(this Graphics graphic ,string s, Font font, Brush brush, RectangleF rectangleF, StringFormat format)
        {
            try
            {
                //如果文字中包含换行符，则不进行计算
                if (s.Contains(@"\r\n"))
                {
                    s = s.Replace(@"\r\n", System.Environment.NewLine);
                    graphic.DrawString(s,font,brush,rectangleF,format);
                    return;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    SizeF sizeF = graphic.MeasureString(s,font);
                    if (rectangleF.Width<sizeF.Width)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        int currentIndex = 0;
                        int len = 1;
                        for (int i = 0; i < s.Length; i++)
                        {
                            string tempStr = s.Substring(currentIndex, len);
                            sizeF = graphic.MeasureString(tempStr,font);
                            if (sizeF.Width>rectangleF.Width&&len>1)
                            {
                                if (len>1)
                                {
                                    string tempStr1 = s.Substring(currentIndex, len - 1);
                                    stringBuilder.Append(tempStr1);
                                    stringBuilder.Append(System.Environment.NewLine);
                                }
                                string tempStr2 = s.Substring(currentIndex+len-1,  1);
                                stringBuilder.Append(tempStr2);
                                currentIndex = i+1;
                                len = 1;
                            }
                            else if (i== s.Length-1)
                            {
                                stringBuilder.Append(tempStr);
                            }
                            else
                            {
                                len++;
                            }    
                        }

                        s = stringBuilder.ToString();

                    }
                } 
                SizeF sizeFcs = graphic.MeasureString(s,font);

                graphic.DrawString(s,font,brush,rectangleF,format);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        
    }
}