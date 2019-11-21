using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AdManagerWebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdManagerWebApp
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

            services.AddDistributedMemoryCache();

            services.AddSession(options => 
            {
                options.Cookie.Name = "UserData";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

            var adConfigSection = Configuration.GetSection("AdConfig");
            var config = adConfigSection.Get<AdConfig>();
            PrincipalContext DomainContext;
            if (string.IsNullOrEmpty(config.Password) || string.IsNullOrEmpty(config.Username))
            {
                DomainContext = new PrincipalContext(ContextType.Domain,
                    config.DomainName, config.Container, ContextOptions.Negotiate);
            }
            else
            {
                DomainContext = new PrincipalContext(ContextType.Domain, 
                    config.DomainName, config.Container, ContextOptions.Negotiate, config.Username, config.Password);
            }
            services.AddSingleton(DomainContext);

            services.AddScoped<SmtpClient>((serviceProvider) =>
            {
                var smtpconfig = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
            {
                Host = smtpconfig.GetValue<String>("EmailConfig:Smtp:Host"),  
                Port = smtpconfig.GetValue<int>("EmailConfig:Smtp:Port"),  
                Credentials = new NetworkCredential(
                smtpconfig.GetValue<String>("EmailConfig:Smtp:Username"),
                smtpconfig.GetValue<String>("EmailConfig:Smtp:Password"))
            };
            });

            services.AddDbContext<ResetContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("ResetContext")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
