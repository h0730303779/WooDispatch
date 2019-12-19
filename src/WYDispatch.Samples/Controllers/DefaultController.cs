using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WYDispatch.Samples.Models;

namespace WYDispatch.Samples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public ErrorViewModel Get()
        {
            return new ErrorViewModel()
            {
                RequestId = System.Guid.NewGuid().ToString(),
                
            };
        }

    }
}