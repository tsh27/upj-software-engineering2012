using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    public class Swarm : Enemy
    {
        public Swarm(Texture2D texture, Vector2 position, float health, int bountyGiven,
            float speed, float resistance, int enemyID, string enemyType, string speciesType)
            : base(texture, position, health, bountyGiven, speed, resistance, enemyID, enemyType, speciesType)
        {

        }
    }
}
