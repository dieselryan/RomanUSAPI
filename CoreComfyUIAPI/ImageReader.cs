using System;
using System.Collections.Generic;
using System.IO;


namespace CoreComfyUIAPI
{

	public class ImageReader
	{
		public List<string> ReadLocalImages()
		{
			List<string> imageFileNames = new List<string>();

			try
			{
				// Get the path to the "bin" folder of the application
				string binFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

				// Combine the "bin" folder path with the "Images" folder path
				string imagesFolderPath = Path.Combine(binFolderPath, "Images");

				// Check if the "Images" folder exists
				if (Directory.Exists(imagesFolderPath))
				{
					// Get all image files in the "Images" folder
					string[] imageFiles = Directory.GetFiles(imagesFolderPath, "*.png");

					// Add the file names to the list
					imageFileNames.AddRange(imageFiles);
				}
				else
				{
					Console.WriteLine("Images folder not found.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}

			return imageFileNames;
		}
	}
}
