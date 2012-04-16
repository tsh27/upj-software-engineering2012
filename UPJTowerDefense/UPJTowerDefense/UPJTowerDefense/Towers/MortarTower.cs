using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class MortarTower : Tower
    {
        /// <summary>
        /// Constructs a MortarTower
        /// </summary>
        public MortarTower(Texture2D texture, Texture2D mortarBulletTexture,
            Vector2 position, string towerType, int towerID, SoundEffect towerShot)
            : base(texture, mortarBulletTexture, position, towerType, towerID, towerShot)
        {
            this.damage = Util.mortarTowerDamage;
            this.cost = Util.mortarTowerCost;
            this.totalWorth = Util.mortarTowerCost;
            this.radius = Util.mortarTowerRadius;
        }

        /// <summary>
        /// Update a MortarTower
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

            // Has enough time passed to shoot an enemy?
            // and is the target still living?
            if (bulletTimer >= tempFireRate && target != null)
            {
                // Make a bullet
                Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center,
                    new Vector2(bulletTexture.Width / 2)), rotation, 30, damage);

                // Add to bulletList
                bulletList.Add(bullet);

                // Reset bulletTimer
                bulletTimer = 0;
            }

            // Update each bullet
            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];

                bullet.SetRotation(rotation);
                bullet.Update(gameTime);

                // If bullet is out of range
                // dismiss it
                if (!IsInRange(bullet.Center))
                    bullet.Kill();

                // Is enemy still living?
                // and has the bullet reached the target
                if (target != null && Vector2.Distance(bullet.Center, target.Center) < 30)
                {
                    // If sound isn't muted
                    if (Options.soundEffectsOn)
                    {
                        towerShot.Play(0.2f, 0f, 0f);
                    }

                    // Decrease enemy health                    
                    target.CurrentHealth -= tempDamage;

                    // Remove bullet
                    bullet.Kill();
                }

                //has bullet reached end of life
                if (bullet.IsDead())
                {
                    bulletList.Remove(bullet);
                    i--;
                }
            }
        }
    }
}