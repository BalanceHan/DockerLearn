using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Web;
using MyCoreBLL;

namespace MyCore.AppFunc
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate next;

        public RequestCultureMiddleware(RequestDelegate request)
        {
            next = request;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var result = string.Empty;
            if (context.Request.ContentType == "application/x-www-form-urlencoded")
            {
                var request = context.Request;
                var stream = request.Body;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEndAsync().Result;
                    var json = RequestBodyDispose.Operation(result);
                    var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                    stream = requestContent.ReadAsStreamAsync().Result;
                    request.Body = stream;
                }
                request.ContentType = "application/json";
            }
            var cultureQuery = context.Request.Query["token"];
            if (!string.IsNullOrEmpty(cultureQuery))
            {
                context.Request.Headers.Add("Authorization", new[] { "Bearer " + cultureQuery });
            }

            return next(context);
        }
    }
}
