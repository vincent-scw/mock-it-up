using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockItUp.Common.Utilities
{
    public static class TypeExtensions
    {
        private static Type[] _simpleTypes;

        static TypeExtensions()
        {
            var types = new[]
            {
                typeof (Enum),
                typeof (String),
                typeof (Char),
                typeof (Guid),

                typeof (Boolean),
                typeof (Byte),
                typeof (Int16),
                typeof (Int32),
                typeof (Int64),
                typeof (Single),
                typeof (Double),
                typeof (Decimal),

                typeof (SByte),
                typeof (UInt16),
                typeof (UInt32),
                typeof (UInt64),

                typeof (DateTime),
                typeof (DateTimeOffset),
                typeof (TimeSpan),
            };

            var nullTypes = from t in types
                            where t.IsValueType
                            select typeof(Nullable<>).MakeGenericType(t);

            _simpleTypes = types.Concat(nullTypes).ToArray();
        }

        /// <summary>
        /// Check if given type is simple type (int, string, guid, datetime, etc...)
        /// </summary>
        /// <returns></returns>
        public static bool IsSimpleType(this Type type)
        {
            if (_simpleTypes.Any(x => x.IsAssignableFrom(type)))
            {
                return true;
            }

            var nut = Nullable.GetUnderlyingType(type);
            return nut != null && nut.IsEnum;
        }
    }
}
