using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

/*
 * 下载网页或图片的类
 * */
namespace FurryDownloader
{
    class Download
    {
        public static List<string[]> result;
        /// <summary>
        /// 下载所给网址指向的页面信息
        /// </summary>
        /// <param name="strUrl">网址</param>
        /// <returns>成功则返回页面信息，失败返回null</returns>
        public static string GetGeneralContent(string strUrl)
        {
            string strMsg = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.CookieContainer = SetCookies();
                
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                strMsg = reader.ReadToEnd();

                reader.Close();
                response.Close();
            }
            catch
            {
                strMsg = null;
            }
            return strMsg;
        }

        /// <summary>
        /// 下载并保存图片
        /// </summary>
        /// <param name="strUrl">下载地址</param>
        /// <param name="indexName">存储目录</param>
        /// <param name="fileName">文件名</param>
        /// <returns>成功则返回null，失败则返回失败信息</returns>
        public static string GetFileContent(string strUrl,string indexName, string fileName)
        {
            try
            {
                //创建存放文件夹
                if (Directory.Exists(indexName) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(indexName);
                }
                //如果已经存在则不再下载
                if (File.Exists(indexName + fileName))
                    return null;
                else//提前占位
                {
                    FileStream fs = File.Create(indexName + fileName);
                    fs.Close();
                }


                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                Stream reader = response.GetResponseStream();

                //可根据实际保存为具体文件
                FileStream writer = new FileStream(indexName + fileName, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[10240];
                int c = 0; //实际读取的字节数 
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                writer.Close();

                reader.Close();
                response.Close();

                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static CookieContainer SetCookies()
        {
            CookieContainer cc = new CookieContainer();
            for (int i = 0; i < result.Count; ++i)
            {
                cc.Add(new System.Uri("http://www.furaffinity.net"), new Cookie(result[i][0], result[i][1]));
            }
            return cc;
        }
    }
}
