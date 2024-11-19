
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Silent_Shadow.Models.AI.Navigation
{
	public class NavMesh
	{
		public List<(Vector2, Vector2, Vector2)> Triangles { get; private set; }
		public List<Node> Nodes { get; private set; }

		public NavMesh(List<(Vector2, Vector2, Vector2)> triangles)
		{
			Triangles = triangles;
			Nodes = [];
		}

		public void BuildGraph()
		{
			
			Nodes.Clear();
			foreach (var triangle in Triangles)
			{
				Vector2 centroid = CalculateTriangleCentroid(triangle);
				Node node = new Node(centroid);
				Nodes.Add(node);
			}

			for (int i = 0; i < Triangles.Count; i++)
			{
				for (int j = i + 1; j < Triangles.Count; j++)
				{
					if (TrianglesShareEdge(Triangles[i], Triangles[j]))
					{
						Nodes[i].AddNeighbor(Nodes[j]);
						Nodes[j].AddNeighbor(Nodes[i]);
					}
				}
			}

			Nodes.RemoveAll(node => node.Neighbors.Count == 0);

			#if DEBUG
			foreach (var node in Nodes)
			{
				Debug.WriteLine($"Node at {node.Position} has {node.Neighbors.Count} neighbors.");
				foreach (var neighbor in node.Neighbors)
				{
					Debug.WriteLine($"  Neighbor at {neighbor.Position}");
				}
			}
			#endif
		}

		private static Vector2 CalculateTriangleCentroid((Vector2, Vector2, Vector2) triangle)
		{
			return new Vector2(
				(triangle.Item1.X + triangle.Item2.X + triangle.Item3.X) / 3,
				(triangle.Item1.Y + triangle.Item2.Y + triangle.Item3.Y) / 3
			);
		}

		private static bool TrianglesShareEdge((Vector2, Vector2, Vector2) t1, (Vector2, Vector2, Vector2) t2)
		{
			int sharedVertices = 0;

			foreach (var v1 in new[] { t1.Item1, t1.Item2, t1.Item3 })
			{
				foreach (var v2 in new[] { t2.Item1, t2.Item2, t2.Item3 })
				{
					if (v1 == v2)
					{
						sharedVertices++;
					}
				}
			}

			return sharedVertices == 2;
		}
	}
}