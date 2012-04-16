using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using GameStateManagement;

namespace UPJTowerDefense
{
    public class TowerManager
    {
        // Reference to the Content Manager
        private ContentManager content;

        // A list of the players towers
        private List<Tower> towers = new List<Tower>();
        private static int towerID = -1;

        // Tower selection
        private int selectedTowerID;
        private Tower selectedTower = null;
        private bool towerIsSelected = false;
        private bool willDrawTowerPanel = false;

        // Preview Tower stats
        private bool willPreviewTowerStats = false;
        private bool towerIsBeingPreviewed = false;
        private string previewType;
        private int previewCost;
        private int previewRadius;
        private int previewDamage;

        // The textures used to draw our tower
        private Texture2D[] towerTextures;

        // The texture used to draw bullets
        private Texture2D[] bulletTextures;

        // Sprite sheets for pulse, freeze, and fire towers
        private Texture2D pulseTowerEffect;
        private Texture2D freezeTowerEffect;
        private Texture2D fireTowerFlames;

        // Sound for placing a tower
        private SoundEffect buildSound;

        // The textures to display the radii
        // for all towers
        private Texture2D[][] allTowerRadii;

        // Mouse state for the current frame.
        private MouseState mouseState;

        // Mouse state for the previous frame.
        private MouseState oldMouseState;

        // Keyboard state for the current frame
        private KeyboardState keyState;

        // Keyboard state for the previous frame
        private KeyboardState oldKeyState;

        // Tower placement.
        private int cellX;
        private int cellY;
        private int tileX;
        private int tileY;

        // The type of tower to add.
        private string newTowerType;

        // The index of the new towers texture.
        private int newTowerIndex;

        // References to other needed objects
        private Player player;
        private Level level;
        private TowerPanel towerPanel;
        private SidePanel sidePanel;
        private WaveManager waveManager;
        private FireTowerPanel fireTowerPanel;

        private SpriteFont panelFont;
        private bool willDrawFireTowerPanel = false;

        // Audio for shooting
        private SoundEffect[] towerShots;

        // Tower Boost Graphic
        private Texture2D towerBoostTexture;

        public string NewTowerType
        {
            get { return newTowerType; }
            set { newTowerType = value; }
        }

        public int NewTowerIndex
        {
            get { return newTowerIndex; }
            set { newTowerIndex = value; }
        }

        public List<Tower> Towers
        {
            get { return towers; }
        }

        /// <summary>
        /// Constructs a TowerManager
        /// </summary>
        public TowerManager(ContentManager content, Player player, Level level,
            WaveManager waveManager, TowerPanel towerPanel, SidePanel sidePanel)
        {
            this.content = content;
            this.player = player;
            this.level = level;
            this.towerPanel = towerPanel;
            this.sidePanel = sidePanel;
            this.waveManager = waveManager;

            // Load graphical content
            LoadContent();

            // Instantiate a FireTowerPanel
            fireTowerPanel = new FireTowerPanel(content, panelFont, new Vector2(0, Util.mapHeight), 1280, 760 - Util.mapHeight);

            // Listen for button clicks
            towerPanel.DamageUpgradeButton.Clicked += new EventHandler(UpgradeButton_Clicked);
            towerPanel.RadiusUpgradeButton.Clicked += new EventHandler(RadiusUpgradeButton_Clicked);
            towerPanel.SellButton.Clicked += new EventHandler(SellButton_Clicked);
            sidePanel.BasicTowerButton.Clicked += new EventHandler(BasicTowerButton_Clicked);
            sidePanel.PulseTowerButton.Clicked += new EventHandler(PulseTowerButton_Clicked);
            sidePanel.FreezeTowerButton.Clicked += new EventHandler(FreezeTowerButton_Clicked);
            sidePanel.FireTowerButton.Clicked += new EventHandler(FireTowerButton_Clicked);
            sidePanel.MortarTowerButton.Clicked += new EventHandler(MortarTowerButton_Clicked);
        }

        void RadiusUpgradeButton_Clicked(object sender, EventArgs e)
        {
            if (towerIsSelected)
            {
                UpgradeTowerRadius();
            }
        }

        void SellButton_Clicked(object sender, EventArgs e)
        {
            if (towerIsSelected)
            {
                SellTower();
            }
        }

        void UpgradeButton_Clicked(object sender, EventArgs e)
        {
            if (towerIsSelected)
            {
                UpgradeTower();
            }
        }

        void BasicTowerButton_Clicked(object sender, EventArgs e)
        {
            if (!willDrawFireTowerPanel)
            {
                if (player.Money >= Util.basicTowerCost)
                {
                    setPreviewState();
                    LoadTowerStats(Util.basicTowerType);
                    newTowerType = Util.basicTowerType;
                    newTowerIndex = Util.basicTowerIndex;
                }
            }
        }

