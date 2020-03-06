using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace FurryDownloader
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        Wait,
        Start,
        Pause,
        Stop,
        Finish,
    }

    /// <summary>
    /// 任务
    /// </summary>
    public class Task
    {
        public int pageNum;
        public int picNum;

        public string Url;
        public string FilePath;
        public string FileName;
        public TaskStatus Status;

        public delegate void TaskStartDelegate(int taskId, Task task);
        public delegate void TaskStopDelegate(int taskId, Task task);
        public delegate void TaskFinishDelegate(int taskId, Task task);
        public delegate void TaskFailDelegate(int taskId, Task task);

        public TaskStartDelegate TaskStart;
        public TaskStopDelegate TaskStop;
        public TaskFinishDelegate TaskFinish;
        public TaskFailDelegate TaskFail;
    }

    /// <summary>
    /// 任务线程
    /// </summary>
    public class TaskWorker
    {
        int taskId;
        Task task;

        int retry = 0;

        public TaskWorker(int taskId, Task task)
        {
            this.taskId = taskId;
            this.task = task;
        }

        public void Start(Object o)
        {
            try
            {
                // 检查是否已经停止
                if (task.Status == TaskStatus.Stop)
                {
                    task.TaskStop(taskId, task);
                    task.Status = TaskStatus.Finish;
                    return;
                }

                task.TaskStart(taskId, task);
                DownloadAndSave(task.Url, task.FilePath, task.FileName);
                Console.WriteLine(taskId + "下载完成");
                task.TaskFinish(taskId, task);
                task.Status = TaskStatus.Finish;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                // 进行最多三次重试
                if (retry++ < 3)
                {
                    Console.WriteLine(String.Format("[{0}]下载失败，第{1}次重试中", taskId, retry));
                    DownloadManager.Start(this);
                }
                else
                {
                    Console.WriteLine(String.Format("[{0}]下载失败已超过三次，请检查网络是否可用", taskId, retry));
                    task.TaskFail(taskId, task);
                    task.Status = TaskStatus.Finish;
                }
            }
        }

        /// <summary>
        /// 下载并保存文件
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="fileDir">存储目录</param>
        /// <param name="fileName">文件名</param>
        void DownloadAndSave(string url, string fileDir, string fileName)
        {
            //创建存放文件夹
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);

            //如果已经存在则不再下载
            if (File.Exists(fileDir + fileName))
                return;

            // header
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项
                Method = "get",//URL     可选项 默认为Get
                Timeout = 10000,//连接超时时间     可选项默认为100000
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
                ContentType = "*/*",//返回类型    可选项有默认值
                Referer = "https://www.furaffinity.net",//来源URL     可选项
                ResultType = ResultType.Byte,//返回数据类型，是Byte还是String
                // ProxyIp = "127.0.0.1:1081",
            };

            // 发送请求
            HttpHelper http = new HttpHelper();
            HttpResult result = http.GetHtml(item);

            //判断状态码
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("状态码错误：" + result.StatusCode);
            }

            // 写入磁盘
            File.WriteAllBytes(fileDir + fileName, result.ResultByte);
        }
    }

    /// <summary>
    /// 多线程下载管理类
    /// </summary>
    public static class DownloadManager
    {
        /// <summary>
        /// 用字典存储所有任务
        /// </summary>
        static ConcurrentDictionary<int, Task> tasks = new ConcurrentDictionary<int, Task>();
        /// <summary>
        /// 每个任务的id生成器
        /// </summary>
        static volatile int taskIndex = 0;

        /// <summary>
        /// 自增锁
        /// </summary>
        static Object incLock = new Object();

        /// <summary>
        /// 线程间同步的自增
        /// </summary>
        /// <param name="value">要自增的值</param>
        /// <returns>自增后的结果</returns>
        static int inc()
        {
            lock (incLock)
            {
                taskIndex++;
            }
            return taskIndex;
        }

        /// <summary>
        /// 设置最大线程数
        /// </summary>
        /// <param name="value"></param>
        public static void SetMaxThreads(int value)
        {
            ThreadPool.SetMaxThreads(value, value);
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="task">任务内容</param>
        /// <returns>任务id</returns>
        public static int Add(Task task)
        {
            // 添加任务并开始
            tasks[taskIndex] = task;
            task.Status = TaskStatus.Wait;
            Start(taskIndex);

            // 返回任务id
            return inc();
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <returns>是否成功添加</returns>
        public static bool Start(int taskId)
        {
            Task task;
            if (tasks.TryGetValue(taskId, out task))
            {
                if (task.Status == TaskStatus.Start)
                    return true;

                // 开启线程执行任务
                Console.WriteLine("开启线程执行任务" + taskId);
                TaskWorker taskWorker = new TaskWorker(taskId, task);
                return ThreadPool.QueueUserWorkItem(new WaitCallback(taskWorker.Start));
            }
            else
            {
                // 抛出错误
                return false;
            }
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="worker">任务worker</param>
        /// <returns>是否成功添加</returns>
        public static bool Start(TaskWorker worker)
        {
            return ThreadPool.QueueUserWorkItem(new WaitCallback(worker.Start));
        }

        /// <summary>
        /// 从字典中移除
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <returns>是否成功移除</returns>
        public static bool Remove(int taskId)
        {
            Task task;
            return tasks.TryRemove(taskId, out task);
        }

        /// <summary>
        /// 向所有还未完成的任务发送停止消息
        /// </summary>
        public static void StopAll()
        {
            foreach (var task in tasks.Values)
            {
                if (task.Status != TaskStatus.Finish)
                    task.Status = TaskStatus.Stop;
            }
        }

        /// <summary>
        /// 等待全部完成
        /// </summary>
        public static void WaitForAllFinished()
        {
            // 循环判定直到所有任务都完成
            bool flag = true;
            while (flag)
            {
                flag = false;
                foreach (var task in tasks.Values)
                {
                    if (task.Status != TaskStatus.Finish)
                        flag = true;
                }
                Thread.Sleep(500);
            }
        }
    }
}
