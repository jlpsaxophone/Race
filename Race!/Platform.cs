using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Race_
{
    public class Platform : IBoundable
    {
        public BoundingRectangle bounds;

        Sprite sprite;

        int tileCount;

        public BoundingRectangle Bounds => bounds;

        public Platform(BoundingRectangle bounds, Sprite sprite)
        {
            this.bounds = bounds;
            this.sprite = sprite;
            tileCount = (int)bounds.Width / sprite.Width;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tileCount; i++)
            {
                sprite.Draw(spriteBatch, new Vector2(bounds.X + i * sprite.Width, bounds.Y), Color.White);
            }
        }
    }
}
