using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace UPJTowerDefense
{
    public class TowerBoostSpell : Spell
    {   
        /// <summary>
        /// Constructs a TowerBoostSpell
        /// </summary>

        public TowerBoostSpell(Texture2D texture, Vector2 position, String spellType, SoundEffect spellSound)
            : base(texture, position, spellType)
        {
            this.cost = Util.boostSpellCost;
            this.radius = Util.boostSpellRadius;
            this.spellSound = spellSound;
        }

        /// <summary>
        /// Updates a TowerBoostSpell
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

                foreach (Tower tower in allTowers)
                {
                    tower.BoostModifier = Util.boostSpellModifier;
                    tower.BoostModifierDuration = Util.boostSpellDuration;
                }

                spellUsed = true;
            }
        }
    }
}
