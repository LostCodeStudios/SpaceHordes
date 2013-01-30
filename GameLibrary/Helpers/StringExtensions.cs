using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GameLibrary.Helpers
{
    public static class StringExtensions
    {
        public static string StripLeadingWhitespace(this string s)
        {
            Regex r = new Regex(@"^\s+", RegexOptions.Multiline);
            return r.Replace(s, string.Empty);
        }

        public static string AddLeadingString(this string s, int indent, string prefix = "")
        {
            Regex r = new Regex(@"^", RegexOptions.Multiline);
            string ind = new string(' ', indent);
            return r.Replace(s, ind + prefix);
        }
    }
}
