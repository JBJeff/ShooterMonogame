using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using MonoGame.Extended;
using Silent_Shadow.Models;
using Silent_Shadow.Models.AI;
using MonoGame.Extended.Shapes;
using Silent_Shadow.Models.AI.Navigation;
using System.Linq;
using MonoGame.Extended.ECS;


namespace Silent_Shadow.Managers
{
	/// <summary>
	/// ILevelManager Implementation
	/// </summary>
	/// 
	/// <remarks>
	/// <para>This is a Singleton Object</para>
	/// </remarks>
	/// 
	/// <authors>
	/// <author>Jeffer Böttcher</author>
	/// <author>Jonas Schwind</author>
	/// </authors>
	/// 
	/// <version>1.0</version>
	/// 
	/// <seealso cref="ILevelManager"/>
	public class LevelManager : ILevelManager
	{

		private EntityManager _entityManager = EntityManager.Instance;
#if DEBUG
		List<Polygon> debugMesh = new List<Polygon>();
		List<(Vector2, Vector2, Vector2)> debugTriangles = new List<(Vector2, Vector2, Vector2)>();

		
#endif

		#region Singleton
		private static LevelManager _instance;
		public static LevelManager Instance


		{
			get
			{
				_instance ??= new LevelManager();
				return _instance;
			}
		}
		private LevelManager() { }
		#endregion

		public Level LoadLevel(string level)
		{


			TmxMap _map = new($"Content/Maps/{level}.tmx");

			// Dictionary für mehrere Tilesets
			Dictionary<int, Texture2D> tilesetTextures = new Dictionary<int, Texture2D>();
			foreach (var tileset in _map.Tilesets)
			{
				Texture2D texture = Globals.Content.Load<Texture2D>($"TileSheets/{tileset.Name}");
				tilesetTextures.Add(tileset.FirstGid, texture);
			}

			
			int _tileWidth = _map.Tilesets[0].TileWidth;
			int _tileHeight = _map.Tilesets[0].TileHeight;
			int _tilesetTilesWide = tilesetTextures.Values.First().Width / _tileWidth;

			// Kollisionserkennung
			List<Rectangle> _colliders = new List<Rectangle>();
			foreach (var o in _map.ObjectGroups["Kollision"].Objects)
			{
				_colliders.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
			}

			List<Rectangle> _exitpoints = new List<Rectangle>();
			foreach (var o in _map.ObjectGroups["ExitPoint"].Objects)
			{
				_exitpoints.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
			}

			// Navmesh
			List<(Vector2, Vector2, Vector2)> triangles = new List<(Vector2, Vector2, Vector2)>();
			foreach (var o in _map.ObjectGroups["Navmesh"].Objects)
			{
				List<Vector2> points = new List<Vector2>();

				foreach (var point in o.Points)
				{
					points.Add(new Vector2((float)point.X, (float)point.Y));
				}

				Polygon poly = new Polygon(points);
				poly.Offset(new Vector2((float)o.X, (float)o.Y));

				List<Vector2> vertices = poly.Vertices.ToList();
				if (!EarClipper.IsClockwise(vertices))
				{
					vertices.Reverse();
				}

				poly = new Polygon(vertices);
				debugMesh.Add(poly);

				List<(Vector2, Vector2, Vector2)> polyTriangles = EarClipper.Triangulate([.. poly.Vertices]);
				debugTriangles.AddRange(polyTriangles);
				triangles.AddRange(polyTriangles);
			}

			
			NavMesh nav = new NavMesh(triangles);
			nav.BuildGraph();

			return new Level(_map, tilesetTextures, _tileWidth, _tileHeight, _tilesetTilesWide, _colliders,_exitpoints, nav);
		}

		
		// Methode zum Bereinigen eines Levels
		//muss überarbeitet werden findet bis jetzt noch KEINE verwendung.
		public void ClearLevel(Level level)
		{
			if (level == null)
			{
				return;
			}

			// Entfernt alle Entitäten
			_entityManager.Entities.Clear();
			_entityManager.AddedEntities.Clear();

			// Entfernt alle Kollisionsobjekte
			level.Colliders.Clear();
			level.ExitPoints.Clear();

			// Alle speziellen Level-Ressourcen löschen (Texturen, Sounds, etc.)
			level.TilesetTextures.Clear();

			//erweitern 
		}

