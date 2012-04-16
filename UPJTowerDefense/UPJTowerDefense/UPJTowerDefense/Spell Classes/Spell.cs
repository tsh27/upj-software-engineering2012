using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class Spell : Sprite
    {
        // Type of spell
        protected string spellType;

        // Sound for the spell
        protected SoundEffect spellSound;

        // Price of the spell
        protected int cost;

        // Radius of the spell
        protected float radius;

        // Enemies on the screen
        protected List<Enemy> enemiesInRange = new List<Enemy>();

        // Placed towers
        protected List<Tower> allTowers = new List<Tower>();

        // Was the spell used?
        protected bool spellUsed = false;

        public string SpellType
        {
            get { return spellType; }
        }

        public int Cost
        {
            get { return cost; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public List<Enemy> EnemiesInRange
        {
            get { return enemiesInRange; }
        }

        public bool SpellUsed
        {
            get { return spellUsed; }
        }

        /// <summary>
        /// Constructs a Spell
        /// </summary>
        /// <param name="texture">Graphic for the Spell</param>
        /// <param name="position">Coordinates of the spell</param>
        /// <param name="spellType">Type of Spell</param>
        public Spell(Texture2D texture, Vector2 position, string spellType)
            : base(texture, position)
        {
            this.texture = texture;
            this.position = position;
            this.spellType = spellType;
        }

        /// <summary>
        /// Finds enemies on the screen
        /// </summary>
        /// <param name="enemyPosition">Coordinates of the enemy</param>
        /// <returns>Returns true if in range</returns>
        public bool IsInRange(Vector2 enemyPosition)
        {
            return Vector2.Distance(center, enemyPosition) <= radius;
        }

        /// <summary>
        /// Get enemies on the screen
        /// </summary>
        /// <param name="enemies">Current enemies traversing path</param>
        public virtual void GetEnemiesInRange(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                if (IsInRange(enemy.Position))
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        /// <summary>
        /// Get placed towers
        /// </summary>
        /// <param name="towers"></param>
        public virtual void GetAllTowers(List<Tower> towers)
        {
            foreach (Tower tower in towers)
            {
                allTowers.Add(tower);
            }
        }
    }
}