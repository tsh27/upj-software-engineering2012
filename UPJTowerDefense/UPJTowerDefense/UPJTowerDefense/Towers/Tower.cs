using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class Tower : Sprite
    {
        //what kind of tower do we have
        protected string towerType;

        //how much will the tower cost to make
        protected int cost; 

        //how much damage the tower can inflict on an enemy
        protected int damage; 

        //how far the tower can shoot
        protected float radius;

        //unique id for each tower placed
        protected int towerID;

        //which enemy the tower is currently targeting
        protected Enemy target;
        
        //how long ago a bullet was fired
        protected float bulletTimer;

        //what a bullet will be rendered as on the screen
        protected Texture2D bulletTexture;

        //bullets the tower has
        protected List<Bullet> bulletList = new List<Bullet>();

        //what a shot sounds like during the game
        protected SoundEffect towerShot;

        public int Cost
        {
            get { return cost; }
        }
        public int Damage
        {
            get { return damage; }
        }

        public float Radius
        {
            get { return radius; }
        }

        public Enemy Target
        {
            get { return target; }
        }

        public string TowerType
        {
            get { return towerType; }
        }

        public int TowerID
        {
            get { return towerID; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Texture2D BulletTexture
        {
            get { return bulletTexture; }
        }

        public SoundEffect TowerShot
        {
            get { return towerShot; }
        }

        /// <summary>
        /// Does the tower currently have a target?
        /// </summary>
        public virtual bool HasTarget
        {
            // Check if the tower has a target.
            get { return target != null; }
        }

        /// <summary>
        /// Constructs a Tower
        /// </summary>
        /// <param name="texture">what the tower will look like</param>
        /// <param name="bulletTexture">what the bullets will look like</param>
        /// <param name="position">Coordinates of the tower</param>
        /// <param name="towerType">what kind of tower we have</param>
        /// <param name="towerID">unique id of the tower</param>
        /// <param name="towerShot">what a shot sounds like</param>
        public Tower(Texture2D texture, Texture2D bulletTexture, Vector2 position, string towerType, int towerID, SoundEffect towerShot)
            : base(texture, position)
        {
            this.bulletTexture = bulletTexture;
            this.towerID = towerID;
            this.towerType = towerType;
            this.towerShot = towerShot;
        }

        /// <summary>
        /// Rotate the tower to face the enemy
        /// that will be attacked
        /// </summary>
        protected void FaceTarget()
        {
            Vector2 direction = center - target.Center;
            direction.Normalize();

            rotation = (float)Math.Atan2(-direction.X, direction.Y);
        }

        /// <summary>
        /// Is the tower close enough to the enemy
        /// to shoot it?
        /// </summary>
        /// <param name="position">Postion of the targeted enemy</param>
        /// <returns>
        /// True if tower is in range
        /// False if tower is out of range
        /// </returns>
        public bool IsInRange(Vector2 position)
        {
            return Vector2.Distance(center, position) <= radius;
        }

        /// <summary>
        /// Gets enemy located closest to the tower
        /// </summary>
        /// <param name="enemies">Enemies of current wave passe in from Player</param>
        public virtual void GetClosestEnemy(List<Enemy> enemies)
        {
            target = null;
            float smallestRange = radius;

            foreach (Enemy enemy in enemies)
            {
                if (Vector2.Distance(center, enemy.Center) < smallestRange)
                {
                    smallestRange = Vector2.Distance(center, enemy.Center);
                    target = enemy;
                }
            }
        }

        /// <summary>
        /// Update a Tower
        /// </summary>
        /// <param name="gameTime">current gameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //update bullet timer
            bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (target != null)
            {
                FaceTarget();

                if (!IsInRange(target.Center) || target.IsDead)
                {
                    target = null;
                    bulletTimer = 0;
                }
            }
        }

        /// <summary>
        /// Draws a Tower and bullets
        /// to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Bullet bullet in bulletList)
                bullet.Draw(spriteBatch);
        }
    }
}
