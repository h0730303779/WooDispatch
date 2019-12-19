﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WYQuartz.Host.Common
{
    public class Constant
    {
        /// <summary>
        /// 调用类型
        /// </summary>
        public static string TARGETTYPE = "TargetType";

        /// <summary>
        /// 调用目标
        /// </summary>
        public static string TARGETCALL = "TargetCall";
        /// <summary>
        /// 请求参数 RequestParameters
        /// </summary>
        public static string REQUESTPARAMETERS = "RequestParameters";
        /// <summary>
        /// Headers（可以包含：Authorization授权认证）
        /// </summary>
        public static string HEADERS = "Headers";
        /// <summary>
        /// 是否发送邮件
        /// </summary>
        public static string MAILMESSAGE = "MailMessage";
        /// <summary>
        /// 请求类型 RequestType
        /// </summary>
        public static string REQUESTTYPE = "RequestType";
        /// <summary>
        /// 日志 LogList
        /// </summary>
        public static string LOGLIST = "LogList";
        /// <summary>
        /// 异常 Exception
        /// </summary>
        public static string EXCEPTION = "Exception";
    }
}