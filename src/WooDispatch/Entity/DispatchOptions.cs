using System;
using System.Collections.Generic;
using System.Text;

namespace WooDispatch.Entity
{
    public class DispatchOptions
    {

        /// <summary>
        /// 预装html静态资源是否装载
        /// </summary>
        public bool IsStaticFile { get; set; } = true;

        /// <summary>
        /// 调度启动器是否启动 默认true
        /// </summary>
        public bool IsStart { get; set; } = true;


        public List<string> ListDll { get; set; }


        public DispatchOptions()
        {
            //Brand = "Log Dashboard";
            //CustomPropertyInfos = new List<PropertyInfo>();
            //FileSource = true;
            //FileFieldDelimiter = "||";
            //FileEndDelimiter = "||end";
            //PathMatch = "/LogDashboard";
            //LogModelType = typeof(LogModel);
            //#if NETSTANDARD2_0
            //            AuthorizeData = new List<IAuthorizeData>();
            //#endif
            //            AuthorizationFiles = new List<ILogDashboardAuthorizationFilter>();
            //            CacheExpires = TimeSpan.FromMinutes(5);
        }



    }
}
