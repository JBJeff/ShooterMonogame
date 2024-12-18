using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silent_Shadow.Models;
using Silent_Shadow.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Silent_Shadow.Controls;
using System.ComponentModel;
using Silent_Shadow.GUI;
using System.Reflection.Metadata;
using System.Diagnostics;
using MonoGame.Extended;
using Silent_Shadow.Models.AI;

namespace Silent_Shadow.States
{
	public class GameState : State
	{

		public const int ScreenWidth = 1280;
		public const int ScreenHeight = 1280;

		public static GameState Instance { get; private set; }
		public GraphicsDeviceManager Graphics { get; set; }
		public static Viewport Viewport { get { return Instance._game.GraphicsDevice.Viewport; } }
		public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }

		private SpriteBatch _spriteBatch;

		private IEntityManager _entityMgr;
		private InfoDisplay _infoDisplay;

		private ILevelManager _levelManager;
		public Level Level;

		// zum bestimmen in welchen Level man sich befindet
		private int _currentLevelIndex = 0;
		
		//Statische Liste der Levelnamen
		private readonly List<string> _levelNames = new List<string> { "firstlevel", "Level1_Bosslevel", "Level_3" };



		public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) 
			: base(game, graphicsDevice, content)
		{
			Instance = this;

			_entityMgr = EntityManagerFactory.GetInstance();
			_levelManager = LevelManagerFactory.GetInstance();



			string _levelname = "firstlevel"; // testlevel
			Level = _levelManager.LoadLevel(_levelname);
			_levelManager.LoadObjects(Level);

			//GUI.cs
			SpriteFont font = content.Load<SpriteFont>("Tahoma");
			Texture2D reloadIcon = content.Load<Texture2D>("Sprites/Ammo"); //Nachlade Icon
			_infoDisplay = new InfoDisplay(font, reloadIcon, new Vector2(graphicsDevice.Viewport.Width - 150, 20));

			
		}

		public override void PostUpdate(GameTime gameTime)
		{
			//throw new NotImplementedException();
			
		}

		public override void Update(GameTime gameTime)
		{

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				_game.ChangeState(new MenuState(_game, _graphicsDevice, _content));

			}

			if (Hero.Instance != null)
			{
				if (Hero.Instance.IsExit)
				{
					LoadNextLevel();
					Debug.WriteLine("EXIT");
				}
				else
				{
				}
			}


			Globals.Update(gameTime);
			_entityMgr.Update();
			InputManager.Update();


			// Überprüft, ob das Level abgeschlossen ist
			if (EntityManager.Instance.GruntKilledCount == EntityManager.Instance.TotalGruntCount)
			{
				// Mission abgeschlossen 
			}

			//base.Update(gameTime);


		}

		//verwendet den Counter um den richtigen Name(Datei) vom Level zu finden
		public void LoadNextLevel()
		{

			if (_currentLevelIndex < _levelNames.Count - 1)
			{

				Hero.Instance.IsExit = false;
				_currentLevelIndex++;
				string nextLevel1 = _levelNames[_currentLevelIndex];

				LoadLevel(nextLevel1);
			}
			else
			{
				// Spielende oder Rückkehr zum Hauptmenü
				Debug.WriteLine("Alle Levels abgeschlossen!");
				_game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
			}

		}

		//lädt ein Level mit einen bestimmten Namen.
		private void LoadLevel(string levelName)
		{
			try
			{

				//Clear Methoden müssten EVENTUELL noch eingefügt werden.

				Hero.Instance.IsExit = false;
				
				var newLevel = _levelManager.LoadLevel(levelName);
				if (newLevel == null)
				{
					Debug.WriteLine("Fehler: Level konnte nicht geladen werden.");
					return;
				}

				Instance.Level = newLevel;
				Debug.WriteLine("Neues Level erfolgreich geladen!");
				Hero.Instance.position = new Vector2(40, 529);  // Setzt die neue Startposition
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Fehler beim Laden des Levels: " + ex.Message);
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{

			spriteBatch.Begin();

			// Zeichnen der Tilemap
			_levelManager.Draw(spriteBatch, Level);
			_entityMgr.Draw(spriteBatch);
			_infoDisplay.Draw(spriteBatch);



#if DEBUG
			// IDK where else to put
			foreach (var node in Level.Navmesh.Nodes)
			{
				spriteBatch.DrawCircle(node.Position, 6f, 30, Color.HotPink);
			}
#endif
			spriteBatch.End();

			//base.Draw(gameTime);
		}
	}
}
