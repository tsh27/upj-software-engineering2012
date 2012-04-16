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
        private int numOfEnemies;

        // What wave is this?
        private int waveNumber;

        // Lists to hold enemy types and species types for
        // wave
        private List<string> enemyType = new List<string>();
        private List<string> enemySpecies = new List<string>();

        // When should we spawn an enemy
        private float spawnTimer = 0;

        // How many enemies have spawned
        private int enemiesSpawned = 0;

        // Has an enemy reached the end of the path?
        private bool enemyAtEnd;

        // Are we still spawing enemies?
        private bool spawningEnemies;

        // References to needed objects
        private Player player;
        private Level level;

        // Enemy graphics in the wave
        private Texture2D[] enemyTextures;

        // Explosion sprite sheet for enemies
        private Texture2D explosionTexture;

        // List of enemies in the wave
        private List<Enemy> enemies = new List<Enemy>();

        // Holds each frame of explosions
        private List<Explosion> explosions = new List<Explosion>();

        // Ice block graphic
        private Texture2D freezeTexture;

        // Random number generator for explosions
        private Random rndGen = new Random();
        private int rndNum;

        public bool RoundOver
        {
            get { return enemies.Count == 0 && enemiesSpawned == numOfEnemies; }
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
            get { return enemies; }
        }

        /// <summary>
        /// Constructs a Wave
        /// </summary>
        public Wave(int waveNumber, List<int> numOfEnemies, Player player, Level level,Texture2D[] enemyTextures, 
            List<String> enemyType, List<String> enemySpecies, Texture2D explosionTexture, Texture2D freezeTexture)
        {
            this.waveNumber = waveNumber;
            this.numOfEnemies = numOfEnemies[0] + numOfEnemies[1] + numOfEnemies[2];
            this.player = player;
            this.level = level;
            this.enemyTextures = enemyTextures;
            this.explosionTexture = explosionTexture;
            this.freezeTexture = freezeTexture;

            for (int i = 0; i < numOfEnemies[0]; i++)
            {
                this.enemyType.Add(enemyType[0]);
                this.enemySpecies.Add(enemySpecies[0]);
            }
            for (int i = 0; i < numOfEnemies[1]; i++)
            {
                this.enemyType.Add(enemyType[1]);
                this.enemySpecies.Add(enemySpecies[1]);
            }
            for (int i = 0; i < numOfEnemies[2]; i++)
            {
                this.enemyType.Add(enemyType[2]);
                this.enemySpecies.Add(enemySpecies[2]);
            }
        }

        /// <summary>
        /// Is the enemy done exploding?
        /// </summary>
        /// <returns></returns>
        public bool DoneExploding()
        {
            bool done = true;
            foreach (Explosion explosion in explosions)
            {
                if (!explosion.IsDone)
                {
                    done = false;
                }
            }
            return done;
        }

        /// <summary>
        /// Add next enemy to be spawned
        /// </summary>
        private void AddEnemy()
        {
            enemyID++;

            // Increase health based on wave number
            int healthMultiplier = 1;
            if (waveNumber / 10 == 1)
            {
                healthMultiplier = 6;
            }
            else if (waveNumber / 10 == 2)
            {
                healthMultiplier = 12;
            }

            // Randomize a spawn point
            rndNum = rndGen.Next(100);
            rndNum %= 2;

            switch (enemyType.ElementAt(enemiesSpawned))
            {
                case "Worker":
                    Worker worker = new Worker(enemyTextures[0], level.Waypoints.ElementAt(rndNum).Peek(),
                        Util.workerHealth * healthMultiplier, 25, 2f, .50f, enemyID, enemyType.ElementAt(enemiesSpawned), enemySpecies.ElementAt(enemiesSpawned));
                    worker.SetWaypoints(level.Waypoints[rndNum]);
                    enemies.Add(worker);
                    break;
                case "Swarm":
                    Swarm swarm = new Swarm(enemyTextures[1], level.Waypoints.ElementAt(rndNum).Peek(),
                        Util.swarmHealth * healthMultiplier, 25, 4f, .25f, enemyID, enemyType.ElementAt(enemiesSpawned), enemySpecies.ElementAt(enemiesSpawned));
                    swarm.SetWaypoints(level.Waypoints.ElementAt(rndNum));
                    enemies.Add(swarm);
                    break;
                case "Warrior":
                    Warrior warrior = new Warrior(enemyTextures[2], level.Waypoints.ElementAt(rndNum).Peek(),
                        Util.warriorHealth * healthMultiplier, 50, 1f, .75f, enemyID, enemyType.ElementAt(enemiesSpawned), enemySpecies.ElementAt(enemiesSpawned));
                    warrior.SetWaypoints(level.Waypoints.ElementAt(rndNum));
                    enemies.Add(warrior);
                    break;
                case "WebGuard":
                    WebGuard webGuard = new WebGuard(enemyTextures[3], level.Waypoints.ElementAt(rndNum).Peek(),
                        Util.webguardHealth * healthMultiplier, 100, 2f, .75f, enemyID, enemyType.ElementAt(enemiesSpawned), enemySpecies.ElementAt(enemiesSpawned));
                    webGuard.SetWaypoints(level.Waypoints.ElementAt(rndNum));
                    enemies.Add(webGuard);
                    break;
                case "WebBeast":
                    WebBeast webBeast = new WebBeast(enemyTextures[4], level.Waypoints.ElementAt(rndNum).Peek(),
                        Util.webbeastHealth * healthMultiplier, 1000, 1f, 1f, enemyID, enemyType.ElementAt(enemiesSpawned), enemySpecies.ElementAt(enemiesSpawned));
                    webBeast.SetWaypoints(level.Waypoints.ElementAt(rndNum));
                    enemies.Add(webBeast);
                    break;
                default:
                    Worker defaultWorker = new Worker(enemyTextures[0], level.Waypoints.ElementAt(rndNum).Peek(),
                        Util.workerHealth * healthMultiplier, 25, 2f, .50f, enemyID, enemyType.ElementAt(enemiesSpawned), enemySpecies.ElementAt(enemiesSpawned));
                    defaultWorker.SetWaypoints(level.Waypoints.ElementAt(rndNum));
                    enemies.Add(defaultWorker);
                    break;
            }

            spawnTimer = 0;
            enemiesSpawned++;
        }

        /// <summary>
        /// Start spawning enemies
        /// </summary>
        public void Start()
        {
            spawningEnemies = true;
        }

        /// <summary>
        /// Update a Wave
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Have we spawned the number of enemies
            // in the wave
            if (enemiesSpawned == numOfEnemies)
            {
                spawningEnemies = false;
            }

            // If spawning is still occurring...
            if (spawningEnemies)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Spawn another enemy if the correct
                // amount of time has passed
                if (spawnTimer > 1)
                {
                    AddEnemy();
                }
            }

            foreach (Explosion explosion in explosions)
                explosion.Update(gameTime);

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                enemy.Update(gameTime);

                if (enemy.IsDead)
                {
                    // If the enemy has died
                    if (enemy.CurrentHealth > 0)
                    {
                        enemyAtEnd = true;
                        rndNum = rndGen.Next(8);
                        Explosion explode = new Explosion(explosionTexture, 0, rndNum * 64, 64, 64, 16);
                        explode.Activate((int)enemy.Position.X, (int)enemy.Position.Y);
                        explosions.Add(explode);

                        // Decrement player's lives based on enemy type
                        if (enemy.EnemyType == "WebBeast")
                        {
                            player.Lives -= 10;
                        }
                        else if (enemy.EnemyType == "WebGuard")
                        {
                            player.Lives -= 2;
                        }
                        else
                        {
                            player.Lives -= 1;
                        }

                        // Has the player lost of of their lives
                        if (player.Lives <= 0)
                        {
                            Util.gameOver = true;
                        }
                    }
                    else
                    {
                        // Add money dropped from enemy to player's total money
                        player.Money += enemy.BountyGiven;
                        rndNum = rndGen.Next(8);
                        Explosion explode = new Explosion(explosionTexture, 0, rndNum * 64, 64, 64, 16);
                        explode.Activate((int)enemy.Position.X, (int)enemy.Position.Y);
                        explosions.Add(explode);
                    }

                    // Delete enemy from the wave
                    enemies.Remove(enemy);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
                if (enemy.SpeedModifier != 0f)
                {
                    spriteBatch.Draw(freezeTexture, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, 48, 48),
                        Color.White);
                }
            }

            foreach (Explosion explosion in explosions)
            {
                explosion.Draw(spriteBatch);
            }
        }
    }
}
