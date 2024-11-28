using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Silent_Shadow._Managers.CollisionManager;
using Silent_Shadow.States;
using System;
using System.Collections.Generic;

namespace Silent_Shadow.Models
{
    public class Bullet : Entity
    {
         public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)(position.X - (sprite.Width / 2 * size)),
                    (int)(position.Y - (sprite.Height / 2 * size)),
                    (int)(sprite.Width * size),
                    (int)(sprite.Height * size)
                );
            }
        }
        public Vector2 Direction { get; set; }
        private int lifespanFrames; // Anzahl Frames, die das Projektil lebt
        private const float speedPerFrame = 24f; // Geschwindigkeit pro Frame
        public int Damage { get; }

        public Bullet(Vector2 position, Vector2 direction, int damage)
        {
            sprite = Globals.Content.Load<Texture2D>("Sprites/bullet");
            this.position = position;
            Direction = direction;
            Damage = damage;
            lifespanFrames = 120; // z. B. 120 Frames Lebensdauer
            rotation = (float)Math.Atan2(Direction.Y, Direction.X);
            size = 1f;
        }

        public override void Update()
        {
			

			// Bewegt das Projektil basierend auf Richtung und konstanter Geschwindigkeit
			position += Direction * speedPerFrame;

			

			//Kollision mit der Wand
			if (CollisionManager.IsCollidingWithAnyWall(position, GameState.Instance.Level.Colliders, -15, -15, 40, 40))
			{
				isExpired = true;
			}

			// Reduziert die Anzahl der verbleibenden Frames
			lifespanFrames--;

            // Markiert das Projektil als abgelaufen, wenn keine Frames mehr übrig sind
            if (lifespanFrames <= 0)
            {
                isExpired = true;
            }
        }

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, tint, rotation, origin, size, 0, 0);

#if DEBUG
			// Zeichnet die Kollisionsbox des Entity in Grün
			spriteBatch.DrawRectangle(Bounds, new Color(0, 255, 0, 100));
#endif
		}

    }
}