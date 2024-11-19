using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Managers;
using Silent_Shadow.Models;

namespace Silent_Shadow.GUI
{
    public class InfoDisplay
    {
        private SpriteFont font;            // Schriftart für Textanzeige
        private Texture2D reloadIcon;       // Bild für das Nachlade-Symbol
        private Vector2 position;           // Position der Anzeige
        private Hero hero;                  // Referenz auf das Hero-Objekt
        protected Texture2D spriteMp5;
        protected Texture2D spriteShotgun;
        protected Texture2D spritePistol;

        public InfoDisplay
        (SpriteFont font, Texture2D reloadIcon, Vector2 position)
        {
            spriteMp5 = Globals.Content.Load<Texture2D>("Sprites/mp5");
            spriteShotgun = Globals.Content.Load<Texture2D>("Sprites/shotgun");
            spritePistol = Globals.Content.Load<Texture2D>("Sprites/pistol");
            this.font = font;
            this.reloadIcon = reloadIcon;
            this.position = position;
            hero = Hero.Instance;           // Zugriff auf das Singleton Hero-Objekt
        }

        public void Update() {}

        public void Draw(SpriteBatch spriteBatch)
        {
            // Waffe explizit als Weapon casten, um auf die Eigenschaften zuzugreifen
            var weapon = hero.GetCurrentWeapon() as Weapon;

            if (weapon != null)
            {
                string ammoText = $"{weapon.Ammo}/{weapon.MaxAmmo}";  // Zeigt verbleibende und max. Schüsse an
                spriteBatch.DrawString(font, ammoText, position, Color.White);

                // Passendes Waffensymbol anzeigen
                float scale = 0.5f;
                if (weapon is MachineGun)
                {
                    // Position des Waffensymbols festlegen (links von der Munition)
                    Vector2 iconPosition = new Vector2(position.X - 140, position.Y);
                    spriteBatch.Draw(spriteMp5, iconPosition, null, Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
                else if (weapon is Shotgun)
                {
                    // Position des Waffensymbols festlegen (links von der Munition)
                    Vector2 iconPosition = new Vector2(position.X - 160, position.Y);
                    spriteBatch.Draw(spriteShotgun, iconPosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                }
                else if (weapon is Pistol)
                {
                    // Position des Waffensymbols festlegen (links von der Munition)
                    Vector2 iconPosition = new Vector2(position.X - 90, position.Y);
                    spriteBatch.Draw(spritePistol, iconPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
                }

                // Wenn die Waffe nachlädt, das Nachlade-Symbol anzeigen
                if (weapon.Reloading)
                {
                    Vector2 reloadPosition = new Vector2(position.X + 100, position.Y); // Rechts neben der Munition
                    spriteBatch.Draw(reloadIcon, reloadPosition, Color.White);
                }

                // Gegneranzahl und -fortschritt anzeigen
                string enemyProgress = $"Grunt: {EntityManager.Instance.GruntKilledCount}/{EntityManager.Instance.TotalGruntCount}";
                spriteBatch.DrawString(font, enemyProgress, new Vector2(20, 20), Color.White);

                // Mission erfolgreich anzeigen
                if (EntityManager.Instance.GruntKilledCount == EntityManager.Instance.TotalGruntCount)
                {
                    string missionSuccess = "Mission erfolgreich!";
                    spriteBatch.DrawString(font, missionSuccess, new Vector2(20, 60), Color.Green);
                }
            
            }
        }
    }
}
