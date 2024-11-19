using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Silent_Shadow.Models.AI;

namespace Silent_Shadow.Tests
{

	[TestFixture]
	public class AIGruntTest
	{
		private Game1 game1; 
		private GameServiceContainer services;
		private GraphicsDeviceManager graphicsDeviceManager;

		private Grunt Grunt;

		[SetUp]
		public void Setup()
		{
			game1 = new Game1();
			services = new GameServiceContainer();
			graphicsDeviceManager = game1.Graphics;
			services.AddService<IGraphicsDeviceService>(graphicsDeviceManager);

			var content = new ContentManager(services)
			{
				RootDirectory = "Content"
			};

			Globals.Content = content;
			
			Grunt = new Grunt();
		}

		[Test]
		public void TestPlayerInVisionCone()
		{
			Vector2 A = new(0, 0);
			Vector2 B = new(2, 7);
			Vector2 C = new(-2, 7);

			Assert.Multiple(() =>
			{
				Assert.That(Grunt, Is.Not.Null);
				Assert.That(Grunt.PlayerInVisionCone(A, B, C, new Vector2(-0.08f, 4.7f)), Is.True);
				Assert.That(Grunt.PlayerInVisionCone(A, B, C, new Vector2(-1.84f, 4.04f)), Is.False);
			});
		}

		[Test]
		public void TestPlayerInDetectionShape()
		{
			Vector2 A = new(-5, 0);
			Vector2 B = new(-3, 0);
			Vector2 C = new(-6, 2);
			Vector2 D = new(-2, 2);
			Vector2 E = new(-6, 7);
			Vector2 F = new(-2, 7);

			Assert.Multiple(() =>
			{
				Assert.That(Grunt, Is.Not.Null);
				Assert.That(Grunt.PlayerInDetectionShape(A, B, C, D, E, F, new Vector2(-4.34855f, 3.97477f)), Is.True);
				Assert.That(Grunt.PlayerInDetectionShape(A, B, C, D, E, F, new Vector2(-3.2366f, 8.19716f)), Is.False);
			});
		}

		[TearDown]
		public void TearDown()
		{
			Globals.Content?.Dispose();
			graphicsDeviceManager?.Dispose();
			game1?.Dispose();
		}
	}
}
