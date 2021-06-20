using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class Projectile : Attack
	{
		private float xSpeed;
		private Face face;
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
