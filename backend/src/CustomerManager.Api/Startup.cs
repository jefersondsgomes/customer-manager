using CustomerManager.Api.Middlewares;
using CustomerManager.Models.Helpers;
using CustomerManager.Models.Helpers.Interfaces;
using CustomerManager.Repositories;
using CustomerManager.Repositories.Interfaces;
using CustomerManager.Services;
using CustomerManager.Services.Interfaces;
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ISettings>(new Settings() { Secret = "kJyyKCh52g2cSYVyc6JGf4h4TEfka2EwkLeLCgCS" })
                .AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>))
                .AddSingleton<ICustomerService, CustomerService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IAuthenticationService, AuthenticationService>()
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

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }});

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