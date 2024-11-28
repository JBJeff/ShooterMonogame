
namespace Silent_Shadow.Tests
{

	[TestFixture]
	public class MathHelperTest
	{
		[Test]
		public void TestToAngle()
		{
			Vector2 vector = new Vector2(1, 0);
			float angle = vector.ToAngle();
			Assert.AreEqual(0f, angle, 1e-5f); // Expect 0 radians for (1,0)

			vector = new Vector2(0, 1);
			angle = vector.ToAngle();
			Assert.AreEqual((float) Math.PI / 2, angle, 1e-5f); // Expect PI/2 radians for (0,1)

			vector = new Vector2(-1, 0);
			angle = vector.ToAngle();
			Assert.AreEqual((float) Math.PI, angle, 1e-5f); // Expect PI radians for (-1,0)
		}

		[Test]
		public void TestGetDirectionVector()
		{
			float rotation = (float) Math.PI / 2;

			Vector2 forward = MathHelpers.GetDirectionVector(rotation, Direction.Forward);
			Assert.AreEqual(new Vector2(-1, 0), forward);

			Vector2 backward = MathHelpers.GetDirectionVector(rotation, Direction.Backward);
			Assert.AreEqual(new Vector2(1, 0), backward);

			Vector2 left = MathHelpers.GetDirectionVector(rotation, Direction.Left);
			Assert.AreEqual(new Vector2(0, -1), left);

			Vector2 right = MathHelpers.GetDirectionVector(rotation, Direction.Right);
			Assert.AreEqual(new Vector2(0, 1), right);
		}

		[Test]
		public void TestGetTriangle()
		{
			Vector2 position = new Vector2(0, 0);
			Vector2 direction = new Vector2(1, 0); // facing right
			float visionLength = 10f;
			float visionAngle = 45f;

			Vector2[] triangle = MathHelpers.GetTriangle(position, direction, visionLength, visionAngle);

			// The exact points depend on the math, but we expect them to form a triangle.
			Assert.AreEqual(2, triangle.Length); // Should return 2 points (left and right)
		}

		[Test]
		public void TestGetHexagon()
		{
			Vector2 origin = new Vector2(0, 0);
			Vector2 direction = new Vector2(1, 0); // facing right
			float midDistance = 10f;
			float farDistance = 15f;
			float nearWidth = 3f;
			float midWidth = 4f;
			float farWidth = 5f;
			float offset = 2f;

			Vector2[] hexagon = MathHelpers.GetHexagon(origin, direction, midDistance, farDistance, nearWidth, midWidth, farWidth, offset);

			Assert.AreEqual(6, hexagon.Length); // A hexagon should have 6 vertices
		}

		[Test]
		public void TestPointInTriangle()
		{
			Vector2 A = new Vector2(0, 0);
			Vector2 B = new Vector2(5, 0);
			Vector2 C = new Vector2(0, 5);
			Vector2 P = new Vector2(2, 2);

			bool isInside = MathHelpers.PointInTriangle(A, B, C, P);
			Assert.IsTrue(isInside); // Point (2,2) should be inside the triangle

			P = new Vector2(6, 6);
			isInside = MathHelpers.PointInTriangle(A, B, C, P);
			Assert.IsFalse(isInside); // Point (6,6) should be outside the triangle
		}

		[Test]
		public void TestPointInPolygon()
		{
			Vector2 A = new Vector2(0, 0);
			Vector2 B = new Vector2(5, 0);
			Vector2 C = new Vector2(5, 5);
			Vector2 D = new Vector2(0, 5);
			Vector2 P = new Vector2(2, 2);

			bool isInside = MathHelpers.PointInPolygon(A, B, C, D, P);
			Assert.IsTrue(isInside); // Point (2,2) should be inside the polygon

			P = new Vector2(6, 6);
			isInside = MathHelpers.PointInPolygon(A, B, C, D, P);
			Assert.IsFalse(isInside); // Point (6,6) should be outside the polygon
		}

		[Test]
		public void TestCrossProduct()
		{
			Vector2 A = new Vector2(1, 0);
			Vector2 B = new Vector2(0, 1);

			float crossProduct = MathHelpers.CrossProduct(A, B);
			Assert.AreEqual(1f, crossProduct); // The cross product of (1,0) and (0,1) is 1

			crossProduct = MathHelpers.CrossProduct(B, A);
			Assert.AreEqual(-1f, crossProduct); // The reverse cross product is -1
		}

		[Test]
		public void TestSignedArea()
		{
			Vector2 A = new Vector2(0, 0);
			Vector2 B = new Vector2(1, 0);
			Vector2 C = new Vector2(0, 1);

			float area = MathHelpers.SignedArea(A, B, C);
			Assert.AreEqual(1f, area); // The area of the triangle (0,0), (1,0), (0,1) is 1

			C = new Vector2(0, -1);
			area = MathHelpers.SignedArea(A, B, C);
			Assert.AreEqual(-1f, area); // The area of the triangle (0,0), (1,0), (0,-1) is -1
		}

		[Test]
		public void TestIsCollinear()
		{
			Vector2 A = new Vector2(0, 0);
			Vector2 B = new Vector2(1, 0);
			Vector2 C = new Vector2(2, 0);

			bool isCollinear = MathHelpers.IsCollinear(A, B, C);
			Assert.IsTrue(isCollinear); // Points are collinear on the x-axis

			C = new Vector2(0, 1);
			isCollinear = MathHelpers.IsCollinear(A, B, C);
			Assert.IsFalse(isCollinear); // Points are not collinear
		}
	}
}
