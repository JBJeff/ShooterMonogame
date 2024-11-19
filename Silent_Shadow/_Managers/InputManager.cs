using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Silent_Shadow.Models;

namespace Silent_Shadow.Managers 
{
	public static class InputManager
	{
		private static KeyboardState _keyboardState;
		private static MouseState _mouseState;
		private static Vector2 _mousePosition { get { return new Vector2(_mouseState.X, _mouseState.Y); } }

		public static void Update()
		{
			_keyboardState = Keyboard.GetState();
			_mouseState = Mouse.GetState();
		}

		public static Vector2 GetMovementDirection()
		{
			Vector2 direction = new Vector2();
			foreach (var key in _keyboardState.GetPressedKeys())
			{
				switch (key)
				{
					case Keys.W:
					case Keys.Up:
						direction.Y -= 1;
						break;
					case Keys.A:
					case Keys.Left:
						direction.X -= 1;
						break;
					case Keys.S:
					case Keys.Down:
						direction.Y += 1;
						break;
					case Keys.D:
					case Keys.Right:
						direction.X += 1;
						break;
				}
			}

			if (direction.LengthSquared() > 1)
			{
				direction.Normalize();
			}
			return direction;
		}

		public static Vector2 GetAimDirection()
		{
			Vector2 direction = _mousePosition - Hero.Instance.position;
			if (direction == Vector2.Zero)
			{
				return Vector2.Zero;
			}
			else
			{
				return Vector2.Normalize(direction);
			}
		}

		// For Fire
		public static bool IsLeftMouseHeld()
        {
        return _mouseState.LeftButton == ButtonState.Pressed;
        }
		//Waffenwechsel durch zahlen
		public static bool IsKeyPressed(Keys key)
        {
        return _keyboardState.IsKeyDown(key) && Keyboard.GetState().IsKeyUp(key);
        }
	}
}
