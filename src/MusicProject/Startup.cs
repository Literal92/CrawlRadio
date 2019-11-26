using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using MusicProject.Services.Identity.Logger;
using MusicProject.ViewModels.Identity.Settings;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicProject.IocConfig;
using MusicProject.DataLayer.Context;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace MusicProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAntiforgery(x => x.HeaderName = "X-XSRF-TOKEN");
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            //});

            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            services.Configure<SiteSettings>(options => Configuration.Bind(options));
            //services.Configure<MvcOptions>(options =>
            //{
            //  options.Filters.Add(new RequireHttpsAttribute());
            //});

            // services.Configure<AntiDosConfig>(options => Configuration.GetSection("AntiDosConfig").Bind(options));
            // Adds all of the ASP.NET Core Identity related services and configurations at once.
            services.AddCustomIdentityServices();

            var siteSettings = services.GetSiteSettings();
            services.AddRequiredEfInternalServices(siteSettings); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.SetDbContextOptions(siteSettings);
                optionsBuilder.UseInternalServiceProvider(serviceProvider); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });



            services.AddAuthentication()
            .AddCookie(cfg => cfg.SlidingExpiration = true)
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
            ,
                    ValidateLifetime = true,
                    //  ClockSkew = TimeSpan.FromDays(1) //5 minute tolerance for the expiration date
                };

            });



            //services.AddHttpsRedirection(options =>
            //{
            //  options.RedirectStatusCode = StatusCodes.Status302Found;

            //});


            services.AddMvc(options =>
            {
                options.UseYeKeModelBinder();
                options.AllowEmptyInputInBodyModelBinding = true;
                // options.Filters.Add(new NoBrowserCacheAttribute());
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);




            services.AddAutoMapper();
            services.AddDNTCommonWeb();
            services.AddDNTCaptcha();
            services.AddCloudscribePagination();
        }

        public void Configure(
                ILoggerFactory loggerFactory,
                IApplicationBuilder app,
                IHostingEnvironment env)
        {
            /*

            app.UseRewriter(new RewriteOptions().Add(ctx =>
            {
                // checking if the hostName has www. at the beginning
                var req = ctx.HttpContext.Request;
                var hostName = req.Host;
                if (hostName.ToString().EndsWith("mousigha.ir"))
                {
                    
                    var newHostName = hostName.ToString().Substring(0, hostName.ToString().Length - 3) + ".com";
                    string newUrl = "";

                    newUrl = "https://" + newHostName + req.PathBase + req.Path + req.QueryString;


                    var response = ctx.HttpContext.Response;

                    response.StatusCode = 301;


                    response.Redirect(newUrl);

                }
                if (hostName.ToString().EndsWith("mousigha.com")&& req.Scheme=="http")
                {

                    var newHostName = hostName;
                    string newUrl = "";

                    newUrl = "https://" + newHostName + req.PathBase + req.Path + req.QueryString;


                    var response = ctx.HttpContext.Response;

                    response.StatusCode = 301;


                    response.Redirect(newUrl);

                }

            }));


            */
            loggerFactory.AddDbLogger(serviceProvider: app.ApplicationServices, minLevel: LogLevel.Warning);


            if (!env.IsDevelopment())
            {
              //  app.UseHsts();
            }


         //   app.UseHttpsRedirection();

            //app.UseAntiDos();


            app.UseExceptionHandler("/error/index/500");
            app.UseStatusCodePagesWithReExecute("/error/index/{0}");

            // Serve wwwroot as root


            app.UseFileServer(new FileServerOptions
            {
                // Don't expose file system
                EnableDirectoryBrowsing = false
            });


            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".apk"] = "application/vnd.android.package-archive";

            provider.Mappings[".webmanifest"] = "application/manifest+json";


            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });


            // Adds all of the ASP.NET Core Identity related initializations at once.
            app.UseCustomIdentityServices();
            app.UseCookiePolicy();
            app.UseRewriter(new RewriteOptions()
           //   .AddRedirectToWww()

            //  .AddRedirectToHttps()
            );
            app.UseMvc(routes =>
                        {
                            routes.MapRoute(
                          name: "areas",
                          template: "{area:exists}/{controller=Account}/{action=Index}/{id?}");

                            routes.MapRoute(
                          name: "default",
                          template: "{controller=Home}/{action=Index}/{id?}");
                        });
        }
    }
}