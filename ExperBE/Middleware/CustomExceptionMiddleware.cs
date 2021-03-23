using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExperBE.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ExperBE.Middleware
{
    public class CustomExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (BadRequestException exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(exception.Error));
            }
        }
    }
}
