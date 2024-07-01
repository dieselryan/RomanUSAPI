using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Runtime;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CreateImageController : ControllerBase
	{
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly ApplicationSettings _settings;
		private readonly IMemoryCache _cache;


		public CreateImageController(IWebHostEnvironment hostingEnvironment, ApplicationSettings settings, IMemoryCache cache)
		{
			_hostingEnvironment = hostingEnvironment;
			_settings = settings;
			_cache = cache;
			
		}
		private string InjectValues(string Json, string primaryImage, string secondaryImage, string templateImage)
		{
		

			JObject jsonObject = JObject.Parse(Json);
			int underscoreIndex = templateImage.IndexOf('_');
			if (underscoreIndex != -1)
			{
				// Extract the substring up to the first underscore
				string artist = templateImage.Substring(0, underscoreIndex);
				jsonObject["prompt"]["6"]["inputs"]["text"] = "a couple hugging, in the style of " + artist.Replace("+"," ");
			}
			jsonObject["prompt"]["15"]["inputs"]["image"] = primaryImage;
			jsonObject["prompt"]["63"]["inputs"]["image"] = secondaryImage;
	     	jsonObject["prompt"]["14"]["inputs"]["image"] = templateImage;

			return jsonObject.ToString();


		}
		[HttpGet]
		public async Task<IActionResult> CreateImage(string primaryImage,string secondaryImage, string templateImage)
		{
			string clientId = Guid.NewGuid().ToString();
			string cacheKey = "ci_requestlist";
			try
			{
				if (!_cache.TryGetValue(cacheKey, out string data))
				{
					data = DateTime.Now.ToString() + ":image_create:"+ primaryImage + ":" + secondaryImage + ":" + templateImage;

				}
				else
				{
					data = data + Environment.NewLine + DateTime.Now.ToString() + ":image_create:" + primaryImage + ":" + secondaryImage + ":" + templateImage;
					
				}
				var cacheEntryOptions = new MemoryCacheEntryOptions()
						.SetAbsoluteExpiration(TimeSpan.FromDays(1));
				_cache.Set(cacheKey, data, cacheEntryOptions);

				if (primaryImage == null) { return BadRequest("No primary image"); }
				if (secondaryImage == null) { return BadRequest("No secondary image"); }
				if (templateImage == null) { return BadRequest("No template image"); }
				templateImage = templateImage.Replace(" ", "+");

				var postData = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/fasterworkflow.json"));

				using (HttpClient client = new HttpClient())
				{
					var modifiedJsonStr = InjectValues(postData, primaryImage, secondaryImage, templateImage);
					var content = new StringContent(modifiedJsonStr, Encoding.UTF8, "application/json");
					// Send POST request
					var response = await client.PostAsync(_settings.MyAppUrl +  $"/prompt", content );
					// Check if the request was successful
					if (response.IsSuccessStatusCode)
					{
						// Read response content as string
						string responseContent = await response.Content.ReadAsStringAsync();

						//	using (var ws = new ClientWebSocket())
						{
							//		await ws.ConnectAsync(new Uri($WebSocketUri + clientId), CancellationToken.None);

							//		var promptId = QueuePrompt(prompt, serverAddress, clientId)["prompt_id"];
							//	var images = await GetImages(ws, promptId, serverAddress);

							// Code to process output images
							//		foreach (var nodeImages in images)
							{
								//		foreach (var imageData in nodeImages.Value)
								{
									// Code to handle image data (e.g., save to file, display)
								}
							}
						}
					
						return Ok(responseContent);
					}
					else
					{
						return BadRequest("Error in creating image");
					}
				}
			}
			catch (Exception ex)
			{
		
		
				// Handle any exceptions
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}
		
		[HttpGet("{GetCache}")]
		public async Task<IActionResult> GetCache()
		{
			if (_cache.TryGetValue("ci_requestlist", out string data))
			{
			
				return Ok(data);
			}
			else
			{
				return Ok("no data");
			}
		}
	}

	/*	static async Task<Dictionary<string, List<byte[]>>> GetImages(ClientWebSocket ws, string promptId, string serverAddress)
		{
			var outputImages = new Dictionary<string, List<byte[]>>();

			while (true)
			{
				var buffer = new ArraySegment<byte>(new byte[4096]);
				var receiveResult = await ws.ReceiveAsync(buffer, CancellationToken.None);

				if (receiveResult.MessageType == WebSocketMessageType.Text)
				{
					var message = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(
						Encoding.UTF8.GetString(buffer.Array, 0, receiveResult.Count));

					if (message["type"] == "executing")
					{
						var data = message["data"];
						if (data["node"] == null && data["prompt_id"] == promptId)
						{
							break; // Execution is done
						}
					}
				}
			}

			var history = GetHistory(promptId, serverAddress);
			foreach (var nodeOutput in history[promptId]["outputs"])
			{
				var imagesOutput = new List<byte[]>();
				foreach (var image in nodeOutput.Value["images"])
				{
					imagesOutput.Add(await GetImage(image.Value["filename"], image.Value["subfolder"], image.Value["type"], serverAddress));
				}
				outputImages.Add(nodeOutput.Key, imagesOutput);
			}

			return outputImages;
		}
		static Dictionary<string, Dictionary<string, dynamic>> GetHistory(string promptId, string serverAddress)
		{
			var request = WebRequest.CreateHttp($"http://{serverAddress}/history/{promptId}");
			var response = request.GetResponse();
			using (var streamReader = new StreamReader(response.GetResponseStream()))
			{
				var responseData = streamReader.ReadToEnd();
				return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, dynamic>>>(responseData);
			}
		}

	}*/
}
