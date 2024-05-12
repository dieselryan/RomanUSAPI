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
		private readonly ILogger<RomanTitles> _logger;

		public Tiles(ILogger<RomanTitles> logger)
		{
			_logger = logger;
		}

		private void populateTitles(RomanTitles rt)
		{
			ImageReader reader = new ImageReader();
			List<string> imagefiles = reader.ReadLocalImages();

			for(int i = 1; i < 10; i++)
			{
				Tile t = new Tile();
				t.Id = i;
				t.Level = 1;
				t.Name = "template " + i.ToString();
				t.Parent = 0;
				t.path = string.Format("http://34.145.0.140:8188/view?filename=temp{0}.png&subfolder=&type=", i);
				t.FileName = "temp" + i +".png";
				t.pro = false;
				rt.tiles.Add(t);
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
