using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using WooDispatch.Common;

namespace WooDispatch
{
    public static class DispatchApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDispatch(
            this IApplicationBuilder builder)
        {
            //创建
            InitnyDB.Current.Create();
            //var sched = builder.ApplicationServices.GetRequiredService<IScheduler>();
            var schedFactory = builder.ApplicationServices.GetRequiredService<ISchedulerFactory>();
            if (schedFactory != null)
            {
                var Scheduler = schedFactory.GetScheduler().GetAwaiter().GetResult();
                Scheduler.JobFactory = builder.ApplicationServices.GetRequiredService<IJobFactory>();
                Scheduler.Start();
            }
            return builder.UseMvc(routes =>
            {

                //routes.MapAreaRoute(
                //  areaName: "Dispatch",
                //  name: "Dispatch",
                //  template: "Dispatch/{controller=Home}/{action=Index}",
                //  defaults: new[] { "WooDispatch" }
                //);
                routes.MapAreaRoute(
                  areaName: "Dispatch",
                  name: "Dispatch",
                  template: "Dispatch/{controller}/{action}",
                  defaults: new { area = "Dispatch", controller = "JobHome" },
                  constraints:null,
                  dataTokens: null
                );

            });

            //namespaces: new[] { "WooDispatch" }
            ////ServiceLocator.Current = builder.ApplicationServices;
            //return builder.Map(pathMatch, app => {
            //    app.UseMiddleware<DispatchMiddleware>();
            //});

        }
    }
}
