using CustomerManager.Repository;
using CustomerManager.Repository.Interfaces;
using CustomerManager.Service;
using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
