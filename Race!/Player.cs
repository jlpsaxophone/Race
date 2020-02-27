using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Race_
{
    enum PlayerAnimState
    {
        Idle,
        JumpingLeft,
        JumpingRight,
        WalkingLeft,
        WalkingRight,
        FallingLeft,
        FallingRight
    }

    enum VerticalMovementState
    {
        OnGround,
        Jumping,
        Falling
    }
    public class Player
    {
        // The speed of the walking animation
        const int FRAME_RATE = 300;

        // The duration of a player's jump, in milliseconds
        const int JUMP_TIME = 500;

        // The player sprite frames
        Sprite[] frames;

        // The currently rendered frame
        int currentFrame = 0;

        // The player's animation state
        PlayerAnimState animationState = PlayerAnimState.Idle;

        // The player's speed
        int speed = 3;

        // The player's vertical movement state
        VerticalMovementState verticalState = VerticalMovementState.OnGround;

        // A timer for jumping
        TimeSpan jumpTimer;

        // A timer for animations
        TimeSpan animationTimer;

        // The currently applied SpriteEffects
        SpriteEffects spriteEffects = SpriteEffects.None;

        // The color of the sprite
        Color color = Color.White;

        // The origin of the sprite (centered on its feet)
        Vector2 origin = new Vector2(10, 21);
        /// <summary>
        /// Gets and sets the position of the player on-screen
        /// </summary>
        public Vector2 Position = new Vector2(200, 200);
        public BoundingRectangle Bounds => new BoundingRectangle(Position - 2 * origin, 38, 41);

        int playerType;
        Keys left;
        Keys right;
        Keys jump;

        Race game;

        public bool dead = false;

        public Player(IEnumerable<Sprite> frames, Race game, int playerType)
        {
            this.frames = frames.ToArray();
            this.playerType = playerType;
            this.game = game;
            animationState = PlayerAnimState.WalkingLeft;
            if (playerType == 1)
            {
                Position.X = 20;
                Position.Y = 285;
                left = Keys.A;
                right = Keys.D;
                jump = Keys.W;
            }
            else if (playerType == 2)
            {
                Position.X = 20;
                Position.Y = 430;
                left = Keys.Left;
                right = Keys.Right;
                jump = Keys.Up;
            }
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if(Position.Y > 522)
            {
                dead = true;
                game.victory.AnnounceVictory();
            }

            if (Position.X < -game.offset.X)
            {
                Position.X = -game.offset.X + 12;
            }
            if (Position.X > game.graphics.PreferredBackBufferWidth - game.offset.X)
            {
                Position.X = game.graphics.PreferredBackBufferWidth - game.offset.X;
            }
                // Vertical movement
                switch (verticalState)
                {
                    case VerticalMovementState.OnGround:
                        if (keyboard.IsKeyDown(jump))
                        {
                            verticalState = VerticalMovementState.Jumping;
                            jumpTimer = new TimeSpan(0);
                        }
                        break;
                    case VerticalMovementState.Jumping:
                        jumpTimer += gameTime.ElapsedGameTime;
                        // Simple jumping with platformer physics
                        Position.Y -= (450 / (float)jumpTimer.TotalMilliseconds);
                        if (jumpTimer.TotalMilliseconds >= JUMP_TIME) verticalState = VerticalMovementState.Falling;
                        break;
                    case VerticalMovementState.Falling:
                        Position.Y += speed;
                        break;
                }


                // Horizontal movement
                if (keyboard.IsKeyDown(left))
                {
                    if (verticalState == VerticalMovementState.Jumping || verticalState == VerticalMovementState.Falling)
                        animationState = PlayerAnimState.JumpingLeft;
                    else animationState = PlayerAnimState.WalkingLeft;
                    Position.X -= speed;
                }
                else if (keyboard.IsKeyDown(right))
                {
                    if (verticalState == VerticalMovementState.Jumping || verticalState == VerticalMovementState.Falling)
                        animationState = PlayerAnimState.JumpingRight;
                    else animationState = PlayerAnimState.WalkingRight;
                    Position.X += speed;
                }
                else
                {
                    animationState = PlayerAnimState.Idle;
                }

                // Apply animations
                switch (animationState)
                {
                    case PlayerAnimState.Idle:
                        currentFrame = 0;
                        animationTimer = new TimeSpan(0);
                        break;

                    case PlayerAnimState.JumpingLeft:
                        spriteEffects = SpriteEffects.FlipHorizontally;
                        currentFrame = 7;
                        break;

                    case PlayerAnimState.JumpingRight:
                        spriteEffects = SpriteEffects.None;
                        currentFrame = 7;
                        break;

                    case PlayerAnimState.WalkingLeft:
                        animationTimer += gameTime.ElapsedGameTime;
                        spriteEffects = SpriteEffects.FlipHorizontally;
                        // Walking frames are 9 & 10
                        if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                        {
                            animationTimer = new TimeSpan(0);
                        }
                        currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                        break;

                    case PlayerAnimState.WalkingRight:
                        animationTimer += gameTime.ElapsedGameTime;
                        spriteEffects = SpriteEffects.None;
                        // Walking frames are 9 & 10
                        if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                        {
                            animationTimer = new TimeSpan(0);
                        }
                        currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 9;
                        break;

            }            
        }

        public void CheckForPlatformCollision(IEnumerable<IBoundable> platforms)
        {
            if (verticalState != VerticalMovementState.Jumping)
            {
                verticalState = VerticalMovementState.Falling;
                foreach (Platform platform in platforms)
                {
                    if (Bounds.CollidesWith(platform.Bounds))
                    {
                        Position.Y = platform.Bounds.Y - 1;
                        verticalState = VerticalMovementState.OnGround;
                    }
                }
            }
        }

        public void CheckForHazardCollision(IEnumerable<IBoundable> hazards)
        {
                foreach (Hazard hazard in hazards)
                {
                    if (Bounds.CollidesWith(hazard.Bounds))
                    {
                        dead = true;
                    game.victory.AnnounceVictory();
                    }
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }
    }
}
