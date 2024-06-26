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
using System.Net.Http;

namespace CoreComfyUIAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UpdateTiles : ControllerBase
	{
		private readonly ILogger<RomanTitles> _logger;
		private int uniqueid { get; set; }
		private readonly ApplicationSettings _settings;

		public UpdateTiles(ILogger<RomanTitles> logger, ApplicationSettings settings)
		{
			_logger = logger;
			uniqueid = 0;
			_settings = settings;
		}

		private Tile CreateTile(FileInfo file, string folder)
		{
			UploadFileAsync(file);

			Tile t = new Tile();
			t.Id = uniqueid;
			t.FileName = file.Name;
			uniqueid = uniqueid + 1;
			if (file.Name.Contains("_pri"))
			{
				t.Level = 1;
				t.pro = false;
			}
            else
            {
				if (t.FileName.Contains("_free"))
				{
					t.pro = false;
				}
				else
				{
					t.pro = true;
				}
				t.Level = 2;
            }
			t.Name = folder;
			t.Parent = 0;

			t.path = string.Format("https://reliable-aloe-422021-u5.uw.r.appspot.com/image?template={0}", file.Name);

			
			return t;

		}
		private async Task UploadFileAsync(FileInfo file)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, _settings.MyAppUrl +  "/upload/image");
			var content = new MultipartFormDataContent();
			content.Add(new StreamContent(System.IO.File.OpenRead(file.FullName)), "image", file.Name);
			content.Add(new StringContent(""), "subfolder");
			content.Add(new StringContent("input"), "type");
			content.Add(new StringContent("true"), "overwrite");
			request.Content = content;
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			Console.WriteLine(await response.Content.ReadAsStringAsync());
		}
		private void populateTitles(RomanTitles rt, string primaryDirectory, string filter)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(primaryDirectory);
	
			
			foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
			{
		

				string folderName = directory.Name;
				foreach (DirectoryInfo subdirectory in directory.GetDirectories())
				{
					RomanTitles subTiles = new RomanTitles();
					if (subdirectory.Name == filter)
					{
						Tile parentTile = new Tile();
						FileInfo[] files = subdirectory.GetFiles();
						foreach (FileInfo file in files)
						{
							Tile responseTile = CreateTile(file,folderName);
							if (responseTile.Level == 1)
							{
								parentTile = responseTile;
							}
							else
							{
								subTiles.tiles.Add(responseTile);
							}
						}
						parentTile.subTiles = subTiles.tiles;
						rt.tiles.Add(parentTile);
					}
				}
			}

		}

		[HttpGet]
		public string Get()
		{
			try
			{
				uniqueid = 0;
				string jsonString;
				//Female to Female
				RomanTitles ffTiles = new RomanTitles();
				populateTitles(ffTiles, Directory.GetCurrentDirectory() + "/wwwroot/templates", "FF");
				jsonString = JsonConvert.SerializeObject(ffTiles);
				System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/wwwroot/femalefemale.json", jsonString);


				//Female to Female
				RomanTitles mmTiles = new RomanTitles();
				populateTitles(mmTiles, Directory.GetCurrentDirectory() + "/wwwroot/templates", "MM");
				jsonString = JsonConvert.SerializeObject(mmTiles);
				System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/wwwroot/malemale.json", jsonString);

				//Female to Female
				RomanTitles mfTiles = new RomanTitles();
				populateTitles(mfTiles, Directory.GetCurrentDirectory() + "/wwwroot/templates", "MF");
				jsonString = JsonConvert.SerializeObject(mfTiles);
				System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "/wwwroot/malefemale.json", jsonString);

				return "success";
			} catch {
				return "failed";
					}

		}
	}
}
