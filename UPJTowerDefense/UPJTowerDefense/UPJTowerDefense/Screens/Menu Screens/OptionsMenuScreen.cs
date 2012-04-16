#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace UPJTowerDefense
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields
        MenuEntry soundMenuEntry;
        MenuEntry cheatsMenuEntry;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            soundMenuEntry = new MenuEntry("Sound");
            cheatsMenuEntry = new MenuEntry("Cheats");
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            soundMenuEntry.Selected += SoundMenuEntrySelected;
            cheatsMenuEntry.Selected += CheatsMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(soundMenuEntry);
            MenuEntries.Add(cheatsMenuEntry);
            MenuEntries.Add(back);
        }


        #endregion

        #region Handle Input

        void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SoundOptionsMenuScreen(), e.PlayerIndex);
        }

        void CheatsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CheatOptionsMenuScreen(), e.PlayerIndex);
        }

        #endregion
    }
}
