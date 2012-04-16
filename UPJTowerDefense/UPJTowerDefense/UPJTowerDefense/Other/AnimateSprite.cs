using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UPJTowerDefense
{
    class AnimatedSprite
    {
        // Sprite texture
        private Texture2D texture;

        // How long each frame is displayed
        private float frameRate = 0.02f;

        // Elapsed time since the frame was last advanced
        private float elapsedTime = 0.0f;

        // Upperleft x coordinate of the first frame
        private int frameOffsetX = 0;

        // Upperleft y coordinate of the first frame
        private int frameOffsetY = 0;

        // Width of the frame
        private int frameWidth;

        // Height of the frame
        private int frameHeight;

        // Total number of frames in the animation
        private int frameCount = 1;

        // Current frame that is being displayed
        private int currentFrame = 0;

        // X coordinate of the on screen sprite
        private int screenX = 0;

        // Y coordinate of the on screen sprite
        private int screenY = 0;

        // Will the sprite automatically update
        private bool animating = true;

        public int X
        {
            get { return screenX; }
            set { screenX = value; }
        }

        public int Y
        {
            get { return screenY; }
            set { screenY = value; }
        }

        public int Frame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frameCount); }
        }

        public float FrameLength
        {
            get { return frameRate; }
            set { frameRate = (float)Math.Max(value, 0f); }
        }

        public bool IsAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public AnimatedSprite(Texture2D texture, int frameOffsetX, int frameOffsetY, int frameWidth,
            int frameHeight, int frameCount)
        {
            this.texture = texture;
            this.frameOffsetX = frameOffsetX;
            this.frameOffsetY = frameOffsetY;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameCount = frameCount;
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(frameOffsetX + (frameWidth * currentFrame), frameOffsetY, frameWidth, frameHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (animating)
            {
                elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (elapsedTime > frameRate)
                {
                    currentFrame = (currentFrame + 1) % frameCount;
                    elapsedTime = 0.0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset, bool needBeginEnd)
        {
            if (needBeginEnd)
            {
                spriteBatch.Begin();
            }

            spriteBatch.Draw(texture, new Rectangle(screenX + xOffset, screenY + yOffset, frameWidth, frameHeight),
                GetSourceRect(), Color.White);

            if (needBeginEnd)
            {
                spriteBatch.End();
            }
        }
    }
}
