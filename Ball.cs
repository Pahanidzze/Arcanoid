using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arcanoid
{
    class Ball
    {
        private static readonly Texture ballTexture = new Texture("Ball.png");

        public Sprite sprite = new Sprite(ballTexture)
        {
            Position = position
        };
        public float speed = 1;
        public float direction = 7 * (float)Math.PI / 4;
        public static Vector2f defaultPosition = new Vector2f(5, 5);

        private static Vector2f position = defaultPosition;
        private static Vector2i size = new Vector2i(25, 25);
        private static bool wallStacked = false;
        private static bool racketStacked = false;

        public Ball(Vector2i ballCenterPosition, float speed, float direction)
        {
            defaultPosition = (Vector2f)(ballCenterPosition - size / 2);
            position = defaultPosition;
            sprite.Position = position;
            this.speed = speed;
            this.direction = direction;
        }

        public void ResetPosition()
        {
            position = defaultPosition;
            sprite.Position = position;
            direction = -direction;
        }

        public void Move()
        {
            position.X = (float)(position.X + Math.Cos(direction) * speed);
            position.Y = (float)(position.Y + Math.Sin(direction) * speed);
            sprite.Position = position;
            // Console.WriteLine($"[MOVE] X: {Math.Cos(direction) * speed}, Y: {Math.Sin(direction) * speed}");
        }

        public bool CheckCollision(Window window, Game.Racket racket, Game.Block[] blocks)
        {
            bool loseStatus = false;
            if (position.X <= 0)
            {
                if (!wallStacked)
                {
                    Console.Write($"[LEFT WALL] : last direction: {direction}, ");
                    direction = ((float)Math.PI - direction) % (2 * (float)Math.PI);
                    if (direction < 0) direction += 2 * (float)Math.PI;
                    wallStacked = true;
                    Console.WriteLine($"new direction: {direction}, X: {position.X}, Y: {position.Y}");
                }
            }
            else if (position.X + size.X >= window.Size.X)
            {
                if (!wallStacked)
                {
                    Console.Write($"[RIGHT WALL] : last direction: {direction}, ");
                    direction = ((float)Math.PI - direction) % (2 * (float)Math.PI);
                    if (direction < 0) direction += 2 * (float)Math.PI;
                    wallStacked = true;
                    Console.WriteLine($"new direction: {direction}, X: {position.X}, Y: {position.Y}");
                }
            }
            else if (position.Y <= 0)
            {
                if (!wallStacked)
                {
                    // Console.Write($"[UP WALL] : last direction: {direction}, ");
                    direction = -direction % (2 * (float)Math.PI) + 2 * (float)Math.PI;
                    wallStacked = true;
                    // Console.WriteLine($"new direction: {direction}, X: {position.X}, Y: {position.Y}");
                }
            }
            else if (position.Y + size.Y >= window.Size.Y)
            {
                loseStatus = true;
                ResetPosition();
            }
            else if (wallStacked)
            {
                wallStacked = false;
                // Console.WriteLine("[UNSTACKED]");
            }

            for (int i = 0; i < blocks.Length; i++)
            {
                if (position.X + size.X >= blocks[i].position.X && position.X <= blocks[i].position.X + blocks[i].size.X &&
                    position.Y + size.Y >= blocks[i].position.Y && position.Y <= blocks[i].position.Y + blocks[i].size.Y && blocks[i].HP > 0)
                {
                    if (!wallStacked)
                    {
                        Console.Write($"[BLOCK] : last direction: {direction}, ");
                        direction = -direction % (2 * (float)Math.PI) + 2 * (float)Math.PI;
                        blocks[i].HP--;
                        Console.WriteLine($"Change direction: {direction}");
                        Console.WriteLine($"new direction: {direction}, X: {position.X}, Y: {position.Y}");
                    }
                }
            }

            if (position.X + size.X >= racket.position.X && position.X <= racket.position.X + racket.size.X &&
                position.Y + size.Y >= racket.position.Y && position.Y + size.Y <= racket.position.Y + racket.size.Y)
            {
                if (!racketStacked)
                {
                    Console.Write($"[RACKET] : last direction: {direction / ((float)Math.PI / 6)} PI/6, ");
                    float layout = (position.X + size.X / 2 - racket.position.X) / (racket.size.X / 4f);
                    direction = (8 - direction / ((float)Math.PI / 6) + layout * 2) * (float)Math.PI / 6;
                    if (direction < 7 * (float)Math.PI / 6)
                    {
                        direction = 7 * (float)Math.PI / 6;
                    }
                    else if (direction > 11 * (float)Math.PI / 6)
                    {
                        direction = 11 * (float)Math.PI / 6;
                    }
                    racketStacked = true;
                    Console.WriteLine($"new direction: {direction / ((float)Math.PI / 6)} PI/6, X: {position.X}, Y: {position.Y}");
                    Console.WriteLine($"LAYOUT: {layout}");
                }
            }
            else if (racketStacked)
            {
                racketStacked = false;
                // Console.WriteLine("[UNSTACKED]");
            }

            return loseStatus;
        }
    }
}