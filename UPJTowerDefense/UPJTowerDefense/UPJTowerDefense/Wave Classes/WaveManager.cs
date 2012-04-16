using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class WaveManager
    {
        private ContentManager content;

        private int numberOfWaves;
        private float timeSinceLastWave;

        private Queue<Wave> waves = new Queue<Wave>();

        private Texture2D[] enemyTextures;

        private bool waveFinished = false;

        private int clickedEnemyID;
        private bool willDrawEnemyPanel = false;

        private int cellX;
        private int cellY;

        private int tileX;
        private int tileY;

        // Mouse state for the current frame.
        private MouseState mouseState;
        // Mouse state for the previous frame.
        private MouseState oldMouseState;

        private KeyboardState keyState;
        private KeyboardState oldKeyState;

        private Level level;
        private SidePanel sidePanel;
        private EnemyPanel enemyPanel;

        private String[] text;
        private String[] enemyTypes;
        private String[] enemySpecies;
        private int[] enemyCount;

        private SoundEffect congratulations;
        private SoundEffect go;

        private Texture2D freezeTexture;

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

        public int TotalRounds
        {
            get { return numberOfWaves - 1; }
        }

        public bool LastRound
        {
            get { return Round == TotalRounds; }
        }

        public bool WillDrawEnemyPanel
        {
            get { return willDrawEnemyPanel; }
            set { willDrawEnemyPanel = value; }
        }

        private void ReadEnemyFile()
        {
            char[] delimiters = { ' ', '\r', '\n' };
            String[] parsedString;

            text = File.ReadAllLines(@"Enemies\EnemyFile" + Options.worldNumber + @".txt");

            numberOfWaves = text.Length - 1;
            enemyTypes = new String[text.Length - 1];
            enemySpecies = new String[text.Length - 1];
            enemyCount = new int[text.Length - 1];

            for (int i = 0; i < text.GetLength(0) - 2; i += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    parsedString = text[i + j].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (int.Parse(parsedString[0]) == -1)
                    {
                        break;
                    }

                    enemyTypes[i + j] = parsedString[1];
                    enemySpecies[i + j] = parsedString[2];
                    enemyCount[i + j] = int.Parse(parsedString[3]);
                }
            }
        }

        public WaveManager(ContentManager content, Player player, Level level, SidePanel sidePanel, EnemyPanel enemyPanel, Texture2D explosionTexture)
        {
            this.content = content;
            this.enemyPanel = enemyPanel;

            this.level = level;
            this.sidePanel = sidePanel;
            //this.congratulations = congratulations;
            //this.go = go;

            LoadContent();
            ReadEnemyFile();

            for (int i = 0; i < numberOfWaves; i += 3)
            {
                List<int> num = new List<int>();
                num.Add(enemyCount.ElementAt(i));
                num.Add(enemyCount.ElementAt(i + 1));
                num.Add(enemyCount.ElementAt(i + 2));
                List<String> type = new List<string>();
                type.Add(enemyTypes[i]);
                type.Add(enemyTypes[i + 1]);
                type.Add(enemyTypes[i + 2]);
                List<String> species = new List<string>();
                species.Add(enemySpecies[i]);
                species.Add(enemySpecies[i + 1]);
                species.Add(enemySpecies[i + 2]);
                Wave wave = new Wave(i / 3, num, player, level, enemyTextures, type, species, explosionTexture, freezeTexture);
                waves.Enqueue(wave);
            }
            

            sidePanel.StartRoundButton.Clicked += new EventHandler(StartRoundButton_Clicked);
        }

        void StartRoundButton_Clicked(object sender, EventArgs e)
        {
            StartNextWave();
            Options.inRound = true;
            if (Options.soundEffectsOn)
            {
                // Play sound
                go.Play(1.0f, 0f, 0f);
            }
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

        private bool WasEnemyClicked()
        {
            bool spaceNotClear = false;
            foreach (Enemy enemy in CurrentWave.Enemies)
            {
                Rectangle bounds = new Rectangle(tileX, tileY, Util.tileSize, Util.tileSize);
                Rectangle enemyBounds = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, Util.tileSize, Util.tileSize);
                spaceNotClear = (bounds.Intersects(enemyBounds));

                if (spaceNotClear)
                {
                    clickedEnemyID = enemy.EnemyID;
                    break;
                }
            }

            return spaceNotClear;
        }

        private void LoadContent()
        {
            enemyTextures = new Texture2D[]
            {
                content.Load<Texture2D>("Enemy Images/Worker"),
                content.Load<Texture2D>("Enemy Images/Swarmer"),
                content.Load<Texture2D>("Enemy Images/Warrior"),
                content.Load<Texture2D>("Enemy Images/Scorpion"),
                content.Load<Texture2D>("Enemy Images/WebBeast"),
            };
            congratulations = content.Load<SoundEffect>("shawshank73-congratulations");
            go = content.Load<SoundEffect>("shawshank73-readygo");
            freezeTexture = content.Load<Texture2D>("ice");
        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            cellX = (int)(mouseState.X / Util.tileSize); // Convert the position of the mouse
            cellY = (int)(mouseState.Y / Util.tileSize); // from array space to level space

            tileX = cellX * Util.tileSize; // Convert from array space to level space
            tileY = cellY * Util.tileSize; // Convert from array space to level space

            if (waves.Count > 1)
            {
                CurrentWave.Update(gameTime); // Update the wave

                if (CurrentWave.RoundOver && CurrentWave.DoneExploding()) // Check if it has finished
                {
                    waveFinished = true;
                    Options.inRound = false;
                    if (Options.soundEffectsOn && !Util.gameOver)
                    {
                        // Play sound
                        congratulations.Play(1.0f, 0f, 0f);
                    }
                    waves.Dequeue();
                }

                if (waveFinished) // If it has finished
                {
                    timeSinceLastWave += (float)gameTime.ElapsedGameTime.TotalSeconds; // Start the timer
                }
            }
            else
            {
                if (Options.worldNumber < Options.numberOfWorlds)
                {
                    Options.worldNumber++;
                    Options.willChangeWorld = true;
                }
                else
                {
                    Util.gameOver = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released
                && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                if (WasEnemyClicked())
                {
                    willDrawEnemyPanel = true;
                }
                else
                {
                    willDrawEnemyPanel = false;
                }
            }

            if (keyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space))
            {
                StartNextWave();
                Options.inRound = true;
            }

            oldKeyState = keyState;
            oldMouseState = mouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);

            if (willDrawEnemyPanel)
            {
                foreach (Enemy enemy in CurrentWave.Enemies)
                {
                    if (enemy.EnemyID == clickedEnemyID)
                    {
                        enemyPanel.Draw(spriteBatch, enemy);
                    }
                }
            }
        }
    }
}
