using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WooDispatch.Common;
using WooDispatch.Entity;

namespace WooDispatch.Service
{
    /// <summary>
    /// Quartz Scheduler 操作服务
    /// </summary>
    public interface ISchedulerService
    {
        /// <summary>
        /// 获取所有Job（详情信息 - 初始化页面调用）
        /// </summary>
        /// <returns></returns>
        Task<List<JobInfo>> GetAllJobsAsync();

        /// <summary>
        /// 获取所有Job（详情信息 - 初始化页面调用）
        /// </summary>
        /// <returns></returns>
        Task<List<JobInfoEntity>> GetAllJobAsync();


        /// <summary>
        /// 开启调度器
        /// </summary>
        /// <returns></returns>
        Task<bool> StartScheduleAsync();


        /// <summary>
        /// 停止任务调度
        /// </summary>
        Task<bool> StopScheduleAsync();

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        Task<bool> TriggerJobAsync(JobKey jobKey);

        /// <summary>
        /// 获取job日志
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        Task<List<string>> GetJobLogsAsync(JobKey jobKey);

        /// <summary>
        /// 获取任务对象
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        Task<ScheduleEntity> GetTaskEntiy(JobKey jobKey);


        /// <summary>
        /// 添加一个工作调度
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ResultValue> AddScheduleHttpJobAsync(ScheduleEntity entity);


        /// <summary>
        /// 添加一个工作调度
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<ResultValue> AddScheduleJobAsync(ScheduleEntity entity);



        // <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务分组</param>
        Task<ResultValue> ResumeJobAsync(string jobGroup, string jobName);


        /// <summary>
        /// 暂停/删除 指定的计划
        /// </summary>
        /// <param name="jobGroup">任务分组</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="isDelete">停止并删除任务</param>
        /// <returns></returns>
        Task<ResultValue> StopOrDelScheduleJobAsync(string jobGroup, string jobName, bool isDelete = false);


    }
}
