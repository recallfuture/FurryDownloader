using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FurryDownloader
{
    /// <summary>
    /// 需要登陆时候抛出的异常
    /// </summary>
    public class NeedLoginException : Exception
    {
        public NeedLoginException(string message) : base(message)
        {
        }
    }
}
