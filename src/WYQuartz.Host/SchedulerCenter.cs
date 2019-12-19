using log4net;
using Microsoft.Data.Sqlite;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Simpl;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WYQuartz.Host.Common;
using WYQuartz.Host.Entity;

namespace WYQuartz.Host
{

    /// <summary>
    /// 调度中心
    /// </summary>
    public class SchedulerCenter
    {

        private static readonly ILog log;

        /// <summary>
        /// 任务调度对象
        /// </summary>
        public static readonly SchedulerCenter Instance;
        static SchedulerCenter()
        {
            Instance = new SchedulerCenter();
            log = LogManager.GetLogger("NETCoreRepository", typeof(Program));
        }

        private IScheduler _scheduler;
        /// <summary>
        /// 返回任务计划（调度器）
        /// </summary>
        /// <returns></returns>
        private IScheduler Scheduler
        {
            get
            {
                if (_scheduler != null)
                {
                    return _scheduler;
                }
                //Directory.GetCurrentDirectory()+ @"\db\quartz_storage.db"
                string dbname = "sqliteScheduler";
                var dbpath = $"db/{dbname}.db";
                var dataSource = $"Data Source={dbpath}";
                //如果不存在sqlite数据库，则创建
                if (!File.Exists(dbpath))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(dbpath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(dbpath));
                    using (var connection = new SqliteConnection(dataSource))
                    {
                        connection.OpenAsync().Wait();
                        string sql = File.ReadAllTextAsync("db\\tables_sqlite.sql").Result;
                        var command = new SqliteCommand(sql, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                
                //MySql存储
                //DBConnectionManager.Instance.AddConnectionProvider("default", new DbProvider("MySql", "server=192.168.10.133;user id=root;password=pass;persistsecurityinfo=True;database=quartz"));
                DBConnectionManager.Instance.AddConnectionProvider("default", new DbProvider("SQLite-Microsoft", dataSource));
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
                _scheduler = SchedulerRepository.Instance.Lookup("benny" + "Scheduler").Result;

                _scheduler.Start();//默认开始调度器
                return _scheduler;
            }
        }

        /// <summary>
        /// 获取所有Job（详情信息 - 初始化页面调用）
        /// </summary>
        /// <returns></returns>
        public async Task<List<JobInfo>> GetAllJobsAsync()
        {
            List<JobInfo> jobInfoList = new List<JobInfo>();
            List<JobKey> jboKeyList = new List<JobKey>();
            var groupNames = await Scheduler.GetJobGroupNames();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jboKeyList.AddRange(await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
                //jobInfoList.Add(new JobInfoEntity() { GroupName = groupName });
            }
            foreach (var jobKey in jboKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await Scheduler.GetJobDetail(jobKey);
                var triggersList = await Scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();

                var interval = string.Empty;
                if (triggers is SimpleTriggerImpl)
                    interval = (triggers as SimpleTriggerImpl)?.RepeatInterval.ToString();
                else
                    interval = (triggers as CronTriggerImpl)?.CronExpressionString;


                jobInfoList.Add(new JobInfo()
                {
                    GroupName = jobKey.Group,
                    Name = jobKey.Name,
                    LastErrMsg = jobDetail.JobDataMap.GetString(Constant.EXCEPTION),
                    TriggerCall = jobDetail.JobDataMap.GetString(Constant.TARGETCALL),
                    TriggerState = await Scheduler.GetTriggerState(triggers.Key),
                    PreviousFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime,
                    NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime,
                    BeginTime = triggers.StartTimeUtc.LocalDateTime,
                    Interval = interval,
                    EndTime = triggers.EndTimeUtc?.LocalDateTime,
                    Description = jobDetail.Description
                });
            }
            return jobInfoList;
        }


        /// <summary>
        /// 获取所有Job（详情信息 - 初始化页面调用）
        /// </summary>
        /// <returns></returns>
        public async Task<List<JobInfoEntity>> GetAllJobAsync()
        {
            List<JobKey> jboKeyList = new List<JobKey>();
            List<JobInfoEntity> jobInfoList = new List<JobInfoEntity>();
            var groupNames = await Scheduler.GetJobGroupNames();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jboKeyList.AddRange(await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
                jobInfoList.Add(new JobInfoEntity() { GroupName = groupName });
            }
            foreach (var jobKey in jboKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await Scheduler.GetJobDetail(jobKey);
                var triggersList = await Scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();

                var interval = string.Empty;
                if (triggers is SimpleTriggerImpl)
                    interval = (triggers as SimpleTriggerImpl)?.RepeatInterval.ToString();
                else
                    interval = (triggers as CronTriggerImpl)?.CronExpressionString;

                foreach (var jobInfo in jobInfoList)
                {
                    if (jobInfo.GroupName == jobKey.Group)
                    {
                        jobInfo.JobInfoList.Add(new JobInfo()
                        {
                            Name = jobKey.Name,
                            LastErrMsg = jobDetail.JobDataMap.GetString(Constant.EXCEPTION),
                            TriggerCall = jobDetail.JobDataMap.GetString(Constant.TARGETCALL),
                            TriggerState = await Scheduler.GetTriggerState(triggers.Key),
                            PreviousFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime,
                            NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime,
                            BeginTime = triggers.StartTimeUtc.LocalDateTime,
                            Interval = interval,
                            EndTime = triggers.EndTimeUtc?.LocalDateTime,
                            Description = jobDetail.Description
                        });
                        continue;
                    }
                }
            }
            return jobInfoList;
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
                log.Info("任务调度启动！");
            }
            return Scheduler.InStandbyMode;
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        public async Task<bool> StopScheduleAsync()
        {
            //判断调度是否已经关闭
            if (!Scheduler.InStandbyMode)
            {
                //等待任务运行完成
                await Scheduler.Standby(); //TODO  注意：Shutdown后Start会报错，所以这里使用暂停。
                log.Info("任务调度暂停！");
            }
            return !Scheduler.InStandbyMode;
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<bool> TriggerJobAsync(JobKey jobKey)
        {
            await Scheduler.TriggerJob(jobKey);
            return true;
        }

        /// <summary>
        /// 获取job日志
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<List<string>> GetJobLogsAsync(JobKey jobKey)
        {
            var jobDetail = await Scheduler.GetJobDetail(jobKey);
            return jobDetail.JobDataMap[Constant.LOGLIST] as List<string>;
        }

        /// <summary>
        /// 添加一个工作调度
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ResultValue> AddScheduleJobAsync(ScheduleEntity entity)
        {
            var result = new ResultValue();
            try
            {
                //检查任务是否已存在
                var jobKey = new JobKey(entity.JobName, entity.JobGroup);
                if (await Scheduler.CheckExists(jobKey))
                {
                    result.MsgCode = 3;
                    result.Msg = "任务已存在";
                    return result;
                }

                ////加载程序集(dll文件地址)，使用Assembly类   
                //var asstypes = Assembly.Load("BusDataExchan").GetTypes()
                //    .Where(t => t.GetInterfaces().Contains(typeof(IJob)))
                //    .ToArray();
                
                if (entity.TargetCall.IndexOf(",") < 0)
                {
                    result.MsgCode = 3;
                    result.Msg = "对象名称格式不符合Job-Type";
                    return result;
                }
                var Assemblys = entity.TargetCall.Split(',');
                string dllname = Assemblys[1];
                string jobname = Assemblys[0];

                //获取类型，参数（名称空间+类）   
                Type _ijob = null;
                try
                {
                    _ijob = Assembly.Load(dllname).GetType(jobname);
                }
                catch (Exception err)
                {
                    //日志err
                    result.MsgCode = 3;
                    result.Msg = "未找到描述的对象";
                    return result;
                }

                //配置
                var DataDir = new Dictionary<string, string>()
                {
                    { Constant.TARGETCALL,entity.TargetCall},
                    { Constant.TARGETTYPE, entity.TargetType},
                    //{ Constant.MAILMESSAGE, ((int)entity.MailMessage).ToString()},
                };
                if (_ijob == null)
                {
                    result.MsgCode = 3;
                    result.Msg = "未找到调度器";
                    return result;
                }

                // 定义这个工作，并将其绑定到我们的IJob实现类                
                IJobDetail job = JobBuilder.Create(_ijob)
                    .SetJobData(new JobDataMap(DataDir))
                    .WithDescription(entity.Description)
                    .WithIdentity(entity.JobName, entity.JobGroup)
                    .Build();
                // 创建触发器
                ITrigger trigger;

                entity.TriggerType = TriggerTypeEnum.Cron;

                //校验是否正确的执行周期表达式
                if (entity.TriggerType == TriggerTypeEnum.Cron)//CronExpression.IsValidExpression(entity.Cron))
                {
                    trigger = CreateCronTrigger(entity);
                }
                else
                {
                    trigger = CreateSimpleTrigger(entity);
                }

                // 告诉Quartz使用我们的触发器来安排作业
                await Scheduler.ScheduleJob(job, trigger);
                result.MsgCode = 1;
                result.Msg = "添加成功";
            }
            catch (Exception ex)
            {
                result.MsgCode = 3;
                result.Msg = ex.Message;
            }
            return result;
        }



        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(ScheduleEntity entity)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(entity.JobName, entity.JobGroup)
                   .StartAt(entity.BeginTime)//开始时间
                   .EndAt(entity.EndTime)//结束时间
                   .WithCronSchedule(entity.Cron)//指定cron表达式
                   .ForJob(entity.JobName, entity.JobGroup)//作业名称
                   .Build();
        }

        /// <summary>
        /// 创建类型Simple的触发器
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(ScheduleEntity entity)
        {
            //作业触发器
            if (entity.RunTimes.HasValue && entity.RunTimes > 0)
            {
                return TriggerBuilder.Create()
               .WithIdentity(entity.JobName, entity.JobGroup)
               .StartAt(entity.BeginTime)//开始时间
               .EndAt(entity.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(entity.IntervalSecond.Value)//执行时间间隔，单位秒
                   .WithRepeatCount(entity.RunTimes.Value))//执行次数、默认从0开始
                   .ForJob(entity.JobName, entity.JobGroup)//作业名称
               .Build();
            }
            else
            {
                return TriggerBuilder.Create()
               .WithIdentity(entity.JobName, entity.JobGroup)
               .StartAt(entity.BeginTime)//开始时间
               .EndAt(entity.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(entity.IntervalSecond.Value)//执行时间间隔，单位秒
                   .RepeatForever())//无限循环
                   .ForJob(entity.JobName, entity.JobGroup)//作业名称
               .Build();
            }

        }


        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务分组</param>
        public async Task<ResultValue> ResumeJobAsync(string jobGroup, string jobName)
        {
            ResultValue result = new ResultValue();
            try
            {
                //检查任务是否存在
                var jobKey = new JobKey(jobName, jobGroup);
                if (await Scheduler.CheckExists(jobKey))
                {
                    //任务已经存在则暂停任务
                    await Scheduler.ResumeJob(jobKey);
                    result.Msg = "恢复任务计划成功！";
                    result.MsgCode = 1;
                    log.Info(string.Format("任务“{0}”恢复运行", jobName));
                }
                else
                {
                    result.Msg = "任务不存在";
                    result.MsgCode = 2;
                }
            }
            catch (Exception ex)
            {
                result.Msg = "恢复任务计划失败！";
                result.MsgCode = 2;
                log.Error(string.Format("恢复任务失败！{0}", ex));
            }
            return result;
        }

        /// <summary>
        /// 暂停/删除 指定的计划
        /// </summary>
        /// <param name="jobGroup">任务分组</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="isDelete">停止并删除任务</param>
        /// <returns></returns>
        public async Task<ResultValue> StopOrDelScheduleJobAsync(string jobGroup, string jobName, bool isDelete = false)
        {
            ResultValue result;
            try
            {
                await Scheduler.PauseJob(new JobKey(jobName, jobGroup));
                if (isDelete)
                {
                    await Scheduler.DeleteJob(new JobKey(jobName, jobGroup));
                    result = new ResultValue
                    {
                        MsgCode = 1,
                        Msg = "删除任务计划成功！"
                    };
                }
                else
                {
                    result = new ResultValue
                    {
                        MsgCode = 1,
                        Msg = "停止任务计划成功！"
                    };
                }

            }
            catch (Exception ex)
            {
                result = new ResultValue
                {
                    MsgCode = 2,
                    Msg = "停止任务计划失败"
                };
            }
            return result;
        }
    }
}