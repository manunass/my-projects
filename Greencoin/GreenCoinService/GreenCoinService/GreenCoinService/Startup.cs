using GreenCoinService.DataContracts;
using GreenCoinService.Migrations;
using GreenCoinService.Services;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores;
using GreenCoinService.Stores.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GreenCoinService
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
            services.AddCors();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GreenCoin API"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });


            services.AddDbContextFactory<GreenCoinServiceDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SqlDatabase")));

            services.AddOptions();
            services.Configure<AzureStorageSettings>(Configuration.GetSection("AzureStorageSettings"));

            services.AddSingleton<IMunicipalitiesStore, MunicipalitiesStore>();
            services.AddSingleton<IMunicipalitiesService, MunicipalitiesService>();

            services.AddSingleton<IEmployeesStore, EmployeesStore>();

            services.AddSingleton<IWalletsStore, WalletsStore>();
            services.AddSingleton<IWalletsService, WalletsService>();

            services.AddSingleton<ITransactionsStore, TransactionsStore>();

            services.AddSingleton<IBusinessesService, BusinessesService>();
            services.AddSingleton<IBusinessesStore, BusinessesStore>();

            services.AddSingleton<IUsersService, UsersService>();
            services.AddSingleton<IUsersStore, UsersStore>();

            services.AddSingleton<IImagesService, ImagesService>();
            services.AddSingleton<IImagesStore, AzureBlobImagesStore>();

            services.AddSingleton<ICodesService, CodesService>();
            services.AddSingleton<ICodesStore, CodesStore>();

            services.AddSingleton<IOperationsService, OperationsService>();
            services.AddSingleton<IBagScansStore, BagScansStore>();
            services.AddSingleton<IBatchesStore, BatchesStore>();
            services.AddSingleton<IRecyclablesStore, RecyclablesStore>();

            services.AddSingleton<IRequestsStore, RequestsStore>();
            services.AddSingleton<IRequestsService, RequestsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GreenCoin API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
