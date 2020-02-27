using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Race_
{
    public class Hazard : IBoundable
    {
        public BoundingRectangle bounds;

        Sprite sprite;

        public BoundingRectangle Bounds => bounds;

        public Hazard(BoundingRectangle bounds, Sprite sprite)
        {
            this.bounds.X = bounds.X;
            this.bounds.Y = bounds.Y;
            this.bounds.Height = bounds.Height;
            this.bounds.Width = bounds.Width;
            this.sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, new Vector2(bounds.X, bounds.Y), Color.White);
        }
    }
}
