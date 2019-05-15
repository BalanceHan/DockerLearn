using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreBLL
{
    /// <summary>
    /// 运行时构建字段值类型
    /// </summary>
    public static class FieldRuntimeTypeBuilder
    {
        public static object ChangeType(object value, Type type)
        {
            var t = type;
            if (type.AssemblyQualifiedName.Contains("System.Boolean"))
            {
                if (value == null || value.ToString().ToLower() == "null")
                {
                    return null;
                }
                value = value.ToString() == "0" ? false : true;
            }
            if (type.AssemblyQualifiedName.Contains("System.String"))
            {
                if (value.ToString().ToLower() == "null")
                {
                    return null;
                }
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }
}
