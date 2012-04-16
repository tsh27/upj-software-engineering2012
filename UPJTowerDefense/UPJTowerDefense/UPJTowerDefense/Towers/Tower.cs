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
        // What kind of tower do we have
        protected string towerType;

        // How much will the tower cost to place
        protected int cost;

        //how much the tower is worth after upgrades
        protected int totalWorth = 0;

        // How much damage the tower can inflict on an enemy
        protected int damage; 

        // How far the tower can shoot
        protected float radius;

        // Unique id for each tower placed
        protected int towerID;

        // Which upgrade the tower is at
        protected int damageUpgradeNumber = 0;

        // Which radius upgrade the tower is at
        protected int radiusUpgradeNumber = 0;

        // Which enemy the tower is currently targeting
        protected Enemy target;
        
        // How long ago a bullet was fired
        protected float bulletTimer;

        // What a bullet will be rendered as on the screen
        protected Texture2D bulletTexture;

        // Bullets the tower has
        protected List<Bullet> bulletList = new List<Bullet>();

        // What a shot sounds like during the game
        protected SoundEffect towerShot;

        // Boost modifier set during tower boost
        protected int boostModifier;

        // Time span of boostModifier
        protected float boostModifierDuration;
        protected float boostModifierCurrentTime;

        protected Vector2 shootingDirection;
        protected bool willShoot = false;

        public int Cost
        {
            get { return cost; }
        }

        public int TotalWorth
        {
            get { return totalWorth; }
            set { totalWorth = value; }
        }

        public int Damage
        {
            get
            {
                if (boostModifier == 0f)
                {
                    return damage;
                }
                else
                {
                    return damage * boostModifier;
                }
            }

            set { damage = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
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

        public int DamageUpgradeNumber
        {
            get { return damageUpgradeNumber; }
            set { damageUpgradeNumber = value; }
        }

        public int RadiusUpgradeNumber
        {
            get { return radiusUpgradeNumber; }
            set { radiusUpgradeNumber = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Texture2D BulletTexture
        {
            get { return bulletTexture; }
            set { bulletTexture = value; }
        }

        public SoundEffect TowerShot
        {
            get { return towerShot; }
        }

        public Vector2 ShootingDirection
        {
            get { return shootingDirection; }
            set { shootingDirection = value; }
        }

        public bool WillShoot
        {
            get { return willShoot; }
            set { willShoot = value; }
        }

        public int BoostModifier
        {
            get { return boostModifier; }
            set { boostModifier = value; }
        }

        public float BoostModifierDuration
        {
            get { return boostModifierDuration; }
            set
            {
                boostModifierDuration = value;
                boostModifierCurrentTime = 0;
            }
        }

        /// <summary>
        /// Constructs a Tower
        /// </summary>
        /// <param name="texture">What the tower will look like</param>
        /// <param name="bulletTexture">What the bullets will look like</param>
        /// <param name="position">Coordinates of the tower</param>
        /// <param name="towerType">What kind of tower we have</param>
        /// <param name="towerID">Unique id of the tower</param>
        /// <param name="towerShot">What a shot sounds like</param>
        public Tower(Texture2D texture, Texture2D bulletTexture, Vector2 position, 
            string towerType, int towerID, SoundEffect towerShot)
            : base(texture, position)
        {
            this.bulletTexture = bulletTexture;
            this.towerID = towerID;
            this.towerType = towerType;
            this.towerShot = towerShot;
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
        /// <param name="enemies">Enemies of current wave</param>
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

            // Update bullet timer
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
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
