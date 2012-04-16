using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UPJTowerDefense
{
    public class Player
    {
        private int lives;
        private int money;

        public int Money
        {
            get { return money; }
            set { money = value; }
        }
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        /// <summary>
        /// Construct a new player.
        /// </summary>
        public Player(int lives, int money)
        {
            this.lives = lives;
            this.money = money;
        }
    }
}