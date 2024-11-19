using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Silent_Shadow.Models
{
    public abstract class Weapon : Entity
    {
        protected float cooldown;
        protected float cooldownLeft;
        public bool IsPickupable { get; set; }  // Eigenschaft für die Aufhebbarkeit der Waffe

        public int MaxAmmo { get; protected set; }
        public int Ammo { get; protected set; }
        protected float reloadTime;
        public bool Reloading { get; protected set; }

        protected Weapon()
        {
            cooldownLeft = 0f;
            Reloading = false;
            size = 1.0f;  // Größe des Sprites, falls benötigt
            IsPickupable = true;  // Standardmäßig auf aufnahmebereit setzen
        }

        public virtual void Reload()
        {
            if (Reloading || (Ammo == MaxAmmo)) return;
            cooldownLeft = reloadTime;
            Reloading = true;
            Ammo = MaxAmmo;
        }

        protected abstract void CreateProjectiles(Hero hero);

        public virtual void Fire(Hero hero)
        {
            if (cooldownLeft > 0 || Reloading) return;

            Ammo--;
            if (Ammo > 0)
            {
                cooldownLeft = cooldown;
            }
            else
            {
                Reload();
            }

            CreateProjectiles(hero);
        }

        public override void Update()
        {
            if (cooldownLeft > 0)
            {
                cooldownLeft -= Globals.TotalSeconds;
            }
            else if (Reloading)
            {
                Reloading = false;
            }
        }
    }
}
