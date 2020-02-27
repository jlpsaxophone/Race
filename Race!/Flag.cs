using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Race_
{
    public class Flag
    {
        public BoundingRectangle bounds;

        Race game;

        Sprite sprite;

        public BoundingRectangle Bounds => bounds;

        public Flag(BoundingRectangle bounds, Sprite sprite, Race game)
        {
            this.bounds.X = bounds.X;
            this.bounds.Y = bounds.Y;
            this.bounds.Height = bounds.Height;
            this.bounds.Width = bounds.Width;
            this.sprite = sprite;
            this.game = game;
        }

        public void Update()
        {
            if(game.player1.Bounds.CollidesWith(this.Bounds)) {
                game.player2.dead = true;
                AnnounceVictory();
            }
            if (game.player2.Bounds.CollidesWith(this.Bounds))
            {
                game.player1.dead = true;
                AnnounceVictory();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, new Vector2(bounds.X, bounds.Y), Color.White);
        }

        public void AnnounceVictory()
        {
            if(game.player1.dead == true)
            {
                game.player2victory = true;
                game.end = true;
            }
            if(game.player2.dead == true)
            {
                game.player1victory = true;
                game.end = true;
            }
        }
    }
}
