using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Server.Model;
using Server.Services;

namespace Server
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
            services.AddAuthorization();
            services.AddControllers();
           
         
            services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("ApplicationContext"));
            });
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISubDepartmentService, SubDepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICommomService, CommonService>();
            services.AddScoped<IThankCardsService, ThankCardsService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddRazorPages();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
