using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Silent_Shadow.Models.AI.Navigation;
using Silent_Shadow.States;

namespace Silent_Shadow.Models.AI
{
	public class Grunt : Agent
	{
		private Node startNode;
		private Node goalNode;
		private List<Node> path;
		private int currentWaypointIndex;
		private float stoppingDistance;
		private bool searchingForPath;

		private Vector2[] _peripheralArea; // NOTE: Only a field for debug display
		private Vector2[] _visibleArea; // NOTE: Only a field for debug display

		private readonly SoundEffect _detectionSound;
		private readonly SoundEffectInstance _detectionSoundInstance;
		private bool _hasPlayedDetectionSound = false;

		private Texture2D _eyecon;
		private Texture2D _alertIcon;

		public Grunt(Vector2 _position)
		{
			sprite = Globals.Content.Load<Texture2D>("Sprites/HSurvivorshade");
			origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
			position = _position;
			rotation = 0f;
			speed = 80f;
			size = 1.6f;
			AlertState = AlertState.IDLE;

			_detectionSound = Globals.Content.Load<SoundEffect>("Sounds/Effects/Alert");
			_detectionSoundInstance = _detectionSound.CreateInstance();

			_eyecon = Globals.Content.Load<Texture2D>("LooseSprites/eye-icon");
			_alertIcon = Globals.Content.Load<Texture2D>("LooseSprites/exclamation-point");

			stoppingDistance = 5f;
		   	path = null;
			currentWaypointIndex = 0;
			searchingForPath = false;
		}

