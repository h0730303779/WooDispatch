using Microsoft.AspNetCore.Mvc;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;
using WooDispatch.Service;
using WooDispatch.Common;
using WooDispatch.Entity;

namespace WooDispatch.Controllers
{
    [Route("Dispatch/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ISchedulerService scheduler;
        public JobsController(ISchedulerService schedulerService)
        {
            scheduler = schedulerService;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
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
        /// 获取单个任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTaskEntiy")]
        public async Task<ScheduleEntity> GetTaskEntiy([FromBody]JobKey job)
        {

            return await scheduler.GetTaskEntiy(job);
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
            return await DistributeJob(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task<ResultValue> DistributeJob(ScheduleEntity entity)
        {
            if (string.IsNullOrEmpty(entity.TargetType))
            {
                return new ResultValue()
                {
                    MsgCode = 2,
                    Msg = "调用类型不能为空"
                };
            }

            if (entity.TargetType == "1")
                return await scheduler.AddScheduleHttpJobAsync(entity);
            else if (entity.TargetType == "2")
                return await scheduler.AddScheduleJobAsync(entity);
            else
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
            var Result = ResultValue.Current;
            await scheduler.StopOrDelScheduleJobAsync(entity.JobGroup, entity.JobName, true);
            Result = await DistributeJob(entity);
            if (Result.MsgCode == 1)
            {
                Result.Msg = "修改计划任务成功";
                return Result;
            }
            else
            {
                return Result;
            }
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