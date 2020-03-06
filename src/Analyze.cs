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
        /// <param name="content"></param>
        /// <returns>存在则返回真</returns>
        public static bool HasUser(string content)
        {
            return !Regex.IsMatch(content, @"The username ""\w+"" could not be found.");
        }

        /// <summary>
        /// 检查是否需要登录后查看
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool NeedLogin(string content)
        {
            return !content.Contains("has elected to make their content available to registered users only.");
        }

        /// <summary>
        /// 分析出当前页面里所有的详情页地址
        /// </summary>
        /// <param name="content">下载好的页面信息</param>
        /// <returns>返回存储着所有页面地址的字符串列表</returns>
        public static List<string> GetPages(string content)
        {
            List<string> pages = new List<string>();

            // <a href="/view/23351569/">
            MatchCollection matchs = Regex.Matches(content, @"<a href=""/view/\d+/"">");
            foreach (Match match in matchs)
            {
                pages.Add("https://www.furaffinity.net" + match.Value.Split('"')[1]);
            }

            return pages;
        }

        /// <summary>
        /// 提取详情页内的原图下载地址
        /// </summary>
        /// <param name="page">下载好的详情页面信息</param>
        /// <returns>返回提取好的原图下载地址</returns>
        public static string GetPictureUrl(string page)
        {
            // <a href="//d.facdn.net/art/rudragon/1493352182/1493352159.rudragon_lil_punk_by_phation-db7d67d.png">Download
            MatchCollection matchs = Regex.Matches(page, @"href="".*?"".*?>Download");

            if (matchs.Count == 0)
                return null;
            else return "http:" + matchs[0].Value.Split('"')[1];
        }

        /// <summary>
        /// 提取原图下载地址内的文件名信息
        /// </summary>
        /// <param name="downloadUrl">图片下载地址</param>
        /// <returns>返回提取出的图片名</returns>
        public static string GetFileName(string downloadUrl)
        {
            // https://d.facdn.net/art/rudragon/1493352182/1493352159.rudragon_lil_punk_by_phation-db7d67d.png
            string[] result = downloadUrl.Split('/');

            return result[result.Length - 1];
        }
    }
}
