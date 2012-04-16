using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPJTowerDefense
{
    class Util
    {
        // Width and height of the map
        public const int xTiles = 20;
        public const int yTiles = 13;

        // Pixel width and height for a tile
        public const int tileSize = 48;
        public const int tileWidth = Util.tileSize;
        public const int tileHeight = Util.tileSize;

        // Pixel width and height for the map
        public const int mapWidth = xTiles * tileWidth;
        public const int mapHeight = yTiles * tileHeight;

        // Game Settings
        public const int screenWidth = 1280;
        public const int screenHeight = 760;
        public static bool gameOver = false;

        /* Enemy Stats */
        public const int workerHealth = 100;
        public const int swarmHealth = 50;
        public const int warriorHealth = 200;
        public const int webguardHealth = 300;
        public const int webbeastHealth = 1000;

        /* Tower Stats */
        // BasicTower initial stats
        public const string basicTowerType = "Basic";
        public const int basicTowerIndex = 0;
        public const int basicTowerCost = 200;
        public const int basicTowerDamage = 15;
        public const int basicTowerRadius = 72;

        // PulseTower initial stats
        public const string pulseTowerType = "Pulse";
        public const int pulseTowerIndex = 1;
        public const int pulseTowerCost = 250;
        public const int pulseTowerDamage = 100;
        public const int pulseTowerRadius = 72;

        // FreezeTower initial stats
        public const string freezeTowerType = "Freeze";
        public const int freezeTowerIndex = 2;
        public const int freezeTowerCost = 300;
        public const int freezeTowerDamage = 5;
        public const int freezeTowerRadius = 72;

        // FireTower initial stats
        public const string fireTowerType = "Fire";
        public const int fireTowerIndex = 3;
        public const int fireTowerCost = 600;
        public const int fireTowerDamage = 4;
        public const int fireTowerRadius = 72;

        // MortarTower initial stats
        public const string mortarTowerType = "Mortar";
        public const int mortarTowerIndex = 4;
        public const int mortarTowerCost = 400;
        public const int mortarTowerDamage = 100;
        public const int mortarTowerRadius = 168;

        /* Spell Stats */
        // Nuke Initial Stats
        public const string nukeSpellType = "Nuke";
        public const int nukeSpellIndex = 0;
        public const int nukeSpellCost = 1000;
        public const int nukeSpellDamage = 100;
        public const int nukeSpellRadius = 1000;

        // Slow Spell Initial Stats
        public const string slowSpellType = "Slow";
        public const int slowSpellIndex = 1;
        public const int slowSpellCost = 750;
        public const float slowSpellModifier = 0.5f;
        public const float slowSpellDuration = 5f;
        public const int slowSpellRadius = 1000;

        // Tower boost spell initial stats
        public const string boostSpellType = "Tower Boost";
        public const int boostSpellIndex = 2;
        public const int boostSpellCost = 500;
        public const int boostSpellModifier = 2;
        public const float boostSpellDuration = 5f;
        public const int boostSpellRadius = 1000;
    }
}