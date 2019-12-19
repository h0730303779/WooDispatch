using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using WooDispatch.Common;
using WooDispatch.DispatchBuilder;
using WooDispatch.Entity;
using WooDispatch.Service;

namespace WooDispatch
{
    public static class DispatchServiceCollectionExtensions
    {
        public static IDispatchBuilder AddDispatch(this IServiceCollection services, 
            Action<DispatchOptions> func = null)
        {
            var builder = new DefaultLogDashboardBuilder()
            {
                Services = services,
                DispatchOptions = new DispatchOptions()
            };
            func?.Invoke(builder.DispatchOptions);
            //RegisterServices(services, func);
            //var builder = new QuartzOptionsBuilder
            //{
            //    Services = services,
            //    Properties = new NameValueCollection { { "quartz.jobStore.useProperties", "true" } }
            //};
            //func.

            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //if (func == null)
            //{
            //    throw new ArgumentNullException(nameof(func));
            //}


            

            services.Configure(func);

            //加载默认html页
            



            //services.AddTransient<Jobs.HttpJob>();
            services.AddSingleton<ISchedulerService, SchedulerService>();
            services.AddSingleton<IHttpService, HttpClientService>();
            services.AddSingleton<IJobFactory, JobFactory>();


            
            services.AddSingleton<ISchedulerFactory>(provider => new StdSchedulerFactory(UseSqlite()));
            //func = func ?? ((o) => {

            if(builder.DispatchOptions.IsStaticFile)
                services.ConfigureOptions(typeof(UIConfigureOptions));


            if (builder.DispatchOptions.ListDll != null && builder.DispatchOptions.ListDll.Count > 0)
            {
                foreach (var item in builder.DispatchOptions.ListDll)
                {
                    //Assembly asm1 = Assembly.LoadFile(fileName1);
                    IocJob(services, Assembly.Load(item));//
                }
            }
            var asmlWooDispatch = Assembly.GetExecutingAssembly();
            IocJob(services, asmlWooDispatch);//

            var asmlThat = Assembly.GetEntryAssembly();
            IocJob(services, asmlThat);//
            //});
            //if (func == null) RDisOps(services, new DispatchOptions());
            //else
            //{
            //    func((o)=> { })
            //    RDisOps(services, new DispatchOptions());
            //}
            //if (IsStaticFile)
            //    services.ConfigureOptions(typeof(UIConfigureOptions));


            return builder;
        }
        private static void IocJob(IServiceCollection services , Assembly asml)
        {
            if (asml == null)
                throw new Exception("injection dllname  is null");
            List<Type> typeList = new List<Type>();  //所有符合注册条件的类集合

            var types = asml.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob)))
                //.SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob))))
                .ToArray();
            if (types != null && types.Count() > 0)
                typeList.AddRange(types);

            foreach (var type in typeList)
            {
                services.AddTransient(type);
            }
        }


        //private static void IocthisModule(IServiceCollection services)
        //{
        //    Assembly asm1 = Assembly.LoadFile(fileName1);
        //    List<Type> typeList = new List<Type>();  //所有符合注册条件的类集合

        //    var types = AppDomain.CurrentDomain.GetAssemblies()
        //        .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob))))
        //        .ToArray();
        //    if (types != null && types.Count() > 0)
        //        typeList.AddRange(types);
            
            

        //}

        private static NameValueCollection UseSqlite()
        {
            return new NameValueCollection()
            {
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.useProperties"] = "true",
                ["quartz.jobStore.dataSource"] = "default",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz",
                ["quartz.dataSource.default.provider"] = "SQLite-Microsoft", 
                ["quartz.dataSource.default.connectionString"] = InitnyDB.dataSource,
                ["quartz.serializer.type"] = "json",
            };
        }

        private static NameValueCollection UseSqlServer()
        {
            return new NameValueCollection()
            {
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.useProperties"] = "false",
                ["quartz.jobStore.dataSource"] = "default",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
                ["quartz.dataSource.default.provider"] = "SqlServer-41", // SqlServer-41 is the new provider for .NET Core
                ["quartz.dataSource.default.connectionString"] = @"Server=(localdb)\MSSQLLocalDB;Database=Quartz;Integrated Security=true",
                ["quartz.serializer.type"] = "json",
            };
        }

        //public static DispatchOptions UseSqlServer(this DispatchOptions builder, string connectString,
        //    string serializerType = "binary", string tablePrefix = "QRTZ_")
        //{
        //    builder.Properties.Set("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
        //    builder.Properties.Set("quartz.jobStore.driverDelegateType",
        //        "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz");
        //    builder.Properties.Set("quartz.jobStore.dataSource", "myDs");
        //    builder.Properties.Set("quartz.dataSource.myDs.provider", "SqlServer");
        //    builder.Properties.Set("quartz.jobStore.tablePrefix", tablePrefix);
        //    builder.Properties.Set("quartz.serializer.type", serializerType);
        //    builder.Properties.Set("quartz.dataSource.myDs.connectionString", connectString);
        //    return builder;
        //}
    }
}
