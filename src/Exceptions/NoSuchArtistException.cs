using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FurryDownloader
{
    /// <summary>
    /// 作者名有误时候引发的异常
    /// </summary>
    public class NoSuchArtistException : Exception
    {
        public NoSuchArtistException(string message) : base(message)
        {
        }
    }
}
