using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DependeceInjection
{
    public class Startup
    {
        private IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFormatadorEndereco>(serviceProvider =>
             {
                 string nomeTipo = configuration["servicos:IFormatadorEndereco"];
                return (IFormatadorEndereco)ActivatorUtilities.CreateInstance(serviceProvider, 
                     nomeTipo == null ? typeof(EnderecoTextual) : Type.GetType(nomeTipo, true));
             });
            services.AddSingleton(typeof(ICollection<>), typeof(List<>));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.Use(async (context, next) =>
            {
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/string", async context =>
                {
                    ICollection<string> lista = context.RequestServices.GetService<ICollection<string>>();
                    lista.Add($"Requisição: {DateTime.Now.ToLongTimeString()}");
                    context.Response.ContentType = "text/plain; charset=utf-8;";
                    foreach (string str in lista)
                    {
                        await context.Response.WriteAsync($"String: {str}\n");
                    }
                });

                endpoints.MapGet("/int", async context =>
                {
                    ICollection<int> lista = context.RequestServices.GetService<ICollection<int>>();
                    lista.Add(lista.Count + 1);
                    context.Response.ContentType = "text/plain; charset=utf-8;";
                    foreach (int val in lista)
                    {
                        await context.Response.WriteAsync($"Int: {val}\n");
                    }
                });
            });


             app.UseMiddleware<MiddlewareConsultaCep>();

             app.Use(async (context, next) =>
             {
                 if (context.Request.Path == "/mw/lambda")
                 {
                     IFormatadorEndereco formatador =
                         context.RequestServices.GetRequiredService<IFormatadorEndereco>();
                     context.Response.ContentType = "text/html; charset=utf-8;";
                     await formatador.Formatar(context,
                         await EndpointConsultaCep.ConsultaCep("01001000"));
                     await formatador.Formatar(context,
                         await EndpointConsultaCep.ConsultaCep("22290270"));
                 }
                 else
                 {
                     await next();
                 }
             });

             app.UseEndpoints(endpoints =>
             {
                 endpoints.MapEndpoint<EndpointConsultaCep>("/ep/classe/{cep:regex(^\\d{{8}}$)?}");
                 endpoints.MapGet("/ep/lambda/{cep:regex(^\\d{{8}}$)?}", async context =>
                 {
                     IFormatadorEndereco formatador =
                         context.RequestServices.GetRequiredService<IFormatadorEndereco>();
                     context.Response.ContentType = "text/html; charset=utf-8;";
                     string cep = context.Request.RouteValues["cep"] as string ?? "01001000";
                     await formatador.Formatar(context,
                         await EndpointConsultaCep.ConsultaCep(cep));
                 });
             });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Middleware terminal.");
            });
        }
    }
}
