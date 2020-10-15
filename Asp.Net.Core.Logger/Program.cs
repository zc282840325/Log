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
            //��ȡ��־����(��Program��������)
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Start");
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config=> {
                   //��ȡJson�����ļ�����ȡ��־�����ã���������Ϊ�ȸ��£�
                config.AddJsonFile("appsettings.json",optional:false,reloadOnChange:true);
               }).ConfigureLogging(log=> {
                   //��־��ӵ�����̨
                   log.AddConsole();
                   //��־��ӵ�debug���Դ���
                   log.AddDebug();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
