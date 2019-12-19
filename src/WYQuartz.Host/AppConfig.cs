using System;
using Microsoft.Extensions.Configuration;

namespace WYQuartz.Host
{
    public class AppConfig
    {
        public static IConfigurationRoot Configuration { get; set; }
    }
}
