using log4net;
using Quartz;
using Quartz.Logging;
using System;
using System.Threading.Tasks;
using WooDispatch.Service;

namespace BusDataExchan
{
    public class SimpleJob : IJob
    {
        //private static ILog logger = LogManager.GetLogger("NETCoreRepository",typeof(SimpleJob));
        //private IHttpService schedulerService;
        //public SimpleJob(IHttpService Service)
        //{
        //    schedulerService = Service;
        //}
        public Task Execute(IJobExecutionContext context)
        {
            //logger.Info("执行Job!");
            //var ss = schedulerService.ToString();
            return Task.CompletedTask;
        }


    }
}
