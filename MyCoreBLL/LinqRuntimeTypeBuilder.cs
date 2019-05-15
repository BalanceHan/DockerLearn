using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MyCoreBLL
{
    public static class LinqRuntimeTypeBuilder
    {
        //定义动态程序集
        private static readonly AssemblyName assemblyName = new AssemblyName() { Name = "DynamicLinqTypes" };
        //定义动态程序集中的模块
        private static readonly ModuleBuilder moduleBuilder = null;
        private static readonly Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();
        static LinqRuntimeTypeBuilder()
        {
            //初始化动态程序集模块
            moduleBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name);
        }

        /// <summary>
        /// 拼接匿名类型'a'(存在问题：字段过多导致类型名称太长。完全限定类型名称必须小于1024个字符。)
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            string key = string.Empty;
            foreach (var field in fields)
            {
                key += field.Key + ";" + field.Value.Name + ";";
            }
            return key;
        }

        /// <summary>
        /// 生成匿名类型
        /// </summary>
        /// <param name="fields">需要生成的匿名类型字段</param>
        /// <returns></returns>
        public static Type GetDynamicType(Dictionary<string, Type> fields)
        {
            try
            {
                Monitor.Enter(builtTypes);
                string className = Guid.NewGuid().ToString().Replace("-", "");
                if (builtTypes.ContainsKey(className))
                {
                    return builtTypes[className];
                }

                //构造指定名称类
                TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                //添加字段至此类
                foreach (var field in fields)
                {
                    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);
                }

                //创建类
                builtTypes[className] = typeBuilder.CreateType();
                return builtTypes[className];
            }
            catch (Exception)
            {

            }
            finally
            {
                Monitor.Exit(builtTypes);
            }
            return null;
        }

        /// <summary>
        /// IEnumerable类型转Dictionary
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
        {
            return GetTypeKey(fields.ToDictionary(s => s.Name, s => s.PropertyType));
        }

        /// <summary>
        /// IEnumerable类型转Dictionary
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
        {
            return GetDynamicType(fields.ToDictionary(s => s.Name, s => s.PropertyType));
        }
    }
}