using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace FurryDownloader
{
    public partial class mainFrom : Form
    {
        private string userName;//作者名
        private int startPageNum = 1;//下载起始页的页号，默认1
        private int startPicNum = 1;//从当前页的第几张开始下载，默认1
        private int maxDownloadNum = 0;//默认下载总量，默认0，0为不限制
        private int fullDownloadNum = 0;//已下载的图片个数
        private string filePath;//下载存放目录
        private string cookie;//cookie

        DateTime beforeDownload;//开始下载前的时间

        private Thread newThread;//主循环线程

        public mainFrom()
        {
            InitializeComponent();
            loadCookie();
        }
        //主循环函数，在单独的线程执行，否则会堵塞主线程
        private void loop()
        {
            bool isfinish = true;//判定是否成功下载

            if (checkBox1.Checked)
            {
                isfinish = isfinish && download("gallery");//如果下载时出错，则isfinish为false
            }

            if (checkBox2.Checked && isfinish)//只有前面没出错，后面才能执行
            {
                isfinish = isfinish && download("scraps");
            }

            if (isfinish)//如果成功下载
            {
                AddItemToTextBox("下载完成,一共下载了" + fullDownloadNum + "张图片\r\n");
                MessageBox.Show("下载完成");
            }
            else
            {
                AddItemToTextBox("下载结束\r\n");
            }

            endDownload();
        }
        /// <summary>
        /// 读取cookie
        /// </summary>
        private void loadCookie()
        {
            //如果已经存在cookie文件则读取
            if (File.Exists("cookie.txt"))
            {
                InputCookie.Text = File.ReadAllText("cookie.txt");
            }
        }
        /// <summary>
        /// 保存cookie
        /// </summary>
        private void saveCookie()
        {
            //为空则不存储
            if (cookie == "")
                return;

            FileStream fs = File.Create("cookie.txt");
            byte[] utf8 = Encoding.UTF8.GetBytes(cookie);
            fs.Write(utf8, 0, utf8.Length);
        }
        /// <summary>
        /// 获取用户给出的所有参数
        /// </summary>
        private void initDownload()
        {
            //已下载总量
            fullDownloadNum = 0;
            //获取画师名
            userName = input_name.Text.Trim();
            //获取下载目录
            if (FilePath.Text == "") filePath = ".\\" + userName + "\\";
            else filePath = FilePath.Text + "\\" + userName + "\\";

            //处理高级选项参数
            string inputPage = InputPageNum.Text.Trim();
            string inputStartPic = InputStartPicNum.Text.Trim();
            string inputPic = InputMaxPicNum.Text.Trim();

            //获取高级参数值
            int startPageNum, startPicNum, maxDownloadNum;
            //起始页数，默认值为1
            if (int.TryParse(inputPage, out startPageNum) && startPageNum > 0)
            {
                this.startPageNum = startPageNum;
            }
            else this.startPageNum = 1;
            //从第几张开始下载，默认值为1
            if (int.TryParse(inputStartPic, out startPicNum) && startPicNum > 0)
            {
                this.startPicNum = startPicNum;
            }
            else this.startPicNum = 1;
            //最大下载数，默认值为0，即不限量
            if (int.TryParse(inputPic, out maxDownloadNum) && maxDownloadNum > 0)
            {
                this.maxDownloadNum = maxDownloadNum;
            }
            else this.maxDownloadNum = 0;

            //输出高级参数下载信息
            AddItemToTextBox(string.Format("从第{0}页第{1}张开始下载，下载数量为{2}。\r\n", 
                                            this.startPageNum, 
                                            this.startPicNum, 
                                            this.maxDownloadNum > 0 ? this.maxDownloadNum.ToString() : "不限量"));
        }
        /// <summary>
        /// 循环下载需要下载的页面，然后逐个下载图片
        /// </summary>
        /// <param name="str"></param>
        private bool download(string str)
        {
            AddItemToTextBox(str + "下载开始");
            initDownload();

            //初始化参数
            str = str + "/";//下载地址
            string[] allPages;//存储详情页地址

            //循环下载第pageNum页
            while (true)
            {
                //存储当前页面地址
                string nowUrl, errorInfo = null;

                nowUrl = "http://www.furaffinity.net/" + str + userName + "/" + startPageNum;

                //下载当前页
                string nowPage = Download.GetGeneralContent(nowUrl, cookie);
                //检查网络连接
                if (nowPage == null)
                {
                    break;
                }
                //检查此用户是否存在
                if (Analyze.checkName(nowPage, userName) == false)
                {
                    AddItemToTextBox("未查询到该作者，请检查作者名称");
                    return false;
                }
                //检查此页是否还有图片，没有的话输出错误日志
                if ((errorInfo = Analyze.checkPage(nowPage)) != null)
                {
                    AddItemToTextBox(errorInfo);
                    break;
                }

                //--------开始正式下载------------

                //获得所有详情页
                allPages = Analyze.getNextPages(nowPage);

                for (int i = startPicNum - 1; i < allPages.Length; ++i)
                {
                    if (!downloadPicture(allPages[i], str)) return false;
                }
                startPicNum = 1;
                startPageNum++;
            }
            return true;
        }
        private bool downloadPicture(string page, string type)
        {
            //如果已经到达最大下载量，则停止下载
            if (maxDownloadNum > 0 && fullDownloadNum > maxDownloadNum)
                return false;

            //获取详情页面信息
            string tempPage = Download.GetGeneralContent(page, cookie);
            if (tempPage == null)
            {
                AddItemToTextBox("网络错误");
                return false;
            }

            //获取图片下载地址
            string pictureUrl = Analyze.getPictureUrl(tempPage);
            if (pictureUrl == null)
                return false;

            //获取文件名字
            string pictureName = Analyze.getFilename(pictureUrl);
            if (pictureName == null)
                return false;

            //开始下载
            AddItemToTextBox("第" + startPageNum + "页" + "第" + startPicNum + "张下载开始");
            AddItemToTextBox("文件名：" + pictureName);

            //文件完整路径
            string fullFilePath = filePath + type + pictureName;

            //判断文件是否已经存在
            if (File.Exists(fullFilePath))
            {
                AddItemToTextBox("文件已存在，跳过下载\r\n");
                startPicNum++;
                return true;
            }

            //获取下载前的时间
            DateTime before = DateTime.Now;

            string message = Download.GetFileContent(pictureUrl, filePath + type, pictureName);
            if (message == null)
            {
                //获取下载后的时间
                DateTime after = DateTime.Now;
                //计算下载所用时间
                TimeSpan speed = after - before;

                AddItemToTextBox(speed.TotalSeconds + "秒下载完成\r\n" );
                startPicNum++;
                fullDownloadNum++;
                return true;
            }
            else
            {
                AddItemToTextBox("下载失败,原因是：" + message);
                return false;
            }
        }
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

        private void endDownload()
        {
            //解冻所有控件
            UnFreeze();
            
            DateTime afterDownload = DateTime.Now;//下载结束后的时间
            TimeSpan fullDownloadTime = afterDownload - beforeDownload;//总下载时间
            AddItemToTextBox("共" + fullDownloadTime.Hours + "小时" + fullDownloadTime.Minutes + "分钟" + fullDownloadTime.Seconds + "秒");
        }

        #region 按钮点击事件
        //确定按钮触发事件
        private void button1_Click(object sender, EventArgs e)
        {
            //开始之前的检查部分
            if (input_name.Text == "")
            {
                MessageBox.Show("作者名不能为空！");
                return;
            }
            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                MessageBox.Show("请至少选择画廊和手稿中的一个");
                return;
            }

            //冻结所有控件
            Freeze();
            //修改回车焦点
            this.AcceptButton = this.ButtonCancle;

            //初始化下载参数
            cookie = InputCookie.Text.Trim();
            saveCookie();
            beforeDownload = DateTime.Now;
            //开始下载
            newThread = new Thread(loop);
            newThread.Start();
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
                    checkBox1.Enabled = checkBox2.Enabled = false;
                    Browse.Enabled = false;
                    InputPageNum.Enabled = InputPageNum.Enabled = InputMaxPicNum.Enabled = false;
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
                    checkBox1.Enabled = checkBox2.Enabled = true;
                    Browse.Enabled = true;
                    InputPageNum.Enabled = InputPageNum.Enabled = InputMaxPicNum.Enabled = true;
                }
            }
            catch
            {

            }
        }
        //取消按钮触发事件
        private void ButtonCancle_Click(object sender, EventArgs e)
        {
            //结束下载进程
            newThread.Abort();
            //解冻所有控件
            UnFreeze();
            //修改回车焦点
            this.AcceptButton = this.ButtonEnter;

            AddItemToTextBox("\r\n下载终止，到此为止共下载" + fullDownloadNum + "张图片");

            endDownload();
        }
        //浏览按钮触发事件
        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = folderBrowserDialog.SelectedPath;
                FilePath.Text = filePath;
            }
        }
        //帮助按钮触发事件
        private void Help_Click(object sender, EventArgs e)
        {
            string help = "本软件用于批量下载fa站图片\r\n" +
                        "请保证网络连接通畅\r\n" +
                        "必要时请给于软件管理员权限\r\n" +
                        "下载图片不成功时，请开启vpn后重新下载\r\n" +
                        "本软件仅供交流，请勿用于商业用途\r\n";

            MessageBox.Show(help,"注意事项");
        }
        #endregion
    }
}
