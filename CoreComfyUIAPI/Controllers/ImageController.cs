using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ImageController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ApplicationSettings _settings;
		private readonly IMemoryCache _cache;

		public ImageController(IHttpClientFactory httpClientFactory, ApplicationSettings settings, IMemoryCache cache)
		{
			_httpClientFactory = httpClientFactory;
			_settings = settings;
			_cache = cache;
		}
		//not checking
		[HttpGet]
		public async Task<IActionResult> GetTemplate(string template)
		{
			string cacheKey = "temp_" + template;
			string imageUrl;
			string contentType = "image/png"; 
			
			
			try
			{
				MemoryStream data = null;
				if (!_cache.TryGetValue(cacheKey, out byte[] imageBytes))
				{
					template = Uri.EscapeDataString(template.Replace(" ", "+"));

					imageUrl = _settings.MyAppUrl + $"/view?filename={template}&type=input&subfolder=";


					// Create HttpClient

					var httpClient = _httpClientFactory.CreateClient();

					// Send GET request to fetch the image
					HttpResponseMessage response = await httpClient.GetAsync(imageUrl);

					// Check if the request was successful
					if (response.IsSuccessStatusCode)
					{
						// Get the image content as a stream
						Stream imageStream = await response.Content.ReadAsStreamAsync();

						var fileSteam = new FileStreamResult(imageStream, contentType);

						imageBytes = await ConvertFileStreamResultToByteArray(fileSteam);
						var cacheEntryOptions = new MemoryCacheEntryOptions()
								.SetAbsoluteExpiration(TimeSpan.FromDays(365));
						_cache.Set(cacheKey, imageBytes, cacheEntryOptions);
					}
					else
					{
						// Handle the case where the image could not be fetched
						return NotFound();
					}

				}
				return File(imageBytes, contentType); 
			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}

		private async Task<byte[]> ConvertFileStreamResultToByteArray(FileStreamResult fileStreamResult)
		{
			using (var memoryStream = new MemoryStream())
			{
				await fileStreamResult.FileStream.CopyToAsync(memoryStream);
				return memoryStream.ToArray();  // Convert the memory stream to a byte array
			}
		}

		[HttpGet("{imageName}")]
		public async Task<IActionResult> GetImage(string imageName)
		{
			
			string imageUrl = _settings.MyAppUrl + $"/view?filename={imageName}.png&type=output&subfolder=";
			// Create HttpClient

			var httpClient = _httpClientFactory.CreateClient();

			// Send GET request to fetch the image
			HttpResponseMessage response = await httpClient.GetAsync(imageUrl);

			// Check if the request was successful
			if (response.IsSuccessStatusCode)
			{
				// Get the image content as a stream
				Stream imageStream = await response.Content.ReadAsStreamAsync();

				// Determine the content type based on the file extension
				string contentType = "image/png"; // Modify according to your image format

				// Return the image as FileStreamResult
				return new FileStreamResult(imageStream, contentType);
			}
			else
			{
				// Handle the case where the image could not be fetched
				return NotFound();
			}
		}
	}
}
