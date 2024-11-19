using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silent_Shadow._Managers.CollisionManager
{
	public static class CollisionManager
	{
		// Methode zur Überprüfung der Kollision mit allgemeinen Rechtecken
		public static bool IsCollidingWithAnyWall(Vector2 newPosition, List<Rectangle> colliders, int offsetX, int offsetY, int width, int height)
		{
			Rectangle newBounds = new Rectangle(
				(int)newPosition.X + offsetX,
				(int)newPosition.Y + offsetY,
				width,
				height
			);

			foreach (var rect in colliders)
			{
				if (newBounds.Intersects(rect))
				{
					return true; // Kollision erkannt
				}
			}

			return false; // Keine Kollision
		}

		// Methode zur Überprüfung der Kollision mit Ausgängen
		public static bool IsCollidingWithExitpoint(Vector2 newPosition, List<Rectangle> exitPoints, int offsetX, int offsetY, int width, int height)
		{
			Rectangle newBounds = new Rectangle(
				(int)newPosition.X + offsetX,
				(int)newPosition.Y + offsetY,
				width,
				height
			);

			foreach (var rect in exitPoints)
			{
				if (newBounds.Intersects(rect))
				{
					return true; // Kollision erkannt
				}
			}

			return false; // Keine Kollision
		}
	}
}
