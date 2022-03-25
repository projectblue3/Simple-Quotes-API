using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Middleware
{
    public class LogHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogHandler> _logger;

        public LogHandler(RequestDelegate next, ILogger<LogHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            _logger.LogInformation($"Req: {context.Request.Method} {context.Request.Path} - Res: {context.Response.StatusCode}");
        }
    }
}
