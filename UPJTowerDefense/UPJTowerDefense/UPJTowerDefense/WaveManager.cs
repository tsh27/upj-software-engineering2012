using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace UPJTowerDefense
{
    public class WaveManager
    {
        private int numberOfWaves; 
        private float timeSinceLastWave;

        private Queue<Wave> waves = new Queue<Wave>(); 

        private Texture2D enemyTexture; 

        private bool waveFinished = false; 

        private Level level; 

        public Wave CurrentWave 
        {
            get { return waves.Peek(); }
        }

        public List<Enemy> Enemies 
        {
            get { return CurrentWave.Enemies; }
        }
        public int Round 
        {
            get { return CurrentWave.RoundNumber + 1; }
        }

        public WaveManager(Player player, Level level, int numberOfWaves, Texture2D enemyTexture)
        {
            this.numberOfWaves = numberOfWaves;
            this.enemyTexture = enemyTexture;

            this.level = level;

            for (int i = 0; i <= numberOfWaves; i++)
            {
                int initialNumerOfEnemies = 10;
                int numberModifier = (i / 10) + 1;

                if (i != 0 && i % 5 == 4)
                {
                    Wave wave = new Wave(i, 1, player, level, enemyTexture, "Boss");
                    waves.Enqueue(wave);
                }
                else
                {
                    Wave wave = new Wave(i, initialNumerOfEnemies
                        * numberModifier, player, level, enemyTexture, "Simple Enemy");
                    waves.Enqueue(wave);
                }
            }

            StartNextWave();
        }

        private void StartNextWave()
        {
            if (waves.Count > 1) // If there are still waves left
            {
                if (waves.Peek() != null)
                {
                    waves.Peek().Start(); // Start the next one
                }
                timeSinceLastWave = 0; // Reset timer
                waveFinished = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (waves.Count > 1)
            {
                CurrentWave.Update(gameTime); // Update the wave

                if (CurrentWave.RoundOver) // Check if it has finished
                {
                    waveFinished = true;
                }

                if (waveFinished) // If it has finished
                {
                    timeSinceLastWave += (float)gameTime.ElapsedGameTime.TotalSeconds; // Start the timer
                }

                if (timeSinceLastWave > 1.0f) // If 30 seconds has passed
                {
                    waves.Dequeue(); // Remove the finished wave
                    StartNextWave(); // Start the next wave
                }
            }
            else
            {
                util.gameOver = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);
        }
    }
}
