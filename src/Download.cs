using System;
using System.IO;
using System.Text;

/*
 * 下载网页或图片的类
 * */
namespace FurryDownloader
{
    class Request
    {
        private static HttpHelper http = new HttpHelper();

        /// <summary>
        /// 下载所给网址指向的页面信息
        /// </summary>
        /// <param name="strUrl">网址</param>
        /// <returns>成功则返回页面信息，失败返回null</returns>
        public static State GetGeneralContent(string strUrl, string cookie)
        {
            try
            {
                HttpItem item = new HttpItem()
                {
                    URL = strUrl,//URL     必需项
                    Encoding = Encoding.UTF8,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                                             //Encoding = Encoding.Default,
                    Method = "get",//URL     可选项 默认为Get
                    Timeout = 10000,//连接超时时间     可选项默认为100000
                    IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                    Cookie = cookie,//字符串Cookie     可选项
                    UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
                    Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
                    ContentType = "text/html",//返回类型    可选项有默认值
                    Referer = "http://www.furaffinity.net",//来源URL     可选项
                    ResultType = ResultType.String,//返回数据类型，是Byte还是String
                    // ProxyIp = "127.0.0.1:1081",
                };
                //得到HTML代码
                HttpResult result = http.GetHtml(item);
                //返回的Html内容
                string html = result.Html;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new State(StateCode.ok, html);
                }
                else return new State(StateCode.error, "网络错误");
            }
            catch (Exception e)
            {
                return new State(StateCode.error, e.Message);
            }
        }
        /// <summary>
        /// 下载并保存图片
        /// </summary>
        /// <param name="strUrl">下载地址</param>
        /// <param name="indexName">存储目录</param>
        /// <param name="fileName">文件名</param>
        /// <returns>成功则返回null，失败则返回失败信息</returns>
        public static State GetFileContent(string strUrl,string indexName, string fileName)
        {
            try
            {
                //创建存放文件夹
                if (Directory.Exists(indexName) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(indexName);
                }
                else
                {
                    //如果已经存在则不再下载
                    if (File.Exists(indexName + fileName))
                        return null;
                    else//提前占位
                    {
                        FileStream fs = File.Create(indexName + fileName);
                        fs.Close();
                    }
                }

                HttpItem item = new HttpItem()
                {
                    URL = strUrl,//URL     必需项
                    Method = "get",//URL     可选项 默认为Get
                    Timeout = 10000,//连接超时时间     可选项默认为100000
                    IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                    UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
                    Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
                    ContentType = "*/*",//返回类型    可选项有默认值
                    Referer = "http://www.furaffinity.net",//来源URL     可选项
                    ResultType = ResultType.Byte,//返回数据类型，是Byte还是String
                    // ProxyIp = "127.0.0.1:1081",
                };
                //得到HTML代码
                HttpResult result = http.GetHtml(item);
                //存储返回的文件内容
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    File.WriteAllBytes(indexName + fileName, result.ResultByte);
                    return new State(StateCode.ok);
                }
                return new State(StateCode.error, "网络错误");
            }
            catch(Exception e)
            {
                return new State(StateCode.error, e.Message);
            }
        }
    }
}
