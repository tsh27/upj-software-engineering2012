using Microsoft.Xna.Framework;

namespace UPJTowerDefense
{
    class CheatOptionsMenuScreen : MenuScreen
    {
        MenuEntry livesCheatMenuEntry;
        MenuEntry moneyCheatMenuEntry;

        static bool livesCheatOn = false;
        static bool moneyCheatOn = false;

        public CheatOptionsMenuScreen()
            : base("Cheat Options")
        {
            livesCheatMenuEntry = new MenuEntry(string.Empty);
            moneyCheatMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            livesCheatMenuEntry.Selected += LivesCheatMenuEntrySelected;
            moneyCheatMenuEntry.Selected += MoneyCheatMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(livesCheatMenuEntry);
            MenuEntries.Add(moneyCheatMenuEntry);
            MenuEntries.Add(back);
        }

        void SetMenuEntryText()
        {
            livesCheatMenuEntry.Text = "Lives Cheat: " + (livesCheatOn ? "on" : "off");
            moneyCheatMenuEntry.Text = "Money Cheat: " + (moneyCheatOn ? "on" : "off");
        }

        void LivesCheatMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            livesCheatOn = !livesCheatOn;
            Options.livesCheatOn = !Options.livesCheatOn;
            SetMenuEntryText();
        }

        void MoneyCheatMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            moneyCheatOn = !moneyCheatOn;
            Options.moneyCheatOn = !Options.moneyCheatOn;
            SetMenuEntryText();
        }
    }
}
