using Microsoft.Xna.Framework;
using Silent_Shadow.Managers;
using System;

namespace Silent_Shadow.Models
{
    public class Knife : Weapon
    {
        private const float Range = 90f;
        private const float Angle = 15f;

        public Knife()
        {
            MaxAmmo = 1; 
            Ammo = MaxAmmo;
            IsPickupable = true;
            reloadTime = 1f; // 1 Sekunden
        }

        protected override void CreateProjectiles(Hero hero)
        {
            
        }

        public override void Fire(Hero hero)
        {
            if (cooldownLeft > 0 || Reloading) return;
        
            // Detect entities within the knife's range and angle
            var aimDirection = InputManager.GetAimDirection();
            DetectAndHitAgents(hero.position, aimDirection);

            Ammo = 0;
            // Start cooldown by calling Reload
            Reload();
        }

        private void DetectAndHitAgents(Vector2 heroPosition, Vector2 aimDirection)
        {
            Vector2 leftDir = Vector2.Transform(aimDirection, Matrix.CreateRotationZ(MathHelper.ToRadians(-Angle)));
            Vector2 rightDir = Vector2.Transform(aimDirection, Matrix.CreateRotationZ(MathHelper.ToRadians(Angle)));

            Vector2 leftPoint = heroPosition + leftDir * Range;
            Vector2 rightPoint = heroPosition + rightDir * Range;

            EntityManager.Instance.CheckForAgentInTriangle(heroPosition, leftPoint, rightPoint);
        }
    }
}
