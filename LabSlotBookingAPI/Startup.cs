using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Services;
using Swashbuckle.AspNetCore.Swagger;

namespace LabSlotBookingAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "LabSlotBookingAPI",
                    Description = "Web API that is used to insert,get, update and delete Lab slot",
                    Contact = new Contact { Name = "IGT", Email = "" },
                    Version = "1.0",
                });


                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                if (Configuration["Swagger:XmlPath"] != null)
                {
                    var xmlPath = Path.Combine(basePath, Configuration["Swagger:XmlPath"]);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                    else
                    {
                        //log.LogWarning($@"File does not exist: ""{xmlPath}""");
                    }

                }
            });

            //CORS
            services.AddCors();

            //Registered DI
            services.AddScoped<ILabInfrastructure, LabInfrastructure>();
            services.AddScoped<ILabServices, LabServices>();
            services.AddScoped<ISettings, Settings>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration["Swagger:Endpoint"], "LabSlotBookingAPI");
                c.RoutePrefix = "";
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Configure CORS
            app.UseCors(c =>
            {
                c.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
