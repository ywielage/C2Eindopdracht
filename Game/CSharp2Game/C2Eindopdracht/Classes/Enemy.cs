﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2Eindopdracht.Classes
{
    class Enemy
    {
        private Vector2 position;
        private Rectangle hitBox;
        public List<Attack> attacks { get; set; }
        public float gravity { get; set; }
        public float xSpeed { get; set; }
        public float ySpeed { get; set; }
        public Face face { get; set; }
        public bool grounded { get; set; }
        public bool canAttack { get; set; }
        public Cooldown attackCooldown { get; set; }
        public Aggression aggression { get; set; }

        public Enemy(int xPos, int yPos, float gravity, float xSpeed)
        {
            this.position = new Vector2(xPos, yPos);
            this.hitBox = new Rectangle(xPos, yPos, 18, 30);
            this.attacks = new List<Attack>();
            this.gravity = gravity;
            this.xSpeed = xSpeed;
            this.ySpeed = 0;
            this.face = Face.RIGHT;
            this.grounded = false;
            this.canAttack = true;
            this.attackCooldown = new Cooldown(0);
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public Rectangle getHitbox()
        {
            return this.hitBox;
        }

        public void setHitbox(Rectangle hitBox)
        {
            this.hitBox = hitBox;
        }

        public void alignHitboxToPosition()
        {
            Rectangle hitbox = this.hitBox;
            hitbox.Location = position.ToPoint();
            this.hitBox = hitbox;
        }

        public void checkCollisions(List<List<LevelComponent>> walls)
        {
            int touchingGrounds = 0;

            foreach (List<LevelComponent> rowList in walls)
            {
                foreach (LevelComponent levelComponent in rowList)
                {
                    foreach (Rectangle wall in levelComponent.colliders)
                    {
                        if (wall.Left < hitBox.Right && wall.Right > hitBox.Left)
                        {
                            if (wall.Top - hitBox.Bottom == 0)
                            {
                                touchingGrounds++;
                            }
                            else if (hitBox.Top - wall.Bottom < 1 && hitBox.Top - wall.Bottom > -10)
                            {
                                ySpeed = 0;
                            }
                            else if (wall.Top - hitBox.Bottom < 1 && wall.Top - hitBox.Bottom > -11)
                            {
                                touchingGrounds++;
                                position.Y = wall.Top - hitBox.Height;
                            }
                        }
                        if (wall.Top < hitBox.Bottom && wall.Bottom > hitBox.Top)
                        {
                            if (hitBox.Left - wall.Right < 1 && hitBox.Left - wall.Right > -5)
                            {
                                position.X = wall.Right + 2;
                            }
                            if (wall.Left - hitBox.Right < 1 && wall.Left - hitBox.Right > -5)
                            {
                                position.X = wall.Left - hitBox.Width - 2;
                            }
                        }
                    }
                }
            }

            if (touchingGrounds >= 1)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
            if (grounded)
            {
                ySpeed = 0;
            }
            else
            {
                if (ySpeed < 10)
                {
                    ySpeed += gravity;
                }
                position.Y += ySpeed;
            }
        }

        public void updateAttacks(GameTime gameTime)
        {
            for (int i = 0; i < attacks.Count; i++)
            {
                if (attacks[i].cooldown.elapsedTime <= attacks[i].cooldown.duration && canAttack)
                {
                    canAttack = false;
                    attackCooldown = new Cooldown(attacks[i].cooldown.duration);
                }
                if (attacks[i].cooldown.elapsedTime >= attacks[i].activeTime)
                {
                    attacks.RemoveAt(i);
                }
                else
                {
                    attacks[i].cooldown.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (!canAttack)
            {
                if (attackCooldown.elapsedTime >= attackCooldown.duration)
                {
                    canAttack = true;
                }
                else
                {
                    attackCooldown.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public void moveLeft(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X -= xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.LEFT;
            }
        }

        public void moveRight(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X += xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.RIGHT;
            }
        }
    }

    enum Aggression 
    {
        FRIENDLY,
        NEUTRAL,
        AGGRESSIVE
    }
}
