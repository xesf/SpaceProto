using System;
using System.Diagnostics;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace net.xesf.games.vita.SpaceProto
{
	public class AppMain
	{
		public static void Main(string[] args)
		{
			Initialize();

			while (true) {
				SystemEvents.CheckEvents();
				Update();
				Render();
			}
		}

		public static void Initialize()
		{
			Director.Initialize();
			SpaceShooterGame.Instance = new SpaceShooterGame();
			SpaceShooterGame.Instance.Initialize();
		}

		public static void Update()
		{
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.SetBlendMode(BlendMode.Normal);
			Director.Instance.Update();
		}

		public static void Render()
		{
			Director.Instance.Render();

			// Present the screen
			Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();
			Director.Instance.PostSwap();
		}
	}
}
