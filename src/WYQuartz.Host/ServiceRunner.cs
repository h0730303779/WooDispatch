using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Topshelf;

namespace WYQuartz.Host
{
    public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {

        private IWebHost webHost;
        private static ILog logger = LogManager.GetLogger(typeof(ServiceRunner));

        public ServiceRunner()
        {
            //ISchedulerFactory sf = new StdSchedulerFactory(QuartzInit.InitDbStore());
            //scheduler = sf.GetScheduler().GetAwaiter().GetResult();

        }

        public bool Start(HostControl hostControl)
        {
            webHost = WebHost.CreateDefaultBuilder().UseConfiguration(AppConfig.Configuration).UseStartup<Startup>().Build();
            webHost.RunAsync();
            //scheduler.Start();
            logger.Info("服务启动！");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //scheduler.Shutdown(false);
            webHost.StopAsync();
            logger.Info("服务停止！");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Continue(HostControl hostControl)
        {

            webHost.StartAsync();
            return true;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Pause(HostControl hostControl)
        {
            //scheduler.PauseAll();
            webHost.WaitForShutdownAsync();
            return true;
        }
    }
}