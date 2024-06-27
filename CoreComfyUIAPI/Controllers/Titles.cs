using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class Tiles : ControllerBase
	{
		private readonly ILogger<RomanTitles> _logger;
		private readonly IMemoryCache _cache;

		public Tiles(ILogger<RomanTitles> logger, IMemoryCache cache)
		{
			_logger = logger;
			_cache = cache;
		}

	
		
		//[HttpGet]
		//public RomanTitles Get()
		//{
		//	RomanTitles romanTitles= new RomanTitles();
		//	populateTitles(romanTitles);
		//	return romanTitles;
		//}
		[HttpGet]
		public string Get(GenderType genderType)
		{
			string cacheKey = "genderjson" + genderType.ToString();
			try
			{
				if (!_cache.TryGetValue(cacheKey, out string data))
				{
					string filename = "malefemale.json";
					switch (genderType)
					{
						case GenderType.malemale: filename = "malemale.json"; break;
						case GenderType.femalefemale: filename = "femalefemale.json"; break;
					}
					data = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/wwwroot/" + filename);
					var cacheEntryOptions = new MemoryCacheEntryOptions()
									.SetAbsoluteExpiration(TimeSpan.FromDays(365));
					_cache.Set(cacheKey, data, cacheEntryOptions);
				}
				return data;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
