using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    public class Wave
    {
        private static int enemyID = -1;
        private int numOfEnemies; // Number of enemies to spawn
        private int waveNumber; // What wave is this?
        private string enemyType;

        private float spawnTimer = 0; // When should we spawn an enemy
        private int enemiesSpawned = 0; // How many enemies have spawned

        private bool enemyAtEnd; // Has an enemy reached the end of the path?
        private bool spawningEnemies; // Are we still spawing enemies?

        private Player player; 
        private Level level; 

        private Texture2D enemyTexture;

        private List<Enemy> enemies = new List<Enemy>();

        public bool RoundOver
        {
            get 
            { 
                return enemies.Count == 0 && enemiesSpawned == numOfEnemies; 
            }
        }
        public int RoundNumber
        {
            get { return waveNumber; }
        }

        public bool EnemyAtEnd
        {
            get { return enemyAtEnd; }
            set { enemyAtEnd = value; }
        }
        public List<Enemy> Enemies
        {
            get { return enemies;}
        }

        public Wave(int waveNumber, int numOfEnemies,
            Player player, Level level, Texture2D enemyTexture, string enemyType)
        {
            this.waveNumber = waveNumber;
            this.numOfEnemies = numOfEnemies;

            this.player = player;
            this.level = level;

            this.enemyTexture = enemyTexture;
            this.enemyType = enemyType;
        }

        private void AddEnemy()
        {
            enemyID++;

            if (enemyType.Equals("Simple Enemy"))
            {
                Enemy enemy = new Enemy(enemyTexture,
                    level.Waypoints.Peek(), 100, 5, 1f, enemyID, enemyType);
                enemy.SetWaypoints(level.Waypoints);
                enemies.Add(enemy);
            }
            else if (enemyType.Equals("Boss"))
            {
                Enemy enemy = new Enemy(enemyTexture,
                    level.Waypoints.Peek(), 1000, 1000, 0.5f, enemyID, enemyType);
                enemy.SetWaypoints(level.Waypoints);
                enemies.Add(enemy);
            }

            spawnTimer = 0;
            enemiesSpawned++;
        }

        public void Start()
        {
            spawningEnemies = true;
        }

        public void Update(GameTime gameTime)
        {
            if (enemiesSpawned == numOfEnemies)
                spawningEnemies = false; // We have spawned enough enemies

            if (spawningEnemies)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (spawnTimer > 2)
                    AddEnemy(); // Time to add a new enemey
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                enemy.Update(gameTime);

                if (enemy.IsDead)
                {
                    if (enemy.CurrentHealth > 0) // Enemy is at the end
                    {
                        enemyAtEnd = true;

                        if (enemy.EnemyType.Equals("Simple Enemy"))
                        {
                            player.Lives -= 1;
                        }
                        else if (enemy.EnemyType.Equals("Boss"))
                        {
                            player.Lives -= 10;
                        }
                    }

                    else
                    {
                        player.Money += enemy.BountyGiven;
                    }

                    enemies.Remove(enemy);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);
        }
    }
}
