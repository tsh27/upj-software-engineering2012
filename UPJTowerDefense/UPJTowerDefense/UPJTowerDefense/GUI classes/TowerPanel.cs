using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace UPJTowerDefense
{
    /// <summary>
    /// Displays information about towers
    /// that are clicked on during the game
    /// </summary>
    public class TowerPanel
    {
        //background texture for the panel
        private Texture2D background;

        //font used to display text
        private SpriteFont font;

        //starting coordinates of the panel
        private Vector2 position;

        //dimensions of the panel
        private int width;
        private int height;

        /// <summary>
        /// Constructs a TowerPanel
        /// </summary>
        /// <param name="texture">The normal texture for the panel</param>
        /// <param name="font">The font used for the text displayed</param>
        /// <param name="position">The starting coordinates for the panel</param>
        /// <param name="width">How wide is the panel</param>
        /// <param name="height">How tall is the panel</param>
        public TowerPanel(Texture2D texture, SpriteFont font, Vector2 position, int width, int height)
        {
            this.background = texture;
            this.font = font;
            this.position = position;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Draws a TowerPanel
        /// </summary>
        /// <param name="spriteBatch">spriteBatch passed from Player</param>
        /// <param name="clickedTower">Tower that was clicked on during game play</param>
        public void Draw(SpriteBatch spriteBatch, Tower clickedTower)
        {
            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            string towerType = string.Format("Tower Type: {0}", clickedTower.TowerType);
            string price = string.Format("Price: {0}", clickedTower.Cost);
            string range = "Range: ???";
            string fireRate = "Fire Rate: ???";
            string upgrade = "Upgrade:";
            string sell = "Sell:";
            string towerID = string.Format("Tower ID: {0}", clickedTower.TowerID);

            spriteBatch.DrawString(font, towerType, new Vector2(position.X + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, price, new Vector2(position.X + 20, position.Y + 90), Color.Chartreuse);
            spriteBatch.DrawString(font, range, new Vector2(position.X + 500, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, fireRate, new Vector2(position.X + 500, position.Y + 90), Color.Chartreuse);
            spriteBatch.DrawString(font, upgrade, new Vector2(position.X + 750, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, sell, new Vector2(position.X + 1000, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, towerID, new Vector2(position.X + 750, position.Y + 90), Color.Chartreuse);
        }
    }
}
