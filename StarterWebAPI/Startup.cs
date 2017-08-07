using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StarterAPI.Middleware;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Diagnostics;
using StarterAPI.Entities;
using Microsoft.EntityFrameworkCore;
using StarterAPI.Repositories;
using StarterAPI.Dtos;
using StarterAPI.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using NLog.Web;
using NLog.Extensions.Logging;

using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Diagnostics;

namespace StarterAPI
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddInMemoryCollection();
            env.ConfigureNLog("nLog.config");
                

            Configuration = builder.Build();

           // Debug.WriteLine($" ---> From Config: {Configuration["firstname"]}");
            //Debug.WriteLine($" ---> From Config: {Configuration["withChild:option1"]}");
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<MyConfiguration>(Configuration);
            services.AddDbContext<PacktDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISeedDataService, SeedDataService>();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Web API documentation", Version = "v1" });
            });

            services.AddMvc(config =>
            {
                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                config.InputFormatters.Add(new XmlSerializerInputFormatter());
            });

            //services.AddApiVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCustomMiddleware();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                    {
                        errorApp.Run(async context =>
                        {
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "text/plain";
                            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (errorFeature != null)
                            {
                                var logger = loggerFactory.CreateLogger("Global Exception Logger");
                                logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                            }

                            await context.Response.WriteAsync("There was an error");
                        });
                    });
            }

            
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API documentation");
            });



            AutoMapper.Mapper.Initialize(mapper =>
            {
                mapper.CreateMap<Customer, CustomerDto>().ReverseMap();
                mapper.CreateMap<Customer, CustomerCreateDto>().ReverseMap();
                mapper.CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            });

            //app.AddSeedData();

            app.UseMvc();
            
           
        }
    }
}
