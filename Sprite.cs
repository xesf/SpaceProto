using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace net.xesf.games.vita.SpaceProto
{
	public class Sprite : SpriteUV
	{	
	 	TextureAtlasSprite _sprite;
		
		public int Width { get { return (int)_sprite.Width; } }
		public int Height { get { return (int)_sprite.Height; } }
		
		public static TRS PixelToUV(TextureInfo texInfo, float frameX, float frameY, float frameWidth, float frameHeight) 
		{
			TRS uv = TRS.Quad0_1;
			
			float spritesheetWidth  = texInfo.TextureSizef.X;
			float spritesheetHeight = texInfo.TextureSizef.Y;
			
			uv.T = new Vector2(frameX/spritesheetWidth, (spritesheetHeight - (frameY + frameHeight))/spritesheetHeight);
			uv.S = new Vector2(frameWidth/spritesheetWidth, frameHeight/spritesheetHeight);
			
			return uv;
		}
		
		public void LoadContent(string asset)
		{		
			_sprite = SpaceShooterGame.TextureAtlas.Get(asset);
			if (_sprite != null)
			{
				this.TextureInfo = SpaceShooterGame.TextureAtlas._textureInfo;
				this.Quad.S = new Vector2(_sprite.Width, _sprite.Height); 
				this.UV = PixelToUV(this.TextureInfo, _sprite.X, _sprite.Y, _sprite.Width, _sprite.Height);
			}
		}
	}
}
