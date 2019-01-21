using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MergeRequestService.Data;
using MergeRequestService.Models;
using MergeRequestService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MergeRequestService
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

//            services.AddHangfire(config =>
//                config.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>() //todo disable register
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<MailMessageConfig>(Configuration.GetSection(nameof(MailMessageConfig)));
            services.Configure<MailServerConfig>(Configuration.GetSection(nameof(MailServerConfig)));

            services.AddTransient<IMergeRequestMailSender, MergeRequestMailSender>();
            services.AddTransient<IMergeRequestMailGenerator, MergeRequestMailGenerator>();
            //todo use html template
            services.AddTransient<IMailTemplate, MailHtmlTemplate>();
            services.AddTransient<ITargetBranchListFactory, TargetBranchListFactory>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

//            app.UseHangfireDashboard();
//            app.UseHangfireServer();
            //todo hangfire is admin only 

            UserDataInitializer.SeedData(userManager, roleManager);
        }
    }
}