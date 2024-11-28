using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Silent_Shadow.GUI;
using Silent_Shadow.Managers;
using Silent_Shadow.Models;
using System.Diagnostics;
using System;
using Silent_Shadow.States;

namespace Silent_Shadow {

	public class Game1 : Game
	{
		// Bildschirmgröße(wird später noch geändert bei bedarf)
		private const int ScreenWidth = 1280;
		private const int ScreenHeight = 1280;

		public static Game1 Instance { get; private set; }
		public GraphicsDeviceManager Graphics { get; set; }
		public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
		public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }

		private SpriteBatch _spriteBatch;

		private State _currentState;
		private State _nextState;

		public Game1()
		{
			Instance = this;

			Graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = ScreenWidth,
				PreferredBackBufferHeight = ScreenHeight
			};
			Graphics.ApplyChanges();

			Content.RootDirectory = "Content";

			Globals.Content = Content;
			
			IsMouseVisible = true;

		}


		protected override void Initialize()
		{
			Graphics.PreferredBackBufferWidth = ScreenWidth;
        	Graphics.PreferredBackBufferHeight = ScreenHeight;
        	Graphics.ApplyChanges();
			//Lade Achievments
			AchievementManager.LoadAchievements();

			

			base.Initialize();
		}

		public void ChangeState(State state)
		{
			_nextState = state;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_currentState = new MenuState(this, Graphics.GraphicsDevice, Content);

		}

		protected override void Update(GameTime gameTime)
		{


			if (_nextState != null)
			{
				_currentState = _nextState;

				_nextState = null;
			}

			_currentState.Update(gameTime);

			_currentState.PostUpdate(gameTime);

			base.Update(gameTime);

			



		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_currentState.Draw(gameTime, _spriteBatch);

			base.Draw(gameTime);
		}
	}
}
