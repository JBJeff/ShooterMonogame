using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Models;

namespace Silent_Shadow.Managers
{
	/// <summary>
	/// Manages Levels
	/// </summary>
	/// 
	/// <author>Jeffrey Böttcher</author>
	/// <version>1.0</version>
	public interface ILevelManager
	{
		/// <summary>
		/// Loads a Level
		/// </summary>
		/// 
		/// <param name="level"><see cref="Level"/> to load</param>
		public Level LoadLevel(string level);

		/// <summary>
		/// Loads objects of a given Level
		/// </summary>
		/// 
		/// <param name="level"><see cref="Level"/> which objects should be loaded</param>
		public void LoadObjects(Level level);

		/// <summary>
		/// Draws a given Level
		/// </summary>
		/// 
		/// <param name="_spriteBatch"></param>
		/// <param name="level"><see cref="Level"/> to Draw</param>
		public void Draw(SpriteBatch _spriteBatch, Level level);
	}
}