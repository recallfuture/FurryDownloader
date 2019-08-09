
using System.Windows.Forms;

namespace FurryDownloader
{
    public partial class FaLogin : Form
    {
        // 登录后的身份cookie
        public string Cookie = "";

        public FaLogin()
        {
            InitializeComponent();
            WebBrowser.Navigated += WebBrowser_Navigated;
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // 获取被httponly保护的cookie
            string cookie = FullWebBrowserCookie.GetCookieInternal(WebBrowser.Url, false);
            
            if (cookie.Contains("__cfduid"))
            {
                Cookie = cookie;
                WebBrowser.Dispose();
                this.Close();
            }
        }
    }
}
