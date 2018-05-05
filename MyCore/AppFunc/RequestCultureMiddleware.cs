using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;

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
            var cultureQuery = context.Request.Query["token"];
            if (!string.IsNullOrEmpty(cultureQuery))
            {
                context.Request.Headers.Add("Authorization", new[] { "Bearer " + cultureQuery });
            }

            return next(context);
        }
    }
}
