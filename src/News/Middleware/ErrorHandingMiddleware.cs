using Contacts.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Contacts.Middleware
{
    public class ErrorHandingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Coś poszło nie tak");
            }
            
        }
    }
}
