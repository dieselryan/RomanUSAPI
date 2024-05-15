using Microsoft.AspNetCore.Mvc;
using System;
using System.IO.Pipes;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Routing.Constraints;


namespace CoreComfyUIAPI.Controllers
{



	[ApiController]
	[Route("[controller]")]
	public class StatusController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetStatus(string PromptId)
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{

					//Check the queue
					var response = await client.GetAsync($"http://34.145.0.140:8188/queue");
					if (response.IsSuccessStatusCode)
					{
						string responseContent = await response.Content.ReadAsStringAsync();
							JObject jsonObject = JObject.Parse(responseContent);
							JArray nodesArray = (JArray)jsonObject["queue_running"];
							if (nodesArray.Count <1 )
							{
								return await GetHistoryCall(PromptId);

							}
							else
							{
								return Ok("still processing");
							}

					
					}
					else { return BadRequest("Error in queue response1"); }

					// Send POST request

				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
		private async Task<IActionResult> GetHistoryCall(string PromptId)
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var response = await client.GetAsync($"http://34.145.0.140:8188/history/" + PromptId);
					// Check if the request was successful
					if (response.IsSuccessStatusCode)
					{
						// Read response content as string
						string responseContent = await response.Content.ReadAsStringAsync();
						if (responseContent.Contains("success"))
						{
							JObject jsonObject = JObject.Parse(responseContent);
							if ((string)jsonObject[PromptId]["status"]["status_str"] == "success")
							{

								string filename = (string)jsonObject[PromptId]["outputs"]["69"]["images"][0]["filename"];
								int lastDotIndex = filename.LastIndexOf('.');
								return Ok(filename = filename.Substring(0, lastDotIndex));
							}
							else
							{
								return Ok("processing");
							}

						}
						return Ok(responseContent);//processing
					}
					else
					{
						return BadRequest("error from post");
					}
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}
