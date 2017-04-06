using IntroducaoAoMediatR.Models;
using IntroducaoAoMediatR.Services;
using IntroducaoAoMediatR.Utilities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace IntroducaoAoMediatR
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            this.Configuration = 
                new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();

            services.AddTransient(typeof(IRepositorioDeProdutos), typeof(RepositorioDeProdutos));
            services.AddTransient(typeof(IRepositorioDePedido), typeof(PedidoDePedidos));
            services.AddTransient(typeof(IRepositorioDeNotasFiscais), typeof(RepositorioDeNotasFiscais));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Timer<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Logging<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Validator<,>));
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.Configure<RazorViewEngineOptions>(o => o.ViewLocationExpanders.Add(new FeatureViewLocator()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Loja}/{action=Vitrine}/{id?}");
            });
        }

        public IConfigurationRoot Configuration { get; }
    }
}