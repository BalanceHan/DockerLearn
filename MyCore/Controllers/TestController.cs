using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCore.ViewModels;
using MyCoreBLL;
using MyCoreBLL.FieldFile;
using MyCoreDAL;
using Newtonsoft.Json.Linq;
using SimplePatch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCore.Controllers
{
    [Route("api")]
    public class TestController : Controller
    {
        private readonly DataContext context;
        private readonly OrmHelp help;
        public TestController(DataContext dataContext, IHttpContextAccessor http)
        {
            context = dataContext;
            help = new OrmHelp(dataContext, http);
        }

        /// <summary>
        /// 统一查询
        /// </summary>
        /// <param name="value">路由表</param>
        /// <param name="field">查询参数</param>
        /// <returns></returns>
        [HttpGet("{value}")]
        public IActionResult Get(string value, [FromQuery]FiltrateField field)
        {
            var obj = Assembly.Load("MyCoreDAL").CreateInstance("MyCoreDAL." + value, true);
            var type = obj.GetType();
            var dbSetMethodInfo = typeof(DbContext).GetMethod("Set").MakeGenericMethod(type);
            var dbSet = (dynamic)dbSetMethodInfo.Invoke(context, null);
            var result = (IQueryable<dynamic>)dbSet;
            var info = help.SelectDynamic(result, field);
            return Ok(info);
        }

        /// <summary>
        /// 统一新增
        /// </summary>
        /// <param name="value">路由表</param>
        /// <param name="parameter">新增数据</param>
        /// <returns></returns>
        [HttpPost("{value}")]
        public IActionResult Insert(string value,[FromBody]JObject parameter)
        {
            var obj = Assembly.Load("MyCoreDAL").CreateInstance("MyCoreDAL." + value, true);
            var type = obj.GetType();
            var dataType = parameter.ToObject(type);
            var addMethodInfo = typeof(DbContext).GetMethods().First(s => s.Name == "Add" && s.IsGenericMethod).MakeGenericMethod(type);
            var addMethod = (dynamic)addMethodInfo.Invoke(context, new[] { dataType });
            context.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// 统一删除
        /// </summary>
        /// <param name="value">路由表</param>
        /// <param name="guid">主键Guid</param>
        /// <returns></returns>
        [HttpDelete("{value}/{guid}")]
        public IActionResult Delete(string value,string guid)
        {
            var obj = Assembly.Load("MyCoreDAL").CreateInstance("MyCoreDAL." + value, true);
            var type = obj.GetType();
            object[] objGuid = new[] { guid };
            var findMethodInfo = typeof(DbContext).GetMethods().First(s => s.Name == "Find" && s.IsGenericMethod).MakeGenericMethod(type);
            var find = (dynamic)findMethodInfo.Invoke(context, new[] { objGuid });

            context.Remove(find);
            context.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// 统一更改
        /// </summary>
        /// <param name="value">路由表</param>
        /// <param name="guid">主键Guid</param>
        /// <param name="parameter">更改参数</param>
        /// <returns></returns>
        [HttpPatch("{value}/{guid}")]
        public IActionResult Update(string value, string guid, [FromBody]JObject parameter)
        {
            var obj = Assembly.Load("MyCoreDAL").CreateInstance("MyCoreDAL." + value, true);
            var type = obj.GetType();
            object[] objGuid = new[] { guid };
            var dataType = parameter.ToObject(type);
            var findMethodInfo = typeof(DbContext).GetMethods().First(s => s.Name == "Find" && s.IsGenericMethod).MakeGenericMethod(type);
            var find = (dynamic)findMethodInfo.Invoke(context, new[] { objGuid });

            var myGenericObject = (dynamic)Activator.CreateInstance(typeof(Delta<>).MakeGenericType(type));
            foreach (var item in parameter.Properties())
            {
                if (item.Value == null || item.Value.ToString().ToLower() == "null")
                {
                    myGenericObject.Add(item.Name, null);
                }
                else
                {
                    myGenericObject.Add(item.Name, item.Value);
                }
            }
            myGenericObject.Patch(find);

            context.SaveChanges();
            return Ok();
        }

        //[HttpGet("data")]
        //public IActionResult GetTest()
        //{
        //    DateTime date = Convert.ToDateTime("2018-11-07 14:00:05");
        //    DateTime dt = Convert.ToDateTime("2018-11-07 13:09:13");
        //    var hh = dt - date;
        //    var han = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        //    return Ok(GetType().Assembly.GetName().Version.ToString());
        //}
    }
}
