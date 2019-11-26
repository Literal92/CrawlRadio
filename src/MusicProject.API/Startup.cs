using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicProject.DataLayer.Context;
using MusicProject.ViewModels.Identity.Settings;
using MusicProject.IocConfig;


namespace MusicProject.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    //public IConfiguration Configuration { get; }

    //// This method gets called by the runtime. Use this method to add services to the container.
    //public void ConfigureServices(IServiceCollection services)
    //{
    //  services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    //}



    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {

   
      services.Configure<SiteSettings>(options => Configuration.Bind(options));
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

      services.AddDNTCommonWeb();
 
      services.AddCloudscribePagination();
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
      //  app.UseHsts();
      }

      //app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}
