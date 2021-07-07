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

		/// <summary>
		/// Constructor, sets all default values
		/// </summary>
		/// <param name="damage">Sets the amount of damage a projectile deals</param> 
		/// <param name="cooldown">Sets the cooldown until the next attack</param> 
		/// <param name="activeTime">Time the projectile is active</param>
		/// <param name="hitbox">Hitbox of projectile</param>  
		/// <param name="xSpeed">Horizontal speed</param> 
		/// <param name="face">Direction which the projectile is facing</param>
		public Projectile(int damage, Cooldown cooldown, float activeTime, Rectangle hitbox, float xSpeed, Face face) : base(damage, cooldown, activeTime, hitbox)
		{
			this.xSpeed = xSpeed;
			this.face = face;
		}

		public override Attack update(GameTime gameTime)
		{
			this.move(gameTime);
			if (cooldown.elapsedTime >= activeTime)
			{
				expired = true;
			}
			else
			{
				cooldown.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			}

			return this;
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
