using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var exceptionType = ex.GetType();

                //Item does not exists
                if(exceptionType == typeof(DeletionException))
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                //ModelState is invalid
                else if (exceptionType == typeof(UpdateException))
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                //Internal creation error
                else if (exceptionType == typeof(CreationException))
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                //All other errors
                else
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                await context.Response.WriteAsync(new ApiError
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                }.ToString());
            }
        }
    }
}
