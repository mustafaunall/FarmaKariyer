using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper;

public static class OsHelper
{
    public static string GetResourcesPath()
    {
        string resourcesPath = string.Empty;
        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            resourcesPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "Resources");
        }
        else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            resourcesPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\", "wwwroot", "Resources");
        }

        return resourcesPath;
    }
    public static string GetResumeFilesPath()
    {
        string resourcesPath = string.Empty;
        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            resourcesPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "Resources", "ResumeFiles");
        }
        else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            resourcesPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\", "wwwroot", "Resources", "ResumeFiles");
        }

        return resourcesPath;
    }

    //public static string GetResumeFilesPath(this string str)
    //{
    //    if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
    //        return string.Empty;

    //    return Path.Combine(str, "ResumeFiles");
    //}
}
