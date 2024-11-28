using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Models.AI.Navigation;
using TiledSharp;

namespace Silent_Shadow.Models
{


	public class Level
	{
		// Eigenschaften der Klasse
		public Dictionary<int, Texture2D> TilesetTextures { get; }
		public float Scale { get; set; }

		public TmxMap Map { get; set; }
		public int TileWidth { get; set; }
		public int TileHeight { get; set; }
		public int TilesPerRow { get; set; }

		public List<Rectangle> Colliders { get; set; }

		// Zu verlassen der Map
		public List<Rectangle> ExitPoints { get; set; } = new List<Rectangle>();


		public NavMesh Navmesh { get; private set; }

		// Liste der tilessets eingefügt
		public Level(TmxMap map, Dictionary<int, Texture2D> tilesetTextures, int tileWidth, int tileHeight, int tilesPerRow, List<Rectangle> colliders, List<Rectangle> exitPoints , NavMesh navmesh)
		{
			Map = map;  
			TilesetTextures = tilesetTextures;  
			TileWidth = tileWidth;  
			TileHeight = tileHeight;  
			TilesPerRow = tilesPerRow;  
			Colliders = colliders;  
			ExitPoints = exitPoints;
			Navmesh = navmesh;  
			
		}
	}
}
