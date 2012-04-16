using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace UPJTowerDefense
{
    public class FireTowerPanel
    {
        // Instance of the ContentManager
        ContentManager content;

        // Background texture for the panel
        private Texture2D background;

        // Font used to display text
        private SpriteFont font;

        // Starting coordinates of the panel
        private Vector2 position;

        // Dimensions of the panel
        private int width;
        private int height;

        /// <summary>
        /// Constructs a FireTowerPanel
        /// </summary>
        /// <param name="content">The ContentManager</param>
        /// <param name="font">Font of the panel</param>
        /// <param name="position">Coordinates of the panel</param>
        /// <param name="width">Width of the panel</param>
        /// <param name="height">Height of the panel</param>
        public FireTowerPanel(ContentManager content, SpriteFont font, Vector2 position, int width, int height)
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
        /// Load graphical images for the FireTowerPanel
        /// </summary>
        private void LoadContent()
        {
            // Load info panel image
            background = content.Load<Texture2D>("InfoPanel");
        }

        /// <summary>
        /// Draw a FireTowerPanel
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            string text = "Use the arrow keys to select the shooting direction of the fire tower.";
            string text1 = "Press Enter when finished.";

            spriteBatch.DrawString(font, text, new Vector2(position.X + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, text1, new Vector2(position.X + 20, position.Y + 55), Color.Chartreuse);
        }
    }
}
