using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Email;
using Serilog.Sinks.MSSqlServer;

namespace Asp.Net.Core.Serilog
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())//设置基础路径
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)//添加配置文件
            .AddEnvironmentVariables()//添加环境变量
            .Build();

      
        public static void Main(string[] args)
        {
            #region 分级文件存储
            // string LogFilePath(string LogEvent) => $@"{AppContext.BaseDirectory}00_Logs\{LogEvent}\log.log";
            //string SerilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 50);

            //Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
            //    .MinimumLevel.Debug()
            //    .Enrich.FromLogContext()//使用Serilog.Context.LogContext中的属性丰富日志事件。
            //    .WriteTo.Console(new RenderedCompactJsonFormatter())
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File(LogFilePath("Debug"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File(LogFilePath("Information"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File(LogFilePath("Warning"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File(LogFilePath("Error"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File(LogFilePath("Fatal"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .CreateLogger();
            #endregion

            #region 存储数据库

            string connecting = "Data Source=.;Initial Catalog=SerilogLog;User ID=sa;Password=123456";


            //autoCreateSqlTable: true自动创建表

            #region 添加列
            //var options = new ColumnOptions();
            //options.AdditionalColumns = new Collection<SqlColumn>
            //    {
            //        new SqlColumn { DataType = SqlDbType.NVarChar, DataLength =-1, ColumnName = "IP" },
            //    };
            #endregion

            #region 移除列
            //var options = new ColumnOptions();
            //options.Store.Remove(StandardColumn.Properties);
            //options.Store.Remove(StandardColumn.TimeStamp);
            #endregion

            //Log.Logger =  new LoggerConfiguration().ReadFrom.Configuration(Configuration)
            //     .MinimumLevel.Information()
            //     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //     .ReadFrom.Configuration(new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json")
            //     .Build())
            //     .WriteTo.MSSqlServer(connecting, "logs", autoCreateSqlTable: true, columnOptions: options, restrictedToMinimumLevel: LogEventLevel.Information)
            //     .CreateLogger();
            #endregion

            #region 邮箱
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
                .WriteTo.Email(new EmailConnectionInfo() { 
                    Port= 465,
                    EmailSubject="邮件日志测试",
                    FromEmail= "18****85@163.com",
                    ToEmail="2***40325@qq.com",
                    MailServer= "smtp.163.com",
                    NetworkCredentials = new NetworkCredential("18****85@163.com", "***"),
                    IsBodyHtml =true,
                    EnableSsl=true
                })
                .CreateLogger();
            #endregion
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            ConfigureLogging(log =>
            {
                log.ClearProviders();
                log.AddDebug();
                log.AddConsole();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog(dispose: true);
    }
}
