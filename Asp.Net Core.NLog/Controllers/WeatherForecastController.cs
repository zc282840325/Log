using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Fluent;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;

namespace Asp.Net_Core.NLog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ILogger<WeatherForecastController> _logger;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        public WeatherForecastController()
        {
          
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.Info("asd");
            _logger.Debug("测试消息Debug");
            _logger.Warn("测试消息Warn");
            _logger.Trace("测试消息Trace");
            _logger.Error("测试消息Error");




            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
