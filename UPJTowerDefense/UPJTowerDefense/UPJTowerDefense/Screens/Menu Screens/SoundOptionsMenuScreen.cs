using Microsoft.Xna.Framework;

namespace UPJTowerDefense
{
    class SoundOptionsMenuScreen : MenuScreen
    {
        MenuEntry musicOnMenuEntry;
        MenuEntry soundEffectsMenuEntry;

        static bool musicOn = true;
        static bool soundEffectsOn = true;
        
        public SoundOptionsMenuScreen()
            : base("Sound Options")
        {
            musicOnMenuEntry = new MenuEntry(string.Empty);
            soundEffectsMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            musicOnMenuEntry.Selected += SoundOnMenuEntrySelected;
            soundEffectsMenuEntry.Selected += SoundEffectsMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(musicOnMenuEntry);
            MenuEntries.Add(soundEffectsMenuEntry);
            MenuEntries.Add(back);
        }

        void SetMenuEntryText()
        {
            musicOnMenuEntry.Text = "Music: " + (musicOn ? "on" : "off");
            soundEffectsMenuEntry.Text = "Sound Effects: " + (soundEffectsOn ? "on" : "off");
        }

        void SoundOnMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicOn = !musicOn;
            Options.musicOn = !Options.musicOn;
            SetMenuEntryText();
        }

        void SoundEffectsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            soundEffectsOn = !soundEffectsOn;
            Options.soundEffectsOn = !Options.soundEffectsOn;
            SetMenuEntryText();
        }
    }
}
