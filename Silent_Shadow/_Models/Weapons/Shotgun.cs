using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Managers;
using System;
namespace Silent_Shadow.Models
{
 	public class Shotgun : Weapon
    {
		private IEntityManager _entityMgr;

		public Shotgun(Vector2 _position) 
        {
			_entityMgr = EntityManagerFactory.GetInstance();

			cooldown = 1.0f;  // Längere Abklingzeit 
            MaxAmmo = 6;      // Maximale Munition für die Schrotflinte
            Ammo = MaxAmmo;   // Setze die Anfangsmunition auf das Maximum
            reloadTime = 2.5f; // Zeit zum Nachladen

            sprite = Globals.Content.Load<Texture2D>("Sprites/shotgun");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
			position = _position;
			rotation = 0f;
			speed = 80f;
			size = 0.4f;
        }

        protected override void CreateProjectiles(Hero hero)
        {
			Vector2 direction = new Vector2((float)Math.Cos(hero.rotation), (float)Math.Sin(hero.rotation));

			Bullet bullet = new Bullet(hero.position, direction, 1); // Erstes Projektil (mittig)
			_entityMgr.Add(bullet);

            // Winkel für die seitlichen Schüsse
            float spreadAngle = MathHelper.ToRadians(4f); // 10 Grad Streuwinkel
            Vector2 leftDirection = Vector2.Transform(direction, Matrix.CreateRotationZ(-spreadAngle));
            Vector2 rightDirection = Vector2.Transform(direction, Matrix.CreateRotationZ(spreadAngle));

			bullet = new Bullet(hero.position, leftDirection, 1); // Zweites Projektil (links)
			_entityMgr.Add(bullet);
			bullet = new Bullet(hero.position, rightDirection, 1); // Drittes Projektil (rechts)
			_entityMgr.Add(bullet);
		}
    }
}
