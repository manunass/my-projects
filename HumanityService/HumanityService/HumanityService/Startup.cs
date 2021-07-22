using System.Text.Json.Serialization;
using HumanityService.Services;
using HumanityService.Services.Interfaces;
using HumanityService.Stores;
using HumanityService.Stores.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HumanityService
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

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IRoutingService, RoutingService>();
            services.AddSingleton<IMatchingService, MatchingService>();
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<ITransactionStore, TransactionStore>();
            services.AddSingleton<IUserStore, UserStore>();
            services.AddSingleton<ILocationStore, LocationStore>();

            services.AddSingleton<IConnectionFactory, SqlConnectionFactory>();
            services.AddSingleton<UserStore>();
            services.AddSingleton<TransactionStore>();
            services.AddSingleton<LocationStore>(); 


            services.Configure<SqlDatabaseSettings>(Configuration.GetSection("SqlDatabaseSettings"));

            services.Configure<RoutingServiceSettings>(Configuration.GetSection("OpenRouteServiceSettings"));

            services.Configure<NotificationServiceSettings>(Configuration.GetSection("NotificationServiceSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
