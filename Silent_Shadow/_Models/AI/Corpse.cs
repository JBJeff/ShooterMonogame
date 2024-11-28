using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Silent_Shadow._Managers.CollisionManager;
using Silent_Shadow.States;

namespace Silent_Shadow.Models.AI
{
	public class Corpse : Entity
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

        // Konstruktor zur Initialisierung der Leiche mit einer Position
        public Corpse(Vector2 _position)
        {
            // Lade das Sprite für die Leiche
            sprite = Globals.Content.Load<Texture2D>("Sprites/corpse");
            position = _position;
            size = 0.15f;
            findPostion();
    }
     
    public override void Update()
{
    
}
    public void findPostion() {
        // Finde eine gültige Position, falls die aktuelle Kollision verursacht
        Vector2 newPosition = CollisionManager.FindValidPosition(position, Bounds, GameState.Instance.Level.Colliders);

        // Aktualisiere die Position nur, wenn sie sich geändert hat
        if (newPosition != position)
        {
            position = newPosition;
        }
    }
    public override void Draw(SpriteBatch spriteBatch)
		{
            // Berechne die Offset-Position für das Zeichnen
            Vector2 drawPosition = new Vector2(position.X - 40f, position.Y - 40f);
			spriteBatch.Draw(sprite, drawPosition, null, tint, rotation, origin, size, 0, 0);

#if DEBUG
			// Zeichnet die Kollisionsbox des Entity in Grün
			spriteBatch.DrawRectangle(Bounds, new Color(0, 255, 0, 100));
#endif
		}

    }
    }

