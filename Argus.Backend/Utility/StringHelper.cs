﻿using System;

namespace Argus.Backend.Utility
{
    public static class StringHelper
    {
        public static void ExtractUserInfo(string sFullName, out string sName, out string sDepartment, out bool isExt )
        {
            if (sFullName.Contains("(") == false)
            {
                sName = sFullName;
                isExt = false;
                sDepartment = "";
                return;
            }
            sName = sFullName.Substring(0, sFullName.IndexOf('(')).Trim();
            isExt = sFullName.Contains("(ext)");
            int iStart = sFullName.LastIndexOf("(", StringComparison.Ordinal);
            int iEnd = sFullName.LastIndexOf(")", StringComparison.Ordinal);
            if (iEnd - iStart - 1 < 0)
            {
                sDepartment = "";
                return;
            }
            sDepartment = sFullName.Substring(iStart + 1, iEnd - iStart - 1).Trim();
        }
    }
}