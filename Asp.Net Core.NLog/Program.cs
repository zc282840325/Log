using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Asp.Net_Core.NLog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //添加Nlog的配置项
            NLogBuilder.ConfigureNLog("NLog.config");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureLogging(log => {
                //从builder移除内置的日志框架（如果引用第三方，一定要设置这属性）
                log.ClearProviders();
                //将日志添加到控制台
                log.AddConsole();
                //将日志添加到Debug窗口
                log.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).
                UseNLog();//引用第三方日志框架
    }
}
