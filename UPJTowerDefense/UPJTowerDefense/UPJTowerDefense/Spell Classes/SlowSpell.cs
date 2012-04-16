using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class SlowSpell : Spell
    {
        /// <summary>
        /// Constructs a SlowSpell
        /// </summary>
        public SlowSpell(Texture2D texture, Vector2 position, String spellType, SoundEffect spellSound)
            : base(texture, position, spellType)
        {
            this.cost = Util.slowSpellCost;
            this.radius = Util.slowSpellRadius;
            this.spellSound = spellSound;
        }

        /// <summary>
        /// Update a SlowSpell
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!spellUsed)
            {
                if (Options.soundEffectsOn)
                {
                    spellSound.Play(0.5f, 0f, 0f);
                }

                // Effect all enemies on screen
                foreach (Enemy enemy in enemiesInRange)
                {
                    enemy.SpeedModifier = Util.slowSpellModifier;
                    enemy.ModifierDuration = Util.slowSpellDuration;
                }

                spellUsed = true;
            }
        }
    }
}
