using System;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Imaging;

namespace net.xesf.games.vita.SpaceProto
{
	public class GameplayBoard : Node
	{
		const int NUM_ASTEROIDS = 2;
		const int NUM_PARTICLES_L = 40;
		const int NUM_PARTICLES_M = 20;
		const int NUM_PARTICLES_S = 10;
		
		//const string LEVEL_FORMAT = "{0}";
		const string SCORE_FORMAT = "{0}: {1}";
			
		private Ship ship;
		private List<Asteroid> Asteroids = new List<Asteroid>();
		private List<Bullet> Bullets = new List<Bullet>();
		private List<Explosion> Particles = new List<Explosion>();
		List<GameObject> ChildsToAdd = new List<GameObject>();
		List<GameObject> ChildsToRemove = new List<GameObject>();
		
		public int Level = 0;
		public int Score = 0;
		public int Lives = 3;
		
		private SoundPlayer _explosionSoundPlayer;
    	private Sound _explosionSound;
		
		private Sprite _bkg;
		
		Font font;
		FontMap fontMap;
		Label lbScore;
		
		bool IsGameOver = false;
		bool IsPaused = false;
		
		List<Sprite> spriteShips = new List<Sprite>(3);
		
		public Sprite _title;
		public Sprite _gameover;
		public Sprite _pause;
		private Sprite _touchorpress;
		private Sprite _pressstart;
		private Sprite _bottom;
		
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
		
		public GameplayBoard()
		{
			ship = new Ship(this);
			
			_explosionSound = new Sound("/Application/resources/boom.wav");
			_explosionSoundPlayer = _explosionSound.CreatePlayer();
			
			font = new Font(FontAlias.System, 28, FontStyle.Regular);
			fontMap = new FontMap(font, 28);
			
			// add title image
			_title = new Sprite();
			_title.LoadContent("title");
			_title.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_title.Width/2),this.Height - 130);
			
