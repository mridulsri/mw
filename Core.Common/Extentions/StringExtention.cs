using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Core.Common.Extentions
{
    public static class StringExtention
    {
        private static readonly Regex SanitizationPattern = new Regex(@"\+|\-|!|\(|\)|\{|\}|\[|\]|\^|~|\*|\?|:|;|,|.|&", RegexOptions.Compiled);

        public static string ConvertToKey(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, "[^0-9a-zA-Z]+", "").Replace(" ","").ToLower().Trim();
        }

        public static string DecodeHtml(this string text)
                {
                    if (string.IsNullOrEmpty(text))
                        return string.Empty;

                    return HttpUtility.HtmlDecode(text).Replace("\r","").Replace("\t","").Replace("\n"," ");
                }
         public static string RemoveNonBreakingSpace(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, @"<[^>]+>|&nbsp;", "").ToLower().Trim();
        }
        public static string EncodeHtml(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return HttpUtility.HtmlEncode(text);
        }
    }
}
