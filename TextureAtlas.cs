using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace net.xesf.games.vita.SpaceProto
{
	public class TextureAtlasSprite
	{
		public string Name;
		public int X;
		public int Y;
		public int Width;
		public int Height;
	}
	
	public class TextureAtlas
	{
		public TextureInfo _textureInfo;
		private Texture2D _texture;
		private System.Collections.Generic.Dictionary<string, TextureAtlasSprite> _sprites;
		
		public TextureAtlas (string asset)
		{
			XDocument doc = XDocument.Load(asset);
			var lines = from sprite in doc.Root.Elements("sprite")
			select new
			{
				Name = sprite.Attribute("n").Value,
				X1 = (int)sprite.Attribute ("x"),
				Y1 = (int)sprite.Attribute ("y"),
				Height = (int)sprite.Attribute ("h"),
				Width = (int)sprite.Attribute("w")
			};
			
			_sprites = new Dictionary<string, TextureAtlasSprite>();
			foreach(var curLine in lines)
			{
				TextureAtlasSprite sp = new TextureAtlasSprite();
				sp.Name = curLine.Name;
				sp.X = curLine.X1;
				sp.Y = curLine.Y1;
				sp.Width = curLine.Width;
				sp.Height = curLine.Height;
				
				_sprites.Add(curLine.Name, sp);
			}
			 
			string textureImage = doc.Root.Attribute("imagePath").Value;
			_texture = new Texture2D(textureImage ,false);
			_textureInfo = new TextureInfo(_texture);
		}
		  
		~TextureAtlas()
		{
			_texture.Dispose();
			_textureInfo.Dispose ();
		}
		
		public TextureAtlasSprite Get(string name)
		{
			return _sprites[name];
		}
	}
}

