using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UploadController : ControllerBase
	{
		private readonly ImageUploader _imageUploader;

		public UploadController()
		{
			//_imageUploader = imageUploader;
		}
		[HttpPost("UploadImage")]
		public async Task<IActionResult> UploadImage([FromForm] IFormFile image, [FromForm] string SessionID, [FromForm] Boolean isFemale, [FromForm] bool isPrimaryProfile)
		{
			ProfilePic profilePic = new ProfilePic
			{
				IsFemale = isFemale,
				IsPrimaryProfile = isPrimaryProfile,
				SessionID = SessionID
			};
			try {
			
				ImageUploader imageUploader = new ImageUploader(); 
				string imagePath = await imageUploader.UploadImageAsync(image, profilePic);

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
