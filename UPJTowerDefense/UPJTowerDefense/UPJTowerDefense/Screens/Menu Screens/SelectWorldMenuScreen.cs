using Microsoft.Xna.Framework;

namespace UPJTowerDefense
{
    class SelectWorldMenuScreen : MenuScreen
    {
        MenuEntry selectWorldMenuEntry;

        static int worldNumber = Options.worldNumber;

        public SelectWorldMenuScreen()
            : base("Select World")
        {
            selectWorldMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            selectWorldMenuEntry.Selected += SelectWorldMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(selectWorldMenuEntry);
            MenuEntries.Add(back);
        }

        void SetMenuEntryText()
        {
            selectWorldMenuEntry.Text = "World #: " + worldNumber;
        }

        void SelectWorldMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (worldNumber != Options.worldNumber)
            {
                worldNumber = Options.worldNumber;
            }

            worldNumber++;
            Options.worldNumber++;

            if (worldNumber > Options.numberOfWorlds)
            {
                worldNumber = 1;
                Options.worldNumber = 1;
            }

            SetMenuEntryText();
        }
    }
}
