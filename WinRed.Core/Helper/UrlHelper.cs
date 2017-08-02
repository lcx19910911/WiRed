using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRed.Core.Helper
{
    public class UrlHelper
    {
        public static string GetFullPath(string path)
        {
            return string.Format("{0}{1}", Params.DomianUrl, path);
        }
    }
}
