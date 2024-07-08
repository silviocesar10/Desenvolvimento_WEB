using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace WebApp
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task ComputarTempo(HttpContext context, Func<Task> next){
            context.Response.ContentType = "text/plain charset=UTF-8";
            var sw = Stopwatch.StartNew();;
            await next();
            sw.Stop();
            var tempo = sw.ElapsedMilliseconds;
            await context.Response.WriteAsync($"\nTempo de execução (ms): {tempo}");
        }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ContadorOptoions>(options =>
            {
                options.Quantidade = 5;
            });
            services.Configure<CronometroOptions>(configuration.GetSection(nameof(CronometroOptions)));
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
         IOptions<ContadorOptoions> options)
        {
               
            app.UsoTempoExecucao();
             app.UseWhen(
                context => context.Request.Query.ContainsKey("caminhosC"), 
                appC =>
                {
                    app.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync("\n Processado pela ramificacao C");
                        await next();
                    });
                    
                }
            );
            app.Map("/caminhoB", appB =>
            {
                appB.Run(async context =>
                {
                    await context.Response.WriteAsync("\n Processado pela ramificacao B!!");
                });
            });
           
            app.Use(async (context, next)=>
            {
                await context.Response.WriteAsync("[[[");
                await next();
                 await context.Response.WriteAsync("]]]\n");
            });
            app.Use(async (context, next)=>
            {
                var contadorOptions = options.Value;
                await context.Response.WriteAsync(new String('<', contadorOptions.Quantidade));
                await next();
                 await context.Response.WriteAsync(new String('>', contadorOptions.Quantidade));
            });
            app.Use(async (context, next)=>
            {
                await context.Response.WriteAsync("===");
                await next();
                await context.Response.WriteAsync("===");
            });
         app.Run(async context =>
         {
             //o ultimo do pipeline
             await context.Response.WriteAsync("Middleware Terminal!!");
         }
         );   
        }
    }
}
