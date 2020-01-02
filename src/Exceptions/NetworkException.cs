using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FurryDownloader
{
    /// <summary>
    /// 网络问题引发的异常
    /// </summary>
    public class NetworkException : Exception
    {
        public NetworkException(string message) : base(message)
        {
        }
    }
}
