using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FurryDownloader
{
    public partial class mainFrom : Form
    {
        //基本参数
        private string userName;            // 作者名
        private string filePath;            // 下载存放目录
        private string cookie = "";         // cookie

        //高级参数
        private int startPageNum = 1;       // 下载起始页的页号，默认1
        private int startPicNum = 1;        // 从当前页的第几张开始下载，默认1

        private int totalDownloadNum = 0;   // 已下载的图片个数
        private int skipDownloadNum = 0;    // 已跳过下载的图片个数

        private Thread workerThread;        // 主循环线程
        private DateTime startTime;         // 开始下载前的时间

        private bool isStop = false;        // 是否停止下载

        private HttpHelper http = new HttpHelper();
        private string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\FaDownloader\\";

        public mainFrom()
        {
            InitializeComponent();
            LoadCookie();
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dir"></param>
        private void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }


        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        private void CreateFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }


        #region cookie文件函数
        /// <summary>
        /// 读取cookie
        /// </summary>
        private void LoadCookie()
        {
            string cookiePath = homeDir + "cookie.txt";
            CreateDir(homeDir);

            //如果已经存在cookie文件则读取
            if (File.Exists(cookiePath))
            {
                cookie = File.ReadAllText(cookiePath);
            }
        }
        /// <summary>
        /// 保存cookie
        /// </summary>
        private void SaveCookie()
        {
            //为空则不存储
            if (cookie == "")
                return;

            // 创建并写入
            string path = homeDir + "cookie.txt";
            CreateDir(homeDir);
            CreateFile(path);
            File.WriteAllText(path, cookie);
        }
        #endregion
        #region 操作控件的工具类
        delegate void TextBoxDelegate(string str);
        /// <summary>在textBox中追加信息，因为在其他线程无法操作主线程的控件，所以需要用这种方法<</summary>
        /// <param name="str">要追加的信息</param>
        public void AddItemToTextBox(string str)
        {
            try
            {
                if (textBox.InvokeRequired)
                {
                    TextBoxDelegate d = AddItemToTextBox;
                    textBox.Invoke(d, str);
                }
                else
                {
                    textBox.AppendText(str + "\r\n");
                }
            }
            catch
            {

            }
        }
        delegate void FreezeDelegate();
        /// <summary>
        /// 冻结
        /// </summary>
        public void Freeze()
        {
            try
            {
                if (ButtonEnter.InvokeRequired)
                {
                    FreezeDelegate d = Freeze;
                    ButtonEnter.Invoke(d);
                }
                else
                {
                    //冻结所有控件
                    ButtonEnter.Enabled = false;
                    ButtonCancle.Enabled = true;
                    input_name.Enabled = false;
                    RadioButtonGallery.Enabled = RadioButtonScraps.Enabled = false;
                    Browse.Enabled = false;
                    InputPageNum.Enabled = InputStartPicNum.Enabled = false;
                }
            }
            catch
            {

            }
        }

        delegate void UnFreezeDelegate();
        /// <summary>
        /// 解冻
        /// </summary>
        public void UnFreeze()
        {
            try
            {
                if (ButtonEnter.InvokeRequired)
                {
                    UnFreezeDelegate d = UnFreeze;
                    ButtonEnter.Invoke(d);
                }
                else
                {
                    //解冻所有控件
                    ButtonEnter.Enabled = true;
                    ButtonCancle.Enabled = false;
                    input_name.Enabled = true;
                    RadioButtonGallery.Enabled = RadioButtonScraps.Enabled = true;
                    Browse.Enabled = true;
                    InputPageNum.Enabled = InputStartPicNum.Enabled = true;
                }
            }
            catch
            {

            }
        }
        #endregion
        #region 按钮回调

        /// <summary>
        /// 确定按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            isStop = false;

            //开始下载
            workerThread = new Thread(DoDownload);
            workerThread.Start();
        }

        /// <summary>
        /// 取消按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancle_Click(object sender, EventArgs e)
        {
            isStop = true;
        }

        /// <summary>
        /// 浏览按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = folderBrowserDialog.SelectedPath;
                FilePath.Text = filePath;
            }
        }

        /// <summary>
        /// 登录fa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            FaLogin faLogin = new FaLogin();
            faLogin.ShowDialog();
            if (faLogin.Cookie != null && faLogin.Cookie != "")
            {
                cookie = faLogin.Cookie;
                SaveCookie();
                MessageBox.Show("登录成功，已保存身份信息以供下次使用");
            }
            faLogin.Dispose();
        }

        /// <summary>
        /// 帮助按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Click(object sender, EventArgs e)
        {
            string help = "本软件用于批量下载fa站图片\r\n" +
                        "请保证网络连接通畅\r\n" +
                        "必要时请给于软件管理员权限\r\n" +
                        "下载图片不成功时，请开启vpn后重新下载\r\n" +
                        "本软件仅供交流，请勿用于商业用途\r\n";

            MessageBox.Show(help, "注意事项");
        }
        #endregion
        #region 初始化函数和收尾函数

        /// <summary>
        /// 获取用户提供的数据
        /// </summary>
        private void GetUserInput()
        {
            // 获取画师名
            userName = input_name.Text.Trim().Replace("_", "");

            // 获取下载目录
            filePath = FilePath.Text.Length == 0
                ? String.Format(".\\{0}\\", userName)
                : String.Format("{0}\\{1}\\", FilePath.Text, userName);

            // 处理高级选项参数
            string inputPage = InputPageNum.Text.Trim();
            string inputStartPic = InputStartPicNum.Text.Trim();

            // 起始页数，默认值为1
            if (!int.TryParse(inputPage, out startPageNum) || startPageNum < 1)
                this.startPageNum = 1;

            // 从第几张开始下载，默认值为1
            if (!int.TryParse(inputStartPic, out startPicNum) || startPicNum < 1)
                this.startPicNum = 1;

            // 输出高级参数下载信息
            AddItemToTextBox(string.Format("从第{0}页第{1}张开始下载\r\n", this.startPageNum, this.startPicNum));
        }

        /// <summary>
        /// 下载前执行
        /// </summary>
        private void BeforeDownload()
        {
            // 冻结所有控件
            Freeze();

            // 修改回车焦点
            this.AcceptButton = this.ButtonCancle;

            // 保存cookie
            SaveCookie();

            // 下载总数和跳过总数清零
            totalDownloadNum = 0;
            skipDownloadNum = 0;

            // 初始化开始时间
            startTime = DateTime.Now;

            // 设置最大线程数
            DownloadManager.SetMaxThreads(16);
        }

        /// <summary>
        /// 结束下载时执行
        /// </summary>
        private void EndDownload()
        {
            // 解冻所有控件
            UnFreeze();

            DateTime now = DateTime.Now;// 下载结束后的时间
            TimeSpan time = now - startTime;// 总下载时间
            AddItemToTextBox(String.Format("共{0}小时{1}分钟{2}秒", time.Hours, time.Minutes, time.Seconds + "秒"));
            AddItemToTextBox(String.Format("下载了{0}张，跳过了{1}张", totalDownloadNum, skipDownloadNum));

            // 修改回车焦点
            this.AcceptButton = this.ButtonEnter;
        }
        #endregion
        #region 下载相关
        /// <summary>
        /// 下载勾选上的图集
        /// </summary>
        private void DoDownload()
        {
            // 检查必须的信息
            if (input_name.Text == "")
            {
                MessageBox.Show("作者名不能为空！");
                return;
            }

            BeforeDownload();

            try
            {
                if (RadioButtonGallery.Checked)
                {
                    download("gallery\\");
                }
                else if (RadioButtonScraps.Checked)
                {
                    download("scraps\\");
                }

                DownloadManager.FinishAll();
            }
            catch (Exception e)
            {
                AddItemToTextBox(e.Message);
                DownloadManager.StopAll();
            }

            EndDownload();
        }

        /// <summary>
        /// 下载图集
        /// </summary>
        /// <param name="type">gallery或者scraps</param>
        private void download(string type)
        {
            AddItemToTextBox(type + "下载开始");
            GetUserInput();

            // 循环下载第pageNum页
            while (true)
            {
                // 存储当前页面地址
                string nowUrl = string.Format("http://www.furaffinity.net/{0}/{1}/{2}",
                                                type,
                                                userName,
                                                startPageNum);

                // 下载当前页
                string html = GetHtml(nowUrl);

                // 检查页面是否下载成功
                if (html == null || html == "")
                    throw new Exception("请检查网络");

                // 检查此用户是否存在
                if (!Analyze.HasUser(html))
                    throw new Exception("未查询到该作者，请检查作者名称");

                // 检查此页是否还有图片，没有的话输出错误日志
                if (!Analyze.HasPicture(html))
                    throw new Exception("需要登陆后才能下载此作者的作品");

                //--------开始详情页下载循环------------

                // 获得所有详情页
                List<String> pages = Analyze.GetPages(html);

                for (int i = startPicNum - 1; i < pages.Count; ++i)
                {
                    // 停止时不再添加新的任务
                    if (isStop)
                    {
                        throw new Exception("正在结束下载");
                    }
                    downloadPicture(pages[i], type);
                }
                startPicNum = 1;
                startPageNum++;
            }
        }

        /// <summary>
        /// 根据详情页url下载此页面，解析其中的图片地址并下载
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="type"></param>
        private void downloadPicture(string pageUrl, string type)
        {
            // 获取缓存路径
            string[] arr = pageUrl.Split('/');
            string pageId = arr[arr.Length - 2];

            string cacheDir = homeDir + "cache\\";
            CreateDir(cacheDir);
            string cacheFile = cacheDir + pageId;
            string pictureUrl = null;

            // 从缓存中获取地址
            if (File.Exists(cacheFile))
            {
                pictureUrl = File.ReadAllText(cacheFile);
                if (pictureUrl == "")
                {
                    pictureUrl = null;
                }
            }

            if (pictureUrl == null)
            {
                //获取详情页面信息
                string html = GetHtml(pageUrl);

                // 检查页面是否下载成功
                if (html == null || html == "")
                    throw new Exception("请检查网络");

                //获取图片下载地址
                pictureUrl = Analyze.GetPictureUrl(html);
                if (pictureUrl == null)
                    throw new Exception("无法分析出图片地址：" + pageUrl);

                // 缓存下来图片地址
                CreateFile(cacheFile);
                File.WriteAllText(cacheFile, pictureUrl);
            }

            // 获取文件名字
            string pictureName = Analyze.GetFileName(pictureUrl);

            // 文件完整路径
            string fullFilePath = filePath + type + pictureName;

            // 判断文件是否已经存在
            if (File.Exists(fullFilePath))
            {
                AddItemToTextBox(String.Format("第{0}页第{1}张已存在，跳过", startPageNum, startPicNum));
                startPicNum++;
                skipDownloadNum++;
                return;
            }

            // 使用多线程下载
            Task task = new Task()
            {
                pageNum = startPageNum,
                picNum = startPicNum,

                Url = pictureUrl,
                FilePath = filePath + type,
                FileName = pictureName,
                TaskStart = new Task.TaskStartDelegate(delegate (int id, Task t)
                {
                    // 下载开始
                }),
                TaskStop = new Task.TaskStopDelegate(delegate (int id, Task t)
                {
                    // AddItemToTextBox("下载被终止" + t.FileName);
                }),
                TaskFinish = new Task.TaskFinishDelegate(delegate (int id, Task t)
                {
                    totalDownloadNum++;

                    // 计算下载所用时间
                    DateTime now = DateTime.Now;
                    TimeSpan time = now - startTime;

                    AddItemToTextBox(String.Format("第{0}页第{1}张下载完成，已经过{2}秒", t.pageNum, t.picNum, time.TotalSeconds));
                }),
                TaskFail = new Task.TaskFailDelegate(delegate (int id, Task t)
                {
                    AddItemToTextBox(String.Format("第{0}页第{1}张下载失败", t.pageNum, t.picNum));
                }),
            };
            DownloadManager.Add(task);
            startPicNum++;
        }

        /// <summary>
        /// 下载所给网址指向的页面信息
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns>页面信息</returns>
        public string GetHtml(string url)
        {
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项
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

            // 返回html
            return http.GetHtml(item).Html;
        }
        #endregion
    }
}
