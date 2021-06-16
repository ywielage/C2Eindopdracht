using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace C2Eindopdracht.Classes
{
    class Character
    {
        private Vector2 position;
        private Rectangle hitBox;
        public List<Attack> attacks { get; set; }
        public float gravity { get; set; }
        public float xSpeed { get; set; }
        public float ySpeed { get; set; }
        public Face face { get; set; }
        public bool grounded { get; set; }
        public bool doubleJumpAvailable { get; set; }
        public bool canDoubleJump { get; set; }
        public bool canAttack { get; set; }
        public Cooldown attackCooldown { get; set; }

        public Character(int xPos, int yPos, float gravity)
        {
            this.position = new Vector2(xPos, yPos);
            this.hitBox = new Rectangle(xPos, yPos, 18, 30);
            this.attacks = new List<Attack>();
            this.gravity = gravity;
            this.xSpeed = 100f;
            this.ySpeed = 0;
            this.face = Face.RIGHT;
            this.grounded = false;
            this.doubleJumpAvailable = false;
            this.canDoubleJump = true;
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

        public void checkCollisions(List<Rectangle> floors)
        {
            int touchingGrounds = 0;
            int i = 0;

            //Debug.WriteLine("====================================");
            foreach (Rectangle floor in floors)
            {
                if (floor.Left < hitBox.Right && floor.Right > hitBox.Left)
                {
                    if (floor.Top - hitBox.Bottom == 0)
                    {
                        touchingGrounds++;
                    }
                    else if (hitBox.Top - floor.Bottom < 1 && hitBox.Top - floor.Bottom > -1)
                    {
                        ySpeed = 0;
                    }
                    else if (floor.Top - hitBox.Bottom < 2 && floor.Top - hitBox.Bottom > -8)
                    {
                        touchingGrounds++;
                        position.Y = floor.Top - hitBox.Height;
                    }
                }
                else if (floor.Left == hitBox.Right)
                {
                    if((floor.Top < hitBox.Bottom && floor.Bottom > hitBox.Bottom) || (floor.Top < hitBox.Top && floor.Bottom > hitBox.Top) || (floor.Top > hitBox.Top && floor.Bottom < hitBox.Bottom))
                    {
                        position.X = floor.Left - 1 - hitBox.Width;
                    }
                    
                }
                else if (floor.Right == hitBox.Left)
                {
                    if ((floor.Top < hitBox.Bottom && floor.Bottom > hitBox.Bottom) || (floor.Top < hitBox.Top && floor.Bottom > hitBox.Top) || (floor.Top > hitBox.Top && floor.Bottom < hitBox.Bottom))
                    {
                        position.X = floor.Right + 2;
                    }
                }
                //Debug.WriteLine(i + ": Floor top " + floor.Top + " | Hiatbox bottom " + hitBox.Bottom);
                //Debug.WriteLine(i + ": Floor left " + floor.Left + " | Hitbox right " + hitBox.Right);
                //Debug.WriteLine(i + ": Floor right " + floor.Right + " | Hitbox left " + hitBox.Left);
                i++;
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
                doubleJumpAvailable = true;
                ySpeed = 0;
            }
            else
            {
                ySpeed += gravity;
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
            if(attackCooldown.elapsedTime >= attackCooldown.duration)
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

        public void jump(float jumpSpeed, float jumpStartHeight)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                if (grounded)
                {
                    ySpeed = jumpSpeed;
                    position.Y -= jumpStartHeight;
                    doubleJumpAvailable = true;
                }
                else
                {
                    if (doubleJumpAvailable)
                    {
                        ySpeed = jumpSpeed;
                        position.Y -= jumpStartHeight;
                        doubleJumpAvailable = false;
                    }
                }
            }
        }

        public void attack(int damage, Cooldown cooldown, float duration, Rectangle hitbox, int hitboxXOffSet)
        {
            if (face == Face.LEFT) 
            {
                hitbox.X -= hitboxXOffSet;
            }
            else
            {
                hitbox.X += hitboxXOffSet;
            }
            attacks.Add(new Attack(damage, cooldown, duration, hitbox));
        }

        public void printCharacterValues()
        {
            Debug.WriteLine("=============================");
            Debug.WriteLine("Character Pos:\tX " + position.X + ",\tY " + position.Y);
            Debug.WriteLine("Hitbox Pos:\t\tX " + hitBox.X + ",\tY " + hitBox.Y);
            Debug.WriteLine("Speed:\t\t\tX " + xSpeed + ",\tY" + ySpeed);
            Debug.WriteLine("Grounded: " + grounded);
            Debug.WriteLine("Double jump available: " + grounded);
        }
    }
    enum Face
    {
        LEFT,
        RIGHT
    }
}
