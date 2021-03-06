﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCoreBLL;
using MyCoreDAL;
using Nest;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCore.Controllers
{
    [Route("api/[controller]")]
    public class ThreadController : Controller
    {
        private readonly DataContext context;
        private readonly OrmHelp help;
        public ThreadController(DataContext dataContext, IHttpContextAccessor http)
        {
            context = dataContext;
            help = new OrmHelp(dataContext, http);
        }

        [HttpGet("delete")]
        public IActionResult GetDelete()
        {
            var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200")).DefaultIndex("default");
            var client = new ElasticClient(settings);
            client.DeleteIndex("default");
            return Ok(1);
        }

        [HttpGet("{value}")]
        public IActionResult GetThread(string value)
        {
            var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200")).DefaultIndex("default");
            var client = new ElasticClient(settings);

            var searchResponse = client.Search<Customer>(s => s.From(0).Size(10).Query(q => q.MatchPhrase(m => m.Field(f => f.CustomerName).Query(value))));
            var people = searchResponse.Documents.ToList();
            return Ok(1);
        }

        [HttpPost]
        public async Task<IActionResult> PostThread([FromBody]Customer value)
        {
            var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200"));
            var client = new ElasticClient(settings);
            value.CustomerGuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            context.Customer.Add(value);
            await context.SaveChangesAsync();
            var asyncIndexResponse = await client.IndexAsync(value, s => s.Index("default"));
            return Ok(1);
        }

        [HttpPut("{value}")]
        public IActionResult PutThread(string value)
        {
            //var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200")).DefaultIndex("default");
            //var client = new ElasticClient(settings);
            //await help.UpdateAsync(s => s.CustomerGuid == value, delta);
            //var item = await help.SelectFirstAsync<Customer>(s => s.CustomerGuid == value);
            //DocumentPath<Customer> document = new DocumentPath<Customer>(value);
            //await client.UpdateAsync(document, (s) => s.Doc(item));
            return Ok(value);
        }

        [HttpDelete("{value}")]
        public IActionResult DeleteThread(string value)
        {
            //var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200")).DefaultIndex("default");
            //var client = new ElasticClient(settings);
            //DocumentPath<Customer> document = new DocumentPath<Customer>(value);
            //client.Delete(document);
            return Ok(value);
        }
    }
}
