using System;

namespace Plarium.Test.FourThreads.Extensions
{
    internal static class FormattingExtensions
    {
        public static string FormatDateForXml(this DateTime dateTime)
        {
            // For XML attribute
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string FormatDateForFileName(this DateTime dateTime)
        {
            // For a XML output file name
            return dateTime.ToString("yyyyMMdd_HHmmss");
        }
    }
}