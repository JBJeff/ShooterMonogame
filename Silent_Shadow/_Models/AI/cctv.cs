using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Silent_Shadow.Models.AI
{
	public class CCTV : Agent
	{

		public CCTV(Vector2 _position)
		{
			sprite = Globals.Content.Load<Texture2D>("LooseSprites/camera");
			origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
			position = _position;
			rotation = 0f;
			speed = 0f;
			size = 0.6f;
			AlertState = AlertState.IDLE;
		}

		public override bool PlayerDetected()
		{
			Vector2 direction = MathHelpers.GetDirectionVector(rotation, Direction.Forward);
			VisionCone = MathHelpers.GetTriangle(position, direction, 170f, 60f);

			if (PlayerInVisionCone(position, VisionCone[0], VisionCone[1], Hero.Instance.position))
			{
				_detectionCounter += 1 * Hero.Instance.visibility * 3f;

				if (_detectionCounter >= _detectionThreshold)
				{
					return true;
				}
			}
			else
			{
				_detectionCounter = 0;
			}

			return false;
		}

		public override void Update()
		{
			PlayerDetected();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, tint, rotation, origin, size, 0, 0);

#if DEBUG
			#region Vision Cone debug
			spriteBatch.DrawLine(position, VisionCone[0], Color.Orange);
			spriteBatch.DrawLine(position, VisionCone[1], Color.Orange);
			spriteBatch.DrawLine(VisionCone[0], VisionCone[1], Color.Orange);
			#endregion

			if (_detectionCounter > 0)
			{
				spriteBatch.DrawLine(position, Hero.Instance.position, Color.Blue);
			}
			
			// Zeichnet die Kollisionsbox des Entity in Grün
			spriteBatch.DrawRectangle(Bounds, new Color(0, 255, 0, 100));

#endif
		}
	}
}