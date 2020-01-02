using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FurryDownloader
{
    /// <summary>
    /// 下载被中断时候引发的异常
    /// </summary>
    public class InterruptException : Exception
    {
        public InterruptException(string message) : base(message)
        {
        }
    }
}
