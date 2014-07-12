using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Imaging;
using System.Collections.Generic;
using Sce.PlayStation.Core;

namespace net.xesf.games.vita.SpaceProto
{
	public class TitleScene : Scene
	{
		public Sprite _title;
		public Sprite _bottom;
		public Sprite _touchorpress;
		private Sprite _bkg;
		
		private List<Asteroid> Asteroids = new List<Asteroid>();
		
		#region Properties
		public int Width
		{
			get { return Director.Instance.GL.Context.Screen.Width; }
		}
		public int Height
		{
			get { return Director.Instance.GL.Context.Screen.Height; }
		}
		#endregion
		
		public TitleScene ()
		{
		}
		
		public void Initialize()
		{		
			Asteroids.Clear();
			
			_bkg = new Sprite();
			_bkg.LoadContent("bkg");
			_bkg.Position = new Sce.PlayStation.Core.Vector2(0,0);
				
			// add title image
			_title = new Sprite();
			_title.LoadContent("title");
			_title.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_title.Width/2),this.Height - 130);
			
			_touchorpress = new Sprite();
			_touchorpress.LoadContent("touchorpress");
			_touchorpress.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_touchorpress.Width/2), 80);
			
			_bottom = new Sprite();
			_bottom.LoadContent("bottom");
			_bottom.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_bottom.Width/2), 0);
			
			// set camera viewport
			this.Camera.SetViewFromViewport();
			
			this.AddChild(_bkg);
			
			for ( int i = 0; i < 10; i++)
			{
				Vector2 pos = new Vector2(SpaceShooterGame.Instance.Random.Next(0, this.Width), SpaceShooterGame.Instance.Random.Next(0, this.Height));
				Asteroid a = new Asteroid(new GameplayBoard(), (AsteroidType)SpaceShooterGame.Instance.Random.Next(0,3), pos);
				Asteroids.Add(a);
				this.AddChild(a);
			}
			
			this.AddChild(_title);
			this.AddChild(_touchorpress);
			this.AddChild(_bottom);
		}
		
		public void StartTitle()
		{		
			SpaceShooterGame.Instance.GameScene.UnscheduleAll();
			this.ScheduleUpdate(1);

			var transition = new TransitionSolidFade(this) { Duration = 1.5f, Tween = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Linear };
			Director.Instance.ReplaceScene(transition);
		}
		
		public override void Update (float dt)
  		{
			base.Update (dt);
			TickTitle(dt);
		}
		
		public void TickTitle(float dt)
		{
			foreach(Asteroid a in this.Asteroids)
			{
				a.Tick(dt);
			}
			
			// wait for transition
			if (Director.Instance.CurrentScene != this)
			{
				return;
			}

			if (Input2.Touch00.Press || Input2.GamePad0.Cross.Press)
			{
				SpaceShooterGame.Instance.GameScene.StartGame();
			}
		}
	}
}

