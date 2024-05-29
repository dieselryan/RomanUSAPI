using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FeedbackController : ControllerBase
	{

		[HttpPost("PostFeedback")]
		public async Task<IActionResult> Feedback([FromForm] string UserId, [FromForm] int StarRating, [FromForm] string Message)
		{
			try
			{
				if (UserId == null || StarRating == 0) {
					return BadRequest("include UserID and Star Rating");
						}
				return Ok("Submitted");
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}
