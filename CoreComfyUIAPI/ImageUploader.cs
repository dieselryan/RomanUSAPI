using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace CoreComfyUIAPI
{

	public class ImageUploader
	{
	/*	public static async Task<Mat> ConvertIFormFileToMat(IFormFile file)
		{
			if (file.Length > 0)
			{
				using (var memoryStream = new MemoryStream())
				{
					await file.CopyToAsync(memoryStream);
					byte[] fileBytes = memoryStream.ToArray();

					// Decode the byte array to a Mat object
					Mat imageMat = new Mat();
					CvInvoke.Imdecode(fileBytes, Emgu.CV.CvEnum.ImreadModes.Color, imageMat);

					return imageMat;
				}
			}
			return null; // or throw an exception if you expect a file to always be provided
		}*/
	/*	private async Task<string> getGender(IFormFile file)
		{
			try
			{
				//string prototxt = Path.Combine(Directory.GetCurrentDirectory(), @"\wwwroot\models\gender_deploy.prototxt");
				//string caffeModel = Path.Combine(Directory.GetCurrentDirectory(), @"\wwwroot\models\gender_deploy.caffemodel");

				string prototxt = @"D:\dev\romanticize me\CoreComfyUIAPI\RomanUSAPI\CoreComfyUIAPI\wwwroot\models\gender_deploy.prototxt";
				string caffeModel = @"D:\dev\romanticize me\CoreComfyUIAPI\RomanUSAPI\CoreComfyUIAPI\wwwroot\models\gender_net.caffemodel";


				// Load the network
				Net net = DnnInvoke.ReadNetFromCaffe(prototxt, caffeModel);

				// Load an image
			//	Mat img = await ConvertIFormFileToMat(file);
				using Mat img = CvInvoke.Imread(@"C:\Users\rdiss\Downloads\brdpsm.png", Emgu.CV.CvEnum.ImreadModes.Color);
				// Convert image to blob

		

				Mat blob = DnnInvoke.BlobFromImage(img, 1.0, new Size(224, 224), new MCvScalar(0, 0, 0), false, false);

				// Set the blob as input to the network
				net.SetInput(blob);

				// Run a forward pass through the network
				Mat output = net.Forward();

				// Process the output as needed (depends on your model output)
				img.Dispose();
				blob.Dispose();
				output.Dispose();
				return output.ToString();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
			finally
			{
			
			}
			// Cleanup
		
	
		}*/

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

					//	var test = getGender(image);
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

