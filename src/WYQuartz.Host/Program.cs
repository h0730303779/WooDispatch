using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Topshelf;

namespace WYQuartz.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {

            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Conf\\log4net.config"));


            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            //var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

            AppConfig.Configuration = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("Conf/appsettings.json", optional: true, reloadOnChange: true)
                      .Build();
            ILog log = LogManager.GetLogger(repository.Name, typeof(Program));
            log.Info("NETCorelog4net log");
            log.Info("test log");
            if (isService)
            {
                HostFactory.Run(x =>
                {
                    x.Service<ServiceRunner>();

                    x.SetDescription(AppConfig.Configuration["WinService:Description"]);
                    x.SetDisplayName(AppConfig.Configuration["WinService:DisplayName"]);
                    x.SetServiceName(AppConfig.Configuration["WinService:ServiceName"]);

                    x.EnablePauseAndContinue();
                });
            }
            else
            {

                CreateWebHostBuilder(args).Build().Run();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(AppConfig.Configuration)
                //.UseKestrel
                .UseStartup<Startup>();
    }
}
