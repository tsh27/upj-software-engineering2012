using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPJTowerDefense
{
    public class Options
    {
        // Sound Options
        public static bool musicOn = true;
        public static bool soundEffectsOn = true;

        // Cheat options
        public static bool livesCheatOn = false;
        public static bool moneyCheatOn = false;

        // Worlds
        public const int numberOfWorlds = 3;
        public static int worldNumber = 1;
        public static bool willChangeWorld = false;

        //Rounds
        public static bool inRound = false;
    }
}
