using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace C2Eindopdracht.Classes
{
    class Player
    {
        private Vector2 position;
        private Rectangle hitBox;
        public int maxHp { get; set; }
        public int currHp { get; set; }
        public static Texture2D tileSet { get; set; }
        public List<Attack> attacks { get; set; }
        public float gravity { get; set; }
        public float xSpeed { get; set; }
        public float ySpeed { get; set; }
        public Face face { get; set; }
        public bool grounded { get; set; }
        public bool doubleJumpAvailable { get; set; }
        public bool canDoubleJump { get; set; }
        public bool canAttack { get; set; }
        public Cooldown knockback { get; set; }
        public Cooldown attackCooldown { get; set; }
        public HealthBar healthBar { get; set; }

        public Player(int xPos, int yPos, int hp, float gravity, float xSpeed)
        {
            this.position = new Vector2(xPos, yPos);
            this.hitBox = new Rectangle(xPos, yPos, 18, 30);
            this.maxHp = hp;
            this.currHp = hp;
            this.attacks = new List<Attack>();
            this.gravity = gravity;
            this.xSpeed = xSpeed;
            this.ySpeed = 0;
            this.face = Face.RIGHT;
            this.grounded = false;
            this.doubleJumpAvailable = false;
            this.canDoubleJump = true;
            this.canAttack = true;
            this.knockback = null;
            this.attackCooldown = new Cooldown(0);
            this.healthBar = new HealthBar(new Rectangle(xPos, yPos, 50, 10), 50, Color.LightGreen, -15, -15);
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

        public void update(GameTime gameTime, List<List<LevelComponent>> levelComponents, List<Enemy> enemies)
		{
            checkCollisions(levelComponents, enemies);
            if(knockback != null)
            {
                updateKnockBack(gameTime);
            }
            else
            {
                checkKeyPresses(gameTime);
            }
            updateAttacks(gameTime);
			alignHitboxToPosition();
            alignHealthBarToPosition();
            //printPlayerValues();
        }

        private void alignHitboxToPosition()
        {
            Rectangle hitbox = this.hitBox;
            hitbox.Location = position.ToPoint();
            this.hitBox = hitbox;
        }

        private void alignHealthBarToPosition()
        {
            int healthBarHeight = this.healthBar.getBar().Height;
            int healthBarWidth = this.healthBar.getBar().Width;
            this.healthBar.setBar(new Rectangle(new Point((int)position.X + healthBar.xOffset, (int)position.Y + healthBar.yOffset), new Point(healthBarWidth, healthBarHeight)));
        }

        private void checkCollisions(List<List<LevelComponent>> walls, List<Enemy> enemies)
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
                            else if (hitBox.Top - wall.Bottom < 1 && hitBox.Top - wall.Bottom > - 10)
                            {
                                ySpeed = 0;
                            }
                            else if (wall.Top - hitBox.Bottom < 1 && wall.Top - hitBox.Bottom > - 11)
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

            foreach(Enemy enemy in enemies)
			{
                foreach(Attack attack in attacks)
				{
                    if(attack.getHitbox().Intersects(enemy.getHitbox()) && !attack.enemiesHit.Contains(enemy))
					{
                        enemy.currHp -= 1;
                        enemy.healthBar.updateHealthBar(enemy.maxHp, enemy.currHp);
                        enemy.knockback = new Cooldown(.5f);
                        if (position.X < enemy.getPosition().X)
                        {
                            enemy.xSpeed = 0 - enemy.xSpeed;
                        }
                        enemy.ySpeed = -5f;
                        enemy.setPosition(new Vector2(enemy.getPosition().X, enemy.getPosition().Y - 5f));
                        attack.hitEnemy(enemy);
                        if (enemy.currHp == 0)
                        {
                            enemy.isAlive = false;
                        }
                    }
				}
			}

            List<Enemy> deadEnemies = new List<Enemy>();
            foreach(Enemy enemy in enemies)
			{
                if(!enemy.isAlive)
				{
                    deadEnemies.Add(enemy);
				}
			}
            enemies.RemoveAll(enemy => deadEnemies.Contains(enemy));

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
                if(ySpeed < 10)
                {
                    ySpeed += gravity;
                }
                position.Y += ySpeed;
            }
        }

        

        private void updateAttacks(GameTime gameTime)
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

        private void updateKnockBack(GameTime gameTime)
        {
            knockback.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X -= xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (knockback.duration <= knockback.elapsedTime)
            {
                knockback = null;
                if(xSpeed < 0)
                {
                    xSpeed = xSpeed - xSpeed * 2;
                }
            }
        }

        private void checkKeyPresses(GameTime gameTime)
        {
            var keyboardState = SmartKeyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveLeft(gameTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveRight(gameTime);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.Space))
            {
                jump(6f, 3f);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.J) && canAttack)
            {
                attack(1, new Cooldown(.5f), .2f, new Rectangle((int)position.X, (int)position.Y, 24, 24), 5);
            }
        }

        private void moveLeft(GameTime gameTime)
        {
            if(attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X -= xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.LEFT;
            }
        }

        private void moveRight(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X += xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.RIGHT;
            }
        }

        private void jump(float jumpSpeed, float jumpStartHeight)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                if (grounded)
                {
                    ySpeed = 0 - jumpSpeed;
                    position.Y -= jumpStartHeight;
                    doubleJumpAvailable = true;
                }
                else
                {
                    if (doubleJumpAvailable)
                    {
                        ySpeed = 0 - jumpSpeed;
                        position.Y -= jumpStartHeight;
                        doubleJumpAvailable = false;
                    }
                }
            }
        }

        private void attack(int damage, Cooldown cooldown, float duration, Rectangle hitbox, int hitboxXOffSet)
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

        public void gameOver()
		{
            Debug.WriteLine("Game Over");
        }

        public void printPlayerValues()
        {
            Debug.WriteLine("=============================");
            Debug.WriteLine("Player Pos:\tX " + position.X + ",\tY " + position.Y);
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
