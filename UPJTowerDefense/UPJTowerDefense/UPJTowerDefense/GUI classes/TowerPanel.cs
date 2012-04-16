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
    /// Displays information about towers
    /// </summary>
    public class TowerPanel
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

        // Avatar Background
        private Texture2D avatarBackground;

        Texture2D[] damageUpgradeButtons;
        private GUIButton damageUpgradeButton;

        Texture2D[] sellButtons;
        private GUIButton sellButton;

        Texture2D[] radiusUpgradeButtons;
        private GUIButton radiusUpgradeButton;

        public GUIButton DamageUpgradeButton
        {
            get { return damageUpgradeButton; }
        }

        public GUIButton RadiusUpgradeButton
        {
            get { return radiusUpgradeButton; }
        }

        public GUIButton SellButton
        {
            get { return sellButton; }
        }

        /// <summary>
        /// Constructs a TowerPanel
        /// </summary>
        /// <param name="content">The ContentManager</param>
        /// <param name="font">Font for the panel</param>
        /// <param name="position">Coordinates of the panel</param>
        /// <param name="width">Width of the panel</param>
        /// <param name="height">Height of the panel</param>
        public TowerPanel(ContentManager content, SpriteFont font, Vector2 position, int width, int height)
        {
            this.content = content;
            this.font = font;
            this.position = position;
            this.width = width;
            this.height = height;

            // Load graphical content
            LoadContent();

            // Instantiate buttons
            int buttonHeight = 45;
            damageUpgradeButton = new GUIButton(damageUpgradeButtons[0], damageUpgradeButtons[1], damageUpgradeButtons[2], new Vector2(position.X + 730, position.Y + buttonHeight));
            radiusUpgradeButton = new GUIButton(radiusUpgradeButtons[0], radiusUpgradeButtons[1], radiusUpgradeButtons[2],
                new Vector2(position.X + 850, position.Y + buttonHeight));
            sellButton = new GUIButton(sellButtons[0], sellButtons[1], sellButtons[2], new Vector2(position.X + 1000, position.Y + buttonHeight));
        }

        private void LoadContent()
        {
            // Background image for the TowerPanel
            background = content.Load<Texture2D>("InfoPanel");

            // Buttons
            damageUpgradeButtons = new Texture2D[]
            {
                content.Load<Texture2D>("UpgradeDamageButtons/UpgradeDamageButtonNormal"),
                content.Load<Texture2D>("UpgradeDamageButtons/UpgradeDamageButtonHover"),
                content.Load<Texture2D>("UpgradeDamageButtons/UpgradeDamageButtonPressed"),
            };

            radiusUpgradeButtons = new Texture2D[]
            {
                content.Load<Texture2D>("UpgradeRadiusButtons/UpgradeRadiusButtonNormal"),
                content.Load<Texture2D>("UpgradeRadiusButtons/UpgradeRadiusButtonHover"),
                content.Load<Texture2D>("UpgradeRadiusButtons/UpgradeRadiusButtonPressed"),
            };

            sellButtons = new Texture2D[]
            {
                content.Load<Texture2D>("SellButtons/SellButtonNormal"),
                content.Load<Texture2D>("SellButtons/SellButtonHover"),
                content.Load<Texture2D>("SellButtons/SellButtonPressed"),
            };

            // Avatar
            avatarBackground = content.Load<Texture2D>("AvatarBackground");
        }

        /// <summary>
        /// Update a TowerPanel
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            damageUpgradeButton.Update(gameTime);
            radiusUpgradeButton.Update(gameTime);
            sellButton.Update(gameTime);
        }

        /// <summary>
        /// Draws a TowerPanel
        /// </summary>
        /// <param name="spriteBatch">spriteBatch passed from Player</param>
        /// <param name="currentTower">Tower selected during game play</param>
        public void Draw(SpriteBatch spriteBatch, Tower currentTower)
        {
            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            string towerType = string.Format("Tower Type: {0}", currentTower.TowerType);
            string damageLevel = string.Format("Damage Level: {0}", currentTower.DamageUpgradeNumber);
            string radiusLevel = string.Format("Radius Level: {0}", currentTower.RadiusUpgradeNumber);
            string totalWorth = string.Format("Total Worth: {0}", currentTower.TotalWorth);
            string range;
            int radius = (((int)currentTower.Radius - (Util.tileSize / 2)) / Util.tileSize);
            if (radius == 1)
            {
                range = string.Format("Range: {0}", (currentTower.Radius - (Util.tileSize / 2)) / Util.tileSize) + " unit";
            }
            else
            {
                range = string.Format("Range: {0}", (currentTower.Radius - (Util.tileSize / 2)) / Util.tileSize) + " units";
            }
            string damage = string.Format("Damage: {0}", currentTower.Damage);
            string fireRateSpeed;
            if (currentTower.TowerType == "Basic")
            {
                if (currentTower.BoostModifier == 0f)
                {
                    fireRateSpeed = "2 shots/sec";
                }
                else
                {
                    fireRateSpeed = "2 shots/0.5 sec";
                }
            }
            else if (currentTower.TowerType == "Fire")
            {
                if (currentTower.BoostModifier == 0f)
                {
                    fireRateSpeed = "1 shot/sec";
                }
                else
                {
                    fireRateSpeed = "1 shot/0.5 sec";
                }
            }
            else if (currentTower.TowerType == "Freeze")
            {
                if (currentTower.BoostModifier == 0f)
                {
                    fireRateSpeed = "Radius/3 sec";
                }
                else
                {
                    fireRateSpeed = "Radius/1.5 sec";
                }
            }
            else if (currentTower.TowerType == "Pulse")
            {
                if (currentTower.BoostModifier == 0f)
                {
                    fireRateSpeed = "Radius/5 sec";
                }
                else
                {
                    fireRateSpeed = "Radius/2.5 sec";
                }
            }
            else if (currentTower.TowerType == "Mortar")
            {
                if (currentTower.BoostModifier == 0f)
                {
                    fireRateSpeed = "1 shots/5 sec";
                }
                else
                {
                    fireRateSpeed = "1 shots/2.5 sec";
                }
            }
            else
            {
                fireRateSpeed = "";
            }

            string fireRate = string.Format("Fire Rate: {0}", fireRateSpeed);
            string upgrade = "Upgrades";
            string sell = "Sell";
            string sellPrice = string.Format("+{0}", (int)(currentTower.TotalWorth * .75));

            int baseDamage = 0;
            int baseRadius = 0;

            if (currentTower.TowerType == "Basic")
            {
                baseDamage = Util.basicTowerCost;
                baseRadius = Util.basicTowerCost;
            }
            else if (currentTower.TowerType == "Pulse")
            {
                baseDamage = Util.pulseTowerCost;
                baseRadius = Util.pulseTowerCost;
            }
            else if (currentTower.TowerType == "Freeze")
            {
                baseDamage = Util.freezeTowerCost;
                baseRadius = Util.freezeTowerCost;
            }
            else if (currentTower.TowerType == "Fire")
            {
                baseDamage = Util.fireTowerCost;
                baseRadius = Util.fireTowerCost;
            }
            else if (currentTower.TowerType == "Mortar")
            {
                baseDamage = Util.mortarTowerCost;
                baseRadius = Util.mortarTowerCost;
            }

            if (currentTower.DamageUpgradeNumber == 0)
            {
                baseDamage = (int)(baseDamage * .25);
            }
            else if (currentTower.DamageUpgradeNumber == 1)
            {
                baseDamage = (int)(baseDamage * .5);
            }
            if (currentTower.RadiusUpgradeNumber == 0)
            {
                baseRadius = (int)(baseRadius * .25);
            }
            else if (currentTower.RadiusUpgradeNumber == 1)
            {
                baseRadius = (int)(baseRadius * .5);
            }

            int col1 = 20;
            int col2 = 260;
            int col3 = 460;
            int col4 = 730;
            int col5 = 1000;
            int col6 = 1140;

            if (currentTower.DamageUpgradeNumber < 3)
            {
                if(damageUpgradeButton.State == ButtonStatus.Disabled)
                    damageUpgradeButton.State = ButtonStatus.Normal;
                string damagePrice = string.Format("-{0}", baseDamage);
                spriteBatch.DrawString(font, damagePrice, new Vector2(position.X + 730 + 30, position.Y + 90), Color.Chartreuse);
                damageUpgradeButton.Draw(spriteBatch);
            }

            if (currentTower.RadiusUpgradeNumber < 3)
            {
                if (radiusUpgradeButton.State == ButtonStatus.Disabled)
                    radiusUpgradeButton.State = ButtonStatus.Normal;
                string radiusPrice = string.Format("-{0}", baseRadius);
                spriteBatch.DrawString(font, radiusPrice, new Vector2(position.X + col4 + 150, position.Y + 90), Color.Chartreuse);
                radiusUpgradeButton.Draw(spriteBatch);
            }
            if (currentTower.RadiusUpgradeNumber == 3 && !(currentTower.DamageUpgradeNumber == 3))
            {
                radiusUpgradeButton.State = ButtonStatus.Disabled;
                string max = "Max";
                string maxRadius = "Radius";
                spriteBatch.DrawString(font, max, new Vector2(position.X + col4 + 146, position.Y + 45), Color.Chartreuse);
                spriteBatch.DrawString(font, maxRadius, new Vector2(position.X + col4 + 135, position.Y + 70), Color.Chartreuse);
            }
            if (currentTower.DamageUpgradeNumber == 3 && !(currentTower.RadiusUpgradeNumber == 3))
            {
                damageUpgradeButton.State = ButtonStatus.Disabled;
                string max = "Max";
                string maxDamage = "Damage";
                spriteBatch.DrawString(font, max, new Vector2(position.X + col4 + 25, position.Y + 45), Color.Chartreuse);
                spriteBatch.DrawString(font, maxDamage, new Vector2(position.X + col4 + 8, position.Y + 70), Color.Chartreuse);
            }
            if (!(currentTower.DamageUpgradeNumber == 3 && currentTower.RadiusUpgradeNumber == 3))
            {
                spriteBatch.DrawString(font, upgrade, new Vector2(position.X + col4 + 56, position.Y + 20), Color.Chartreuse);
            }
            else
            {
                damageUpgradeButton.State = ButtonStatus.Disabled;
                radiusUpgradeButton.State = ButtonStatus.Disabled;
                string fullyUpgraded = "Fully Upgraded";
                spriteBatch.DrawString(font, fullyUpgraded, new Vector2(position.X + col4 + 24, position.Y + 55), Color.Chartreuse);
            }

            string avatarText = "Avatar";

            spriteBatch.DrawString(font, towerType, new Vector2(position.X + col1, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, damageLevel, new Vector2(position.X + col2, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, radiusLevel, new Vector2(position.X + col2, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, totalWorth, new Vector2(position.X + col1, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, damage, new Vector2(position.X + col3, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, range, new Vector2(position.X + col3, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, fireRate, new Vector2(position.X + col3, position.Y + 90), Color.Chartreuse);
            spriteBatch.DrawString(font, sell, new Vector2(position.X + col5 + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, sellPrice, new Vector2(position.X + col5 + 20, position.Y + 90), Color.Chartreuse);
            spriteBatch.DrawString(font, avatarText, new Vector2(position.X + col6, position.Y + 20), Color.Chartreuse);

            Rectangle avatarBounds = new Rectangle((int)position.X + col6 + 14, (int)position.Y + 54, Util.tileWidth, Util.tileHeight);
            Rectangle backgroundBounds = new Rectangle((int)position.X + col6 + 8, (int)position.Y + 48, 60, 60);
            spriteBatch.Draw(avatarBackground, backgroundBounds, Color.White);
            spriteBatch.Draw(currentTower.Texture, avatarBounds, Color.White);

            sellButton.Draw(spriteBatch);
        }

        public void DrawPreview(SpriteBatch spriteBatch, String towerType, int cost, int radius, int damage)
        {
            spriteBatch.Draw(background, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            string towerTypeText = string.Format("Tower Type: {0}", towerType);
            string priceText = string.Format("Price: {0}", cost);
            int range = ((radius - (Util.tileSize / 2)) / Util.tileSize);
            string rangeText;
            if (range == 1)
            {
                rangeText = string.Format("Range: {0}", range + " unit");
            }
            else
            {
                rangeText = string.Format("Range: {0}", range + " units");
            }
            string damageText = string.Format("Damage: {0}", damage);
            string fireRateSpeed;
            if (towerType == "Basic")
            {
                fireRateSpeed = "2 shots/sec";
            }
            else if (towerType == "Fire")
            {
                fireRateSpeed = "1 shot/sec";
            }
            else if (towerType == "Freeze")
            {
                fireRateSpeed = "Radius/3 sec";
            }
            else if (towerType == "Pulse")
            {
                fireRateSpeed = "Radius/5 sec";
            }
            else if (towerType == "Mortar")
            {
                fireRateSpeed = "1 shot/5 sec";
            }
            else
            {
                fireRateSpeed = "";
            }
            string fireRateText = string.Format("Fire Rate: {0}", fireRateSpeed);

            spriteBatch.DrawString(font, towerTypeText, new Vector2(position.X + 20, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, priceText, new Vector2(position.X + 20, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, damageText, new Vector2(position.X + 460, position.Y + 20), Color.Chartreuse);
            spriteBatch.DrawString(font, rangeText, new Vector2(position.X + 460, position.Y + 55), Color.Chartreuse);
            spriteBatch.DrawString(font, fireRateText, new Vector2(position.X + 460, position.Y + 90), Color.Chartreuse);
        }
    }
}
