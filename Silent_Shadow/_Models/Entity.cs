using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silent_Shadow.Models 
{
	public abstract class Entity
	{
		#region variables
		protected Vector2 origin;
		protected Texture2D sprite;
		protected Color tint = Color.White;
		public Vector2 position { get; set; }
		public float rotation { get; set; }
		public bool isExpired { get; set; }
		public float size { get; set; }
		public float speed;
		#endregion

		public abstract void Update();

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, tint, rotation, origin, size, 0, 0);
		}
	}
}
