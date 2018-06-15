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
    public class TestController : Controller
    {
        private DataContext context;
        private OrmHelp help;
        public TestController(DataContext dataContext)
        {
            context = dataContext;
            help = new OrmHelp(dataContext);
        }

        [HttpGet]
        [Route("api/test")]
        public IActionResult Get()
        {
            return Ok(help.SelectAllDescDate<Customer>(s => s.AdditionDate));
        }
    }
}
