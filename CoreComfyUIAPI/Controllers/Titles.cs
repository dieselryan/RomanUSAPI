﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

		public static readonly string[] Tempfiles = {
		"Al+Feldstein_00002_.png",
		"Chris+Cunningham_00002_.png",
		"Emilia+Wilk_00001_.png",
		"Henri-Edmond+Cross_00001_.png",
		"Tim+Doyle_00001_.png",
		"Willem+de+Kooning_00002_.png",
		"Vincent+Desiderio_00003_.png",
		"Willem+de+Kooning_00001_.png",
		"Ian+Davenport_00005_.png",
		"Garry+Winogrand_00001_.png"
	};
		private void populateTitles(RomanTitles rt)
		{
			ImageReader reader = new ImageReader();
			List<string> imagefiles = reader.ReadLocalImages();

			for (int i = 0; i < 9; i++)
			{
				Tile t = new Tile();
				t.Id = i;
				t.Level = 1;
				t.Name = Tempfiles[i];
				t.Parent = 0;
				t.path = string.Format("http://34.145.0.140:8188/view?filename={0}&subfolder=&type=input", Uri.EscapeDataString(Tempfiles[i]));
				t.FileName = Tempfiles[i];
				if (i > 3)
				{
					t.pro = true;
				}
				else { 
					t.pro = false;
				}
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
