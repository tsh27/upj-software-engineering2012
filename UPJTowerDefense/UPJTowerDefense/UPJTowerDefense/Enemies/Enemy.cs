using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    /// <summary>
    /// Basic template for an enemy
    /// Inherits from a Sprite
    /// </summary>
    public class Enemy : Sprite
    {
        // Queue of the waypoints on the map
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        // Unique id for each instance of an enemy
        private int enemyID;

        // The type of enemy
        private string enemyType;

        // The type of species
        protected string speciesType;

        // Resistance percentage for enemy
        protected float resistance;

        // Alters the speed of the enemy
        protected float speedModifier;

        // Track the duration of a speed modifier
        protected float modifierDuration;
        protected float modiferCurrentTime;

        // Health values to "fade" enemy while it takes damage
        protected float startHealth;
        protected float currentHealth;

        // Is the enemy living?
        protected bool alive = true;

        // Speed of the enemy
        protected float speed = 0.5f;

        // The amount of vermeerium dropped upon death
        protected int bountyGiven;

        /// <summary>
        /// Alters the speed of the enemy.
        /// </summary>
        public float SpeedModifier
        {
            get { return speedModifier; }
            set { speedModifier = value; }
        }
        /// <summary>
        /// Defines how long the speed modification will last.
        /// </summary>
        public float ModifierDuration
        {
            get { return modifierDuration; }
            set 
            { 
                modifierDuration = value;
                modiferCurrentTime = 0;
            }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public bool IsDead
        {
            get { return !alive; }
        }

        public int BountyGiven
        {
            get { return bountyGiven; }
        }

        public int EnemyID
        {
            get { return enemyID; }
        }

        public string EnemyType
        {
            get { return enemyType; }
        }

        public string SpeciesType
        {
            get { return speciesType; }
        }

        public float Resistance
        {
            get { return resistance; }
        }

        /// <summary>
        /// Returns the distance between Enemy and next waypoint
        /// </summary>
        public float DistanceToDestination
        {
            get { return Vector2.Distance(position, waypoints.Peek()); }
        }

        /// <summary>
        /// Defines speed Enemy with respect to speed modifiers
        /// </summary>
        public float Speed
        {
            get
            {
                if (speedModifier == 0f)
                {
                    return speed;
                }
                else
                {
                    return speed * speedModifier;
                }
            }
        }

        public Texture2D Avatar
        {
            get { return texture; }
        }

        /// <summary>
        /// Constructs an Enemy
        /// </summary>
        /// <param name="texture">Enemy graphic</param>
        /// <param name="position">Coordinates of the Enemy</param>
        /// <param name="health">Initial health of the Enemy</param>
        /// <param name="bountyGiven">The amount of vermeerium dropped upon death</param>
        /// <param name="speed">The speed of the Enemy</param>
        /// <param name="resistance">The resistance percentage based on type</param>
        /// <param name="enemyID">Unique identifier for the Enemy</param>
        /// <param name="enemyType">The type of Enemy</param>
        /// <param name="speciesType">The species type of the Enemy</param>
        public Enemy(Texture2D texture, Vector2 position, float health, int bountyGiven, 
            float speed, float resistance, int enemyID, string enemyType, string speciesType)
            : base(texture, position)
        {
            this.startHealth = health;
            this.currentHealth = startHealth;
            this.bountyGiven = bountyGiven;
            this.speed = speed;
            this.enemyID = enemyID;
            this.enemyType = enemyType;
            this.speciesType = speciesType;
            this.resistance = resistance;
        }

        /// <summary>
        /// Sets the directional coordiantes for the Enemy
        /// </summary>
        /// <param name="waypoints">Waypoints for current map</param>
        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            foreach (Vector2 waypoint in waypoints)
                this.waypoints.Enqueue(waypoint);

            this.position = this.waypoints.Dequeue();
        }

        /// <summary>
        /// Update an Enemy
        /// </summary>
        /// <param name="gameTime">Current gameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // If enemy has not reached the base, 
            if (waypoints.Count > 0)
            {
                // If the enemy is at a turning point
                if (DistanceToDestination < 1f)
                {
                    // Load next waypoint
                    position = waypoints.Peek();
                    waypoints.Dequeue();
                }
                else
                {
                    // Get the current direction
                    Vector2 direction = waypoints.Peek() - position;
                    direction.Normalize();

                    // Rotate enemy based on direction
                    if (direction.X > 0)
                    {
                        rotation = MathHelper.ToRadians(0);
                    }
                    else if (direction.X < 0)
                    {
                        rotation = MathHelper.ToRadians(180);
                    }
                    else if (direction.Y > 0)
                    {
                        rotation = MathHelper.ToRadians(90);
                    }
                    else
                    {
                        rotation = MathHelper.ToRadians(270);
                    }

                    // Store the original speed.
                    float temporarySpeed = speed;

                    // If the modifier has finished,
                    if (modiferCurrentTime > modifierDuration)
                    {
                        // Reset the modifier.
                        speedModifier = 0;
                        modiferCurrentTime = 0;
                    }

                    if (speedModifier != 0 && modiferCurrentTime <= modifierDuration)
                    {
                        // Modify the speed of the enemy.
                        temporarySpeed *= speedModifier;
                        // Update the modifier timer.
                        modiferCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    // Set velocity based with respect to speed modifiers
                    velocity = Vector2.Multiply(direction, temporarySpeed);
                    position += velocity;
                }
            }
            else
            {
                alive = false;
            }   

            // If towers have killed the enemy
            if (currentHealth <= 0)
            {
                alive = false;
            }
        }

        /// <summary>
        /// Draw an Enemy
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float healthPercentage = (float)currentHealth / (float)startHealth;
                Color speciesColor = Color.White;

                switch (SpeciesType)
                {
                    case "Basic":
                        speciesColor = Color.Green;
                        break;
                    case "Equator":
                        speciesColor = Color.Firebrick;
                        break;
                    case "Pole":
                        speciesColor = Color.MediumAquamarine;
                        break;
                    case "Deep":
                        speciesColor = Color.SaddleBrown;
                        break;
                    case "Armored":
                        speciesColor = Color.DimGray;
                        break;
                    default:
                        speciesType = "Basic";
                        speciesColor = Color.Green;
                        break;
                }

                // Colorize enemy based on health and species type
                Color color = Color.Lerp(speciesColor, Color.White, 1 - healthPercentage);

                base.Draw(spriteBatch, color);
            }
        }
    }
}