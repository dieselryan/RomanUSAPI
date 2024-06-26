using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoreComfyUIAPI
{

	public enum GenderType
	{
		malemale,
		femalefemale,
		malefemale
	}

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
		public List<Tile> subTiles { get; set; }	
		public Tile() {
			subTiles = new List<Tile>();
		}
	}
}
