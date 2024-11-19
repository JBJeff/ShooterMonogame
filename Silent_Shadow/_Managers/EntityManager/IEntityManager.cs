using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Models;

namespace Silent_Shadow.Managers
{
	/// <summary>
	/// Manages all Entitys
	/// </summary>
	/// 
	/// <author>Jonas Schwind</author>
	/// <version>1.0</version>
	public interface IEntityManager
	{
		/// <summary>
		/// Add an Entity
		/// </summary>
		/// <param name="entity">Entity to add</param>
		public void Add(Entity entity);

		/// <summary>
		/// Updates all entitys
		/// </summary>
		public void Update();

		/// <summary>
		/// Draws all entitys
		/// </summary>
		public void Draw(SpriteBatch spriteBatch);
	}
}
