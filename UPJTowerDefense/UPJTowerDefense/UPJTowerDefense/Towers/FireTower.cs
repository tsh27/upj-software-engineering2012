using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class FireTower : Tower
    {
        // Enemies in range
        private List<Enemy> targets = new List<Enemy>();

        // Sprite sheet graphics
        private List<Explosion> fireTowerFlamesVertical = new List<Explosion>();
        private List<Explosion> fireTowerFlamesHorizontal = new List<Explosion>();

        // The direction of the FireTower
        private string direction;
        private int flameX, flameY;

        // Number of shots
        private int shotCount;

        /// <summary>
        /// Constructs a FireTower
        /// </summary>
        public FireTower(Texture2D texture, Texture2D bulletTexture,
            Vector2 position, string towerType, int towerID, SoundEffect towerShot)
            : base(texture, bulletTexture, position, towerType, towerID, towerShot)
        {
            this.damage = Util.fireTowerDamage;
            this.cost = Util.fireTowerCost;
            this.radius = Util.fireTowerRadius;
            this.totalWorth = Util.fireTowerCost;
            this.shootingDirection = new Vector2(1, 0);
            this.flameX = (int)position.X;
            this.flameY = (int)position.Y;
            shotCount = 0;

            // Add vertical graphics
            fireTowerFlamesVertical.Add(new Explosion(bulletTexture, 0, 0, 48, 48, 9));
            fireTowerFlamesVertical.Add(new Explosion(bulletTexture, 0, 0, 48, 48, 9));
            fireTowerFlamesVertical.Add(new Explosion(bulletTexture, 0, 0, 48, 48, 9));
            fireTowerFlamesVertical.Add(new Explosion(bulletTexture, 0, 0, 48, 48, 9));

            // Add horizontal graphics
            fireTowerFlamesHorizontal.Add(new Explosion(bulletTexture, 0, 48, 48, 48, 9));
            fireTowerFlamesHorizontal.Add(new Explosion(bulletTexture, 0, 48, 48, 48, 9));
            fireTowerFlamesHorizontal.Add(new Explosion(bulletTexture, 0, 48, 48, 48, 9));
            fireTowerFlamesHorizontal.Add(new Explosion(bulletTexture, 0, 48, 48, 48, 9));
        }

        /// <summary>
        /// Find enemies in range
        /// </summary>
        /// <param name="enemies">List of enemies on screen</param>
        public override void GetClosestEnemy(List<Enemy> enemies)
        {
            // Do a fresh search for targets
            targets.Clear();

            // Loop over all the enemies.
            foreach (Enemy enemy in enemies)
            {
                Vector2 direction = this.Center - enemy.Center;
                direction.Normalize();

                // Some wierd code to change x an y coordinates.
                // I don't know why this is the case, but it works.
                if (direction.X == 1 || direction.X == -1)
                {
                    direction.Y = direction.X;
                    direction.X = 0;
                }
                else if (direction.Y == 1 || direction.Y == -1)
                {
                    direction.X = direction.Y;
                    direction.Y = 0;
                }

                if (direction.X == 1)
                {
                    direction.X = -1;
                }
                else if (direction.X == -1)
                {
                    direction.X = 1;
                }

                // Check wether this enemy is in shooting distance
                if (IsInRange(enemy.Center))
                {
                    if (direction + shootingDirection == Vector2.Zero)
                    {
                        // Make it a target
                        targets.Add(enemy);
                    }
                }
            }
        }

        /// <summary>
        /// Update a FireTower
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Determine shooting direction and rotate accordingly
            if (shootingDirection.X > 0)
            {
                rotation = MathHelper.ToRadians(0);
                direction = "down";
            }
            else if (shootingDirection.X < 0)
            {
                rotation = MathHelper.ToRadians(180);
                direction = "up";
            }
            else if (shootingDirection.Y > 0)
            {
                rotation = MathHelper.ToRadians(90);
                direction = "right";
            }
            else
            {
                rotation = MathHelper.ToRadians(270);
                direction = "left";
            }

            flameX = (int)position.X;
            flameY = (int)position.Y;

            // Up and Down direction flame effects
            if (fireTowerFlamesVertical[0].IsActive == false && radiusUpgradeNumber >= 1 && (direction == "up" || direction == "down"))
            {
                flameY = (direction == "up") ? (int)position.Y + (48 * 2) : (int)position.Y - (48 * 2);
                fireTowerFlamesVertical[1].Activate(flameX, flameY);
            }
            if (fireTowerFlamesVertical[0].IsActive == false && radiusUpgradeNumber >= 2 && (direction == "up" || direction == "down"))
            {
                flameY = (direction == "up") ? (int)position.Y + (48 * 3) : (int)position.Y - (48 * 3);
                fireTowerFlamesVertical[2].Activate(flameX, flameY);
            }
            if (fireTowerFlamesVertical[0].IsActive == false && radiusUpgradeNumber == 3 && (direction == "up" || direction == "down"))
            {
                flameY = (direction == "up") ? (int)position.Y + (48 * 4) : (int)position.Y - (48 * 4);
                fireTowerFlamesVertical[3].Activate(flameX, flameY);
            }
            if (fireTowerFlamesVertical[0].IsActive == false && (direction == "up" || direction == "down"))
            {
                flameY = (direction == "up") ? (int)position.Y + 48 : (int)position.Y - 48;
                fireTowerFlamesVertical[0].Activate(flameX, flameY);
            }

            // Left and Right direction flame effects
            if (fireTowerFlamesHorizontal[0].IsActive == false && radiusUpgradeNumber >= 1 && (direction == "right" || direction == "left"))
            {
                flameX = (direction == "right") ? (int)position.X + (48 * 2) : (int)position.X - (48 * 2);
                fireTowerFlamesHorizontal[1].Activate(flameX, flameY);
            }
            if (fireTowerFlamesHorizontal[0].IsActive == false && radiusUpgradeNumber >= 2 && (direction == "right" || direction == "left"))
            {
                flameX = (direction == "right") ? (int)position.X + (48 * 3) : (int)position.X - (48 * 3);
                fireTowerFlamesHorizontal[2].Activate(flameX, flameY);
            }
            if (fireTowerFlamesHorizontal[0].IsActive == false && radiusUpgradeNumber == 3 && (direction == "right" || direction == "left"))
            {
                flameX = (direction == "right") ? (int)position.X + (48 * 4) : (int)position.X - (48 * 4);
                fireTowerFlamesHorizontal[3].Activate(flameX, flameY);
            }
            if (fireTowerFlamesHorizontal[0].IsActive == false && (direction == "right" || direction == "left"))
            {
                flameX = (direction == "right") ? (int)position.X + 48 : (int)position.X - 48;
                fireTowerFlamesHorizontal[0].Activate(flameX, flameY);
            }

            // Update vertical explosions
            foreach (Explosion flames in fireTowerFlamesVertical)
            {
                flames.Update(gameTime);
            }

            // Update horizontal explosions
            foreach (Explosion flames in fireTowerFlamesHorizontal)
            {
                flames.Update(gameTime);
            }


            int tempDamage = damage;

            // If the modifier has finished,
            if (boostModifierCurrentTime > boostModifierDuration)
            {
                // Reset the modifier.
                boostModifier = 0;
                boostModifierCurrentTime = 0;
            }

            if (boostModifier != 0 && boostModifierCurrentTime <= boostModifierDuration)
            {
                tempDamage *= boostModifier;
                boostModifierCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Loop through all the possible targets
            for (int t = 0; t < targets.Count; t++)
            {
                // If this bullet hits a target and is in range
                if (targets[t] != null)
                {
                    if (Options.soundEffectsOn && shotCount == 0)
                    {
                        towerShot.Play(0.5f, 0f, 0f);
                    }
                    ++shotCount;
                    if (shotCount > 3)
                    {
                        shotCount = 0;
                    }

                    // Inflict damage
                    if (targets[t].SpeciesType == "Equator")
                    {
                        targets[t].CurrentHealth -= tempDamage * (1 - targets[t].Resistance);
                    }
                    else
                    {
                        targets[t].CurrentHealth -= tempDamage;
                    }
                }
            }
        }

        /// <summary>
        /// Draws a FireTower
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (willShoot)
            {
                foreach (Explosion flames in fireTowerFlamesVertical)
                {
                    flames.Draw(spriteBatch);
                }

                foreach (Explosion flames in fireTowerFlamesHorizontal)
                {
                    flames.Draw(spriteBatch);
                }
            }
        }
    }
}
