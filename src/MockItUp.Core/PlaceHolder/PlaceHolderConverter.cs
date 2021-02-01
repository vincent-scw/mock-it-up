using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Core.PlaceHolder
{
    public class PlaceHolderConverter
    {
        public string Convert(string placeHolder)
        {
            if (!placeHolder.StartsWith("${") || !placeHolder.EndsWith("}"))
                return placeHolder;

            var ph = placeHolder.Substring(2, placeHolder.Length - 3);
            
            return ph;
        }
    }
}
