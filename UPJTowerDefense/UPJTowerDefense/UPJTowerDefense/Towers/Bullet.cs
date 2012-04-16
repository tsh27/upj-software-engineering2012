using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    public class Bullet : Sprite
    {
        // How much damage the bullet can inflict
        private int damage;

        // How old the bullet is
        private int age;

        // How fast the bullet moves
        private int speed;

        public int Damage
        {
            get { return damage; }
        }

        public bool IsDead()
        {
            return age > 100;
        }

        /// <summary>
        /// Constructs a bullet with a given rotation
        /// </summary>
        /// <param name="texture">What the bullet looks like</param>
        /// <param name="position">Coordinates of the bullet</param>
        /// <param name="rotation">Rotation component of the bullet</param>
        /// <param name="speed">How fast the bullet is</param>
        /// <param name="damage">How much damage it will inflict on an enemy</param>
        public Bullet(Texture2D texture, Vector2 position, float rotation, int speed, int damage)
            : base(texture, position)
        {
            this.rotation = rotation;
            this.damage = damage;

            this.speed = speed;

            velocity = Vector2.Transform(new Vector2(0, -speed),
                Matrix.CreateRotationZ(rotation));
        }

        /// <summary>
        /// Constructs a bullet with a given velocity
        /// </summary>
        /// <param name="texture">What the bullet looks like</param>
        /// <param name="position">Coordinates of the bullet</param>
        /// <param name="velocity">Velocity component of the bullet</param>
        /// <param name="speed">How fast the bullet is</param>
        /// <param name="damage">How much damage it will inflict on an enemy</param>
        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, int speed, int damage)
            : base(texture, position)
        {
            this.rotation = rotation;
            this.damage = damage;

            this.speed = speed;

            this.velocity = velocity * speed;
        }

        /// <summary>
        /// Kills a bullet
        /// and sets age to 200
        /// </summary>
        public void Kill()
        {
            this.age = 200;
        }

        /// <summary>
        /// Set the rotation of the bullet
        /// </summary>
        /// <param name="value">Rotation value passed from a tower</param>
        public void SetRotation(float value)
        {
            rotation = value;

            velocity = Vector2.Transform(new Vector2(0, -speed), 
                Matrix.CreateRotationZ(rotation));
        }

        /// <summary>
        /// Update a bullet
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Increment age and position
            age++;
            position += velocity;

            base.Update(gameTime);
        }
    }
}