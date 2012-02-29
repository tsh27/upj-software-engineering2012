using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class Player
    {
        const int tileSize = 48;

        // Player state.
        private int money = 500;
        private int lives = 100;

        // The textures used to draw our tower.
        private Texture2D[] towerTextures;
        // The texture used to draw bullets.
        private Texture2D bulletTexture;

        // A list of the players towers
        private List<Tower> towers = new List<Tower>();
        private static int towerID = -1;
        private int clickedTowerID;
        private bool willDrawTowerPanel = false;
        private List<Vector2> clickedTowerRadius = new List<Vector2>();

        private int clickedEnemyID;
        private bool willDrawEnemyPanel = false;

        // Mouse state for the current frame.
        private MouseState mouseState;
        // Mouse state for the previous frame.
        private MouseState oldState;

        // Tower placement.
        private int cellX;
        private int cellY;

        private int tileX;
        private int tileY;

        // The type of tower to add.
        private string newTowerType;
        // The index of the new towers texture.
        private int newTowerIndex;

        // A reference to the level.
        private Level level;
        private TowerPanel towerPanel;
        private EnemyPanel enemyPanel;

        //audio for shooting
        private SoundEffect towerShot;

        public int Money
        {
            get { return money; }
            set { money = value; }
        }
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public string NewTowerType
        {
            set { newTowerType = value; }
        }
        public int NewTowerIndex
        {
            set { newTowerIndex = value; }
        }

        /// <summary>
        /// Construct a new player.
        /// </summary>
        public Player(Level level, TowerPanel towerPanel, EnemyPanel enemyPanel, 
            Texture2D[] towerTextures, Texture2D bulletTexture, SoundEffect towerShot)
        {
            this.level = level;
            this.towerPanel = towerPanel;
            this.enemyPanel = enemyPanel;

            this.towerTextures = towerTextures;
            this.bulletTexture = bulletTexture;
            this.towerShot = towerShot;
        }

        private void AddRadiusPoints()
        {
            clickedTowerRadius.Add(new Vector2(tileX - 48, tileY - 48));
            clickedTowerRadius.Add(new Vector2(tileX, tileY - 48));
            clickedTowerRadius.Add(new Vector2(tileX + 48, tileY - 48));
            clickedTowerRadius.Add(new Vector2(tileX - 48, tileY));
            clickedTowerRadius.Add(new Vector2(tileX + 48, tileY));
            clickedTowerRadius.Add(new Vector2(tileX - 48, tileY + 48));
            clickedTowerRadius.Add(new Vector2(tileX, tileY + 48));
            clickedTowerRadius.Add(new Vector2(tileX + 48, tileY + 48));
        }

        private bool CheckTower()
        {
            bool spaceNotClear = false;
            foreach (Tower tower in towers)
            {
                spaceNotClear = (tower.Position == new Vector2(tileX, tileY));

                if (spaceNotClear)
                {
                    clickedTowerID = tower.TowerID;
                    clickedTowerRadius.Clear();
                    AddRadiusPoints();
                    break;
                }
            }

            return spaceNotClear;
        }

        private bool CheckEnemy(List<Enemy> enemies)
        {
            bool spaceNotClear = false;
            foreach (Enemy enemy in enemies)
            {
                Rectangle bounds = new Rectangle(tileX, tileY, tileSize, tileSize);
                Rectangle enemyBounds = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, tileSize, tileSize);
                spaceNotClear = (bounds.Intersects(enemyBounds));

                if (spaceNotClear)
                {
                    clickedEnemyID = enemy.EnemyID;
                    break;
                }
            }

            return spaceNotClear;
        }

        /// <summary>
        /// Returns wether the current cell is clear
        /// </summary>
        private bool IsCellClear()
        {
            // Make sure tower is within limits
            bool inBounds = cellX >= 0 && cellY >= 0 && 
                cellX < level.Width && cellY < level.Height; 

            bool spaceClear = true;

            // Check that there is no tower in this spot
            foreach (Tower tower in towers) 
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));

                if (!spaceClear)
                {
                    break;
                }
            }

            bool onPath = (level.GetIndex(cellX, cellY) < 1);

            return inBounds && spaceClear && onPath; // If both checks are true return true
        }

        /// <summary>
        /// Adds a tower to the player's collection.
        /// </summary>
        public void AddTower()
        {
            Tower towerToAdd = null;
            towerID++;
            towerToAdd = new ArrowTower(towerTextures[0], bulletTexture, 
                new Vector2(tileX, tileY), "Arrow Tower", towerID, towerShot);
            
            // Only add the tower if there is a space and if the player can afford it.
            if (towerToAdd.Cost <= money && IsCellClear() == true)
            {
                towers.Add(towerToAdd);
                money -= towerToAdd.Cost;
            }

            // Reset the newTowerType field.
            newTowerType = string.Empty;     
        }

        /// <summary>
        /// Updates the player.
        /// </summary>
        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            mouseState = Mouse.GetState();

            cellX = (int)(mouseState.X / tileSize); // Convert the position of the mouse
            cellY = (int)(mouseState.Y / tileSize); // from array space to level space

            tileX = cellX * tileSize; // Convert from array space to level space
            tileY = cellY * tileSize; // Convert from array space to level space

            if (mouseState.LeftButton == ButtonState.Released
                && oldState.LeftButton == ButtonState.Pressed)
            {
                if (string.IsNullOrEmpty(newTowerType) == false)
                {
                        AddTower();
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (CheckTower())
                {
                    willDrawTowerPanel = true;
                }
                else
                {
                    willDrawTowerPanel = false;
                    clickedTowerRadius.Clear();
                }

                if (CheckEnemy(enemies))
                {
                    willDrawEnemyPanel = true;
                }
                else
                {
                    willDrawEnemyPanel = false;
                }
            }

            foreach (Tower tower in towers)
            {
                // Make sure the tower has no targets.
                if (tower.HasTarget == false)
                {
                    tower.GetClosestEnemy(enemies);
                }

                tower.Update(gameTime);
            }
            

            oldState = mouseState; // Set the oldState so it becomes the state of the previous frame.
        }

        public void Draw(SpriteBatch spriteBatch, List<Enemy> enemies)
        {
            level.Draw(spriteBatch, clickedTowerRadius);
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
            }

            if (willDrawTowerPanel)
            {
                foreach (Tower tower in towers)
                {
                    if (tower.TowerID == clickedTowerID)
                    {
                        towerPanel.Draw(spriteBatch, tower);
                        break;
                    }
                }
            }
            else if (willDrawEnemyPanel)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.EnemyID == clickedEnemyID)
                    {
                        enemyPanel.Draw(spriteBatch, enemy);
                    }
                }
            }
        }

        public void DrawPreview(SpriteBatch spriteBatch)
        {
            // Draw the tower preview.
            if (string.IsNullOrEmpty(newTowerType) == false)
            {
                int cellX = (int)(mouseState.X / tileSize); // Convert the position of the mouse
                int cellY = (int)(mouseState.Y / tileSize); // from array space to level space

                int tileX = cellX * tileSize; // Convert from array space to level space
                int tileY = cellY * tileSize; // Convert from array space to level space

                Texture2D previewTexture = towerTextures[newTowerIndex];

                spriteBatch.Draw(previewTexture, new Rectangle(tileX, tileY,
                    previewTexture.Width, previewTexture.Height), Color.White);
            }
        }
    }
}