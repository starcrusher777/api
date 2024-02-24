using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Targets;

namespace UniApiTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            // uncomment line bellow if need to get all webapi low-level logs 
            //builder.Host.UseNLog();
            var builder = WebApplication.CreateBuilder(args);
            
            
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
           

            app.UseExceptionHandler(x => x.Run(async context =>
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                context.Response.Headers.ContentType = "application/json; charset=utf-8";

                await context.Response.WriteAsync(context.Features.Get<IExceptionHandlerPathFeature>().Error.Message);
            }));
            app.UseHsts();

            app.UseCors("corsapp");
            
            app.Run();
            
            Console.WriteLine("Hello world!");
        }
    }
}