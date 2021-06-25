using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AVATI.Data;
using AVATI.Data.DatabaseConnection;
using AVATI.Data.EmployeeDetailFiles;
using AVATI.Pages;
using AVATI.Pages.Project;
using BlazorDownloadFile;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AVATI
{
    public class Startup
    {
        private JsonImport _import = new JsonImport();
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _import.ImportJsonFile();
            _import.JsonFileToDatabase(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazorContextMenu();
            services.AddSingleton<SearchService>();
            services.AddSingleton<IHardskillService, HardskillService>();
            services.AddSingleton<IBasicDataService, BasicDataService>();
            services.AddSingleton<ProjectServiceSimple>();
            services.AddSingleton<IProjektService, ProjectServiceSimple>();
            services.AddSingleton<JsonImport>();
            services.AddSingleton<Projectedit>();
            services.AddSingleton<ILoginService,LoginServiceSimple>();
            services.AddSingleton<IProposalService, ProposalService>();
            services.AddSingleton<SearchService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddBlazorDownloadFile();
            services.AddSingleton<IEmployeeDetailService, EmployeeDetailService>();
            services.AddSingleton<IProjectActivityService, ProjectActivityServiceSimple>();
            services.AddSingleton<ProjectActivityServiceSimple>();
            services.AddSingleton<EmployeeServiceSimple>();
            services.AddSingleton<ProjectActivityServiceSimple>();
            services.AddSingleton<DatabaseUtils>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}