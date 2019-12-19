using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WYDispatch.Samples.Models;
using Microsoft.Extensions.Logging;
using NLog;

namespace WYDispatch.Samples.Controllers
{
    public class HomeController : Controller
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger(); //获得日志实;;
        public IActionResult Index()
        {
            return Redirect("/Dispatch/JobHome");
        }

        
    }
}
