using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace UPJTowerDefense
{
    /// <summary>
    /// Displays information for an enemy
    /// that is clicked on during game time
    /// </summary>
    public class EnemyPanel
    {
        //background texture for the panel
        private Texture2D background;
        
        //font used to display the text
        private SpriteFont font;
        
        //starting coordinates of the panel
        private Vector2 position;

        //the dimensions of the panel
        private int width;
        private int height;

        /// <summary>
        /// Constructs an EnemyPanel
        /// </summary>
        /// <param name="texture">The normal texture for the panel</param>
        /// <param name="font">The font for the text displayed</param>
        /// <param name="position">The starting coordinates of the panel</param>
        /// <param name="width">How wide is the panel</param>
        /// <param name="height">How tall is the panel</param>
        public EnemyPanel(Texture2D texture, SpriteFont font, Vector2 position, int width, int height)
        {
            this.background = texture;
            this.font = font;
            this.position = position;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Draws an EnemyPanel to the screen
        /// </summary>
        /// <param name="spriteBatch">spriteBatch passed from Player</param>
        /// <param name="clickedEnemy">Enemy that was clicked on during game play</param>
        public void Draw(SpriteBatch spriteBatch, Enemy clickedEnemy)
        {
            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            string enemyType = string.Format("Enemy Type: {0}", clickedEnemy.EnemyType);
            string health = string.Format("Health: {0}", clickedEnemy.CurrentHealth);

            spriteBatch.DrawString(font, enemyType, new Vector2(position.X + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, health, new Vector2(position.X + 500, position.Y + 20), Color.Chartreuse);
        }
    }
}