			_touchorpress = new Sprite();
			_touchorpress.LoadContent("touchorpress");
			_touchorpress.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_touchorpress.Width/2), 80);
			
			_pressstart = new Sprite();
			_pressstart.LoadContent("pressstart");
			_pressstart.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_pressstart.Width/2), 80);
			
			_bottom = new Sprite();
			_bottom.LoadContent("bottom");
			_bottom.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_bottom.Width/2), 0);
			
			// add title image
			_gameover = new Sprite();
			_gameover.LoadContent("gameover");
			_gameover.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_gameover.Width/2),this.Height - 250);
			
			_pause = new Sprite();
			_pause.LoadContent("pause");
			_pause.Position = new Sce.PlayStation.Core.Vector2((this.Width/2) - (_pause.Width/2),this.Height - 250);

			_bkg = new Sprite();
			_bkg.LoadContent("bkg");
			_bkg.Position = new Sce.PlayStation.Core.Vector2(0,0);
		}
		
		public void Initialize()
		{		
			IsPaused = false;
			IsGameOver = false;
			this.RemoveAllChildren(true);
			
			this.Asteroids.Clear();
			this.Bullets.Clear();
			this.Particles.Clear();
			
			ChildsToAdd.Clear();
			ChildsToRemove.Clear();
			
			this.AddChild(_bkg);
			
			Level = 0;
			Score = 0;
			Lives = 3;
			
			CreateHUD();
			
			ship.Initialize();
			this.AddChild(ship);
			
			LevelUp();
		}
		
		public void LevelUp()
		{
			Level++;
			
			if (Level > 3 && Lives < 10)
			{
				Lives++;
				UpdateLivesHUD();
			}
				
			for ( int i = 0; i < NUM_ASTEROIDS + Level; i++)
			{
				Vector2 pos = GetRandomPosition();
				Asteroid a = new Asteroid(this, AsteroidType.Large, pos);
				Asteroids.Add(a);
				this.AddChild(a);
			}
		}
		
		public void CreateHUD()
		{				
			lbScore = new Label(string.Format(SCORE_FORMAT, "1", "0"), fontMap);
			lbScore.Color = new Vector4(0.6f,0.6f,0.6f,1f);
			lbScore.Position = new Vector2(5, this.Height - fontMap.CharPixelHeight - 5);
			this.AddChild(lbScore, 999);
		
			UpdateLivesHUD();
		}
		
		public void UpdateLivesHUD()
		{
			foreach (Sprite spr in spriteShips)
			{
				this.RemoveChild(spr, false);
			}
			
			if (spriteShips.Count != 0)
			{
				spriteShips.RemoveAt(0);
			}
			
			int x = this.Width - ship.Width/2 - 5;
			int y = this.Height - ship.Height/2 - 5;
			for (int i = 0; i < Lives; i++)
			{
				Sprite spr = new Sprite();
				spr.Position = new Vector2(x, y);
				spr.Scale = new Vector2(0.65f, 0.65f);
				spr.LoadContent("ship");
				this.AddChild(spr, 980);
				this.spriteShips.Add(spr);
				x -= 20;
			}
		}
		
		public void Tick(float dt)
		{			
			if (IsPaused && Input2.GamePad0.Start.Press)
			{
				Pause(false);
			} 
			else if (!IsPaused && Input2.GamePad0.Start.Press)
			{
				Pause(true);
			}
			
			if(IsPaused)
				return;
			
			if (IsGameOver && (Input2.Touch00.Press  || Input2.GamePad0.Cross.Press))
			{
				Initialize();
			}
			
			// increase level
			if (Asteroids.Count == 0)
			{
				LevelUp();
			}
			
			// update HUD Score and Level
			lbScore.Text = string.Format(SCORE_FORMAT, Level, Score);
			
			if (!IsGameOver)
			{
				ship.Tick(dt);
			}
		
			foreach(Bullet b in this.Bullets)
			{
				b.Tick(dt);
				if (b.IsDead)
				{
					ChildsToRemove.Add(b);
				}
			}
			
			//if (!IsGameOver)
			{
				foreach(Asteroid a in this.Asteroids)
				{
					if(a.IsDead)
						continue;
					
					a.Tick(dt);
					
					foreach(Bullet b in this.Bullets)
					{				
						if(b.IsDead)
							continue;
						
						//b.Tick(dt);
						
						if (a.CollidesWith(b))
						{
							int partNUM = NUM_PARTICLES_S;
							
							Score += 120 / ((int)a.aType + 1);
							
							a.IsDead = true;
							b.IsDead = true;
							
							switch(a.aType)
							{
								case AsteroidType.Large:
									partNUM = NUM_PARTICLES_L;
									ChildsToAdd.Add(new Asteroid(this, AsteroidType.Medium, a.Position));
									ChildsToAdd.Add(new Asteroid(this, AsteroidType.Medium, a.Position));
									break;
								case AsteroidType.Medium:
									partNUM = NUM_PARTICLES_M;
									ChildsToAdd.Add(new Asteroid(this, AsteroidType.Small, a.Position));
									ChildsToAdd.Add(new Asteroid(this, AsteroidType.Small, a.Position));
									break;
							}
							
							_explosionSoundPlayer.Play();
							for (int i=0; i < partNUM; i++)
							{
								ChildsToAdd.Add(new Explosion(this, a.CenterPosition, a.Radius/2));
							}
							
							ChildsToRemove.Add(a);
							ChildsToRemove.Add(b);
						}
					}
					
					if (!IsGameOver && a.CollidesWith(ship))
					{	
						int partNUM = NUM_PARTICLES_S;
						
						Lives--;
						UpdateLivesHUD();
						
						ship.StopPlayer();
						
						_explosionSoundPlayer.Play();
						for (int i=0; i < partNUM; i++)
						{
							ChildsToAdd.Add(new Explosion(this, a.CenterPosition, a.Radius));
						}
						
						a.IsDead = true;
						ChildsToRemove.Add(a);
					}
				}
			}
			
			foreach(Explosion e in this.Particles)
			{
				e.Tick(dt);
				
				if (e.IsDead)
				{
					ChildsToRemove.Add(e);
				}
			}
			
			foreach(GameObject obj in ChildsToRemove)
			{
				if (obj is Asteroid)
				{
					this.Asteroids.Remove((Asteroid)obj);
				}
				else if (obj is Bullet)
				{
					this.Bullets.Remove((Bullet)obj);
				}
				else if (obj is Explosion)
				{
					this.Particles.Remove((Explosion)obj);
				}
				this.RemoveChild(obj, false);
			}
			ChildsToRemove.Clear();
			
			foreach(GameObject obj in ChildsToAdd)
			{
				if (obj is Asteroid)
				{
					this.Asteroids.Add((Asteroid)obj);
				}
				else if (obj is Bullet)
				{
					this.Bullets.Add((Bullet)obj);
				}
				else if (obj is Explosion)
				{
					this.Particles.Add((Explosion)obj);
				}

				this.AddChild(obj);
			}
			ChildsToAdd.Clear();
			
			// if we have no more lives
			if (Lives == 0 && !IsGameOver)
			{
				GameOver();
			}
		}
		
		private void Pause(bool paused)
		{
			IsPaused = paused;
			
			if (IsPaused)
			{
				this.AddChild(_title,900);
				this.AddChild(_pause,901);
				this.AddChild(_pressstart,902);
				this.AddChild(_bottom,903);
			}
			else
			{
				this.RemoveChild(_title, false);
				this.RemoveChild(_pause, false);
				this.RemoveChild(_pressstart, false);
				this.RemoveChild(_bottom, false);				
			}
		}
		
		private void GameOver()
		{
			IsGameOver = true;
			this.RemoveChild(ship, false);
			this.RemoveChild(lbScore, false);
			
			this.AddChild(_title,900);
			this.AddChild(_gameover,901);
			this.AddChild(_touchorpress,902);
			this.AddChild(_bottom,903);
		}
				
		private Vector2 GetRandomPosition()
		{
			Vector2 pos;
			float x = 0;
			float y = 0;
			while(true)
			{
				x = SpaceShooterGame.Instance.Random.Next(0, this.Width);
				y = SpaceShooterGame.Instance.Random.Next(0, this.Height);
				pos = new Vector2(x,y); 
				if (((ship.CenterPosition).Distance(pos) > 300))
					break;
			}
			return pos;
		}
		
		public void AddBullet(Bullet b)
		{
			this.Bullets.Add(b);
			this.AddChild(b);	
		}
	}
}

