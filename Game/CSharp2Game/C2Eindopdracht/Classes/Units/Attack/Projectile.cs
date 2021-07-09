using C2Eindopdracht.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
	class Projectile : Attack, ITileSet
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
		/// <param name="knockbackTime">Time the knockback should last</param>
		/// <param name="xSpeed">Horizontal speed</param> 
		/// <param name="face">Direction which the projectile is facing</param>
		public Projectile(int damage, Cooldown cooldown, float activeTime, Rectangle hitbox, float knockbackTime, float xSpeed, Face face) : base(damage, cooldown, activeTime, hitbox, knockbackTime)
		{
			this.xSpeed = xSpeed;
			this.face = face;
		}

		/// <summary>
		/// Update the attack, elapsing the duration and moving it
		/// </summary>
		/// <param name="gameTime">Holds the timestate of a Game</param>
		/// <returns></returns>
		public override void update(GameTime gameTime)
		{
			move(gameTime);
			if (cooldown.elapsedTime >= activeTime)
			{
				expired = true;
			}
			else
			{
				cooldown.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
		}

		/// <summary>
		/// Move the attack to the direction it faces
		/// </summary>
		/// <param name="gameTime">Holds the timestate of a Game</param>
		public void move(GameTime gameTime)
		{
			Rectangle tempHitbox = hitbox;
			if (face == Face.LEFT)
			{
				tempHitbox.X -= (int)(xSpeed * gameTime.ElapsedGameTime.TotalSeconds);
			}
			else if(face == Face.RIGHT)
			{
				tempHitbox.X += (int)(xSpeed * gameTime.ElapsedGameTime.TotalSeconds);
			}
			hitbox = tempHitbox;
		}

		/// <summary>
		/// Draw the projectile to the screen
		/// </summary>
		/// <param name="spriteBatch">Helper class for drawing text strings and sprites in one or more optimized batches</param>
		/// <param name="renderHitboxes">Renders just the hitbox Rectangles if true</param>
		public override void draw(SpriteBatch spriteBatch, bool renderHitboxes)
		{
			if(renderHitboxes)
			{
				spriteBatch.Draw(
					Game1.blankTexture,
					hitbox,
					Color.Red
				);
			}
			else
			{
				spriteBatch.Draw(
					tileSet,
					new Vector2(hitbox.X, hitbox.Y),
					new Rectangle(face == Face.LEFT ? 0 : 20, 0, 20, 15),
					Color.White
				);
			}
		}
	}
}
