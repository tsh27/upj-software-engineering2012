using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    public class Worker : Enemy
    {
        public Worker(Texture2D texture, Vector2 position, float health, int bountyGiven,
            float speed, float resistance, int enemyID, string enemyType, string speciesType)
            : base(texture, position, health, bountyGiven, speed, resistance, enemyID, enemyType, speciesType)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            base.Draw(spriteBatch, color);
        }
    }
}
