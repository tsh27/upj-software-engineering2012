using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class FreezeTower : Tower
    {
        // Enemies in range
        private List<Enemy> targets = new List<Enemy>();

        // Time elapsed since last update
        private TimeSpan elapsedTime = TimeSpan.Zero;

        // Graphics for FreezeTower
        private List<Explosion> freezeExplosion = new List<Explosion>();

        // Defines how fast an enemy will move when hit.
        private float speedModifier;
        // Defines how long this effect will last.
        private float speedModifierDuration;

        public override bool HasTarget
        {
            // The tower will never have just one target.
            get { return false; }
        }

        public FreezeTower(Texture2D texture, Texture2D freezeBulletTexture, 
            Vector2 position, string towerType, int towerID, SoundEffect towerShot)
            : base(texture, freezeBulletTexture, position, towerType, towerID, towerShot)
        {
            this.damage = Util.freezeTowerDamage;
            this.cost = Util.freezeTowerCost;
            this.totalWorth = Util.freezeTowerCost;
            this.radius = Util.freezeTowerRadius;

            this.speedModifier = 0.5f;
            this.speedModifierDuration = 2.0f;
            freezeExplosion.Add(new Explosion(bulletTexture, 0, 1008, 144, 144, 9));
            freezeExplosion.Add(new Explosion(bulletTexture, 0, 768, 240, 240, 9));
            freezeExplosion.Add(new Explosion(bulletTexture, 0, 432, 336, 336, 9));
            freezeExplosion.Add(new Explosion(bulletTexture, 0, 0, 432, 432, 9));
        }

        public override void GetClosestEnemy(List<Enemy> enemies)
        {
            // Do a fresh search for targets.
            targets.Clear();

            // Loop over all the enemies.
            foreach (Enemy enemy in enemies)
            {
                // Check wether this enemy is in shooting distance.
                if (IsInRange(enemy.Center))
                {
                    // Make it a target.
                    targets.Add(enemy);
                }
            }
        }

        /// <summary>
        /// Update a FreezeTower
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            int tempDamage = damage;
            float tempFireRate = 3f;

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
                tempFireRate /= boostModifier;
                boostModifierCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(tempFireRate) && Options.inRound)
            {
                switch (RadiusUpgradeNumber)
                {
                    case 1:
                        freezeExplosion[1].Activate((int)Position.X - (Texture.Width * 2), (int)Position.Y - (Texture.Height * 2));
                        break;
                    case 2:
                        freezeExplosion[2].Activate((int)Position.X - (Texture.Width * 2) - (Texture.Width / 2) - (Util.tileWidth / 2), (int)Position.Y - (Texture.Height * 2) - (Texture.Height / 2) - (Util.tileHeight / 2));
                        break;
                    case 3:
                        freezeExplosion[3].Activate((int)Position.X - (Texture.Width * 2) - (Util.tileWidth * 2), (int)Position.Y - (Texture.Height * 2) - (Util.tileHeight * 2));
                        break;
                    default:
                        freezeExplosion[0].Activate((int)Position.X - Texture.Width, (int)Position.Y - Texture.Height);
                        break;
                }
                elapsedTime = TimeSpan.Zero;
                willShoot = true;
                if (Options.soundEffectsOn)
                {
                    // Play shot sound
                    towerShot.Play(0.3f, 0f, 0f);
                }

                // Loop through all the possible targets
                for (int t = 0; t < targets.Count; t++)
                {
                    // If this bullet hits a target and is in range,
                    if (targets[t] != null)
                    {
                        // hurt the enemy.
                        if (targets[t].SpeciesType == "Pole")
                        {
                            targets[t].CurrentHealth -= tempDamage * (1 - targets[t].Resistance);
                        }
                        else
                        {
                            targets[t].CurrentHealth -= tempDamage;
                        }
                    }

                    // Apply our speed modifier if it is better than
                    // the one currently affecting the target :
                    if (targets[t].SpeedModifier <= speedModifier && targets[t].SpeciesType != "Pole")
                    {
                        targets[t].SpeedModifier = speedModifier;
                        targets[t].ModifierDuration = speedModifierDuration;
                    }
                }

                if (willShoot && elapsedTime > TimeSpan.FromSeconds(.2) && Options.inRound)
                {
                    willShoot = false;
                    elapsedTime = TimeSpan.Zero;
                }
            }

            foreach (Explosion explosion in freezeExplosion)
            {
                explosion.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws a FreezeTower
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Explosion explosion in freezeExplosion)
            {
                explosion.Draw(spriteBatch);
            }
        }
    }
}
