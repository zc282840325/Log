using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.Net.Core.Logger
{
    /// <summary>
    /// 日志辅助类
    /// </summary>
    public class LoggerHelper
    {
        ILogger<LoggerHelper> _logger;
        public LoggerHelper(ILogger<LoggerHelper> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string msg)
        {
            _logger.LogInformation(Write(msg));
        }
        public void LogDebug(string msg)
        {
            _logger.LogDebug(Write(msg));
        }
        public void LogError(string msg)
        { 
            _logger.LogError(Write(msg));
        }
        public void LogWarning(string msg)
        {
            _logger.LogWarning(Write(msg));
        }

        protected string Write(string msg)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + msg;
        }
    }
}
