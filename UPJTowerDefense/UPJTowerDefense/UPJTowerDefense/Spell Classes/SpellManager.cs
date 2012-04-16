using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class SpellManager
    {
        // Current spell being used
        private Spell currentSpell;
        private bool spellIsSelected = false;

        // Preview Spell stats
        private bool willPreviewSpellStats = false;
        private bool spellIsBeingPreviewed = false;
        private string previewType;
        private int previewCost;
        private int previewRadius;
        private int previewDamage;

        // The textures used to draw our spells
        private Texture2D[] spellTextures;

        private SoundEffect[] spellSounds;

        // Mouse state for the current frame.
        private MouseState mouseState;

        // Mouse state for the previous frame.
        private MouseState oldMouseState;

        // Keyboard state for the current frame
        private KeyboardState keyState;

        // Keyboard state for the previous frame
        private KeyboardState oldKeyState;

        // Spell Type for used spell
        private string newSpellType;

        // References to other needed objects
        private Player player;
        private Level level;
        private SpellPanel spellPanel;
        private SidePanel sidePanel;
        private WaveManager waveManager;
        private TowerManager towerManager;

        /// <summary>
        /// Constructs a SpellManger
        /// </summary>
        /// <param name="player"></param>
        /// <param name="level"></param>
        /// <param name="waveManager"></param>
        /// <param name="towerManager"></param>
        /// <param name="spellPanel"></param>
        /// <param name="sidePanel"></param>
        /// <param name="spellTextures"></param>
        /// <param name="spellSounds"></param>
        public SpellManager(Player player, Level level, WaveManager waveManager, TowerManager towerManager,
            SpellPanel spellPanel, SidePanel sidePanel, Texture2D[] spellTextures, SoundEffect[] spellSounds)
        {
            this.player = player;
            this.level = level;
            this.sidePanel = sidePanel;
            this.waveManager = waveManager;
            this.towerManager = towerManager;
            this.spellPanel = spellPanel;
            this.spellTextures = spellTextures;
            this.spellSounds = spellSounds;

            // Listen for button clicks
            sidePanel.NukeSpellButton.Clicked += new EventHandler(NukeSpellButton_Clicked);
            sidePanel.SlowSpellButton.Clicked += new EventHandler(SlowSpellButton_Clicked);
            sidePanel.TowerBoostSpellButton.Clicked += new EventHandler(TowerBoostSpellButton_Clicked);
        }

        void TowerBoostSpellButton_Clicked(object sender, EventArgs e)
        {
            UseTowerBoost();
        }

        void SlowSpellButton_Clicked(object sender, EventArgs e)
        {
            UseSlow();
        }

        void NukeSpellButton_Clicked(object sender, EventArgs e)
        {
            UseNuke();
        }

        /// <summary>
        /// Load preview stats based on type
        /// </summary>
        /// <param name="spellType"></param>
        private void LoadSpellStats(string spellType)
        {
            if (spellType == Util.nukeSpellType)
            {
                previewType = Util.nukeSpellType;
                previewCost = Util.nukeSpellCost;
                previewRadius = Util.nukeSpellRadius;
                previewDamage = Util.nukeSpellDamage;
            }
            else if (spellType == Util.slowSpellType)
            {
                previewType = Util.slowSpellType;
                previewCost = Util.slowSpellCost;
                previewRadius = Util.slowSpellRadius;
                previewDamage = 0;
            }
            else
            {
                previewType = Util.boostSpellType;
                previewCost = Util.boostSpellCost;
                previewRadius = Util.boostSpellRadius;
                previewDamage = 0;
            }
        }

        /// <summary>
        /// Instantiates the spell to be used
        /// </summary>
        public void InitializeSpell()
        {
            Spell spellToUse = null;

            if (newSpellType.Equals(Util.nukeSpellType))
            {
                spellToUse = new NukeSpell(spellTextures[Util.nukeSpellIndex],
                    new Vector2(504, 360), Util.nukeSpellType, spellSounds[0]);
            }
            else if (newSpellType.Equals(Util.slowSpellType))
            {
                spellToUse = new SlowSpell(spellTextures[0],
                    new Vector2(504, 360), Util.slowSpellType, spellSounds[1]);
            }
            else
            {
                spellToUse = new TowerBoostSpell(spellTextures[0],
                    new Vector2(504, 360), Util.boostSpellType, spellSounds[1]);
            }

            // Only use the spell if the player can afford it
            if (spellToUse.Cost <= player.Money)
            {
                currentSpell = spellToUse;
                player.Money -= currentSpell.Cost;
                spellIsBeingPreviewed = false;
                willPreviewSpellStats = false;
                spellIsSelected = true;
            }

            // Reset the newSpellType field
            newSpellType = string.Empty;
        }

        /// <summary>
        /// Launches the nuke
        /// </summary>
        private void UseNuke()
        {
            if (player.Money >= Util.nukeSpellCost)
            {
                spellIsSelected = false;
                spellIsBeingPreviewed = true;
                willPreviewSpellStats = true;
                LoadSpellStats(Util.nukeSpellType);
                newSpellType = Util.nukeSpellType;
            }
        }

        /// <summary>
        /// Fires the slow spell
        /// </summary>
        private void UseSlow()
        {
            if (player.Money >= Util.slowSpellCost)
            {
                spellIsSelected = false;
                spellIsBeingPreviewed = true;
                willPreviewSpellStats = true;
                LoadSpellStats(Util.slowSpellType);
                newSpellType = Util.slowSpellType;
            }
        }

        /// <summary>
        /// Fires the tower boost
        /// </summary>
        private void UseTowerBoost()
        {
            if (player.Money >= Util.boostSpellCost)
            {
                spellIsSelected = false;
                spellIsBeingPreviewed = true;
                willPreviewSpellStats = true;
                LoadSpellStats(Util.boostSpellType);
                newSpellType = Util.boostSpellType;
            }
        }

        /// <summary>
        /// Updates the SpellManager
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            // Displays SpellPanel if a spell is being hovered over by mouse
            if (sidePanel.NukeSpellButton.State != ButtonStatus.Normal)
            {
                willPreviewSpellStats = true;
                waveManager.WillDrawEnemyPanel = false;
                LoadSpellStats(Util.nukeSpellType);
            }
            else if (sidePanel.SlowSpellButton.State != ButtonStatus.Normal)
            {
                willPreviewSpellStats = true;
                waveManager.WillDrawEnemyPanel = false;
                LoadSpellStats(Util.slowSpellType);
            }
            else if (sidePanel.TowerBoostSpellButton.State != ButtonStatus.Normal)
            {
                willPreviewSpellStats = true;
                waveManager.WillDrawEnemyPanel = false;
                LoadSpellStats(Util.boostSpellType);
            }
            else
            {
                if (!spellIsBeingPreviewed)
                {
                    willPreviewSpellStats = false;
                }
            }

            // Makes an instance of the spell to be used
            if (string.IsNullOrEmpty(newSpellType) == false)
            {
                InitializeSpell();
            }
            else
            {
                spellIsSelected = false;
            }

            // Get enemies and towers if spell is going to be fired
            if (spellIsSelected)
            {
                currentSpell.GetEnemiesInRange(waveManager.Enemies);
                currentSpell.GetAllTowers(towerManager.Towers);
                currentSpell.Update(gameTime);
                if (currentSpell.SpellUsed)
                {
                    spellIsSelected = false;
                    currentSpell = null;
                }
            }

            // Hot key for nuke
            if (keyState.IsKeyDown(Keys.D1) && oldKeyState.IsKeyUp(Keys.D1))
            {
                UseNuke();
            }

            // Hot key for slow
            if (keyState.IsKeyDown(Keys.D2) && oldKeyState.IsKeyUp(Keys.D2))
            {
                UseSlow();
            }

            // Hot key for tower boost
            if (keyState.IsKeyDown(Keys.D3) && oldKeyState.IsKeyUp(Keys.D3))
            {
                UseTowerBoost();
            }

            oldKeyState = keyState;
            oldMouseState = mouseState;
            
        }

        /// <summary>
        /// Draws the spells and panel if necessary
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (spellIsSelected)
            {
                currentSpell.Draw(spriteBatch);
            }

            if (willPreviewSpellStats)
            {
                spellPanel.DrawPreview(spriteBatch, previewType, previewCost, previewRadius, previewDamage);
            }
        }
    }
}
