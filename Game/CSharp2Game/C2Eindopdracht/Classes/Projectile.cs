using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class Projectile : Attack
	{
		public float xSpeed { get; set; }
		public Face face { get; set; }
		public static Texture2D tileSet { get; set; }
		public Projectile(int damage, Cooldown cooldown, float activeTime, Rectangle hitbox, float xSpeed, Face face) : base(damage, cooldown, activeTime, hitbox)
		{
			this.xSpeed = xSpeed;
			this.face = face;
		}

		public void move(GameTime gameTime)
		{
			if (face == Face.LEFT)
			{
				hitbox.X -= (int)(xSpeed * gameTime.ElapsedGameTime.TotalSeconds);
			}
			else if(face == Face.RIGHT)
			{
				hitbox.X += (int)(xSpeed * gameTime.ElapsedGameTime.TotalSeconds);
			}
		}
	}
}