        void PulseTowerButton_Clicked(object sender, EventArgs e)
        {
            if (!willDrawFireTowerPanel)
            {
                if (player.Money >= Util.pulseTowerCost)
                {
                    setPreviewState();
                    LoadTowerStats(Util.pulseTowerType);
                    newTowerType = Util.pulseTowerType;
                    newTowerIndex = Util.pulseTowerIndex;
                }
            }
        }

        void FreezeTowerButton_Clicked(object sender, EventArgs e)
        {
            if (!willDrawFireTowerPanel)
            {
                if (player.Money >= Util.freezeTowerCost)
                {
                    setPreviewState();
                    LoadTowerStats(Util.freezeTowerType);
                    newTowerType = Util.freezeTowerType;
                    newTowerIndex = Util.freezeTowerIndex;
                }
            }
        }

        void FireTowerButton_Clicked(object sender, EventArgs e)
        {
            if (!willDrawFireTowerPanel)
            {
                if (player.Money >= Util.fireTowerCost)
                {
                    setPreviewState();
                    LoadTowerStats(Util.fireTowerType);
                    newTowerType = Util.fireTowerType;
                    newTowerIndex = Util.fireTowerIndex;
                }
            }
        }

        void  MortarTowerButton_Clicked(object sender, EventArgs e)
        {
            if (!willDrawFireTowerPanel)
            {
                if (player.Money >= Util.mortarTowerCost)
                {
                    setPreviewState();
                    LoadTowerStats(Util.mortarTowerType);
                    newTowerType = Util.mortarTowerType;
                    newTowerIndex = Util.mortarTowerIndex;
                }
            }
        }

        /// <summary>
        /// Set if a tower is being previewed
        /// </summary>
        private void setPreviewState()
        {
            towerIsSelected = false;
            willDrawTowerPanel = false;
            towerIsBeingPreviewed = true;
            willPreviewTowerStats = true;
            waveManager.WillDrawEnemyPanel = false;
        }

        /// <summary>
        /// Set if some tower is selected
        /// </summary>
        private void setSelectedState()
        {
            towerIsSelected = true;
            willDrawTowerPanel = true;
            towerIsBeingPreviewed = false;
            willPreviewTowerStats = false;
            waveManager.WillDrawEnemyPanel = false;
        }

        /// <summary>
        /// Set if no tower is selected
        /// </summary>
        private void setDeselectedState()
        {
            towerIsSelected = false;
            willDrawTowerPanel = false;
            towerIsBeingPreviewed = false;
            willPreviewTowerStats = false;
        }

        // Load preview stats based on tower type
        private void LoadTowerStats(string towerType)
        {
            if (towerType == Util.basicTowerType)
            {
                previewType = Util.basicTowerType;
                previewCost = Util.basicTowerCost;
                previewRadius = Util.basicTowerRadius;
                previewDamage = Util.basicTowerDamage;
            }
            else if (towerType == Util.pulseTowerType)
            {
                previewType = Util.pulseTowerType;
                previewCost = Util.pulseTowerCost;
                previewRadius = Util.pulseTowerRadius;
                previewDamage = Util.pulseTowerDamage;
            }
            else if (towerType == Util.freezeTowerType)
            {
                previewType = Util.freezeTowerType;
                previewCost = Util.freezeTowerCost;
                previewRadius = Util.freezeTowerRadius;
                previewDamage = Util.freezeTowerDamage;
            }
            else if (towerType == Util.fireTowerType)
            {
                previewType = Util.fireTowerType;
                previewCost = Util.fireTowerCost;
                previewRadius = Util.fireTowerRadius;
                previewDamage = Util.fireTowerDamage;
            }
            else if (towerType == Util.mortarTowerType)
            {
                previewType = Util.mortarTowerType;
                previewCost = Util.mortarTowerCost;
                previewRadius = Util.mortarTowerRadius;
                previewDamage = Util.mortarTowerDamage;
            }
        }

        /// <summary>
        /// Determines if a tower was clicked
        /// </summary>
        /// <returns></returns>
        private bool WasTowerClicked()
        {
            bool spaceNotClear = false;
            foreach (Tower tower in towers)
            {
                spaceNotClear = (tower.Position == new Vector2(tileX, tileY));

                if (spaceNotClear)
                {
                    selectedTowerID = tower.TowerID;
                    break;
                }
            }

            return spaceNotClear;
        }

