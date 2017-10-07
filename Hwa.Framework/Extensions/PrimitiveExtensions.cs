using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa
{
    public static class PrimitiveExtensions
    {
        public static string ToNullSafeString(this object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            return null;
        }
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
