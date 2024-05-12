using System;
using System.Collections.Generic;

namespace CoreComfyUIAPI
{
	public class RomanTitles
	{
		public List<Tile> tiles { get; set; }
		public RomanTitles()
		{
			tiles = new List<Tile>();
		}
	}

	public class Tile
	{
		public int Level { get; set; }
		public int Parent { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public string path { get; set; }
		public bool pro { get; set; }

	}
}
