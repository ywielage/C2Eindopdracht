using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace C2Eindopdracht.Classes
{
    abstract class Enemy
    {
        protected Vector2 position;
        protected Rectangle hitBox;
        public abstract int maxHp { get; set; }
        public abstract int currHp { get; set; }
        public List<Attack> attacks { get; set; }
        public float gravity { get; set; }
        public abstract float xSpeed { get; set; }
        public float ySpeed { get; set; }
        public Face face { get; set; }
        public bool grounded { get; set; }
        public bool canAttack { get; set; }
        public Cooldown attackCooldown { get; set; }
        public abstract Aggression aggression { get; set; }
        public bool isAlive { get; set; }
        public Cooldown knockback { get; set; }
        public abstract int attackRange { get; set; }
        public HealthBar healthBar { get; set; }

        /// <summary>
        /// Algemene klas voor enemies
        /// </summary>
        /// <param name="xPos"></param> Horizontale positie
        /// <param name="yPos"></param> Verticale positie
        /// <param name="width"></param> Wijdte enemy 
        /// <param name="height"></param> Hoogte enemy
        public Enemy(int xPos, int yPos, int width, int height)
        {
            this.position = new Vector2(xPos, yPos);
            this.hitBox = new Rectangle(xPos, yPos, width, height);
            this.attacks = new List<Attack>();
            this.gravity = gravity;
            this.ySpeed = 0;
            this.face = Face.RIGHT;
            this.grounded = false;
            this.canAttack = true;
            this.knockback = null;
            this.attackCooldown = new Cooldown(0);
            this.isAlive = true;
            this.healthBar = new HealthBar(new Rectangle(xPos, yPos, 20, 10), 20, Color.Purple, 0, -15);
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
        /// <summary>
        /// Update van enemy 
        /// </summary>
        /// <param name="gameTime"></param> Tijdsduur van game
        /// <param name="levelComponents"></param> Lijst van levelcomponents
        /// <param name="player"></param> // Player object
        /// <param name="ui"></param> // UI object
        public void update(GameTime gameTime, List<List<LevelComponent>> levelComponents, Player player, UI ui)
        {
            checkCollisions(levelComponents, player, ui);
            if (knockback != null)
            {
                updateKnockBack(gameTime);
            }
            else
            {
                decideMovement(gameTime, player);
            }
            updateAttacks(gameTime);
            alignHitboxToPosition();
            alignHealthBarToPosition();
            //printEnemyValues();
        }
        /// <summary>
        /// Set hitbox naar positie enemy
        /// </summary>
        private void alignHitboxToPosition()
        {
            Rectangle hitbox = this.hitBox;
            hitbox.Location = position.ToPoint();
            this.hitBox = hitbox;
        }
        /// <summary>
        /// Set healthbar naar positie enemy
        /// </summary>
        private void alignHealthBarToPosition()
        {
            int healthBarHeight = this.healthBar.getBar().Height;
            int healthBarWidth = this.healthBar.getBar().Width;
            this.healthBar.setBar(new Rectangle(new Point((int)position.X + healthBar.xOffset, (int)position.Y + healthBar.yOffset), new Point(healthBarWidth, healthBarHeight)));
        }
        /// <summary>
        /// Bekijkt botsingen tussen enemy en walls
        /// </summary>
        /// <param name="walls"></param> Muren van het level
        /// <param name="player"></param> Speler object
        /// <param name="ui"></param> UI object
        private void checkCollisions(List<List<LevelComponent>> walls, Player player, UI ui)
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
            
            foreach (Attack attack in attacks)
            {
                if (attack.getHitbox().Intersects(player.getHitbox()) && attack.playerHit == false && !player.shieldActive)
                {
                    player.currHp -= 1;
                    player.healthBar.updateHealthBar(player.maxHp, player.currHp);
                    player.knockback = new Cooldown(.2f);
                    if(position.X < player.getPosition().X)
                    {
                        player.xSpeed = 0 - player.xSpeed;
                    }
                    player.ySpeed = -5f;
                    player.setPosition(new Vector2(player.getPosition().X, player.getPosition().Y - 5f));
                    attack.hitPlayer();
                    if(player.currHp <= 0)
					{
                        player.gameOver(ui);
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
        /// <summary>
        /// Update de attacks
        /// </summary>
        /// <param name="gameTime"></param> Tijdsduur van game
        private void updateAttacks(GameTime gameTime)
        {
            for (int i = 0; i < attacks.Count; i++)
            {
                if (attacks[i].cooldown.elapsedTime <= attacks[i].cooldown.duration && canAttack)
                {
                    canAttack = false;
                    attackCooldown = new Cooldown(attacks[i].cooldown.duration);
                }
                if (attacks[i] is Projectile)
                {
                    Projectile projectile = (Projectile)attacks[i];
                    projectile.move(gameTime);
                    attacks[i] = projectile;
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
                if (xSpeed < 0)
                {
                    xSpeed = xSpeed - xSpeed * 2;
                }
            }
        }

        public abstract void decideMovement(GameTime gameTime, Player player);

        protected void moveLeft(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X -= xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.LEFT;
            }
        }

        protected void moveRight(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X += xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.RIGHT;
            }
        }

        protected void jump(float jumpSpeed, float jumpStartHeight)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                if (grounded)
                {
                    ySpeed = 0 - jumpSpeed;
                    position.Y -= jumpStartHeight;
                }
            }
        }

        public abstract Attack attack(int damage, Cooldown cooldown, float duration, Rectangle hitbox, int hitboxXOffSet);
        /// <summary>
        /// Print waarden van enemy
        /// </summary>
        public void printEnemyValues()
        {
            Debug.WriteLine("=============================");
            Debug.WriteLine("Enemy Pos:\tX " + position.X + ",\tY " + position.Y);
            Debug.WriteLine("Hitbox Pos:\t\tX " + hitBox.X + ",\tY " + hitBox.Y);
            Debug.WriteLine("Speed:\t\t\tX " + xSpeed + ",\tY" + ySpeed);
            Debug.WriteLine("Grounded: " + grounded);
            Debug.WriteLine("Double jump available: " + grounded);
        } 
    }
    /// <summary>
    /// Enum waarin alle verschillende mogelijke aggressieniveau's staan
    /// </summary>
    enum Aggression
    {
        FRIENDLY,
        NEUTRAL,
        AGGRESSIVE
    }
}
