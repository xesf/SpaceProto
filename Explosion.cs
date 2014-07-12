using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;

namespace net.xesf.games.vita.SpaceProto
{
	public class Explosion : GameObject
	{
		const int PARTICLE_MIN_SPEED = 50;
		const int PARTICLE_MAX_SPEED = 100;
		
		float elapsedTime = 0;
		
		public Explosion (GameplayBoard b, Vector2 pos, int radius)
			: base (b)
		{
			this.LoadContent("fragments");
			
			this.Position = Vector2.Rotate(new Vector2(0,radius), new Vector2(SpaceShooterGame.Instance.Random.NextSignedFloat(), SpaceShooterGame.Instance.Random.NextSignedFloat()));
			this.Velocity = this.Position.Normalize() * SpaceShooterGame.Instance.Random.Next(PARTICLE_MIN_SPEED, PARTICLE_MAX_SPEED);
			this.Position += pos;
		}
		
		public override void Tick (float dt)
		{
			base.Tick (dt);
			
			this.Color.A -= 0.015f;
			elapsedTime += dt;
			
			if (elapsedTime > 2 || 
			    this.Color.A == 0)
			{
				this.IsDead = true;
			}
		}
	}
}

