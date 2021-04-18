using CustomerManager.Api.Helpers;
using CustomerManager.Model.Helper;
using CustomerManager.Repository;
using CustomerManager.Repository.Interfaces;
using CustomerManager.Service;
using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace CustomerManager.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IMongoSettings _mongoSettings;
        private readonly AppSettings _appSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _mongoSettings = Configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
            _appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton(_mongoSettings)
                .AddSingleton(_appSettings)
                .AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>))
                .AddScoped<ICustomerService, CustomerService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Customer Manager API",
                    Description = "Web API to perform operations with customers.",
                    Contact = new OpenApiContact
                    {
                        Name = "Jeferson Gomes",
                        Email = "jefersondsgomes@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/jefersondsgomes/"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Manager API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}