        // Determines if placement tile is in the boundaries of the map
        private bool InBounds()
        {
            return cellX >= 0 && cellY >= 0 &&
                cellX < level.Width && cellY < level.Height;
        }

        /// <summary>
        /// Returns wether the current cell is clear
        /// </summary>
        private bool IsCellClear()
        {
            bool spaceClear = true;

            // Check that there is no tower in this spot
            foreach (Tower tower in towers)
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));

                if (!spaceClear)
                {
                    break;
                }
            }

            bool onPath = (level.GetIndex(cellX, cellY) < 1);

            return InBounds() && spaceClear && onPath; // If both checks are true return true
        }

        /// <summary>
        /// Adds a tower to the player's collection.
        /// </summary>
        public void AddTower()
        {
            keyState = Keyboard.GetState();

            Tower towerToAdd = null;
            towerID++;

            if (newTowerType.Equals(Util.basicTowerType))
            {
                towerToAdd = new BasicTower(towerTextures[Util.basicTowerIndex], bulletTextures[0],
                    new Vector2(tileX, tileY), Util.basicTowerType, towerID, towerShots[0]);
            }
            else if (newTowerType.Equals(Util.pulseTowerType))
            {
                towerToAdd = new PulseTower(towerTextures[Util.pulseTowerIndex], pulseTowerEffect,
                    new Vector2(tileX, tileY), Util.pulseTowerType, towerID, towerShots[1]);
            }
            else if (newTowerType.Equals(Util.freezeTowerType))
            {
                towerToAdd = new FreezeTower(towerTextures[Util.freezeTowerIndex], freezeTowerEffect,
                    new Vector2(tileX, tileY), Util.freezeTowerType, towerID, towerShots[2]);
            }
            else if (newTowerType.Equals(Util.fireTowerType))
            {
                towerToAdd = new FireTower(towerTextures[Util.fireTowerIndex], fireTowerFlames,
                    new Vector2(tileX, tileY), Util.fireTowerType, towerID, towerShots[3]);
            }
            else if (newTowerType.Equals(Util.mortarTowerType))
            {
                towerToAdd = new MortarTower(towerTextures[Util.mortarTowerIndex], bulletTextures[1],
                    new Vector2(tileX, tileY), Util.mortarTowerType, towerID, towerShots[4]);
            }
            

            // Only add the tower if there is a space and if the player can afford it.
            if (towerToAdd.Cost <= player.Money && IsCellClear())
            {
                towers.Add(towerToAdd);
                player.Money -= towerToAdd.Cost;
                setSelectedState();
                selectedTower = towerToAdd;
                selectedTowerID = towerToAdd.TowerID;
                if (Options.soundEffectsOn)
                {
                    // Play build sound
                    buildSound.Play(0.2f, 0f, 0f);
                }

                if (selectedTower.TowerType == "Fire")
                {
                    willDrawFireTowerPanel = true;
                    setDeselectedState();                  
                }
            }

            // Reset the newTowerType field.
            newTowerType = string.Empty;
        }

        /// <summary>
        /// Upgrades damage of a Tower
        /// </summary>
        private void UpgradeTower()
        {
            setSelectedState();
            int index = towers.IndexOf(selectedTower);
            int baseDamage = 0;
            int baseCost = 0;

            if (towers[index].DamageUpgradeNumber < 3)
            {
                towers[index].DamageUpgradeNumber += 1;

                if (towers[index].TowerType == "Basic")
                {
                    baseDamage = Util.basicTowerDamage;
                    baseCost = Util.basicTowerCost;
                }
                else if (towers[index].TowerType == "Pulse")
                {
                    baseDamage = Util.pulseTowerDamage;
                    baseCost = Util.pulseTowerCost;
                }
                else if (towers[index].TowerType == "Freeze")
                {
                    baseDamage = Util.freezeTowerDamage;
                    baseCost = Util.freezeTowerCost;
                }
                else if (towers[index].TowerType == "Fire")
                {
                    baseDamage = Util.fireTowerDamage;
                    baseCost = Util.fireTowerCost;
                }
                else if (towers[index].TowerType == "Mortar")
                {
                    baseDamage = Util.mortarTowerDamage;
                    baseCost = Util.mortarTowerCost;
                }

                if (towers[index].DamageUpgradeNumber == 1)
                {
                    if (player.Money >= (int)(baseCost * .25))
                    {
                        towers[index].Damage += (int)(baseDamage * .25);
                        towers[index].TotalWorth += (int)(baseCost * .25);
                        player.Money -= (int)(baseCost * .25);
                    }
                    else
                    {
                        towers[index].DamageUpgradeNumber--;
                    }
                }
                else if (towers[index].DamageUpgradeNumber == 2)
                {
                    if (player.Money >= (int)(baseCost * .5))
                    {
                        towers[index].Damage += (int)(baseDamage * .5);
                        towers[index].TotalWorth += (int)(baseCost * .5);
                        player.Money -= (int)(baseCost * .5);
                    }
                    else
                    {
                        towers[index].DamageUpgradeNumber--;
                    }
                }
                else if (towers[index].DamageUpgradeNumber == 3)
                {
                    if (player.Money >= baseCost)
                    {
                        towers[index].Damage = baseDamage * 2;
                        towers[index].TotalWorth += baseCost;
                        player.Money -= baseCost;
                    }
                    else
                    {
                        towers[index].DamageUpgradeNumber--;
                    }
                }
            }
        }

        /// <summary>
        /// Upgrades radius of a Tower
        /// </summary>
        private void UpgradeTowerRadius()
        {
            setSelectedState();
            int index = towers.IndexOf(selectedTower);
            int baseCost = 0;

            if (towers[index].RadiusUpgradeNumber < 3)
            {
                towers[index].RadiusUpgradeNumber++;

                if (towers[index].TowerType == "Basic")
                {
                    baseCost = Util.basicTowerCost;
                }
                else if (towers[index].TowerType == "Pulse")
                {
                    baseCost = Util.pulseTowerCost;
                }
                else if (towers[index].TowerType == "Freeze")
                {
                    baseCost = Util.freezeTowerCost;
                }
                else if (towers[index].TowerType == "Fire")
                {
                    baseCost = Util.fireTowerCost;
                }
                else if (towers[index].TowerType == "Mortar")
                {
                    baseCost = Util.mortarTowerCost;
                }

                if (towers[index].RadiusUpgradeNumber == 1)
                {
                    if (player.Money >= (int)(baseCost * .25))
                    {
                        towers[index].Radius += Util.tileSize;
                        towers[index].TotalWorth += (int)(baseCost * .25);
                        player.Money -= (int)(baseCost * .25);
                    }
                    else
                    {
                        towers[index].RadiusUpgradeNumber--;
                    }
                }
                else if (towers[index].RadiusUpgradeNumber == 2)
                {
                    if (player.Money >= (int)(baseCost * .5))
                    {
                        towers[index].Radius += Util.tileSize;
                        towers[index].TotalWorth += (int)(baseCost * .5);
                        player.Money -= (int)(baseCost * .5);
                    }
                    else
                    {
                        towers[index].RadiusUpgradeNumber--;
                    }
                }
                else if (towers[index].RadiusUpgradeNumber == 3)
                {
                    if (player.Money >= baseCost)
                    {
                        towers[index].Radius += Util.tileSize;
                        towers[index].TotalWorth += baseCost;
                        player.Money -= baseCost;
                    }
                    else
                    {
                        towers[index].RadiusUpgradeNumber--;
                    }
                }
            }
        }

        /// <summary>
        /// Sells a placed Tower
        /// </summary>
        private void SellTower()
        {
            int index = towers.IndexOf(selectedTower);
            if (index >= 0)
            {
                player.Money += (int)(towers[index].TotalWorth * .75);
                towers.Remove(selectedTower);
                setDeselectedState();
                selectedTower = null;
            }
        }

        /// <summary>
        /// Load graphical content for TowerManager
        /// </summary>
        private void LoadContent()
        {
            // Bullets
            bulletTextures = new Texture2D[]
            {
                content.Load<Texture2D>("Bullet Images/Bullet7"),
                content.Load<Texture2D>("Bullet Images/bullet"),
            };

            // Sprite sheets
            pulseTowerEffect = content.Load<Texture2D>("PulseTowerEffects/PulseTowerEffectSheet1152x3456");
            freezeTowerEffect = content.Load<Texture2D>("FreezeTowerEffects/FreezeTowerEffectSheet1152x3456");
            fireTowerFlames = content.Load<Texture2D>("FlameEffect96x384");

            // Tower images
            towerTextures = new Texture2D[]
            {
                content.Load<Texture2D>("Tower Images/basic tower"),
                content.Load<Texture2D>("Tower Images/pulse tower"),
                content.Load<Texture2D>("Tower Images/freeze tower"),
                content.Load<Texture2D>("Tower Images/fire tower"),
                content.Load<Texture2D>("Tower Images/mortar tower"),
            };

            /* Tower Radii */
            Texture2D[] basicTowerRadii = new Texture2D[]
            {
                content.Load<Texture2D>("BasicTowerRadii/BasicTowerRadius1"),
                content.Load<Texture2D>("BasicTowerRadii/BasicTowerRadius2"),
                content.Load<Texture2D>("BasicTowerRadii/BasicTowerRadius3"),
                content.Load<Texture2D>("BasicTowerRadii/BasicTowerRadius4"),
            };

            Texture2D[] pulseTowerRadii = new Texture2D[]
            {
                content.Load<Texture2D>("PulseTowerRadii/PulseTowerRadius1"),
                content.Load<Texture2D>("PulseTowerRadii/PulseTowerRadius2"),
                content.Load<Texture2D>("PulseTowerRadii/PulseTowerRadius3"),
                content.Load<Texture2D>("PulseTowerRadii/PulseTowerRadius4"),
            };

            Texture2D[] freezeTowerRadii = new Texture2D[]
            {
                content.Load<Texture2D>("FreezeTowerRadii/FreezeTowerRadius1"),
                content.Load<Texture2D>("FreezeTowerRadii/FreezeTowerRadius2"),
                content.Load<Texture2D>("FreezeTowerRadii/FreezeTowerRadius3"),
                content.Load<Texture2D>("FreezeTowerRadii/FreezeTowerRadius4"),
            };

            Texture2D[] fireTowerRadii = new Texture2D[]
            {
                content.Load<Texture2D>("FireTowerRadii/FireTowerRadius1"),
                content.Load<Texture2D>("FireTowerRadii/FireTowerRadius2"),
                content.Load<Texture2D>("FireTowerRadii/FireTowerRadius3"),
                content.Load<Texture2D>("FireTowerRadii/FireTowerRadius4"),
            };

            Texture2D[] mortarTowerRadii = new Texture2D[]
            {
                content.Load<Texture2D>("MortarTowerRadii/MortarTowerRadius1"),
                content.Load<Texture2D>("MortarTowerRadii/MortarTowerRadius2"),
                content.Load<Texture2D>("MortarTowerRadii/MortarTowerRadius3"),
                content.Load<Texture2D>("MortarTowerRadii/MortarTowerRadius4"),
            };

            allTowerRadii = new Texture2D[][]
            {
                basicTowerRadii,
                pulseTowerRadii,
                freezeTowerRadii,
                fireTowerRadii,
                mortarTowerRadii,
            };

            // Font of the TowerPanel
            panelFont = content.Load<SpriteFont>("PanelFont");

            // Sound effects for the towers
            towerShots = new SoundEffect[]
            {
                content.Load<SoundEffect>("alienbomb-gunburst"),
                content.Load<SoundEffect>("connum-electricsparks"),
                content.Load<SoundEffect>("timbre-clang"),
                content.Load<SoundEffect>("lollosound-pass"),
                content.Load<SoundEffect>("emsiarma-m21shot"),
            };

            // Sound effect for tower placement
            buildSound = content.Load<SoundEffect>("laya-kick");

            // Tower Boost Graphic
            towerBoostTexture = content.Load<Texture2D>("towerBoost");
        }

        /// <summary>
        /// Update a TowerManager
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="enemies"></param>
        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            // Convert the position of the mouse from array space to level space
            cellX = (int)(mouseState.X / Util.tileSize);
            cellY = (int)(mouseState.Y / Util.tileSize);

            // Convert from array space to level space
            tileX = cellX * Util.tileSize;
            tileY = cellY * Util.tileSize;

            // Load tower stats if the tower button is being hovered over 
            if (!willDrawFireTowerPanel)
            {
                if (sidePanel.BasicTowerButton.State != ButtonStatus.Normal)
                {
                    willPreviewTowerStats = true;
                    waveManager.WillDrawEnemyPanel = false;
                    LoadTowerStats(Util.basicTowerType);
                }
                else if (sidePanel.PulseTowerButton.State != ButtonStatus.Normal)
                {
                    willPreviewTowerStats = true;
                    waveManager.WillDrawEnemyPanel = false;
                    LoadTowerStats(Util.pulseTowerType);
                }
                else if (sidePanel.FreezeTowerButton.State != ButtonStatus.Normal)
                {
                    willPreviewTowerStats = true;
                    waveManager.WillDrawEnemyPanel = false;
                    LoadTowerStats(Util.freezeTowerType);
                }
                else if (sidePanel.FireTowerButton.State != ButtonStatus.Normal)
                {
                    willPreviewTowerStats = true;
                    waveManager.WillDrawEnemyPanel = false;
                    LoadTowerStats(Util.fireTowerType);
                }
                else if (sidePanel.MortarTowerButton.State != ButtonStatus.Normal)
                {
                    willPreviewTowerStats = true;
                    waveManager.WillDrawEnemyPanel = false;
                    LoadTowerStats(Util.mortarTowerType);
                }
                else
                {
                    if (!towerIsBeingPreviewed)
                    {
                        willPreviewTowerStats = false;
                    }
                }
            }

            // If a tower was placed on the map
            if (mouseState.LeftButton == ButtonState.Released
                && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                if (string.IsNullOrEmpty(newTowerType) == false)
                {
                    AddTower();
                }
                else if (WasTowerClicked() && !willDrawFireTowerPanel)
                {
                    setSelectedState();
                    foreach (Tower tower in towers)
                    {
                        if (tower.TowerID == selectedTowerID)
                        {
                            selectedTower = tower;
                            break;
                        }
                    }
                }
                else
                {
                    if (!towerPanel.DamageUpgradeButton.InBounds && !towerPanel.RadiusUpgradeButton.InBounds
                        && !towerPanel.SellButton.InBounds)
                    {
                        setDeselectedState();
                    }
                }
            }

            // Update each placed tower
            foreach (Tower tower in towers)
            {
                // Make sure the tower has no targets.
                if (tower.HasTarget == false)
                {
                    tower.GetClosestEnemy(enemies);
                }

                tower.Update(gameTime);
            }


            keyState = Keyboard.GetState();

            // Hot key to cancel tower preview
            if (keyState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape))
            {
                if (towerIsBeingPreviewed)
                {
                    setDeselectedState();
                    newTowerType = String.Empty;
                    newTowerIndex = -1;
                }

                if (towerIsSelected)
                {
                    setDeselectedState();
                    selectedTower = null;
                }
            }

            // Hot key for damage upgrade
            if (keyState.IsKeyDown(Keys.D) && oldKeyState.IsKeyUp(Keys.D))
            {
                if (towerIsSelected)
                {
                    UpgradeTower();
                }
            }

            // Hot key for radius upgrade
            if (keyState.IsKeyDown(Keys.R) && oldKeyState.IsKeyUp(Keys.R))
            {
                if (towerIsSelected)
                {
                    UpgradeTowerRadius();
                }
            }

            // Hot key for selling a tower
            if (keyState.IsKeyDown(Keys.S) && oldKeyState.IsKeyUp(Keys.S))
            {
                if (towerIsSelected)
                {
                    SellTower();
                }
            }

            // Hot key for BasicTower
            if (keyState.IsKeyDown(Keys.Z) && oldKeyState.IsKeyUp(Keys.Z))
            {
                if (!willDrawFireTowerPanel)
                {
                    if (player.Money >= Util.basicTowerCost)
                    {
                        setPreviewState();
                        LoadTowerStats(Util.basicTowerType);
                        newTowerType = Util.basicTowerType;
                        newTowerIndex = Util.basicTowerIndex;
                    }
                }
            }

            // Hot key for PulseTower
            if (keyState.IsKeyDown(Keys.X) && oldKeyState.IsKeyUp(Keys.X))
            {
                if (!willDrawFireTowerPanel)
                {
                    if (player.Money >= Util.pulseTowerCost)
                    {
                        setPreviewState();
                        LoadTowerStats(Util.pulseTowerType);
                        newTowerType = Util.pulseTowerType;
                        newTowerIndex = Util.pulseTowerIndex;
                    }
                }
            }

            // Hot key for FreezeTower
            if (keyState.IsKeyDown(Keys.C) && oldKeyState.IsKeyUp(Keys.C))
            {
                if (!willDrawFireTowerPanel)
                {
                    if (player.Money >= Util.freezeTowerCost)
                    {
                        setPreviewState();
                        LoadTowerStats(Util.freezeTowerType);
                        newTowerType = Util.freezeTowerType;
                        newTowerIndex = Util.freezeTowerIndex;
                    }
                }
            }

            // Hot key for FireTower
            if (keyState.IsKeyDown(Keys.V) && oldKeyState.IsKeyUp(Keys.V))
            {
                if (!willDrawFireTowerPanel)
                {
                    if (player.Money >= Util.fireTowerCost)
                    {
                        setPreviewState();
                        LoadTowerStats(Util.fireTowerType);
                        newTowerType = Util.fireTowerType;
                        newTowerIndex = Util.fireTowerIndex;
                    }
                }
            }

            // Hot key for MortarTower
            if (keyState.IsKeyDown(Keys.B) && oldKeyState.IsKeyUp(Keys.B))
            {
                if (!willDrawFireTowerPanel)
                {
                    if (player.Money >= Util.mortarTowerCost)
                    {
                        setPreviewState();
                        LoadTowerStats(Util.mortarTowerType);
                        newTowerType = Util.mortarTowerType;
                        newTowerIndex = Util.mortarTowerIndex;
                    }
                }
            }

            // Cycles through placed towers from oldest to newest if selected
            if (keyState.IsKeyDown(Keys.Right) && oldKeyState.IsKeyUp(Keys.Right))
            {
                if (towerIsSelected)
                {
                    int selectedTowerIndex = towers.IndexOf(selectedTower);
                    int nextTowerIndex = selectedTowerIndex + 1;
                    if (nextTowerIndex == towers.Count)
                    {
                        selectedTower = towers[0];
                    }
                    else
                    {
                        selectedTower = towers[nextTowerIndex];
                    }
                }
                else if (willDrawFireTowerPanel)
                {
                    selectedTower.ShootingDirection = new Vector2(0, 1);
                }
            }

            // Cycles through placed towers from newest to oldest if selected
            if (keyState.IsKeyDown(Keys.Left) && oldKeyState.IsKeyUp(Keys.Left))
            {
                if (towerIsSelected)
                {
                    int selectedTowerIndex = towers.IndexOf(selectedTower);
                    if (selectedTowerIndex == 0)
                    {
                        selectedTower = towers[towers.Count - 1];
                    }
                    else
                    {
                        selectedTower = towers[selectedTowerIndex - 1];
                    }
                }
                else if (willDrawFireTowerPanel)
                {
                    selectedTower.ShootingDirection = new Vector2(0, -1);
                }
            }

            // Selects/Deselects newest placed tower
            if (keyState.IsKeyDown(Keys.Up) && oldKeyState.IsKeyUp(Keys.Up))
            {
                if (!towerIsSelected && !willDrawFireTowerPanel)
                {
                    if (towers.Count > 0)
                    {
                        setSelectedState();
                        selectedTower = towers[towers.Count - 1];
                    }
                }
                else if (willDrawFireTowerPanel)
                {
                    selectedTower.ShootingDirection = new Vector2(1, 0);
                }
                else
                {
                    setDeselectedState();
                }
            }

            // Selects/Deselects oldest placed tower
            if (keyState.IsKeyDown(Keys.Down) && oldKeyState.IsKeyUp(Keys.Down))
            {
                if (!towerIsSelected && !willDrawFireTowerPanel)
                {
                    if (towers.Count > 0)
                    {
                        setSelectedState();
                        selectedTower = towers[0];
                    }
                }
                else if (willDrawFireTowerPanel)
                {
                    selectedTower.ShootingDirection = new Vector2(-1, 0);
                }
                else
                {
                    setDeselectedState();
                }
            }

            // Hot key to enter the shooting direction of the FireTower
            if (keyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter))
            {
                if (willDrawFireTowerPanel)
                {
                    int index = towers.IndexOf(selectedTower);

                    towers[index].WillShoot = true;
                    willDrawFireTowerPanel = false;
                    setSelectedState();
                }
            }

            oldKeyState = keyState;
            oldMouseState = mouseState;
        }

        /// <summary>
        /// Draws managed items for TowerManger
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="enemies"></param>
        public void Draw(SpriteBatch spriteBatch, List<Enemy> enemies)
        {
            // Draw each placed tower
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
                if (tower.BoostModifier != 0f)
                {
                    spriteBatch.Draw(towerBoostTexture, new Rectangle((int)tower.Position.X, (int)tower.Position.Y,
                        Util.tileSize, Util.tileSize), Color.White);
                }
            }

            // Draw radius if tower is selected
            if (towerIsSelected)
            {
                DrawRadius(spriteBatch);
            }

            // Draw TowerPanel if tower is selected
            if (willDrawTowerPanel)
            {
                towerPanel.Draw(spriteBatch, selectedTower);
            }

            // Draw TowerPanel if tower is being previewing
            if (willPreviewTowerStats)
            {
                towerPanel.DrawPreview(spriteBatch, previewType, previewCost, previewRadius, previewDamage);
            }

            // Draw FireTowerPanel if a shooting direction needs selected
            if (willDrawFireTowerPanel)
            {
                fireTowerPanel.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Draws a preview of a tower before placement that follows the mouse
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawPreview(SpriteBatch spriteBatch)
        {
            // Draw the tower preview
            if (string.IsNullOrEmpty(newTowerType) == false)
            {
                // Convert the position of the mouse from array space to level space
                cellX = (int)(mouseState.X / Util.tileSize);
                cellY = (int)(mouseState.Y / Util.tileSize);

                // Convert from array space to level space
                int tileX = cellX * Util.tileSize;
                int tileY = cellY * Util.tileSize;

                Texture2D previewTexture = towerTextures[newTowerIndex];

                spriteBatch.Draw(previewTexture, new Rectangle(tileX, tileY,
                    previewTexture.Width, previewTexture.Height), Color.White);

                Color myColor;
                int x = tileX + (Util.tileWidth / 2);
                int y = tileY + (Util.tileHeight / 2);

                if (InBounds())
                {
                    if (IsCellClear())
                    {
                        myColor = Color.White * 0.3f;
                    }
                    else
                    {
                        myColor = Color.Red * 0.3f;
                    }

                    if (newTowerType == Util.basicTowerType)
                    {
                        spriteBatch.Draw(allTowerRadii[Util.basicTowerIndex][0], new Rectangle(x - Util.basicTowerRadius, y - Util.basicTowerRadius,
                            Util.basicTowerRadius * 2, Util.basicTowerRadius * 2), myColor);
                    }
                    else if (newTowerType == Util.pulseTowerType)
                    {
                        spriteBatch.Draw(allTowerRadii[Util.pulseTowerIndex][0], new Rectangle(x - Util.pulseTowerRadius, y - Util.pulseTowerRadius,
                            Util.pulseTowerRadius * 2, Util.pulseTowerRadius * 2), myColor);
                    }
                    else if (newTowerType == Util.freezeTowerType)
                    {
                        spriteBatch.Draw(allTowerRadii[Util.freezeTowerIndex][0], new Rectangle(x - Util.freezeTowerRadius, y - Util.freezeTowerRadius,
                            Util.freezeTowerRadius * 2, Util.freezeTowerRadius * 2), myColor);
                    }
                    else if (newTowerType == Util.fireTowerType)
                    {
                        spriteBatch.Draw(allTowerRadii[Util.fireTowerIndex][0], new Rectangle(x - Util.fireTowerRadius, y - Util.fireTowerRadius,
                            Util.fireTowerRadius * 2, Util.fireTowerRadius * 2), myColor);
                    }
                    else if (newTowerType == Util.mortarTowerType)
                    {
                        spriteBatch.Draw(allTowerRadii[Util.mortarTowerIndex][0], new Rectangle(x - Util.mortarTowerRadius, y - Util.mortarTowerRadius,
                            Util.mortarTowerRadius * 2, Util.mortarTowerRadius * 2), myColor);
                    }
                }
            }
        }

        /// <summary>
        /// Draws radius of the current selected tower
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawRadius(SpriteBatch spriteBatch)
        {
            int x = (int)selectedTower.Center.X;
            int y = (int)selectedTower.Center.Y;
            int width = (int)selectedTower.Radius * 2;
            int height = (int)selectedTower.Radius * 2;
            int radius = (int)selectedTower.Radius;
            Color myColor = Color.White * 0.3f;

            // Need to check tower type and tower radius upgrade number
            if (selectedTower.TowerType == Util.basicTowerType)
            {
                spriteBatch.Draw(allTowerRadii[Util.basicTowerIndex][selectedTower.RadiusUpgradeNumber], new Rectangle(x - radius, y - radius,
                    width, height), myColor);
            }
            else if (selectedTower.TowerType == Util.pulseTowerType)
            {
                spriteBatch.Draw(allTowerRadii[Util.pulseTowerIndex][selectedTower.RadiusUpgradeNumber], new Rectangle(x - radius, y - radius,
                    width, height), myColor);
            }
            else if (selectedTower.TowerType == Util.freezeTowerType)
            {
                spriteBatch.Draw(allTowerRadii[Util.freezeTowerIndex][selectedTower.RadiusUpgradeNumber], new Rectangle(x - radius, y - radius,
                    width, height), myColor);
            }
            else if (selectedTower.TowerType == Util.fireTowerType)
            {
                Vector2 origin = new Vector2(allTowerRadii[Util.fireTowerIndex][selectedTower.RadiusUpgradeNumber].Width / 2,
                    allTowerRadii[Util.fireTowerIndex][selectedTower.RadiusUpgradeNumber].Height / 2);

                Vector2 center = new Vector2(selectedTower.Position.X + Util.tileSize / 2,
                    selectedTower.Position.Y + Util.tileSize / 2);

                spriteBatch.Draw(allTowerRadii[Util.fireTowerIndex][selectedTower.RadiusUpgradeNumber], center, 
                    null, myColor, selectedTower.Rotation, origin, 1.0f, SpriteEffects.None, 0);
            }
            else if (selectedTower.TowerType == Util.mortarTowerType)
            {
                spriteBatch.Draw(allTowerRadii[Util.mortarTowerIndex][selectedTower.RadiusUpgradeNumber], new Rectangle(x - radius, y - radius,
                    width, height), myColor);
            }
        }
    }
}