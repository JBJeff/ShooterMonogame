
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Silent_Shadow.Models.AI.Navigation
{
	public class Pathfinding
	{
		public static List<Node> FindPath(Node start, Node goal)
		{
			var openSet = new PriorityQueue<Node, float>();
			var openSetTracker = new HashSet<Node>();
			var closedSet = new HashSet<Node>();

			var cameFrom = new Dictionary<Node, Node>();
			var gScore = new Dictionary<Node, float>();
			var fScore = new Dictionary<Node, float>();

			Debug.WriteLine($"A*: Starting pathfinding from {start.Position} to {goal.Position}");

			gScore[start] = 0;
			fScore[start] = Heuristic(start, goal);

			openSet.Enqueue(start, fScore[start]);
			openSetTracker.Add(start);

			while (openSet.Count > 0)
			{
				Node current = openSet.Dequeue();
				openSetTracker.Remove(current);

				Debug.WriteLine($"A*: Exploring node at {current.Position}");

				if (current == goal)
				{
					return ReconstructPath(cameFrom, current);
				}

				closedSet.Add(current);

				foreach (var neighbor in current.Neighbors)
				{
					Debug.WriteLine($"A*: Checking neighbor at {neighbor.Position}");
					if (closedSet.Contains(neighbor))
					{
						continue;
					}

					float tentativeGScore = gScore[current] + Vector2.Distance(current.Position, neighbor.Position);

					// Check if this path to the neighbor is better
					if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
					{
						Debug.WriteLine($"A*: Updating path to neighbor at {neighbor.Position}");
						cameFrom[neighbor] = current;
						gScore[neighbor] = tentativeGScore;
						fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

						if (!openSetTracker.Contains(neighbor))
						{
							openSet.Enqueue(neighbor, fScore[neighbor]);
							openSetTracker.Add(neighbor);
						}
					}
				}

				if (openSet.Count == 0)
				{
					break;
				}
			}

			Debug.WriteLine("A*: Failed to find a path.");
			return null;
		}

		private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
		{
			List<Node> path = new List<Node> { current };
			while (cameFrom.ContainsKey(current))
			{
				current = cameFrom[current];
				path.Insert(0, current);
			}
			return path;
		}

		/// <summary>
		/// Calculates the distance between two nodes.
		/// </summary>
		/// 
		/// <param name="a">Node A</param>
		/// <param name="b">Node B</param>
		/// 
		/// <returns>Distance</returns>
		private static float Heuristic(Node a, Node b)
		{
			float distance = Vector2.Distance(a.Position, b.Position);
			Debug.WriteLine($"Heuristic from {a.Position} to {b.Position}: {distance}");
			return distance;
		}
	}
}

	