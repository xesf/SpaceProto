using System;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace net.xesf.games.vita.SpaceProto
{
	public class GameScene : Scene
	{
		public GameplayBoard Board;
		
		public GameScene ()
		{
		}
		
		public void Initialize()
		{		
			// set camera viewport
			this.Camera.SetViewFromViewport();
			
			// add gameplay board
			Board = new GameplayBoard();
			this.AddChild(Board);
		}
		
		public void StartGame()
		{		
			Board.Initialize();
			
			SpaceShooterGame.Instance.TitleScene.UnscheduleAll();
			this.ScheduleUpdate(1);
			
			var transition = new TransitionSolidFade(this) { Duration = 1.5f, Tween = Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Linear };
			Director.Instance.ReplaceScene(transition);
		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			TickGame(dt);
		}
		
		public void TickGame(float dt)
		{
			Board.Tick(dt);
		}
	}
}

