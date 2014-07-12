using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace net.xesf.games.vita.SpaceProto
{
	public enum AsteroidType
	{
		Large,
		Medium,
		Small
	}
	
	public class Asteroid : GameObject
	{
		const float ASTEROID_ROT_SPEED = .5f;
		const float ASTEROID_LAR_SPEED = 50.0f;
		const float ASTEROID_MED_SPEED = 100.0f;
		const float ASTEROID_SMA_SPEED = 150.0f;
		
		public AsteroidType aType;
		
		public Asteroid (GameplayBoard b, AsteroidType type, Vector2 pos)
			: base (b)
		{
			aType = type;
			
			switch(type)
			{
				case AsteroidType.Large:
					Radius = 100;
					this.Velocity = Vector2.Rotate(new Vector2(ASTEROID_LAR_SPEED,0), new Vector2(SpaceShooterGame.Instance.Random.NextSignedFloat(), SpaceShooterGame.Instance.Random.NextSignedFloat()));
					LoadContent("asteroidb" + SpaceShooterGame.Instance.Random.Next(1,4).ToString());
					break;
				case AsteroidType.Medium:
					Radius = 50;
					this.Velocity = Vector2.Rotate(new Vector2(ASTEROID_MED_SPEED,0), new Vector2(SpaceShooterGame.Instance.Random.NextSignedFloat(), SpaceShooterGame.Instance.Random.NextSignedFloat()));
					LoadContent("asteroidm" + SpaceShooterGame.Instance.Random.Next(1,4).ToString());
					break;
				case AsteroidType.Small:
					Radius = 25;
					this.Velocity = Vector2.Rotate(new Vector2(ASTEROID_SMA_SPEED,0), new Vector2(SpaceShooterGame.Instance.Random.NextSignedFloat(), SpaceShooterGame.Instance.Random.NextSignedFloat()));
					LoadContent("asteroids" + SpaceShooterGame.Instance.Random.Next(1,4).ToString());
					break;
			}

			this.RotationVelocity = SpaceShooterGame.Instance.Random.NextSign() * ASTEROID_ROT_SPEED;
			this.Rotation = new Vector2(SpaceShooterGame.Instance.Random.NextFloat(), SpaceShooterGame.Instance.Random.NextFloat());
			this.Pivot = new Vector2(this.Width/2, this.Height/2);
			this.Position = pos;
		}
		
		protected override void CheckBounds()
		{
			int w = board.Width;
			int h = board.Height;
			
			Vector2 pos = this.Position;
			float x = pos.X;
			float y = pos.Y;
			
			if (pos.X + this.Width < 0)
			{
				x = w;
			}
			else if (pos.X > w)
			{
				x = -this.Width;
			}
			if (pos.Y + this.Height < 0)
			{
				y = h;
			}
			else if (pos.Y > h)
			{
				y = -this.Height;
			}
			
			this.Position = new Vector2(x, y);
		}
	}
}

