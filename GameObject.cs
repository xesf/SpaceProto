using System;
using Sce.PlayStation.Core;

namespace net.xesf.games.vita.SpaceProto
{
	public class GameObject : Sprite
	{
		protected GameplayBoard board;
			
		public int Radius;
		public float RotationVelocity;
		public Vector2 Velocity;
				
		#region Properties
		public Vector2 CenterPosition
		{
			get { return this.Position + this.Pivot; }
		}
		public bool IsDead { get; set; }
		#endregion
		
		public GameObject (GameplayBoard b)
		{
			board = b;
		}
		
		public virtual void Tick(float dt)
		{
			// rotate ship
			this.Angle += RotationVelocity * dt;
			// update ship position
			this.Position += this.Velocity * dt;
			
			CheckBounds();
		}
		
		public bool CollidesWith(GameObject other)
		{
			return ((this.CenterPosition).Distance(other.CenterPosition) < Radius + other.Radius);			
		}
		
		protected virtual void CheckBounds()
		{
			int w = board.Width;
			int h = board.Height;
			
			Vector2 pos = this.Position;
			float x = pos.X;
			float y = pos.Y;
			
			if (pos.X < 0)
			{
				this.IsDead = true;
			}
			else if (pos.X > w)
			{
				this.IsDead = true;
			}
			if (pos.Y < 0)
			{
				this.IsDead = true;
			}
			else if (pos.Y > h)
			{
				this.IsDead = true;
			}
		}
	}
}

