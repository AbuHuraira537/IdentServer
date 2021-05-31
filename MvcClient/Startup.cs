using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
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
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = "https://localhost:44386/";
               
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_serect_mvc";
                    config.Scope.Add("offline_access");
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                    //Deleting extra claims whcih due to profile scope
                    //config.ClaimActions.DeleteClaim("amr");
                    //config.ClaimActions.DeleteClaim("name");

                    //this is the first method to load claims 
                    //this will load claims in cookie
                    //configure cookie claim mapping
                    //you can map as many claims as you want
                    //config.ClaimActions.MapUniqueJsonKey("myclaimscookiemapping", "my.claim");
                    //geting all claims from user to get claims it go to user end point to get claims en
                    //two trips to load claims in cookie.not in id_token
                    //config.GetClaimsFromUserInfoEndpoint = true;


                    //this is othermethod to load claims in 
                    //this will load claims in idToken
                    //configura custome scope
                    //config.Scope.Clear();
                    //config.Scope.Add("user.scope");
                    //config.Scope.Add("openid");
                    //config.Scope.Add("ApiOne");
                    //config.Scope.Add("offline_scopre");
                    //you can clear scopes also

                    //config.Scope.Clear();

                    //how to add scope


                    //to call api you must add scope of that api
                    // config.Scope.Add("ApiOne");
                    //config.Scope.Add("offline_access");
                    
                   // config.RequireHttpsMetadata = false;

                });
            services.AddHttpClient();
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
