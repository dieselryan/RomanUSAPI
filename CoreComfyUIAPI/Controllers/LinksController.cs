﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LinksController : ControllerBase
	{
		public class UrlEntry
		{
			public string Name { get; set; }
			public string Url { get; set; }
		}
		[HttpGet]

		public async Task<IActionResult> GetLinks()
			{
			try
			{
				var urls = new List<UrlEntry>
				{
					new UrlEntry { Name = "Privacy Policy", Url = "https://www.diffuseai.io/romanus/privacypolicy" },
					new UrlEntry { Name = "Terms of Use", Url = "https://www.diffuseai.io/romanus/termsofuse" },
					new UrlEntry { Name = "Open Source Licences", Url = "https://www.diffuseai.io/romanus/opensourcelicence" }
				}	;

				// Convert the list to JSON format
				string json = JsonConvert.SerializeObject(urls, Formatting.Indented);
				return Ok(json);
			}
				catch (Exception ex)
				{
					return BadRequest(ex);
				}
			}
		}
}
