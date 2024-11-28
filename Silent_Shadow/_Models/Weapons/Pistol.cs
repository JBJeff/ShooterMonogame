using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Managers;
using System;

namespace Silent_Shadow.Models
{
    public class Pistol : Weapon
    {
		private IEntityManager _entityMgr;

		public Pistol(Vector2 _position)
        {
			_entityMgr = EntityManagerFactory.GetInstance();

			cooldown = 0.5f;  // Längere Abklingzeit 
            MaxAmmo = 10;     // Maximale Munition 
            Ammo = MaxAmmo;   // Setze die Anfangsmunition auf das Maximum
            reloadTime = 1.5f; // Zeit zum Nachladen

            sprite = Globals.Content.Load<Texture2D>("Sprites/pistol");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
			position = _position;
			rotation = 0f;
			speed = 80f;
			size = 0.4f;
        }

        protected override void CreateProjectiles(Hero hero)
        {
            Vector2 direction = new Vector2((float)Math.Cos(hero.rotation), (float)Math.Sin(hero.rotation));
			Bullet bullet = new Bullet(hero.position, direction, 1); // Erstellt ein Projektil in die Richtung des Helden
			_entityMgr.Add(bullet);
		}
    }
}