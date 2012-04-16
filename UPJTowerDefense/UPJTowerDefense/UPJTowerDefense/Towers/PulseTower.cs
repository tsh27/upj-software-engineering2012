using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class PulseTower : Tower
    {
        // Enemies in range
        private List<Enemy> targets = new List<Enemy>();

        // Time elapsed since last update
        private TimeSpan elapsedTime = TimeSpan.Zero;

        // Graphics for the PulseTower
        private List<Explosion> pulseExplosion = new List<Explosion>();

        public override bool HasTarget
        {
            // The tower will never have just one target.
            get { return false; }
        }

        /// <summary>
        /// Constructs a PulseTower
        /// </summary>
        public PulseTower(Texture2D texture, Texture2D pulseEffect,
            Vector2 position, string towerType, int towerID, SoundEffect towerShot)
            : base(texture, pulseEffect, position, towerType, towerID, towerShot)
        {
            this.damage = Util.pulseTowerDamage;
            this.cost = Util.pulseTowerCost;
            this.totalWorth = Util.pulseTowerCost;
            this.radius = Util.pulseTowerRadius;

            pulseExplosion.Add(new Explosion(bulletTexture, 0, 1008, 144, 144, 9));
            pulseExplosion.Add(new Explosion(bulletTexture, 0, 768, 240, 240, 9));
            pulseExplosion.Add(new Explosion(bulletTexture, 0, 432, 336, 336, 9));
            pulseExplosion.Add(new Explosion(bulletTexture, 0, 0, 432, 432, 9));
        }

        /// <summary>
        /// Get enemies in range
        /// </summary>
        /// <param name="enemies">Enemies on screen</param>
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
        /// Update a PulseTower
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            int tempDamage = damage;
            float tempFireRate = 5f;

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
                        pulseExplosion[1].Activate((int)Position.X - (Texture.Width * 2), (int)Position.Y - (Texture.Height * 2));
                        break;
                    case 2:
                        pulseExplosion[2].Activate((int)Position.X - (Texture.Width * 2) - (Texture.Width / 2) - (Util.tileWidth / 2), (int)Position.Y - (Texture.Height * 2) - (Texture.Height / 2) - (Util.tileHeight / 2));
                        break;
                    case 3:
                        pulseExplosion[3].Activate((int)Position.X - (Texture.Width * 2) - (Util.tileWidth * 2), (int)Position.Y - (Texture.Height * 2) - (Util.tileHeight * 2));
                        break;
                    default:
                        pulseExplosion[0].Activate((int)Position.X - Texture.Width, (int)Position.Y - Texture.Height);
                        break;
                }
                elapsedTime = TimeSpan.Zero;
                willShoot = true;
                if (Options.soundEffectsOn)
                {
                    // Play shot sound
                    towerShot.Play(0.5f, 0f, 0f);
                }

                // Loop through all the possible targets
                for (int t = 0; t < targets.Count; t++)
                {
                    // If this bullet hits a target and is in range,
                    if (targets[t] != null)
                    {
                        // hurt the enemy.
                        if (targets[t].SpeciesType == "Deep")
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

            
            if (willShoot && elapsedTime > TimeSpan.FromSeconds(.2) && Options.inRound)
            {
                willShoot = false;
                elapsedTime = TimeSpan.Zero;
            }

            foreach (Explosion explosion in pulseExplosion)
            {
                explosion.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Explosion explosion in pulseExplosion)
            {
                explosion.Draw(spriteBatch);
            }
        }
    }
}
