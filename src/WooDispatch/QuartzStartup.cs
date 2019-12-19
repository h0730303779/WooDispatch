using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Simpl;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WooDispatch
{
    public class QuartzStartup
    {
        public IScheduler Scheduler;
        public ISchedulerFactory schedulerFactory;

        public QuartzStartup(ISchedulerFactory _schedulerFactory)
        {
            //scheduler = _scheduler;
            schedulerFactory = _schedulerFactory;
        }


        /// <summary>
        /// 开启调度器
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartScheduleAsync()
        {
            //开启调度器
            if (Scheduler.InStandbyMode)
            {
                await Scheduler.Start();
                //log.Info("任务调度启动！");
            }
            return Scheduler.InStandbyMode;
        }

        public async Task Inity()
        {
            //1、声明一个调度工厂
            //2、通过调度工厂获得调度器
            Scheduler = await schedulerFactory.GetScheduler();

            DBConnectionManager.Instance.AddConnectionProvider("default", new DbProvider("SQLite-Microsoft", SqliteHelper.dataSource));
            var serializer = new JsonObjectSerializer();
            serializer.Initialize();
            var jobStore = new JobStoreTX
            {
                DataSource = "default",
                TablePrefix = "QRTZ_",
                InstanceId = "AUTO",
                //DriverDelegateType = typeof(MySQLDelegate).AssemblyQualifiedName, //MySql存储
                DriverDelegateType = typeof(SQLiteDelegate).AssemblyQualifiedName,  //SQLite存储
                ObjectSerializer = serializer
            };
            DirectSchedulerFactory.Instance.CreateScheduler("benny" + "Scheduler", "AUTO", new DefaultThreadPool(), jobStore);
            Scheduler = SchedulerRepository.Instance.Lookup("benny" + "Scheduler").Result;

            //scheduler.Start();

            //默认开始调度器
            await Scheduler.Start();
            
            //return await Task.FromResult("将触发器和任务器绑定到调度器中完成");
        }
    }
}
