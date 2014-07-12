using System;
using Sce.PlayStation.Core;

namespace net.xesf.games.vita.SpaceProto
{
	public class Bullet : GameObject
	{
		public Bullet (GameplayBoard b, Vector2 pos, Vector2 vel, Vector2 rot)
			: base (b)
		{
			Radius = 1;

			this.Position = pos;
			this.Rotation = rot;
			this.Velocity = vel;
			
			LoadContent("bullet");
		}
	}
}

