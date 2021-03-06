﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Asp.Net.Core.Logger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly LoggerHelper _logger;

        public WeatherForecastController(LoggerHelper logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            for (int i = 0; i < 5; i++)
            {
                _logger.LogInformation("LogInformation"+Guid.NewGuid().ToString("N"));
                _logger.LogDebug("LogDebug" + Guid.NewGuid().ToString("N"));
                _logger.LogWarning("LogWarning" + Guid.NewGuid().ToString("N"));
                _logger.LogError("LogError" + Guid.NewGuid().ToString("N"));
            }

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
