using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Race_
{
    public class Race : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont text;
        SpriteSheet sheet;
        public Vector2 offset = new Vector2(0,0);
        public Matrix t;
        public Player player1;
        public Player player2;

        public List<Platform> platforms;
        public List<Hazard> hazards;
        public Flag victory;
        public bool end = false;
        public bool player1victory = false;
        public bool player2victory = false;

        public Race()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            platforms = new List<Platform>();
            hazards = new List<Hazard>();
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 814;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var t = Content.Load<Texture2D>("spritesheet");
            text = Content.Load<SpriteFont>("text");
            sheet = new SpriteSheet(t, 21, 21, 3, 2);
            var playerFrames1 = from index in Enumerable.Range(19, 30) select sheet[index];
            var playerFrames2 = from index in Enumerable.Range(49, 60) select sheet[index];
            player1 = new Player(playerFrames1, this, 1);
            player2 = new Player(playerFrames2, this, 2);

            platforms.Add(new Platform(new BoundingRectangle(0, 459, 189, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(0, 333, 189, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(231, 390, 105, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(231, 270, 105, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(380, 333, 105, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(780, 459, 231, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1095, 390, 105, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1053, 285, 84, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1158, 180, 84, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1242, 459, 105, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1389, 390, 84, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1557, 459, 63, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1599, 375, 84, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1557, 270, 21, 21), sheet[122]));
            platforms.Add(new Platform(new BoundingRectangle(1557, 165, 21, 21), sheet[122]));

            platforms.Add(new Platform(new BoundingRectangle(1725, 459, 231, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(1725, 274, 252, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(1725, 85, 231, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2040, 459, 84, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2040, 64, 84, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2124, 354, 105, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2124, 169, 105, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2313, 459, 105, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2313, 274, 105, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2502, 232, 21, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2523, 459, 21, 21), sheet[1]));
            platforms.Add(new Platform(new BoundingRectangle(2523, 354, 21, 21), sheet[1]));

            platforms.Add(new Platform(new BoundingRectangle(2710, 459, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(2710, 354, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(2626, 211, 105, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(2815, 459, 126, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(2836, 375, 168, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(3046, 459, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(3046, 354, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(3109, 249, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(3088, 144, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(2962, 123, 63, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(2878, 144, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(2983, 207, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(3172, 375, 21, 21), sheet[304]));
            platforms.Add(new Platform(new BoundingRectangle(3256, 459, 126, 21), sheet[122]));

            hazards.Add(new Hazard(new BoundingRectangle(422, 312, 21, 10), sheet[70]));
            hazards.Add(new Hazard(new BoundingRectangle(822, 438, 21, 10), sheet[70]));
            hazards.Add(new Hazard(new BoundingRectangle(1242, 438, 21, 10), sheet[70]));
            hazards.Add(new Hazard(new BoundingRectangle(1326, 438, 21, 10), sheet[70]));           
            hazards.Add(new Hazard(new BoundingRectangle(1851, 64, 21, 21), sheet[18]));
            hazards.Add(new Hazard(new BoundingRectangle(1851, 438, 21, 21), sheet[18]));
            hazards.Add(new Hazard(new BoundingRectangle(1767, 438, 21, 21), sheet[18]));
            hazards.Add(new Hazard(new BoundingRectangle(1830, 253, 21, 21), sheet[18]));
            hazards.Add(new Hazard(new BoundingRectangle(2355, 438, 21, 21), sheet[18]));
            hazards.Add(new Hazard(new BoundingRectangle(2313, 253, 21, 21), sheet[18]));
            hazards.Add(new Hazard(new BoundingRectangle(2397, 253, 21, 21), sheet[18]));
            hazards.Add(new Hazard(new BoundingRectangle(2878, 354, 21, 21), sheet[316]));
            hazards.Add(new Hazard(new BoundingRectangle(2941, 354, 21, 21), sheet[316]));
            hazards.Add(new Hazard(new BoundingRectangle(2962, 102, 21, 21), sheet[316]));
            hazards.Add(new Hazard(new BoundingRectangle(2983, 102, 21, 21), sheet[316]));
            hazards.Add(new Hazard(new BoundingRectangle(3004, 102, 21, 21), sheet[316]));

            victory = new Flag(new BoundingRectangle(3361, 438, 21, 21), sheet[310], this);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            if (end == false)
            {               
                player1.Update(gameTime);
                player2.Update(gameTime);
                player1.CheckForPlatformCollision(platforms);
                player2.CheckForPlatformCollision(platforms);
                player1.CheckForHazardCollision(hazards);
                player2.CheckForHazardCollision(hazards);
                victory.Update();

                base.Update(gameTime);
            }           
        }
        
        protected override void Draw(GameTime gameTime)
        {          
            if(end == true)
            {
                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.CornflowerBlue);
                if(player1victory == true)
                {
                spriteBatch.DrawString(
                text,
                "Player 1 Wins!",
                new Vector2(360, 219),
                Color.Black
                );
                }
                if(player2victory == true)
                {
                spriteBatch.DrawString(
                text,
                "Player 2 Wins!",
                new Vector2(360, 219),
                Color.Black
                );
                }
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                offset = new Vector2(-(float)gameTime.TotalGameTime.TotalMilliseconds / 15, 0);
                t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
                player1.Draw(spriteBatch);
                player2.Draw(spriteBatch);
                platforms.ForEach(platform =>
                {
                    platform.Draw(spriteBatch);
                });
                hazards.ForEach(hazard =>
                {
                    hazard.Draw(spriteBatch);
                });
                victory.Draw(spriteBatch);
                base.Draw(gameTime);
                spriteBatch.End();
            }
        }
    }
}
