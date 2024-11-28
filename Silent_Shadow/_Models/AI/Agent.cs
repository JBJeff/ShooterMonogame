using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Silent_Shadow.Models.AI
{
	public enum AlertState { IDLE, ALERT, HOSTILE }

	public abstract class Agent : Entity
	{
		public AlertState AlertState { get; set; }
		 // Standard-Hitbox, kann von Unterklassen angepasst werden
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

		protected Vector2[] VisionCone { get; set; }
		protected float _detectionCounter = 0;
		protected const float _detectionThreshold = 100f;

		public bool PlayerInVisionCone(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
		{
			return MathHelpers.PointInTriangle(A, B, C, P);
		}

		public virtual bool PlayerDetected()
		{
			// (\(\
			// ( –.–)
			// o_(")(")
			//
			// Not implemeted, overrideable

			throw new NotImplementedException();
		}

		public override void Update()
		{
			// (\(\
			// ( –.–)
			// o_(")(")
			//
			// Not implemeted, overrideable

			throw new NotImplementedException();
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, tint, rotation, origin, size, 0, 0);

#if DEBUG
			// Zeichnet die Kollisionsbox des Helden in Grün
			spriteBatch.DrawRectangle(Bounds, new Color(0, 255, 0, 100));
#endif
		}

    }
	}
