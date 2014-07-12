using System;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core;

namespace AsteroidsVita
{
	/*public class SpriteHelper
	{
		public static TextureFilterMode DefaultTextureFilterMode = TextureFilterMode.Linear;
		public static Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();
		public static Dictionary<string, TextureInfo> TextureInfoCache = new Dictionary<string, TextureInfo>();
		
		public static Sce.PlayStation.HighLevel.GameEngine2D.SpriteUV SpriteUVFromFile(string filename)
		{
			if (TextureCache.ContainsKey(filename) == false)
			{
				TextureCache[filename] = new Texture2D(filename, false);
				TextureInfoCache[filename] = new TextureInfo(TextureCache[filename]);
			}

			var tex = TextureCache[filename];
			var info = TextureInfoCache[filename];
			var result = new Sce.PlayStation.HighLevel.GameEngine2D.SpriteUV() { TextureInfo = info };

			result.Quad.S = new Vector2(info.Texture.Width, info.Texture.Height);

			tex.SetFilter(DefaultTextureFilterMode);

			return result;
		}
	}*/
}

