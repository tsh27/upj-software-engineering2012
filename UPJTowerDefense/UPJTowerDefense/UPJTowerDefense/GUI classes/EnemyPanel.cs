using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace UPJTowerDefense
{
    /// <summary>
    /// Displays information for an enemy
    /// that is clicked on during game time
    /// </summary>
    public class EnemyPanel
    {
        // Instance of the ContentManager
        ContentManager content;

        // Background texture for the panel
        private Texture2D background;
        
        // Font used to display the text
        private SpriteFont font;
        
        // Starting coordinates of the panel
        private Vector2 position;

        // The dimensions of the panel
        private int width;
        private int height;

        // Avatar background image
        private Texture2D avatarBackground;

        // Avatar species color
        private Color avatarColor;

        /// <summary>
        /// Constructs an EnemyPanel
        /// </summary>
        /// <param name="content">The ContentManager</param>
        /// <param name="font">Font for the panel</param>
        /// <param name="position">Coordinates of the panel</param>
        /// <param name="width">Width of the panel</param>
        /// <param name="height">Height of the panel</param>
        public EnemyPanel(ContentManager content, SpriteFont font, Vector2 position, int width, int height)
        {
            this.content = content;
            this.font = font;
            this.position = position;
            this.width = width;
            this.height = height;

            // Load graphical content
            LoadContent();
        }

        /// <summary>
        /// Loads graphical images for the EnemyPanel
        /// </summary>
        private void LoadContent()
        {
            // Load info panel image
            background = content.Load<Texture2D>("InfoPanel");

            // Load avatar background
            avatarBackground = content.Load<Texture2D>("AvatarBackground");
        }

        /// <summary>
        /// Draws an EnemyPanel to the screen
        /// </summary>
        /// <param name="spriteBatch">spriteBatch for the game</param>
        /// <param name="clickedEnemy">Enemy that was clicked on during game play</param>
        public void Draw(SpriteBatch spriteBatch, Enemy clickedEnemy)
        {
            string resistantType;
            string resistance;

            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            string enemyType = string.Format("Enemy Type: {0}", clickedEnemy.EnemyType);
            string speciesType = string.Format("Species Type: {0}", clickedEnemy.SpeciesType);
            string health = string.Format("Health: {0}", clickedEnemy.CurrentHealth);
            string speed = string.Format("Speed: {0}X", clickedEnemy.Speed);
            string avatarText = string.Format("Avatar");

            if (clickedEnemy.SpeciesType == "Equator")
            {
                resistantType = "Fire";
                avatarColor = Color.Firebrick;
            }
            else if (clickedEnemy.SpeciesType == "Pole")
            {
                resistantType = "Cold";
                avatarColor = Color.MediumAquamarine;
            }
            else if (clickedEnemy.SpeciesType == "Deep")
            {
                resistantType = "Pulse";
                avatarColor = Color.SaddleBrown;
            }
            else if (clickedEnemy.SpeciesType == "Armored")
            {
                resistantType = "Basic";
                avatarColor = Color.DimGray;
            }
            else
            {
                resistantType = "";
                avatarColor = Color.Green;
            }

            if (resistantType == "")
            {
                resistance = "Resistance: No Resistance";
            }
            else
            {
                resistance = string.Format("Resistance: {0}% to {1}", clickedEnemy.Resistance * 100, resistantType);
            }

            spriteBatch.DrawString(font, enemyType, new Vector2(position.X + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, speciesType, new Vector2(position.X + 20, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, resistance, new Vector2(position.X + 20, position.Y + 90), Color.Chartreuse);
            spriteBatch.DrawString(font, health, new Vector2(position.X + 500, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, speed, new Vector2(position.X + 500, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, avatarText, new Vector2(position.X + 900, position.Y + 20), Color.Chartreuse);

            Rectangle avatarBounds = new Rectangle((int)position.X + 917, (int)position.Y + 61, Util.tileWidth, Util.tileHeight);
            Rectangle backgroundBounds = new Rectangle((int)position.X + 911, (int)position.Y + 55, 60, 60);
            spriteBatch.Draw(avatarBackground, backgroundBounds, Color.White);
            spriteBatch.Draw(clickedEnemy.Avatar, avatarBounds, avatarColor);
        }
    }
}
