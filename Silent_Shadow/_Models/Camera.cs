using Silent_Shadow.Models;
using Silent_Shadow.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Silent_Shadow._Models
{
	public class Camera
	{
		private Matrix _transform;
		private Vector2 _position;

		public float Zoom { get; set; } = 1.0f; // Zoom-Faktor
		public float Rotation { get; set; } = 0.0f; // Rotation der Kamera

		public Matrix Transform => _transform;

		public void Follow()
		{
			// Kamera zentriert auf den Spieler
			_position = Hero.Instance.position;

			// Transformationsmatrix
			_transform = Matrix.CreateTranslation(-_position.X, -_position.Y, 0) *
						 Matrix.CreateRotationZ(Rotation) *
						 Matrix.CreateScale(Zoom) *
						 Matrix.CreateTranslation(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2, 0);
		}
	}
}
