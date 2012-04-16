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
    /// Displays player information and
    /// GUI controls for the user
    /// </summary>
    public class SidePanel
    {
        // Instance of the ContentManager
        private ContentManager content;

        // Background texture for the panel
        private Texture2D background;

        // Font used to display the text
        private SpriteFont font;

        // Starting coordinates of the panel
        private Vector2 position;

        // Dimensions of the panel
        private int width;
        private int height;

        private Texture2D[] basicTowerButtons;
        private GUIButton basicTowerButton;

        private Texture2D[] pulseTowerButtons;
        private GUIButton pulseTowerButton;

        private Texture2D[] freezeTowerButtons;
        private GUIButton freezeTowerButton;

        private Texture2D[] fireTowerButtons;
        private GUIButton fireTowerButton;

        private Texture2D[] mortarTowerButtons;
        private GUIButton mortarTowerButton;

        private Texture2D[] nukeSpellButtons;
        private GUIButton nukeSpellButton;

        private Texture2D[] slowSpellButtons;
        private GUIButton slowSpellButton;

        private Texture2D[] towerBoostSpellButtons;
        private GUIButton towerBoostSpellButton;

        private Texture2D[] startRoundButtons;
        private GUIButton startRoundButton;

        public GUIButton BasicTowerButton
        {
            get { return basicTowerButton; }
        }

        public GUIButton PulseTowerButton
        {
            get { return pulseTowerButton; }
        }

        public GUIButton FreezeTowerButton
        {
            get { return freezeTowerButton; }
        }

        public GUIButton FireTowerButton
        {
            get { return fireTowerButton; }
        }

        public GUIButton MortarTowerButton
        {
            get { return mortarTowerButton; }
        }

        public GUIButton NukeSpellButton
        {
            get { return nukeSpellButton; }
        }

        public GUIButton SlowSpellButton
        {
            get { return slowSpellButton; }
        }

        public GUIButton TowerBoostSpellButton
        {
            get { return towerBoostSpellButton; }
        }

        public GUIButton StartRoundButton
        {
            get { return startRoundButton; }
        }

        /// <summary>
        /// Constructs a SidePanel
        /// </summary>
        /// <param name="texture">The normal texture for the panel</param>
        /// <param name="font">The font used for the text displayed</param>
        /// <param name="position">The starting coordinates of the panel</param>
        /// <param name="width">Width of the panel</param>
        /// <param name="height">Height of the panel</param>
        public SidePanel(ContentManager content, SpriteFont font, Vector2 position, 
            int width, int height)
        {
            this.content = content;
            this.font = font;
            this.position = position;
            this.width = width;
            this.height = height;

            // Load graphical content
            LoadContent();

            // Instantiate buttons
            basicTowerButton = new GUIButton(basicTowerButtons[0], basicTowerButtons[1],
                basicTowerButtons[2], new Vector2(1000, 200));

            pulseTowerButton = new GUIButton(pulseTowerButtons[0], pulseTowerButtons[1],
                pulseTowerButtons[2], new Vector2(1050, 200));

            freezeTowerButton = new GUIButton(freezeTowerButtons[0], freezeTowerButtons[1],
                freezeTowerButtons[2], new Vector2(1100, 200));

            fireTowerButton = new GUIButton(fireTowerButtons[0], fireTowerButtons[1],
                fireTowerButtons[2], new Vector2(1150, 200));

            mortarTowerButton = new GUIButton(mortarTowerButtons[0], mortarTowerButtons[1],
                mortarTowerButtons[2], new Vector2(1200, 200));

            nukeSpellButton = new GUIButton(nukeSpellButtons[0], nukeSpellButtons[1],
                nukeSpellButtons[2], new Vector2(1050, 330));

            slowSpellButton = new GUIButton(slowSpellButtons[0], slowSpellButtons[1],
                slowSpellButtons[2], new Vector2(1100, 330));

            towerBoostSpellButton = new GUIButton(towerBoostSpellButtons[0], towerBoostSpellButtons[1],
                towerBoostSpellButtons[2], new Vector2(1150, 330));

            startRoundButton = new GUIButton(startRoundButtons[0], startRoundButtons[1],
                startRoundButtons[2], new Vector2(1080, 520));
        }

        /// <summary>
        /// Loads the graphical content for the SidePanel
        /// </summary>
        private void LoadContent()
        {
            // Panel background
            background = content.Load<Texture2D>("minimapBack");

            /* Tower Buttons */
            basicTowerButtons = new Texture2D[]
            {
                content.Load<Texture2D>("BasicTowerButtons/BasicTowerNormal"),
                content.Load<Texture2D>("BasicTowerButtons/BasicTowerHover"),
                content.Load<Texture2D>("BasicTowerButtons/BasicTowerPressed"),
            };

            pulseTowerButtons = new Texture2D[]
            {
                content.Load<Texture2D>("PulseTowerButtons/PulseTowerNormal"),
                content.Load<Texture2D>("PulseTowerButtons/PulseTowerHover"),
                content.Load<Texture2D>("PulseTowerButtons/PulseTowerPressed"),
            };

            freezeTowerButtons = new Texture2D[]
            {
                content.Load<Texture2D>("FreezeTowerButtons/FreezeTowerNormal"),
                content.Load<Texture2D>("FreezeTowerButtons/FreezeTowerHover"),
                content.Load<Texture2D>("FreezeTowerButtons/FreezeTowerPressed"),
            };

            fireTowerButtons = new Texture2D[]
            {
                content.Load<Texture2D>("FireTowerButtons/FireTowerNormal"),
                content.Load<Texture2D>("FireTowerButtons/FireTowerHover"),
                content.Load<Texture2D>("FireTowerButtons/FireTowerPressed"),
            };

            mortarTowerButtons = new Texture2D[]
            {
                content.Load<Texture2D>("MortarTowerButtons/MortarTowerNormal"),
                content.Load<Texture2D>("MortarTowerButtons/MortarTowerHover"),
                content.Load<Texture2D>("MortarTowerButtons/MortarTowerPressed"),
            };

            /* Spell Buttons */
            nukeSpellButtons = new Texture2D[]
            {
                content.Load<Texture2D>("NukeButtons/NukeReadyNormal"),
                content.Load<Texture2D>("NukeButtons/NukeReadyHover"),
                content.Load<Texture2D>("NukeButtons/NukeReadyPressed"),
            };

            slowSpellButtons = new Texture2D[]
            {
                content.Load<Texture2D>("SlowButtons/SlowNormal"),
                content.Load<Texture2D>("SlowButtons/SlowHover"),
                content.Load<Texture2D>("SlowButtons/SlowPressed"),
            };

            towerBoostSpellButtons = new Texture2D[]
            {
                content.Load<Texture2D>("TowerBoostButtons/TowerBoostNormal"),
                content.Load<Texture2D>("TowerBoostButtons/TowerBoostHover"),
                content.Load<Texture2D>("TowerBoostButtons/TowerBoostPressed"),
            };

            // Start Round Button
            startRoundButtons = new Texture2D[]
            {
                content.Load<Texture2D>("StartRoundButtons/StartRoundButtonNormal"),
                content.Load<Texture2D>("StartRoundButtons/StartRoundButtonHover"),
                content.Load<Texture2D>("StartRoundButtons/StartRoundButtonPressed"),
            };
        }

        /// <summary>
        /// Update a SidePanel
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            basicTowerButton.Update(gameTime);
            pulseTowerButton.Update(gameTime);
            freezeTowerButton.Update(gameTime);
            fireTowerButton.Update(gameTime);
            mortarTowerButton.Update(gameTime);

            nukeSpellButton.Update(gameTime);
            slowSpellButton.Update(gameTime);
            towerBoostSpellButton.Update(gameTime);

            if (!Options.inRound)
            {
                startRoundButton.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws a SidePanel to the screen
        /// </summary>
        /// <param name="spriteBatch">spriteBatch passed from Game1</param>
        /// <param name="player">A reference of the player</param>
        /// <param name="round">Current round</param>
        public void Draw(SpriteBatch spriteBatch, Player player, int round, int totalRounds, bool lastRound)
        {
            if (Options.willChangeWorld || Util.gameOver)
            {
                round--;
            }

            // Since there are 3 parts for each wave,
            // divide the total rounds by 3
            totalRounds = totalRounds / 3;

            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Gray);
            string lives = string.Format("Lives: {0}", player.Lives) ;
            string cornflowers = string.Format("Vermeerium: {0}", player.Money);
            string worldNumber = string.Format("World #: {0}", Options.worldNumber);
            string roundNumber = string.Format("Round #: {0} of {1}", round, totalRounds);
            string towers = "TOWERS";
            string spells = "SPELLS";

            spriteBatch.DrawString(font, lives, new Vector2(980, 40), Color.Chartreuse);
            spriteBatch.DrawString(font, cornflowers, new Vector2(980, 90), Color.Chartreuse);
            spriteBatch.DrawString(font, towers, new Vector2(1080, 150), Color.Chartreuse);
            spriteBatch.DrawString(font, spells, new Vector2(1087, 280), Color.Chartreuse);
            spriteBatch.DrawString(font, worldNumber, new Vector2(980, 420), Color.Chartreuse);
            spriteBatch.DrawString(font, roundNumber, new Vector2(980, 470), Color.Chartreuse);

            basicTowerButton.Draw(spriteBatch);
            pulseTowerButton.Draw(spriteBatch);
            freezeTowerButton.Draw(spriteBatch);
            fireTowerButton.Draw(spriteBatch);
            mortarTowerButton.Draw(spriteBatch);

            nukeSpellButton.Draw(spriteBatch);
            slowSpellButton.Draw(spriteBatch);
            towerBoostSpellButton.Draw(spriteBatch);

            if (!Options.inRound)
            {
                startRoundButton.Draw(spriteBatch);
            }
        }
    }
}