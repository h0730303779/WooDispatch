using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WYQuartz.Host.Entity;
using WYQuartz.Host.Models;

namespace WYQuartz.Host.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ///// <summary>
        ///// 查询任务
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<ScheduleEntity> QueryJob([FromBody]JobKey job)
        //{
        //    return await scheduler.QueryJobAsync(job.Group, job.Name);
        //}


        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<JobInfoEntity>> GetAllJob()
        {
            var jobs = new JobInfoEntity
            {
                GroupName = "name",
                JobInfoList = null,
            };

            return  new List<JobInfoEntity>() { jobs };
        }
    }
}
