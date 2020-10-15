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
            .SetBasePath(Directory.GetCurrentDirectory())//���û���·��
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)//��������ļ�
            .AddEnvironmentVariables()//��ӻ�������
            .Build();

      
        public static void Main(string[] args)
        {
            #region �ּ��ļ��洢
            // string LogFilePath(string LogEvent) => $@"{AppContext.BaseDirectory}00_Logs\{LogEvent}\log.log";
            //string SerilogOutputTemplate = "{NewLine}{NewLine}Date��{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel��{Level}{NewLine}Message��{Message}{NewLine}{Exception}" + new string('-', 50);

            //Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
            //    .MinimumLevel.Debug()
            //    .Enrich.FromLogContext()//ʹ��Serilog.Context.LogContext�е����Էḻ��־�¼���
            //    .WriteTo.Console(new RenderedCompactJsonFormatter())
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File(LogFilePath("Debug"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File(LogFilePath("Information"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File(LogFilePath("Warning"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File(LogFilePath("Error"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File(LogFilePath("Fatal"), rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate))
            //    .CreateLogger();
            #endregion

            #region �洢���ݿ�

            string connecting = "Data Source=.;Initial Catalog=SerilogLog;User ID=sa;Password=123456";


            //autoCreateSqlTable: true�Զ�������

            #region �����
            //var options = new ColumnOptions();
            //options.AdditionalColumns = new Collection<SqlColumn>
            //    {
            //        new SqlColumn { DataType = SqlDbType.NVarChar, DataLength =-1, ColumnName = "IP" },
            //    };
            #endregion

            #region �Ƴ���
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

            #region ����
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
                .WriteTo.Email(new EmailConnectionInfo() { 
                    Port= 465,
                    EmailSubject="�ʼ���־����",
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
