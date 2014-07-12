using System;

using Sce.PlayStation.Core;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;
using Sce.PlayStation.Core.Graphics;

namespace net.xesf.games.vita.SpaceProto
{

	public class SpaceShooterGame
	{
		public static SpaceShooterGame Instance;
		
		public Random Random = new Random(DateTime.Now.Millisecond);
		
		public GameScene GameScene;
		public TitleScene TitleScene;
		
		public static Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();
		public static Dictionary<string, TextureInfo> TextureInfoCache = new Dictionary<string, TextureInfo>();
		
		public static TextureAtlas TextureAtlas;
		
		public SpaceShooterGame ()
		{
		}
		
		public void Initialize()
		{
			TextureAtlas = new TextureAtlas("/Application/resources/sprites.xml");
			
			// Scenes
			TitleScene = new TitleScene();
			GameScene = new GameScene();
			
			TitleScene.Initialize();
			GameScene.Initialize();

			Director.Instance.RunWithScene(new Scene(),true);

			// force tick so the scene is set
			Director.Instance.Update();
			
			SpaceShooterGame.Instance.TitleScene.StartTitle();
		}
	}
}