		// PERF: Probalby hella expensive
		public override bool PlayerDetected()
		{
			Vector2 direction = MathHelpers.GetDirectionVector(rotation, Direction.Left);
			VisionCone = MathHelpers.GetTriangle(position, direction, 170f, 55f);

			_peripheralArea = MathHelpers.GetHexagon(position, direction, 280f, 180f, 170f, 220f, 170f, -25f);
			_visibleArea = MathHelpers.GetHexagon(position, direction, 180f, 220f, 90f, 170f, 90f, 0f);

			static bool PlayerInDetectionShape(Vector2 A, Vector2 B, Vector2 C, Vector2 D, Vector2 E, Vector2 F, Vector2 P)
			{
				return MathHelpers.PointInPolygon(A, B, C, D, P) || MathHelpers.PointInPolygon(C, D, E, F, P);
			}

			if (PlayerInDetectionShape(_peripheralArea[0], _peripheralArea[1], _peripheralArea[2], _peripheralArea[3], _peripheralArea[4], _peripheralArea[5], Hero.Instance.position))
			{
				_detectionCounter += 1 * Hero.Instance.visibility * 0.8f; // TODO: Tweak detection rate
			}
			else if (PlayerInDetectionShape(_visibleArea[0], _visibleArea[1], _visibleArea[2], _visibleArea[3], _visibleArea[4], _visibleArea[5], Hero.Instance.position))
			{
				_detectionCounter += 1 * Hero.Instance.visibility; // TODO: Tweak detection rate
			}
			else if (PlayerInVisionCone(position, VisionCone[0], VisionCone[1], Hero.Instance.position))
			{
				_detectionCounter += 1 * Hero.Instance.visibility * 3f; // TODO: Tweak detection rate
			}
			else
			{
				_detectionCounter = 0;
			}

			if (_detectionCounter >= _detectionThreshold)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Sets a new path for the AI to follow.
		/// </summary>
		/// <param name="path">List of nodes representing the path.</param>
		public void SetPath(List<Node> _path)
		{
			this.path = _path;
			currentWaypointIndex = 0;
		}

		/// <summary>
		/// Move towards a point in the world
		/// </summary>
		/// 
		/// <param name="targetPosition"></param>
		/// <param name="deltaTime">Elapsed game time</param>
		public void Goto(Vector2 targetPosition, float deltaTime)
		{
			Vector2 direction = targetPosition - position;
			direction.Normalize();
			position += direction * speed * deltaTime;
			rotation = MathHelpers.ToAngle(direction);
		}

		private void PlayDetectionSound()
		{
			if (!_hasPlayedDetectionSound)
			{
				_detectionSoundInstance.Play();
				_hasPlayedDetectionSound = true;
			}
		}

		public static Node FindRandomNodeWithinRadius(Vector2 startPosition, float radius, List<Node> nodes)
		{
			// Create a list to store nodes within the radius
			var candidates = new List<Node>();

			// Loop through all nodes
			foreach (var node in nodes)
			{
				// Calculate the distance between the start position and the node's position
				float distance = Vector2.Distance(startPosition, node.Position);

				// If the node is within the radius, add it to the candidates
				if (distance <= radius)
				{
					candidates.Add(node);
				}
			}

			// If no candidates found, return null
			if (candidates.Count == 0)
			{
				return null;
			}

			// Pick a random node from the candidates
			Random random = new Random();
			int randomIndex = random.Next(candidates.Count);
			return candidates[randomIndex];
		}

		private Node FindNearestValidNode(Vector2 position, List<Node> nodes)
		{
			Node nearestNode = null;
			float nearestDistance = float.MaxValue;

			foreach (var node in nodes)
			{
				if (node.Neighbors.Count > 0)
				{
					float distance = Vector2.Distance(position, node.Position);
					if (distance < nearestDistance)
					{
						nearestDistance = distance;
						nearestNode = node;
					}
				}
			}

			return nearestNode;
		}

		public override void Update()
		{
			switch (AlertState)
			{
				case AlertState.IDLE:

					List<Node> nodes = GameState.Instance.Level.Navmesh.Nodes;
					if (path == null)
					{
						
						startNode = FindNearestValidNode(position, nodes);
						if (startNode == null)
						{
							Console.WriteLine("No valid start node found!");
							return;
						}

						// Find a random valid goal node
						if (goalNode == null)
						{
							goalNode = FindRandomNodeWithinRadius(position, 600f, nodes);
							return;
						}

						if (!searchingForPath)
						{
							searchingForPath = true;
							var foundPath = Pathfinding.FindPath(startNode, goalNode);
							if (foundPath != null || foundPath.Count != 0)
							{
								SetPath(foundPath);
								Debug.WriteLine("Path found. Starting movement.");
							}
						}
						
					}
					else
					{
						Debug.WriteLine($"Following path: {path.Count} waypoints remaining.");

						Vector2 targetPosition = path[currentWaypointIndex].Position;
						float distanceToTarget = Vector2.Distance(position, targetPosition);

						if (distanceToTarget <= stoppingDistance)
						{
							currentWaypointIndex++;

							if (currentWaypointIndex >= path.Count)
							{
								Debug.WriteLine("Reached destination.");
								path = null;
								goalNode = FindRandomNodeWithinRadius(position, 600f, nodes);
								searchingForPath = false;
								return;
							}

							targetPosition = path[currentWaypointIndex].Position;

						}

						Goto(targetPosition, Globals.TotalSeconds);
					}

					_hasPlayedDetectionSound = false;
					if (PlayerDetected())
					{
						AlertState = AlertState.HOSTILE;
						PlayDetectionSound();
						_hasPlayedDetectionSound = true;
					}
					break;
				case AlertState.HOSTILE:
					break;
				default:
					break;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, null, tint, rotation, origin, size, 0, 0);

			if (_detectionCounter >= 100)
			{
				spriteBatch.Draw(_alertIcon, position, null, tint, 0, origin + new Vector2(600, 1500), .05f, 0, 0);
			}
			else if (_detectionCounter > 0)
			{
				spriteBatch.Draw(_eyecon, position, null, tint, 0, origin + new Vector2(400, 800), .1f, 0, 0);
			}

#if DEBUG
			#region Vision Cone debug
			spriteBatch.DrawLine(position, VisionCone[0], Color.Orange);
			spriteBatch.DrawLine(position, VisionCone[1], Color.Orange);
			spriteBatch.DrawLine(VisionCone[0], VisionCone[1], Color.Orange);
			#endregion

			#region Peripheral Vision Debug
			spriteBatch.DrawLine(_peripheralArea[0], _peripheralArea[1], Color.Yellow);
			spriteBatch.DrawLine(_peripheralArea[4], _peripheralArea[5], Color.Yellow);
			spriteBatch.DrawLine(_peripheralArea[0], _peripheralArea[2], Color.Yellow);
			spriteBatch.DrawLine(_peripheralArea[1], _peripheralArea[3], Color.Yellow);
			spriteBatch.DrawLine(_peripheralArea[2], _peripheralArea[4], Color.Yellow);
			spriteBatch.DrawLine(_peripheralArea[3], _peripheralArea[5], Color.Yellow);
			#endregion

			#region Visible Area debug
			spriteBatch.DrawLine(_visibleArea[0], _visibleArea[1], Color.Red);
			spriteBatch.DrawLine(_visibleArea[4], _visibleArea[5], Color.Red);
			spriteBatch.DrawLine(_visibleArea[0], _visibleArea[2], Color.Red);
			spriteBatch.DrawLine(_visibleArea[1], _visibleArea[3], Color.Red);
			spriteBatch.DrawLine(_visibleArea[2], _visibleArea[4], Color.Red);
			spriteBatch.DrawLine(_visibleArea[3], _visibleArea[5], Color.Red);
			#endregion

			#region Walk path debug
			if (path != null && path.Count > 1)
			{
				for (int i = 0; i < path.Count - 1; i++)
				{
					Vector2 from = path[i].Position;
					Vector2 to = path[i + 1].Position;
					spriteBatch.DrawLine(from, to, Color.LimeGreen, 3f);
				}
				spriteBatch.DrawCircle(path[^1].Position, 6f, 30, Color.LimeGreen);

				// Highlight the current segment
				if (currentWaypointIndex < path.Count)
				{
					Vector2 currentPosition = position;
					Vector2 nextPosition = path[currentWaypointIndex].Position;
					spriteBatch.DrawLine(currentPosition, nextPosition, Color.Purple, 3f);
					spriteBatch.DrawCircle(nextPosition, 6f, 30, Color.Purple);
				}
			}
			#endregion

			if (_detectionCounter > 0)
			{
				spriteBatch.DrawLine(position, Hero.Instance.position, Color.Blue);
			}
			// Zeichnet die Kollisionsbox des Entity in Grün
			spriteBatch.DrawRectangle(Bounds, new Color(0, 255, 0, 100));
#endif
		}
	}
}