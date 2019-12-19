using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using WYQuartz.Host.Common;
using WYQuartz.Host.Entity;

namespace WYQuartz.Host.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private SchedulerCenter scheduler;
        public JobsController()
        {
            scheduler = SchedulerCenter.Instance;
        }


        [HttpGet]
        public async Task<List<JobInfo>> Get()
        {
            return await scheduler.GetAllJobsAsync();
        }

        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <returns></returns>
        [HttpGet("startSchedule")]
        public async Task<bool> StartSchedule()
        {
            return await scheduler.StartScheduleAsync();
        }

        //
        /// <summary>
        /// 停止调度器
        /// </summary>
        /// <returns></returns>
        [HttpGet("stopSchedule")]
        public async Task<bool> StopSchedule()
        {
            return await scheduler.StopScheduleAsync();
        }

        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary> 
        /// <returns></returns>
        [HttpPost("jobrun")]
        public async Task<ResultValue> ResumeJob([FromBody]JobKey job)
        {
            return await scheduler.ResumeJobAsync(job.Group, job.Name);
        }

        /// <summary>
        /// 暂停 指定的计划
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPost("jobstop")]
        public async Task<ResultValue> JobStop([FromBody]JobKey job)
        {

            return await scheduler.StopOrDelScheduleJobAsync(job.Group, job.Name, false);
        }


        /// <summary>
        /// 删除 指定的计划
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPost("jobdelete")]
        public async Task<ResultValue> JobDelete([FromBody]JobKey job)
        {

            return await scheduler.StopOrDelScheduleJobAsync(job.Group, job.Name, true);
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("addjob")]
        public async Task<ResultValue> AddJob([FromBody]ScheduleEntity entity)
        {
            return await scheduler.AddScheduleJobAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("modifyJob")]
        public async Task<ResultValue> ModifyJob([FromBody]ScheduleEntity entity)
        {
            await scheduler.StopOrDelScheduleJobAsync(entity.JobGroup, entity.JobName, true);
            await scheduler.AddScheduleJobAsync(entity);
            return new ResultValue() { Msg = "修改计划任务成功！",MsgCode = 1 };
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpPost("TriggerJob")]
        public async Task<bool> TriggerJob([FromBody]JobKey job)
        {
            await scheduler.TriggerJobAsync(job);
            return true;
        }



        ///// <summary>
        ///// 获取所有任务
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async  Task<List<JobInfo>> GetAllJob()
        //{
        //    return await scheduler.GetAllJobsAsync();
        //}


    }
}