using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyCore.AppFunc
{
    public class HandleRequestBodyFormatter : InputFormatter
    {
        public HandleRequestBodyFormatter()
        {
            SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded"));
        }

        public override bool CanRead(InputFormatterContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var contentType = context.HttpContext.Request.ContentType;
            if (string.IsNullOrEmpty(contentType) || contentType == "application/x-www-form-urlencoded")
            {
                return true;
            }
            return false;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var contentType = context.HttpContext.Request.ContentType;
            if (string.IsNullOrEmpty(contentType) || contentType == "application/x-www-form-urlencoded")
            {
                using (var reader = new StreamReader(request.Body))
                {
                    var content = reader.ReadToEndAsync().Result;
                    return await InputFormatterResult.SuccessAsync(content);
                }
            }

            return await InputFormatterResult.FailureAsync();
        }
    }
}
