#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;
#endregion

namespace UPJTowerDefense
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;

        float pauseAlpha;
        InputAction pauseAction;

        private Level level;
        private Player player;
        private TowerManager towerManager;
        private WaveManager waveManager;
        private SpellManager spellManager;

        // Font used on the title screen
        private SpriteFont gameFont;

        // Create panels for GUI
        private SpriteFont panelFont;
        private SidePanel sidePanel;
        private TowerPanel towerPanel;
        private EnemyPanel enemyPanel;
        private SpellPanel spellPanel;

        private Song gameSong;
        private SoundEffect pauseSound;
        private SoundEffect gameOverSound;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                null,
                new Keys[] { Keys.P },
                true);

            Util.gameOver = false;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                {
                    content = new ContentManager(ScreenManager.Game.Services, "content");
                }

                // Load fonts
                gameFont = content.Load<SpriteFont>("Pericles");
                panelFont = content.Load<SpriteFont>("PanelFont");

                Texture2D mapTexture = content.Load<Texture2D>("CornflowerTile");
                Texture2D pathTexture = content.Load<Texture2D>("MapTile");
                Texture2D baseTexture = content.Load<Texture2D>("TowerBase");
                Texture2D webTexture = content.Load<Texture2D>("StartBase");

                Texture2D[] spellTextures = new Texture2D[]
                {
                    content.Load<Texture2D>("NukeButtons/NukeReadyNormal"),
                };

                // Load info panel image
                Texture2D infoPanelImage = content.Load<Texture2D>("InfoPanel");

                //load explosion sprite sheet
                Texture2D explosionTexture = content.Load<Texture2D>("explosions");

                SoundEffect[] spellSounds = new SoundEffect[]
                {
                    content.Load<SoundEffect>("juskiddink-nukeexplosion"),
                    content.Load<SoundEffect>("daveincamas-joedeshon-tickbell"),
                };

                gameSong = content.Load<Song>("TitleScreenSong");
                MediaPlayer.Volume = 0.5f;
                MediaPlayer.Play(gameSong);
                MediaPlayer.IsRepeating = true;

                pauseSound = content.Load<SoundEffect>("guitarguy1985-buzzer");
                gameOverSound = content.Load<SoundEffect>("klankbeeld-gameover");

                //Load player based on cheat codes
                int playerLives, playerMoney;
                playerLives = Options.livesCheatOn ? 1000000 : 50;
                playerMoney = Options.moneyCheatOn ? 1000000 : 1000;

                // Initialize player
                player = new Player(playerLives, playerMoney);

                level = new Level();
                level.AddTileTexture(mapTexture);
                level.AddTileTexture(pathTexture);
                level.AddTileTexture(baseTexture);
                level.AddTileTexture(webTexture);

                // Initialize towerPanel
                towerPanel = new TowerPanel(content, panelFont, new Vector2(0, Util.mapHeight), 1280, 760 - Util.mapHeight);

                // Initialize enemyPanel
                enemyPanel = new EnemyPanel(content, panelFont, new Vector2(0, Util.mapHeight), 1280, 760 - Util.mapHeight);

                // Initialize spellPanel
                spellPanel = new SpellPanel(infoPanelImage, panelFont,new Vector2(0, Util.mapHeight), 1280, 760 - Util.mapHeight);

                // Initialize sidePanel
                sidePanel = new SidePanel(content, panelFont, new Vector2(Util.mapWidth, 0),
                    1280 - Util.mapWidth, Util.mapHeight);

                // Initialize the waveManager
                waveManager = new WaveManager(content, player, level, sidePanel, enemyPanel, explosionTexture);

                // Initialize the towerManager
                towerManager = new TowerManager(content, player, level, waveManager, towerPanel, sidePanel);

                // Initialize the spellManager
                spellManager = new SpellManager(player, level, waveManager, towerManager,
                    spellPanel, sidePanel, spellTextures, spellSounds);

                // A real game would probably have more content than this sample, so
                // it would take longer to load. We simulate that by delaying for a
                // while, giving you a chance to admire the beautiful loading screen.
                Thread.Sleep(3000);

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            if (IsActive)
            {
                
                if (!Util.gameOver)
                {
                    if (Options.willChangeWorld)
                    {
                        Options.willChangeWorld = !Options.willChangeWorld;
                        MessageBoxScreen confirmWorldMessageBox = new MessageBoxScreen("You completed this world");
                        confirmWorldMessageBox.Accepted += ConfirmWorldMessageBoxAccepted;
                        ScreenManager.AddScreen(new GameplayScreen(), ControllingPlayer);
                        ScreenManager.AddScreen(confirmWorldMessageBox, ControllingPlayer);
                        ScreenManager.RemoveScreen(this);
                    }
                    else
                    {
                        waveManager.Update(gameTime);
                        towerManager.Update(gameTime, waveManager.Enemies);
                        spellManager.Update(gameTime);
                        sidePanel.Update(gameTime);
                        towerPanel.Update(gameTime);
                    }
                }
                else
                {
                    if (Options.soundEffectsOn)
                    {
                        gameOverSound.Play(1.0f, 0f, 0f);
                    }
                    ScreenManager.AddScreen(new GameOverMenuScreen(), null);
                    Options.worldNumber = 1;
                }
            }

            if (!Options.musicOn && MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
            else if (Options.musicOn && MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(gameSong);
            }
        }

        void ConfirmWorldMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            //bool gamePadDisconnected = !gamePadState.IsConnected &&
            //                           input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player)) //|| gamePadDisconnected)
            {
                if (Options.soundEffectsOn)
                {
                    // Play pause sound
                    pauseSound.Play(0.7f, 0f, 0f);
                }
                ScreenManager.AddScreen(new PauseMenuScreen(pauseSound), ControllingPlayer);
            }
        }

        private void DrawHelpScreen(SpriteBatch spriteBatch)
        {
            char[] delimiters = { '\r', '\n' };
            String[] text = File.ReadAllLines(@"Other\HelpFile.txt");
            int textY = 10;

            for (int i = 0; i < text.Length; i++)
            {
                spriteBatch.DrawString(panelFont, text[i], new Vector2(10, textY), Color.Chartreuse);
                textY += 20;
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            level.Draw(spriteBatch);
            waveManager.Draw(spriteBatch);
            towerManager.Draw(spriteBatch, waveManager.Enemies);
            spellManager.Draw(spriteBatch);
            sidePanel.Draw(spriteBatch, player, waveManager.Round, waveManager.TotalRounds, waveManager.LastRound);
            towerManager.DrawPreview(spriteBatch);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}