using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Core.Utilities
{
    public static class SmartFormatter
    {
        public static string Format(this string str, Func<string, string> resolvePlaceHolder)
        {
            var sb = new StringBuilder();
            var queue = new Queue<char>();
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if ((c == '$' && i < str.Length - 1 && str[i + 1] == '{')
                    || queue.Count > 0)
                {
                    queue.Enqueue(c);
                    
                    if (c == '}')
                    {
                        var placeHolder = queue.ToArray().ToString();
                        sb.Append(resolvePlaceHolder(placeHolder));
                        queue.Clear();
                    }
                }
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
