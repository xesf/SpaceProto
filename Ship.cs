using System;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.UI;
using System.Collections.Generic;
using Sce.PlayStation.Core.Audio;

namespace net.xesf.games.vita.SpaceProto
{
	public class Ship : GameObject
	{
		const float SHIP_ROT_SPEED = 5.0f;
		const float SHIP_ACCELERATION = 10.0f;
		const float SHIP_MAX_SPEED = 400.0f;
		const float SHIP_DAMPENING_FACTOR = 0.05f;
		const float BULLET_SPEED = 700.0f;
		const float SHIP_SIDE_ACCELERATION = 5.0f;
				
		private SoundPlayer _bulletSoundPlayer;
    	private Sound _bulletSound;
		private SoundPlayer _thrustSoundPlayer;
    	private Sound _thrustSound;
		
		private GameObject thrust;
		
		public Ship (GameplayBoard b) 
			: base (b)
		{
			Radius = 20;
			
			LoadContent("ship");
			RecenterShip();
			
			thrust = new GameObject(this.board);
			thrust.LoadContent("thruster");
			thrust.Position = new Vector2(0, -2);
			
			_bulletSound = new Sound("/Application/resources/fire.wav");
			_bulletSoundPlayer = _bulletSound.CreatePlayer();
			_thrustSound = new Sound("/Application/resources/thrust.wav");
			_thrustSoundPlayer = _thrustSound.CreatePlayer();
			_thrustSoundPlayer.Loop = true;
		}
		
		public void Initialize()
		{
			this.Rotation = Vector2.One;
			this.Velocity = Vector2.Zero;
			this.RotationVelocity = 0;
			
			this.RecenterShip();
		}
		
		public void RecenterShip()
		{
			this.Pivot = new Vector2(this.Width/2, this.Height/2);
			this.Position = SpaceShooterGame.Instance.GameScene.Camera.CalcBounds().Center;
			this.Position = new Vector2(this.Position.X - this.Pivot.X,
			                            this.Position.Y - this.Pivot.Y);
		}
		
		public void StopPlayer()
		{
			_thrustSoundPlayer.Stop();
		}
		
		public override void Tick(float dt)
		{	
			UpdateInput(dt);
			base.Tick(dt);
		}
		
		private void AddThruster()
		{
			if (!this.Children.Contains(thrust))
			{
				this.AddChild(thrust);
			}
		}
		
		private void RemoveThruster()
		{
			if (this.Children.Contains(thrust))
			{
				this.RemoveChild(thrust, false);
			}
		}
		
		private void UpdateInput(float dt)
		{
			if(Input2.GamePad0.Left.On)
			{
				RotationVelocity = SHIP_ROT_SPEED;			
			}
			else if (Input2.GamePad0.Right.On) 
			{
	            RotationVelocity = -SHIP_ROT_SPEED;
	        }
			else
			{
				RotationVelocity = 0;				
			}
			
			if (Input2.GamePad0.AnalogLeft != Vector2.Zero)
			{
				RotationVelocity = -SHIP_ROT_SPEED * Input2.GamePad0.AnalogLeft.X;
			}		
			
			
			if (Input2.GamePad0.Up.On || Input2.GamePad0.Cross.On)
			{
				if(_thrustSoundPlayer.Status != SoundStatus.Playing)
				{
					_thrustSoundPlayer.Play();
					AddThruster();
				}
				this.Velocity += Vector2.Rotate(new Vector2(0,-SHIP_ACCELERATION), -this.Rotation);
			}
			if (Input2.GamePad0.AnalogRight != Vector2.Zero &&
			    Input2.GamePad0.AnalogRight.Y < 0)
			{
				if(_thrustSoundPlayer.Status != SoundStatus.Playing)
				{
					_thrustSoundPlayer.Play();
					AddThruster();
				}
				this.Velocity += Vector2.Rotate(new Vector2(0, SHIP_ACCELERATION * Input2.GamePad0.AnalogRight.Y), -this.Rotation);
			}
			
			if (Input2.GamePad0.Up.Release || 
			    Input2.GamePad0.Cross.Release ||
			    (Input2.GamePad0.AnalogRight == Vector2.Zero &&  
			     !Input2.GamePad0.Up.On &&
			 	 !Input2.GamePad0.Cross.On))
			{
				if(_thrustSoundPlayer.Status == SoundStatus.Playing)
				{
					_thrustSoundPlayer.Stop();
					RemoveThruster();
				}
			}
			
			if (Input2.GamePad0.Square.On)
			{
				this.Velocity += Vector2.Rotate(new Vector2(SHIP_SIDE_ACCELERATION,0), -this.Rotation);
			}
			if (Input2.GamePad0.Circle.On)
			{
				this.Velocity += Vector2.Rotate(new Vector2(-SHIP_SIDE_ACCELERATION,0), -this.Rotation);
			}
				 
			// check full speed
			if (this.Velocity.Length() > SHIP_MAX_SPEED)
			{
				this.Velocity = this.Velocity.Normalize() * SHIP_MAX_SPEED;
			}
	
			// dampen ship velocity:
			this.Velocity *= (1 - SHIP_DAMPENING_FACTOR * dt);
					
			// Add bullets
			if (Input2.GamePad0.L.Press ||
			    Input2.GamePad0.R.Press)
			{
				AddBullet();
			}
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

		private void AddBullet()
		{
			Vector2 pos = this.Position + this.Pivot - Vector2.Rotate((new Vector2(-4, 10)), -this.Rotation);
			Vector2 vel = Vector2.Rotate(new Vector2(0, -BULLET_SPEED), -this.Rotation);
			Bullet b = new Bullet(board, pos, vel, this.Rotation);
			
			board.AddBullet(b);
			
			_bulletSoundPlayer.Play();
		}
	}
}

