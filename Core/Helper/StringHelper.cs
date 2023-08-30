using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper;

public static class StringHelper
{
    public static string LimitString(this string str, int limit = 50)
    {
        if (str.Length== 0) return string.Empty;

        if (str.Length > limit)
        {
            return str.Substring(0, limit) + "...";
        }
        else
        {
            return str;
        }
    }
}
