using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware // exception handling klasa
    {
         public RequestDelegate Next { get; }
        public ILogger<ExceptionMiddleware> Logger { get; }
        public IHostEnvironment Env { get; }
        
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            Next = next;
            Logger = logger;
            Env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/jason"; //respond json-u
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

              var Response = Env.IsDevelopment()
              ? new ApiException (context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) // operator sta da uradi ako je enviroment development mode
              : new ApiException (context.Response.StatusCode, "Internal Server Error ! "); // sta ako nije, posto mi radimo u dev mod trenurno
            
            var options =new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; // send back in normal json response in camelcase
            var json = JsonSerializer.Serialize(Response,options);
            await context.Response.WriteAsync(json);
          }
        }

    }
}