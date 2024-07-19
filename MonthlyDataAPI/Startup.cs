using Data;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MonthlyDataAPI
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
            services.AddControllers();

            services.AddSingleton(Configuration);

            services.AddApplicationInsightsTelemetry();

            services.AddMemoryCache();

            services.AddScoped<IAzureSQLDB, AzureSQLDB>();
            services.AddScoped<IProcessMonthlyDataUsage, ProcessMonthlyDataUsage>();

            services.AddControllers(option => option.EnableEndpointRouting = false);
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",

                builder => builder.AllowAnyOrigin()

                .AllowAnyMethod()

                .AllowAnyHeader());
            });
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

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
