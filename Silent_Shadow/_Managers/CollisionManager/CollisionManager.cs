using Microsoft.Xna.Framework;
using Silent_Shadow._Models;
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
		public static Vector2 FindValidPosition(Vector2 position, Rectangle bounds, List<Rectangle> colliders)
        {
            const int maxAttemptsPerDirection = 100; // Maximale Anzahl der Versuche pro Richtung
            const float stepSize = 1f;              // Schrittgröße, mit der die Position verschoben wird
            Vector2[] directions = new[]
            {
                new Vector2(-stepSize, 0), // Links
                new Vector2(stepSize, 0),  // Rechts
                new Vector2(0, -stepSize), // Oben
                new Vector2(0, stepSize)   // Unten
            };

            // Prüfe jede Richtung separat
            foreach (var direction in directions)
            {
                for (int attempt = 0; attempt < maxAttemptsPerDirection; attempt++)
                {
                    Vector2 newPosition = position + direction * attempt;

                    if (!IsCollidingWithAnyWall(newPosition, colliders, bounds.X, bounds.Y, bounds.Width, bounds.Height))
                    {
                        Console.WriteLine($"Gültige Position fuer Corspe gefunden: {newPosition}");
                        return newPosition;
                    }
                }
            }

            Console.WriteLine("Keine gültige Position gefunden");
            // Gib die ursprüngliche Position zurück, falls keine gültige gefunden wurde
            return position;
        }

	}
}
