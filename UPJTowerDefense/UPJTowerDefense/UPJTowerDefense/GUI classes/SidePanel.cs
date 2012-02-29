using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UPJTowerDefense
{
    /// <summary>
    /// Displays player information and
    /// GUI controls for the user
    /// </summary>
    public class SidePanel
    {
        //background texture for the panel
        private Texture2D background;

        //font used to display the text
        private SpriteFont font;

        //starting coordinates of the panel
        private Vector2 position;

        //dimensions of the panel
        private int width;
        private int height;


        /// <summary>
        /// Constructs a SidePanel
        /// </summary>
        /// <param name="texture">The normal texture for the panel</param>
        /// <param name="font">The font used for the text displayed</param>
        /// <param name="position">The starting coordinates of the panel</param>
        /// <param name="width">How wide is the panel</param>
        /// <param name="height">How tall is the panel</param>
        public SidePanel(Texture2D texture, SpriteFont font, Vector2 position, int width, int height)
        {
            this.background = texture;
            this.font = font;
            this.position = position;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Draws a SidePanel to the screen
        /// </summary>
        /// <param name="spriteBatch">spriteBatch passed from Game1</param>
        /// <param name="player">A reference of the player</param>
        /// <param name="round">Which round are we currently in</param>
        public void Draw(SpriteBatch spriteBatch, Player player, int round)
        {
            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Gray);
            string lives = string.Format("Lives: {0}", player.Lives) ;
            string cornflowers = string.Format("Cornflowers: {0}", player.Money);
            string roundNumber = string.Format("Round #: {0}", round);
            string towers = "TOWERS";
            string spells = "SPELLS";

            spriteBatch.DrawString(font, lives, new Vector2(980, 30), Color.Chartreuse);
            spriteBatch.DrawString(font, cornflowers, new Vector2(980, 90), Color.Chartreuse);
            spriteBatch.DrawString(font, towers, new Vector2(1080, 220), Color.Chartreuse);
            spriteBatch.DrawString(font, spells, new Vector2(1087, 320), Color.Chartreuse);
            spriteBatch.DrawString(font, roundNumber, new Vector2(980, 500), Color.Chartreuse);
        }
    }
}
