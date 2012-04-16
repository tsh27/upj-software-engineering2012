using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    /// <summary>
    /// Displays information about spells
    /// </summary>
    public class SpellPanel
    {
        // Background texture for the panel
        private Texture2D background;
        
        // Font used to display the text
        private SpriteFont font;
        
        // Starting coordinates of the panel
        private Vector2 position;

        // The dimensions of the panel
        private int width;
        private int height;

        /// <summary>
        /// Constructs a SpellPanel
        /// </summary>
        /// <param name="texture">The normal texture for the panel</param>
        /// <param name="font">The font for the text displayed</param>
        /// <param name="position">The starting coordinates of the panel</param>
        /// <param name="width">Width of the panel</param>
        /// <param name="height">Height of the panel</param>
        public SpellPanel(Texture2D texture, SpriteFont font, Vector2 position, int width, int height)
        {
            this.background = texture;
            this.font = font;
            this.position = position;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Draws a SpellPanel
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="selectedSpell">The current spell being used</param>
        public void Draw(SpriteBatch spriteBatch, Spell selectedSpell)
        {
            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            string spellType = string.Format("Spell Type: {0}", selectedSpell.SpellType);
            string price = string.Format("Price: {0}", selectedSpell.Cost);

            spriteBatch.DrawString(font, spellType, new Vector2(position.X + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, price, new Vector2(position.X + 500, position.Y + 20), Color.Chartreuse);
        }

        /// <summary>
        /// Draw a spell being hovered over by the mouse
        /// </summary>
        public void DrawPreview(SpriteBatch spriteBatch, String spellType, int cost, int radius, int damage)
        {
            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            string towerTypeText = string.Format("Tower Type: {0}", spellType);
            string priceText = string.Format("Price: {0}", cost);
            string rangeText = "Range: Full Screen";
            string damageText = string.Format("Damage: {0}", damage);

            spriteBatch.DrawString(font, towerTypeText, new Vector2(position.X + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, priceText, new Vector2(position.X + 20, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, damageText, new Vector2(position.X + 500, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, rangeText, new Vector2(position.X + 500, position.Y + 55), Color.Chartreuse);
        }
    }
}