using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Silent_Shadow
{
    public static class Globals
    {
        //vereinfacht zugriff auf unterschiedliche häufig benutzte Variablen
        public static float TotalSeconds { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

		public static float TileMapScale = 1.0f;

		public static void Update(GameTime gt)
		{
            TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