		public void LoadObjects(Level level)
		{
			IEntityManager _entityMgr = EntityManagerFactory.GetInstance();

			foreach (var o in level.Map.ObjectGroups["Entitys"].Objects)
			{
				Vector2 _position = new Vector2((float)o.X, (float)o.Y);

				switch (o.Type.ToString()) // NOTE: field is called class in tiled editor
				{
					#region Player Spawner
					case "player":
						// NOTE: Only one player can exist
						Hero.Initialize(_position);
						_entityMgr.Add(Hero.Instance);
						break;
					#endregion


					#region Items Spawner
					case "pistol":
						_entityMgr.Add(new Pistol(_position));
						break;
					case "machinegun":
						_entityMgr.Add(new MachineGun(_position));
						break;
					case "shotgun":
						_entityMgr.Add(new Shotgun(_position));
						break;
					#endregion

					#region Enemy Spawner
					case "grunt":
						_entityMgr.Add(new Grunt(_position));
						break;
					case "cctv":
						_entityMgr.Add(new CCTV(_position));
						break;
					#endregion

					default:
						break;
				}
			}
		}

		public void Draw(SpriteBatch _spriteBatch, Level level)
		{
			for (var i = 0; i < level.Map.TileLayers.Count; i++)
			{
				for (var j = 0; j < level.Map.TileLayers[i].Tiles.Count; j++)
				{
					int gid = level.Map.TileLayers[i].Tiles[j].Gid;
					if (gid != 0)
					{
						Texture2D texture = null;
						Rectangle tilesetRec = Rectangle.Empty;

						// Bestimmt das richtige Tileset anhand des GID
						foreach (var tileset in level.Map.Tilesets)
						{
							if (gid >= tileset.FirstGid && gid < tileset.FirstGid + tileset.TileCount)
							{
								texture = level.TilesetTextures[tileset.FirstGid];
								int localTileId = gid - tileset.FirstGid;
								int column = localTileId % (texture.Width / tileset.TileWidth);
								int row = localTileId / (texture.Width / tileset.TileWidth);

								tilesetRec = new Rectangle(
									column * tileset.TileWidth,
									row * tileset.TileHeight,
									tileset.TileWidth,
									tileset.TileHeight);

								break;
							}
						}

						if (texture != null)
						{
							float x = (j % level.Map.Width) * level.Map.TileWidth * Globals.TileMapScale;
							float y = (j / level.Map.Width) * level.Map.TileHeight * Globals.TileMapScale;

							_spriteBatch.Draw(
								texture,
								new Rectangle((int)x, (int)y, (int)(level.TileWidth * Globals.TileMapScale), (int)(level.TileHeight * Globals.TileMapScale)),
								tilesetRec,
								Color.White);
						}
					}
				}
			}

#if DEBUG
			// Zeichnet alle Kollisionsboxen aus dem Level in Rot            
			foreach (var rect in level.Colliders)
			{
				_spriteBatch.FillRectangle(rect, new Color(200, 0, 0, 4));
				_spriteBatch.DrawRectangle(rect, Color.Red);
			}

			foreach (var mesh in debugMesh)
			{
				_spriteBatch.DrawPolygon(Vector2.Zero, mesh, Color.Cyan, 3f);
			}

			foreach (var triangle in debugTriangles)
			{
				Vector2 vertex1 = triangle.Item1;
				Vector2 vertex2 = triangle.Item2;
				Vector2 vertex3 = triangle.Item3;

				_spriteBatch.DrawLine(vertex1, vertex2, Color.Black);
				_spriteBatch.DrawLine(vertex2, vertex3, Color.Black);
				_spriteBatch.DrawLine(vertex3, vertex1, Color.Black);
			}
#endif
		}
	}
}
