using Common;
using Common.ApplicationSettings;
using Core.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Persistence.Commands;
using Persistence.Queries;
using System;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(action =>
                {
                    action.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            // load configuration before any other modules
            services
                .Configure<ImageRepositorySettings>(_configuration.GetSection("ImageRepository"));
            services
                .Configure<ConnectionStrings>(_configuration.GetSection("ConnectionStrings"));

            services
                .AddCommon();

            services
                .AddApplication();

            services
                .AddPersistenceForCommands(_configuration);

            services
                .AddPersistenceForQueries();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
