
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Silent_Shadow.Models.AI.Navigation
{
	public class Poly
	{
		public List<Vector2> Vertices { get; } // List of vertices defining the polygon
		public Node Node { get; private set; } // The navigation node associated with this polygon

		public Poly(List<Vector2> vertices)
		{
			Vertices = vertices;
		}

		public void AssignNode(Node node)
		{
			Node = node;
		}

		public bool SharesEdge(Poly other)
		{
			int sharedVertices = 0;
			foreach (var vertex in Vertices)
			{
				if (other.Vertices.Contains(vertex))
				{
					sharedVertices++;
				}
			}
			return sharedVertices >= 2; // Polygons share an edge if they have at least 2 shared vertices
		}

		public Vector2 CalculateCentroid()
		{
			float x = 0, y = 0;
			foreach (var vertex in Vertices)
			{
				x += vertex.X;
				y += vertex.Y;
			}
			return new Vector2(x / Vertices.Count, y / Vertices.Count);
		}
	}
}

	