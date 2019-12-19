using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Quartz;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Alexinea.Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Topshelf;
using Topshelf.Autofac;

namespace WYDispatch.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            //var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());
            var Configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("Conf/appsettings.json", optional: true, reloadOnChange: true)
                     .Build();




            var builder = new ContainerBuilder();
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            //builder.RegisterModule(new QuartzAutofacJobsModule(typeof(BackupJob).Assembly));

            //builder.Populate(Configuration);

            var container = builder.Build();

            if (isService)
            {

                HostFactory.Run(x =>
                {
                    x.UseAutofacContainer(container);
                    x.Service<ServiceCenter>(setting => {

                        setting.ConstructUsingAutofacContainer();
                        setting.ConstructUsing(name => new ServiceCenter(container));
                    });
                    x.RunAsLocalSystem();
                    x.SetDescription("WYDispatch调度示例");//Configuration["WinService:Description"]
                    x.SetDisplayName("WYDispatch调度示例");//Configuration["WinService:DisplayName"]
                    x.SetServiceName("WYDispatch调度示例");//AppConfig.Configuration["WinService:ServiceName"]

                    x.EnablePauseAndContinue();
                });
            }
            else
            {
                //ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
                //XmlConfigurator.Configure(repository, new FileInfo(Directory.GetCurrentDirectory() + "\\Conf\\log4net.config"));

                CreateWebHostBuilder(args).Build().Run();
            }
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build())
                .UseStartup<Startup>();
    }
}
