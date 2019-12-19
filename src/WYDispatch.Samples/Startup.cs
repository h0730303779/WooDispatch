using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
//using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WooDispatch;

namespace WYDispatch.Samples
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
            services.AddDispatch(o=> {
                o.IsStart = false;
                o.IsStaticFile = true;
                o.ListDll = new List<string>() { "BusDataExchan"};
            });


            //
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region Swagger

            //services.AddSwaggerGen(options =>
            //{
            //    //var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
            //    //options.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。

            //    options.SwaggerDoc("v1", new Info
            //    {
            //        Version = "Version 1.0",
            //        Title = "WYDispatch调度器接口",
            //        Description = ""
            //    });
            //    //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, "WYDispatch.Samples.xml");
            //    options.IncludeXmlComments(xmlPath);

            //    //options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //    //{
            //    //    Description = "JWT Bearer 授权 \"Authorization:     Bearer+空格+token\"",
            //    //    Name = "Authorization",
            //    //    In = "header",
            //    //    Type = "apiKey"
            //    //});

            //}); 
            #endregion
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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


            #region Nlog配置

            //使用NLog作为日志记录工具
            //env.ConfigureNLog("Conf/Nlog.config");
            //注入上下文连接Nlog
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            LogManager.Configuration.Variables["connectionString"] = WooDispatch.Common.InitnyDB.dataSource;

            #endregion

            #region UseSwagger

            //app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    //c.RoutePrefix = "swagger/ui";
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreApi");
            //}); 
            #endregion
            //app.UseMvc();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseDispatch();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Run(ctx =>
            //{
            //    ctx.Response.Redirect("/Dispatch"); //可以支持虚拟路径或者index.html这类起始页.
            //    return Task.FromResult(0);
            //});
        }
    }
}
