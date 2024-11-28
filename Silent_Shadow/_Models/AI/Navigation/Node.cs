using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Node
{
	public Vector2 Position { get; } // Position of the node
	public List<Node> Neighbors { get; } // Connected nodes
	public float Cost { get; set; } // Cost for weighted graphs (default is 1)

	public Node(Vector2 position)
	{
		Position = position;
		Neighbors = new List<Node>();
		Cost = 1f;
	}

	public void AddNeighbor(Node neighbor)
	{
		if (!Neighbors.Contains(neighbor))
		{
			Neighbors.Add(neighbor);
		}
	}
}

