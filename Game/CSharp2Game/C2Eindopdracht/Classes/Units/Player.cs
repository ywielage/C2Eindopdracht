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
        public bool shieldActive { get; set; }
        public bool isAlive { get; set; }
        public Cooldown knockback { get; set; }
        public Cooldown attackCooldown { get; set; }
        public HealthBar healthBar { get; set; }

        private int shieldTime { get; set; }

        private int maxShieldTime { get; set; }
        private int shieldRefill { get; set; }
        private bool shieldUsed { get; set; }

        /// <summary>
        /// Sets default values
        /// </summary>
        /// <param name="xPos">Horizontal position</param> 
        /// <param name="yPos">Vertical position</param> 
        /// <param name="hp">Amount of lives</param>
        /// <param name="gravity">Takes the player back to ground</param> 
        /// <param name="xSpeed">Horizontal speed</param> 
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
            this.shieldActive = false;
            this.isAlive = true;
            this.knockback = null;
            this.attackCooldown = new Cooldown(0);
            this.healthBar = new HealthBar(new Rectangle(xPos, yPos, 50, 10), 50, Color.Gold, -17, -15);

            this.maxShieldTime = 120;
            this.shieldRefill = 1;
            this.shieldUsed = false;
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
        /// Update game
        /// </summary>
        /// <param name="gameTime">Used to measure time</param>
        /// <param name="levelComponents">List of levelcomponents</param>
        /// <param name="enemies">List of enemies</param>
        /// <param name="enemyCounter">Count of alive enemies</param>
        public void update(GameTime gameTime, List<List<LevelComponent>> levelComponents, List<Enemy> enemies, UIElementLabelValue enemyCounter)
		{
            checkCollisions(levelComponents, enemies, enemyCounter);
            if(knockback != null)
            {
                updateKnockBack(gameTime);
            }
            else if (knockback == null && isAlive)
            {
                checkKeyPresses(gameTime);
            }
            updateAttacks(gameTime);
			alignHitboxToPosition();
            alignHealthBarToPosition();
            //printPlayerValues();
        }

        /// <summary>
        /// Update hitbox position
        /// </summary>
        private void alignHitboxToPosition()
        {
            Rectangle hitbox = this.hitBox;
            hitbox.Location = position.ToPoint();
            this.hitBox = hitbox;
        }

        /// <summary>
        /// Update healthbar position
        /// </summary>
        private void alignHealthBarToPosition()
        {
            int healthBarHeight = this.healthBar.getBar().Height;
            int healthBarWidth = this.healthBar.getBar().Width;
            this.healthBar.setBar(new Rectangle(new Point((int)position.X + healthBar.xOffset, (int)position.Y + healthBar.yOffset), new Point(healthBarWidth, healthBarHeight)));
        }

        /// <summary>
        /// Check if collides with walls
        /// </summary>
        /// <param name="walls">All walls it can collide with</param>
        /// <param name="enemies">List of enemies</param>
        /// <param name="enemyCounter">Count of alive enemies</param>
        private void checkCollisions(List<List<LevelComponent>> walls, List<Enemy> enemies, UIElementLabelValue enemyCounter)
        {
            int touchingGrounds = 0;

            foreach (List<LevelComponent> rowList in walls)
            {
                foreach (LevelComponent levelComponent in rowList)
                {
                    foreach (Rectangle wall in levelComponent.colliders)
                    {
                        touchingGrounds = countTouchingGrounds(touchingGrounds, wall);
                    }
                }
            }

            foreach(Enemy enemy in enemies)
			{
                foreach(Attack attack in attacks)
                {
                    setEnemyLogic(enemyCounter, enemy, attack);
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

        /// <summary>
        /// Set all enemy logic
        /// </summary>
        /// <param name="enemyCounter">Count of alive enemies</param>
        /// <param name="enemy">The enemy getting attacked</param>
        /// <param name="attack">The player attack</param>
        private void setEnemyLogic(UIElementLabelValue enemyCounter, Enemy enemy, Attack attack)
        {
            if (attack.getHitbox().Intersects(enemy.getHitbox()) && !attack.enemiesHit.Contains(enemy))
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
                if (enemy.currHp <= 0)
                {
                    enemy.isAlive = false;
                    enemyCounter.value--;
                }
            }
        }

        /// <summary>
        /// Check with how many walls the player collides
        /// </summary>
        /// <param name="touchingGrounds">Amount of colliding walls</param>
        /// <param name="wall">The current wall</param>
        /// <returns>How many walls it collides with</returns>
        private int countTouchingGrounds(int touchingGrounds, Rectangle wall)
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

            return touchingGrounds;
        }

        /// <summary>
        /// Update all player attacks
        /// </summary>
        /// <param name="gameTime"></param>
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

        /// <summary>
        /// Update the knockback
        /// </summary>
        /// <param name="gameTime"></param>
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

        /// <summary>
        /// Check if a key is pressed
        /// A move left, D move right
        /// W or Space to jump
        /// J to attack or K to shield
        /// </summary>
        /// <param name="gameTime">The game time</param>
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

            if (SmartKeyboard.HasBeenPressed(Keys.Space) || SmartKeyboard.HasBeenPressed(Keys.W))
            {
                jump(6f, 3f);
            }

            if (SmartKeyboard.HasBeenPressed(Keys.J) && canAttack && !shieldActive)
            {
                attack(1, new Cooldown(.5f), .2f, new Rectangle((int)position.X, (int)position.Y, 24, 24), 5);
            }

            if (keyboardState.IsKeyDown(Keys.K) && canAttack && shieldTime <= maxShieldTime && shieldUsed == false)
            {
                shieldActive = true;
                shieldTime++;
                if(shieldTime == maxShieldTime) {
                    shieldUsed = true;
                }
            }
            else if(shieldUsed == true)
            {
                shieldActive = false;
                shieldTime = shieldTime - shieldRefill;
                if (shieldTime == 0)
                {
                    shieldUsed = false;
                }
            }
            else if(keyboardState.IsKeyUp(Keys.K))
            {
                shieldActive = false;
            }
        }

        /// <summary>
        /// Move the player left
        /// </summary>
        /// <param name="gameTime"></param>
        private void moveLeft(GameTime gameTime)
        {
            if(attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X -= xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.LEFT;
            }
        }

        /// <summary>
        /// Move the player right
        /// </summary>
        /// <param name="gameTime"></param>
        private void moveRight(GameTime gameTime)
        {
            if (attackCooldown.elapsedTime >= attackCooldown.duration)
            {
                position.X += xSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                face = Face.RIGHT;
            }
        }

        /// <summary>
        /// Let the player jump with a given speed
        /// </summary>
        /// <param name="jumpSpeed">Speed to jump with</param>
        /// <param name="jumpStartHeight">Offset to start jump from</param>
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

        /// <summary>
        /// Player attack 
        /// </summary>
        /// <param name="damage">Amount of damage the attack does</param>
        /// <param name="cooldown">Cooldown for the next attack</param>
        /// <param name="duration">Duration of the attack</param>
        /// <param name="hitbox">Hitbox of the attack which enemies can colide with</param>
        /// <param name="hitboxXOffSet">Offset of the hitbox, if the player is facing left the attack comes out slightly more to that side</param>
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

        /// <summary>
        /// Game over
        /// </summary>
        /// <param name="ui">UI to add element to</param>
        public void gameOver(UI ui)
		{
            isAlive = false;
            ui.addUIElement("You've died!", new Vector2(5, 50), 0);
        }

        /// <summary>
        /// Print values of the player
        /// </summary>
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
