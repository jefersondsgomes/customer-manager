using CustomerManager.Api.Helpers;
using CustomerManager.Repository;
using CustomerManager.Repository.Interfaces;
using CustomerManager.Service;
using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _mongoSettings = Configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton(_mongoSettings)
                .AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>))
                .AddScoped<ICustomerService, CustomerService>()
                .AddScoped<IUserService, UserService>()
                .AddControllers();

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

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

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "basic authorization header using the bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
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
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Manager API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}