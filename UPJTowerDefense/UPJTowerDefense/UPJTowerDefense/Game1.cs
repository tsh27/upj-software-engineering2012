using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UPJTowerDefense
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int xTiles = 20;
        const int yTiles = 13;
        const int tileSize = 48;
        const int tileWidth = tileSize;
        const int tileHeight = tileSize;
        const int mapWidth = xTiles * tileWidth;
        const int mapHeight = yTiles * tileHeight;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle screenRectangle;

        Level level = new Level();
        //Enemy enemy;
        Player player;
        WaveManager waveManager;

        Button arrowButton;
        
        Texture2D mapTexture;
        Texture2D pathTexture;
        Texture2D baseTexture;
        SpriteFont gameFont;
        Texture2D t2DTitleScreen;
        Texture2D gameOverScreen;
        Vector2 startTextLocation = new Vector2(30, 350);

        SpriteFont panelFont;
        SidePanel sidePanel;
        TowerPanel towerPanel;
        EnemyPanel enemyPanel;

        //Sounds
        SoundEffect towerShot;
        Song splashBackgroundSong;

        int gameStarted = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 760;

            screenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        protected override void Initialize()
        {
            //Make mouse visible
            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load audio files
            towerShot = Content.Load<SoundEffect>("scifi002");
            splashBackgroundSong = Content.Load<Song>("glomag-red_pixel_dust");
            MediaPlayer.Play(splashBackgroundSong);

            t2DTitleScreen = Content.Load<Texture2D>("TitleScreen");
            gameOverScreen = Content.Load<Texture2D>("GameOver");

            gameFont = Content.Load<SpriteFont>("Pericles");
            panelFont = Content.Load<SpriteFont>("PanelFont");
            mapTexture = Content.Load<Texture2D>("Square");
            pathTexture = Content.Load<Texture2D>("Square");
            baseTexture = Content.Load<Texture2D>("Square");

            level.AddTexture(mapTexture);
            level.AddTexture(pathTexture);
            level.AddTexture(baseTexture);

            Texture2D arrowTowerImage = Content.Load<Texture2D>("arrow tower");
            Texture2D arrowNormal = Content.Load<Texture2D>("arrow button");
            Texture2D arrowHover = Content.Load<Texture2D>("arrow hover");
            Texture2D arrowPressed = Content.Load<Texture2D>("arrow pressed");

            Texture2D bulletTexture = Content.Load<Texture2D>("bullet");

            Texture2D[] towerTextures = new Texture2D[]
            {
                Content.Load<Texture2D>("arrow tower"),
            };

            Texture2D infoPanelImage = Content.Load<Texture2D>("InfoPanel");
            towerPanel = new TowerPanel(infoPanelImage, panelFont, 
                new Vector2(0, mapHeight), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - mapHeight);
            enemyPanel = new EnemyPanel(infoPanelImage, panelFont, 
                new Vector2(0, mapHeight), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - mapHeight);

            player = new Player(level, towerPanel, enemyPanel, towerTextures, bulletTexture, towerShot);

            // Initialize the arrow button.
            arrowButton = new Button(arrowNormal, arrowHover,
                arrowPressed, new Vector2(1100, 245));
            arrowButton.OnPress += new EventHandler(arrowButton_OnPress);

            Texture2D enemyTexture = Content.Load<Texture2D>("enemy");

            waveManager = new WaveManager(player, level, 5, enemyTexture);

            Texture2D sidePanelImage = Content.Load<Texture2D>("SidePanel");
            sidePanel = new SidePanel(sidePanelImage, panelFont, new Vector2(mapWidth, 0), 
                graphics.PreferredBackBufferWidth - mapWidth, mapHeight);
        }

        private void arrowButton_OnPress(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
            player.NewTowerIndex = 0;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //Get keyboard values
            KeyboardState keyState = Keyboard.GetState();

            //Get mouse state
            MouseState mouseState = Mouse.GetState();

            if (gameStarted == 0)
            {
                if (keyState.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }

                if (keyState.IsKeyDown(Keys.Space) || mouseState.LeftButton == ButtonState.Pressed)
                {
                    gameStarted = 1;
                }

                if (keyState.IsKeyDown(Keys.M))
                {
                    util.soundOn = false;
                    MediaPlayer.Pause();
                }

                if (keyState.IsKeyDown(Keys.L))
                {
                    util.livesCheat = true;
                    player.Lives = 1000000;
                }

                if (keyState.IsKeyDown(Keys.D4))
                {
                    util.moneyCheat = true;
                    player.Money = 1000000;
                }
            }
            else
            {
                if (util.gameOver && keyState.IsKeyDown(Keys.Q))
                {
                    this.Exit();
                }
                else
                {
                    waveManager.Update(gameTime);
                    player.Update(gameTime, waveManager.Enemies);
                    arrowButton.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (gameStarted == 0)
            {
                spriteBatch.Draw(t2DTitleScreen, 
                    new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.DrawString(gameFont, "Dedicated to Johannes Vermeer", new Vector2(30, 730), Color.White);
                spriteBatch.DrawString(gameFont, "Press M to turn sound off.", new Vector2(30, 500), Color.White);
                
                if (gameTime.TotalGameTime.Milliseconds % 1000 < 500)
                {
                    spriteBatch.DrawString(gameFont, "Press SPACE or CLICK Mouse to Begin", startTextLocation, Color.Gold);
                }

                if (util.livesCheat)
                {
                    spriteBatch.DrawString(gameFont, "Unlocked lives cheat!", new Vector2(30, 230), Color.CornflowerBlue);
                }

                if (util.moneyCheat)
                {
                    spriteBatch.DrawString(gameFont, "Unlocked money cheat!", new Vector2(30, 260), Color.CornflowerBlue);
                }
            }
            else if (gameStarted == 1)
            {
                if (!util.gameOver)
                {
                    player.Draw(spriteBatch, waveManager.Enemies);
                    waveManager.Draw(spriteBatch);
                    sidePanel.Draw(spriteBatch, player, waveManager.Round);
                    arrowButton.Draw(spriteBatch);
                    player.DrawPreview(spriteBatch);
                }
                else
                {
                    spriteBatch.Draw(gameOverScreen, 
                        new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
