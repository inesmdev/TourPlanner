using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TourPlanner.Api.Services.TourService;
using TourPlanner.Api.Services.MapQuestService;
using TourPlanner.Api.Services.ReportService;
using TourPlanner.Api.Services.TourLogService;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;
using TourPlanner.Api.Services.ImportService;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace TourPlanner.Api
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
            services.AddSingleton<ITourService, TourService>(); 
            services.AddSingleton<ITourRepository, TourRepository>();
            services.AddSingleton<ITourLogRepository, TourLogRepository>();
            services.AddSingleton<IMapQuestService, MapQuestService>(); 
            services.AddSingleton<IReportService, ReportService>();
            services.AddSingleton<PostgresAccess>();
            services.AddSingleton<ITourLogService, TourLogService>();
            services.AddSingleton<IImportService, ImportService>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TourPlanner.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TourPlanner.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/StaticFiles",
                EnableDefaultFiles = true
            });
        }
    }
}
