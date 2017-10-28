using System.Collections.Generic;
using System.Text.RegularExpressions;
/**
 * 分析网页内容并提取指定内容的类
 * */
namespace FurryDownloader
{
    public class Analyze
    {
        /// <summary>
        /// 检查是否存在此用户名
        /// </summary>
        /// <param name="page">页面信息</param>
        /// <param name="name">作者名</param>
        /// <returns>存在则返回真</returns>
        public static bool checkName(string page, string name)
        {
            if (page.Contains("The username \"" + name + "\" could not be found."))
                return false;
            else return true;
        }

        /// <summary>
        /// 检查是否存在此页,存在则返回true
        /// </summary>
        /// <param name="page">页面信息</param>
        /// <returns>成功返回ok，失败返回错误信息</returns>
        public static State checkPage(string page)
        {
            if (page.Contains("There are no submissions to list"))
                return new State(StateCode.finish,"此页无任何图片");
            else if (page.Contains("has elected to make their content available to registered users only."))
                return new State(StateCode.error, "很抱歉，此页需要登录授权才可查看，请用IE浏览器打开http://www.furaffinity.net/并登录后重新下载。【提示：必须用IE登录才可以，如果依旧不成功，请给予本软件管理员权限】");
            else return new State(StateCode.ok);
        }

        /// <summary>
        /// 分析出当前页面里所有的详情页地址
        /// </summary>
        /// <param name="nowPage">下载好的页面信息</param>
        /// <returns>返回存储着所有页面地址的字符串数组</returns>
        public static string[] getNextPages(string nowPage)
        {
            List <string> pages = new List<string>();//一页最多48张图片

            //<a href="/view/23351569/">
            MatchCollection matchs = Regex.Matches(nowPage, @"<a href=""/view/\d+/"">");
            foreach (Match match in matchs)
            {
                pages.Add("http://www.furaffinity.net" + match.Value.Split('"')[1]);
            }

            return pages.ToArray();
        }

        /// <summary>
        /// 提取详情页内的原图下载地址
        /// </summary>
        /// <param name="page">下载好的详情页面信息</param>
        /// <returns>返回提取好的原图下载地址</returns>
        public static string getPictureUrl(string page)
        {
            //<a href="//d.facdn.net/art/rudragon/1493352182/1493352159.rudragon_lil_punk_by_phation-db7d67d.png">Download
            MatchCollection matchs = Regex.Matches(page, @"<a href=.*>Download");

            if (matchs.Count == 0)
                return null;
            else return "http:" + matchs[0].Value.Split('"')[1];
        }
        
        /// <summary>
        /// 提取原图下载地址内的文件名信息
        /// </summary>
        /// <param name="downloadUrl">图片下载地址</param>
        /// <returns>返回提取出的图片名</returns>
        public static string getFilename(string downloadUrl)
        {
            //http://d.facdn.net/art/rudragon/1493352182/1493352159.rudragon_lil_punk_by_phation-db7d67d.png
            string[] result = downloadUrl.Split('/');

            return result[result.Length - 1];
        }
    }
}
