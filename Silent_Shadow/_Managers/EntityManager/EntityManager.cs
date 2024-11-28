using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Models;
using Silent_Shadow.Models.AI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Silent_Shadow.Managers
{
	/// <summary>
	/// IEntityManager Implementation
	/// </summary>
	/// 
	/// <remarks>
	/// <para>This is a Singleton Object</para>
	/// </remarks>
	/// 
	/// <author>Jonas Schwind</author>
	/// <version>1.0</version>
	/// 
	/// <seealso cref="IEntityManager"/>
	public class EntityManager : IEntityManager
	{
		#region variables
		public bool IsUpdating { get; set; }
		public List<Entity> Entities { get; private set; }
		public List<Entity> AddedEntities { get; private set; }
		public int Count { get { return Entities.Count; } }
		public int TotalGruntCount { get; private set; }
		public int GruntKilledCount { get; private set; }
		#endregion

		#region Singleton
		private static EntityManager _instance;
		public static EntityManager Instance
		{
			get
			{
				_instance ??= new EntityManager();
				return _instance;
			}
		}

		private EntityManager()
		{
			Entities = new List<Entity>();
			AddedEntities = new List<Entity>();
		}
		#endregion

		public void Add(Entity entity)
		{
			if (!IsUpdating)
			{
				Entities.Add(entity);
			}
			else
			{
				AddedEntities.Add(entity);
			}
			ResetCounters();
		}

		public void CheckForWeaponPickup()
		{
			for (int i = 0; i < Entities.Count; i++)
			{
				if (Entities[i] is Weapon weapon && weapon.IsPickupable)
				{
					float distance = Vector2.Distance(Hero.Instance.position, weapon.position);

					// Wenn die Distanz weniger als 3 Einheiten beträgt
					if (distance < 25f)
					{
						Hero.Instance.currentWeapon = weapon;  // Setze die Waffe als aktuelle Waffe des Helden
						weapon.IsPickupable = false;  // Setze die Waffe auf nicht mehr aufnehmbar

						Entities.RemoveAt(i);  // Lösche die Waffe aus der Liste
						break;  // Beende die Suche, da nur eine Waffe aufgenommen werden kann
					}
				}
			}
		}

		public void CheckForAgentInTriangle(Vector2 heroPos, Vector2 leftPoint, Vector2 rightPoint)
		{
			foreach (var entity in Entities)
			{
				if (entity is Agent agent)
				{
					// Use helper method to check if agent's position is inside the triangle
					if (MathHelpers.PointInTriangle(heroPos, leftPoint, rightPoint, agent.position))
					{
						KillEntity(entity);
						
					}
				}
			}
		}
		public void ResetCounters()
		{
			TotalGruntCount = Entities.Count(e => e is Grunt);
			GruntKilledCount = 0;
		}

		public void ResetAll()
		{
			// Behalte den Spieler (Hero) und entferne alle anderen Entitäten
			var player = Hero.Instance;
			Entities = Entities.Where(e => e == player).ToList();

			// Setzt alle Zähler zurück
			ResetCounters();
		}

		public void KillEntity(Entity entity)
		{
			
			// Überprüfe den Typ der Entität und führe spezifische Logik aus
			if (entity is Grunt)
			{
				GruntKilledCount++; // Zähle Grunt-Kill
				AchievementManager.IncrementAchievement("Grunt"); // Erhöhe Erfolgsfortschritt

				 // Erstelle einen neuen Corpse an der Position des Grunts und füge ihn der Entitätenliste hinzu
        		Corpse corpse = new Corpse(entity.position);
        		Entities.Add(corpse); // Füge die Leiche zur Entitätenliste hinzu
			}
			else if (entity is CCTV)
			{
				AchievementManager.IncrementAchievement("CCTV"); // Erfolgsfortschritt für CCTV
			}
			// Entferne die Entität aus der Liste
			entity.isExpired = true;


			// Rufe SaveAchievements auf, um Fortschritt zu speichern
			AchievementManager.SaveAchievements();
		}

		private void CheckForPlayerAgentCollision()
		{
			// Iteriere über alle Entities und prüfe Kollisionen
			foreach (var entity in Entities.OfType<Agent>().ToList()) // Nur Agent-Entities
			{
				foreach (var bullet in Entities.OfType<Bullet>().ToList()) // Nur Bullet-Entities
				{
					if (bullet.Bounds.Intersects(entity.Bounds))
					{
						// Entferne Agent und Bullet aus dem Spiel
						KillEntity(entity);
						KillEntity(bullet);

						Debug.WriteLine($"Agent {entity.GetType().Name} von Bullet getroffen und entfernt.");
						break; // Wenn ein Treffer vorliegt, breche die Schleife für diesen Agent ab
					}
				}
			}
		}



		public void Update()
		{
			IsUpdating = true;
			foreach (var entity in Entities)
			{
				entity.Update();
			}
			IsUpdating = false;
			Entities.AddRange(AddedEntities);
			AddedEntities.Clear();
			CheckForPlayerAgentCollision();
			Entities = Entities.Where(x => !x.isExpired).ToList();

			// Überprüfe, ob eine Waffe aufgenommen werden kann
			CheckForWeaponPickup();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var entity in Entities)
			{
				entity.Draw(spriteBatch);
			}
		}
	}
}
