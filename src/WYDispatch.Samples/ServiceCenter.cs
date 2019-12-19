using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Topshelf;

namespace WYDispatch.Samples
{

    public sealed class ServiceCenter : ServiceControl, ServiceSuspend
    {
        private IWebHost webHost;
        private IConfigurationRoot Configuration;
        private IContainer Container;

        public ServiceCenter(IContainer container){
            Container = container;
        }




        public bool Start(HostControl hostControl)
        {
            webHost = WebHost.CreateDefaultBuilder().UseConfiguration(Configuration).UseStartup<Startup>().Build();
            webHost.RunAsync();
            ////scheduler.Start();
            //logger.Info("服务启动！");
            //if (!logger.IsInfoEnabled)
            //{
            //    throw new System.Exception("手动检查日志没有加载");
            //}
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            //scheduler.Shutdown(false);
            webHost.StopAsync();
            //logger.Info("服务停止！");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostControl"></param>
        /// <returns></returns>
        public bool Continue(HostControl hostControl)
        {

            //webHost.StartAsync();
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
            //webHost.WaitForShutdownAsync();
            return true;
        }
    }
}
