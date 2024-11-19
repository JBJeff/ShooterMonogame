using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Controls;


namespace Silent_Shadow.States
{
	public class MenuState : State
	{
		private List<Component> _components;

		public MenuState(Game1 game, GraphicsDevice graphicsDevice,ContentManager content)
			: base(game, graphicsDevice, content)
		{
			var buttonTexture = _content.Load<Texture2D>("Controls/Button200");
			var buttonFont = _content.Load<SpriteFont>("Tahoma");

			var newGameButton = new Button(buttonTexture, buttonFont)
			{
				Position = new Vector2(graphicsDevice.Viewport.Width / 2- (buttonTexture.Width/2), 200),
				Text = "Neues Spiel",
			};
			newGameButton.Click += Button_NewGame_Clicked;

			var loadGameButton = new Button(buttonTexture, buttonFont)
			{
				Position = new Vector2(graphicsDevice.Viewport.Width / 2 - (buttonTexture.Width / 2), 350),
				Text = "Spiel Laden",
			};
			loadGameButton.Click += Button_LoadGame_Clicked;

			var exitGameButton = new Button(buttonTexture, buttonFont)
			{
				Position = new Vector2(graphicsDevice.Viewport.Width / 2 - (buttonTexture.Width / 2), 500),
				Text = "Verlassen",
			};
			exitGameButton.Click += Button_ExitGame_Clicked;

			_components = new List<Component>()
			{
				newGameButton,
				loadGameButton,
				exitGameButton,
			};
		}

		private void Button_NewGame_Clicked(object sender, EventArgs e)
		{
			_game.ChangeState(new GameState(_game, _graphicsDevice, _content));
		}
/*
 *	#TODO
 *	funktion zum Speichern und Laden Implementieren
 *
 */
		private void Button_LoadGame_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Spiel Laden");
		}

		private void Button_ExitGame_Clicked(object sender, EventArgs e)
		{
			_game.Exit();
		}

		public override void Update(GameTime gameTime)
		{
			foreach(var component in _components)
				component.Update(gameTime);
		}
		public override void PostUpdate(GameTime gameTime)
		{
			//Sprites enfernen wenn sie nicht benötigt werden
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			foreach (var component in _components)
				component.Draw(gameTime, spriteBatch);

			spriteBatch.End();
		}
	}
}
