using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silent_Shadow.Managers;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using Silent_Shadow.States;
using Silent_Shadow._Managers.CollisionManager;

namespace Silent_Shadow.Models
{
	public class Hero : Entity
	{
		public Vector2 velocity { get; set; }
		public float visibility { get; set; }
		public Weapon currentWeapon;

		
		#region Singleton
		private static Hero _instance;

		//für die Kollision
		public Rectangle heroRect;

		//Bestimmt wann der Spieler das Level verlassen kann
		public Boolean IsExit { get; set; } = false;

		public static Hero Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new InvalidOperationException("Hero is not initialized. Call Initialize(Vector2 position) first.");
				}
				return _instance;
			}
		}

		private Hero(Vector2 _position)
		{
			sprite = Globals.Content.Load<Texture2D>("Sprites/hero");
			origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
			position = _position;
			rotation = 0f;
			size = 0.25f;

			visibility = 1f; // TODO: add stuff that changes that value

			//Startwaffe Maschinengewehr
            currentWeapon = new MachineGun(new Vector2(0,0));

			heroRect = Bounds;

			// Setze die Textur für Projektile im ProjektileManager
			//ProjectileManager.SetTexture(Globals.Content.Load<Texture2D>("Sprites/bullet"));
			currentWeapon = new MachineGun(new Vector2(0, 0));
		}

		public static void Initialize(Vector2 position)
		{

			
			if (_instance != null)
			{
				throw new InvalidOperationException("Hero is already initialized.");
			}
			_instance = new Hero(position);
		}
		#endregion

		public override void Update()
		{
			speed = 4f;
			velocity = speed * InputManager.GetMovementDirection();
			Vector2 previousPosition = position;

			// Berechne die neue Position basierend auf der Eingabe
			position += velocity;


			// Prüfe auf Kollision auf wände
			if (CollisionManager.IsCollidingWithAnyWall(position, GameState.Instance.Level.Colliders, -15, -15, 40, 40))
			{
				position = previousPosition; // Wenn eine Kollision erkannt wird, zurück zur alten Position
			}

			// Prüft auf Kollision mit einem Ausgangspunkt
			if (CollisionManager.IsCollidingWithExitpoint(position, GameState.Instance.Level.ExitPoints, -15, -15, 40, 40))
			{
				
			}


			var aim = InputManager.GetAimDirection();

			if (aim.LengthSquared() > 0)
			{
				rotation = aim.ToAngle();
			}

			// Schießen
			if (InputManager.IsLeftMouseHeld())
    		{
        		currentWeapon.Fire(this);
    		}

			//Überprüfen, ob die Waffe bereit ist (Nachladen)
    		currentWeapon.Update();
			//Waffenwechsel
			SwitchWeapon();

			// Setzt die Kollisionsrechteck-Position basierend auf der neuen Position des Helden
			heroRect.X = (int)position.X;
			heroRect.Y = (int)position.Y;

			// Debug-Ausgabe für die aktuelle Position
			//Debug.WriteLine($"Spielerposition: X={position.X}, Y={position.Y}");
		}
		
        // Waffenwchseln 1=Nahkampf 2=MachineGun 3=Pistol 4=Shotgun
        private void SwitchWeapon()
		{
			if (InputManager.IsKeyPressed(Keys.D2))
			{
				currentWeapon = new MachineGun(new Vector2(0, 0));
			}
			else if (InputManager.IsKeyPressed(Keys.D3))
			{
				currentWeapon = new Pistol(new Vector2(0, 0));
			}
			else if (InputManager.IsKeyPressed(Keys.D4))
			{
				currentWeapon = new Shotgun(new Vector2(0, 0));
			}
			else if (InputManager.IsKeyPressed(Keys.D1))
			{
				currentWeapon = new Knife();
			}
		}

		internal object GetCurrentWeapon(){
            return currentWeapon;
        }



		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, tint, rotation, origin, size, 0, 0);

#if DEBUG
			// Zeichnet die Kollisionsbox des Helden in Grün
			spriteBatch.DrawRectangle(Bounds, new Color(0, 255, 0, 100));
#endif
		}

		
		public Rectangle Bounds
		{
			get
			{
				// Verschieben des Rechtecks um - 15Pixel, für genauere Kollision
				int offsetX = -15;
				int offsetY = -15;

				return new Rectangle(
					(int)position.X + offsetX,  // verschiebung auf der X-Achse
					(int)position.Y + offsetY,  // verschiebing auf der Y-Achse
					40,  // Breite und Höhe des Rechtecks
					40
				);
			}
		}

	}
}
