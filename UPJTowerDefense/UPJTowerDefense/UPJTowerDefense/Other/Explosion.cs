using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UPJTowerDefense
{
    class Explosion
    {
        private AnimatedSprite sprite;
        private int x = 0;
        private int y = 0;
        bool active = true;
        bool done = false;
        int frames;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool IsActive
        {
            get { return active; }
        }

        public bool IsDone
        {
            get { return done; }
        }

        public Explosion(Texture2D texture, int x, int y, int w, int h, int frames)
        {
            sprite = new AnimatedSprite(texture, x, y, w, h, frames);
            sprite.FrameLength = 0.02f;
            active = false;
            done = false;
            this.frames = frames;
        }

        public void Activate(int x, int y)
        {
            this.x = x;
            this.y = y;
            sprite.Frame = 0;
            active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
                sprite.Update(gameTime);
                if (sprite.Frame >= frames - 1)
                {
                    active = false;
                    done = true;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (active)
            {
                sprite.Draw(batch, x, y, false);
            }
        }
    }
}
