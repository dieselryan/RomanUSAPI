using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ImageController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ImageController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		//not checking
		[HttpGet]
		public async Task<IActionResult> GetTemplate(string template)
		{
			string imageUrl;
			// URL of the image on the remote server
			template = Uri.EscapeDataString (template.Replace(" ", "+"));
			imageUrl = $"http://34.145.0.140:8188/view?filename={template}&type=input&subfolder=";
			

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

		[HttpGet("{imageName}")]
		public async Task<IActionResult> GetImage(string imageName)
		{
			
			string imageUrl = $"http://34.145.0.140:8188/view?filename={imageName}.png&type=output&subfolder=";
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
