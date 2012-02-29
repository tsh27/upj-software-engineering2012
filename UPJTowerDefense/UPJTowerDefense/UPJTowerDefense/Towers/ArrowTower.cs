using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    /// <summary>
    /// Basic Tower called an Arrow Tower
    /// Inherits from a Tower
    /// </summary>
    public class ArrowTower : Tower
    {
        /// <summary>
        /// Constructs an ArrowTower
        /// </summary>
        /// <param name="texture">what the ArrowTower will look like</param>
        /// <param name="bulletTexture">what its bullets look like</param>
        /// <param name="position">where is the tower located</param>
        /// <param name="towerType">"Arrow Tower" is the type</param>
        /// <param name="towerID">unique id for the tower</param>
        /// <param name="towerShot">what the shot sounds like</param>
        public ArrowTower(Texture2D texture, Texture2D bulletTexture, 
            Vector2 position, string towerType, int towerID, SoundEffect towerShot)
            : base(texture, bulletTexture, position, towerType, towerID, towerShot)
        {
            this.damage = 10;
            this.cost = 150;

            //set the radius
            //[tileSize + (tileSize/2)]
            this.radius = 72;

            this.towerType = towerType;
            this.towerID = towerID;
            this.towerShot = towerShot;
        }

        /// <summary>
        /// Update an ArrowTower
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //has enough time passed to shoot an enemy?
            //and is the target still living?
            if (bulletTimer >= 0.5f && target != null)
            {
                //make a bulled
                Bullet bullet = new Bullet(bulletTexture, Vector2.Subtract(center,
                    new Vector2(bulletTexture.Width / 2)), rotation, 6, damage);

                //add to bulletList
                bulletList.Add(bullet);

                //reset bulletTimer
                bulletTimer = 0;
            }

            //update each bullet
            for (int i = 0; i < bulletList.Count; i++)
            {
                Bullet bullet = bulletList[i];

                bullet.SetRotation(rotation);
                bullet.Update(gameTime);

                //if bullet is out of range
                //dismiss it
                if (!IsInRange(bullet.Center))
                    bullet.Kill();

                //is enemy still living?
                //and has the bullet reached the target
                if (target != null && Vector2.Distance(bullet.Center, target.Center) < 8)
                {
                    //if sound isn't muted
                    if (util.soundOn)
                    {
                        //play shot sound
                        towerShot.Play(0.5f, 0f, 0f);
                    }

                    //decrease enemy health
                    target.CurrentHealth -= bullet.Damage;

                    //remove bullet
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
