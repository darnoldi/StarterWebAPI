using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StarterAPI.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MyConfiguration _myconfig;

        public CustomMiddleware (RequestDelegate next, IOptions<MyConfiguration > myConfig)
        {
            _next = next;
            _myconfig = myConfig.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Debug.WriteLine($" ---> Request asked for {httpContext.Request.Path } from {_myconfig.Firstname} {_myconfig.Lastname } ");

            //call the next middleware in the pipeline
            await _next.Invoke(httpContext);
        }
    }
}
