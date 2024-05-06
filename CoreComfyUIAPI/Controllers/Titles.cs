using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class Tiles : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public Tiles(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		private void populateTitles(RomanTitles rt)
		{
			for (int i = 0; i < 10;i++)
			{
				Tile t = new Tile();
				{
					t.Id = 1;
					t.Level = 1;
					t.Parent = 0;
					t.path = "/test/image" + i.ToString() + ".jpg";
					t.pro = false;
					rt.tiles.Add(t);
				}
			}
			
		}
		[HttpGet]
		public RomanTitles Get()
		{
			RomanTitles romanTitles= new RomanTitles();
			populateTitles(romanTitles);
			return romanTitles;
		}
	}
}
