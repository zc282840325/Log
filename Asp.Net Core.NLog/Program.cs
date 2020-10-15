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
            //���Nlog��������
            NLogBuilder.ConfigureNLog("NLog.config");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureLogging(log => {
                //��builder�Ƴ����õ���־��ܣ�������õ�������һ��Ҫ���������ԣ�
                log.ClearProviders();
                //����־��ӵ�����̨
                log.AddConsole();
                //����־��ӵ�Debug����
                log.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).
                UseNLog();//���õ�������־���
    }
}
