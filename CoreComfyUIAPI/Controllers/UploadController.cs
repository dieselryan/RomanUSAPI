using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
//using Emgu.CV.Dnn;
//using Emgu.CV.Structure;
//using Emgu.CV;
//using System.Drawing;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UploadController : ControllerBase
	{
		
		private readonly ApplicationSettings _settings;
		private readonly IMemoryCache _cache;

		public UploadController(ApplicationSettings settings, IMemoryCache cache)
		{
			//_imageUploader = imageUploader;
			_settings = settings;
			_cache = cache;
		}
	
		[HttpPost("UploadImage")]
		public async Task<IActionResult> UploadImage([FromForm] IFormFile image, [FromForm] string SessionID, [FromForm] Boolean isFemale, [FromForm] bool isPrimaryProfile)
		{
			string cacheKey = "ci_requestlist";
			if (!_cache.TryGetValue(cacheKey, out string data))
			{
				data = DateTime.Now.ToString() + ":upload image:" + SessionID + ":" + isFemale + ":" + isPrimaryProfile;
			}
			else
			{
				data = data + Environment.NewLine+ DateTime.Now.ToString() + ":upload image:" + SessionID + ":" + isFemale + ":" + isPrimaryProfile;
			}


				ProfilePic profilePic = new ProfilePic
			{
				IsFemale = isFemale,
				IsPrimaryProfile = isPrimaryProfile,
				SessionID = SessionID
			};
			try {
				ImageUploader imageUploader = new ImageUploader(); 

				string imagePath = await imageUploader.UploadImageAsync(image, profilePic, _settings.MyAppUrl);
				// Return the file path as a response
		
				return Ok(imagePath);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred: " + ex.Message);
			}	

		}
	}
}
