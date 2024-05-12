using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.IO.Pipes;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreComfyUIAPI
{

	public class ImageUploader
	{
		public HttpContent ConvertIFormFileToHttpContent(IFormFile file, string filename)
		{
			// Check if the file is null
			if (file == null || file.Length == 0)
			{
				throw new ArgumentException("File is null or empty.");
			}

			// Create a StreamContent to wrap the file stream
			StreamContent fileContent = new StreamContent(file.OpenReadStream());
			fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
			{
				Name = "image",
				FileName = filename + Path.GetExtension(file.FileName)
			};

			return fileContent;
		}
		public async Task<string> CreateProfileName(ProfilePic profilePic)
		{
			string result = "profile_";
			if (profilePic.IsPrimaryProfile)
			{
				result = result +  "_pri_";
			}
            else
            {
				result = result + "_sec_";

			}
            if (profilePic.IsFemale)
			{
				result = result + "fem";
			}
			else
			{
				result = result + "male";
			}
			return profilePic.SessionID +result;
		}
		public async Task<string> UploadImageAsync(IFormFile image, ProfilePic profilePic)
		{
			
			try
			{
				if (string.IsNullOrEmpty(profilePic.SessionID))
				{
					throw new ArgumentException("Need a Session ID.");
				}
				
				
				// Check if the file is null or has no content
				if (image == null || image.Length == 0)
				{
					throw new ArgumentException("image is null or empty.");
				}

				// Generate a unique file name for the uploaded image
				using (HttpClient client = new HttpClient())
				{
					// Create a new multipart form data content
					using (var formData = new MultipartFormDataContent())
					{
						string filename = await CreateProfileName(profilePic);
						// Add the file content to the form data
						formData.Add(ConvertIFormFileToHttpContent(image, filename));
					

						// Add additional parameter to the form data

						formData.Add(new StringContent("true"), "overwrite");
						formData.Add(new StringContent("input"), "type");

						// Send the multipart form data request
						var response = await client.PostAsync("http://34.145.0.140:8188/upload/image", formData);

					

						// Check if the request was successful
						if (response.IsSuccessStatusCode)
						{
							// Read and return the response content
							return await response.Content.ReadAsStringAsync();
						}
						else
						{
							// Handle the error response
							throw new Exception($"HTTP request failed with status code {response.StatusCode}");
						}

					}
					

				}
	
			}

			catch (Exception ex)
			{
				// Handle any exceptions
				Console.WriteLine("An error occurred: " + ex.Message);
				throw;
			}
		}
	}
}

