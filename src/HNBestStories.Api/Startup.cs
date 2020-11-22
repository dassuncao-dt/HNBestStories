using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using HNBestStories.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HNBestStories
{
    public class Startup
    {
        /// <summary>
        /// The swagger base endpoint.
        /// </summary>
        private const string SwaggerBaseEndpoint = @"/swagger/v1/swagger.json";

        /// <summary>
        /// The executing assembly.
        /// </summary>
        private readonly Assembly executingAssembly;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this.executingAssembly = typeof(Startup).GetTypeInfo().Assembly;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationComponents(this.Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
            });

            services.AddSwaggerGen(this.SetSwaggerConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(SwaggerBaseEndpoint, "Privacy Manager Api");
                options.DocumentTitle = "Privacy Manager Swagger";
            });

        }

        private void SetSwaggerConfiguration(SwaggerGenOptions config)
        {
            var doc = new OpenApiInfo
            {
                Title = "PrivacyManager",
                Version = "v1",
                Description = "PrivacyManager Api",
                Contact = new OpenApiContact
                {
                    Name = "Personalization Accounting and Provisioning Squad",
                    Email = "squad.provisioningmanagement@nos.pt",
                },
            };

            var xmlFilename = $"{this.executingAssembly.GetName().Name}.xml";
            var xmlCommentsFilepath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

            config.DescribeAllParametersInCamelCase();
            config.SwaggerDoc("v1", doc);
            config.IncludeXmlComments(xmlCommentsFilepath);
        }
    }
}
