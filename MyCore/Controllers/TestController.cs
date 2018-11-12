using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCore.ViewModels;
using MyCoreBLL;
using MyCoreDAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCore.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly DataContext context;
        private OrmHelp help;
        public TestController(DataContext dataContext)
        {
            context = dataContext;
            help = new OrmHelp(dataContext);
        }

        /// <summary>
        /// jenkins测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(help.SelectAllDescDate<Customer>(s => s.AdditionDate));
        }

        [HttpGet("data")]
        public IActionResult GetTest()
        {
            DateTime date = Convert.ToDateTime("2018-11-07 14:00:05");
            DateTime dt = Convert.ToDateTime("2018-11-07 13:09:13");
            var hh = dt - date;
            var han = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            return Ok(han);
        }
    }
}
