using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Asp.Net.Core.Logger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //获取日志服务(在Program作用域类)
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Start");
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config=> {
                   //读取Json配置文件（读取日志的配置，这是设置为热更新）
                config.AddJsonFile("appsettings.json",optional:false,reloadOnChange:true);
               }).ConfigureLogging(log=> {
                   //日志添加到控制台
                   log.AddConsole();
                   //日志添加到debug调试窗口
                   log.AddDebug();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
