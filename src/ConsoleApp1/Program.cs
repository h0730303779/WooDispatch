using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace WooDispatch.Service.Sampels
{
    class Program
    {
        static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            //var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());
            var Configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("Conf/appsettings.json", optional: true, reloadOnChange: true)
                     .Build();
            if (isService)
            {

                HostFactory.Run(x =>
                {
                    x.Service<ServiceRunner>();
                    x.RunAsLocalSystem();
                    x.SetDescription(AppConfig.Configuration["WinService:Description"]);
                    x.SetDisplayName(AppConfig.Configuration["WinService:DisplayName"]);
                    x.SetServiceName(AppConfig.Configuration["WinService:ServiceName"]);

                    x.EnablePauseAndContinue();
                });
            }
            else
            {
                //ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
                //XmlConfigurator.Configure(repository, new FileInfo(Directory.GetCurrentDirectory() + "\\Conf\\log4net.config"));

                CreateWebHostBuilder(args).Build().Run();
            }
        }
    }
}
