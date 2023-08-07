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
    class Game
    {

        private static RenderWindow window;
        private static readonly Texture blockTexture = new Texture("Block.png");
        private static readonly Texture hardBlockTexture = new Texture("HardBlock.png");
        private static readonly Texture racketTexture = new Texture("Racket.png");
        public struct Block
        {
            public Vector2i position;
            public Vector2i size;
            public int HP;
            public Sprite lightBlockSprite;
            public Sprite hardBlockSprite;
        }
        public struct Racket
        {
            public Vector2i position;
            public Vector2i size;
            public Sprite sprite;
        }
        private static Ball ball;

        public static void OpenGame(RenderWindow win, int mode)
        {
            window = win;
            int active = 1;
            Block[] blocks = new Block[0];
            if (mode == 1) blocks = InitEasyLevel();
            Racket racket = InitRacket();
            ball = new Ball(new Vector2i(racket.position.X + racket.size.X / 2, racket.position.Y - 50), 4 * mode, 7 * (float)Math.PI / 4);
            int attemptes = 3;
            while (window.IsOpen && active != 0 && attemptes > 0)
            {
                window.Clear();
                window.DispatchEvents();
                RacketProcessing(ref racket);
                if (ball.CheckCollision(window, racket, blocks) == true) attemptes--;
                ball.Move();
                active = 0;
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i].HP == 1)
                    {
                        window.Draw(blocks[i].lightBlockSprite);
                        active++;
                    }
                    else if (blocks[i].HP >= 2)
                    {
                        window.Draw(blocks[i].hardBlockSprite);
                        active++;
                    }
                }
                window.Draw(racket.sprite);
                window.Draw(ball.sprite);
                window.Display();
            }
        }

        private static Block[] InitEasyLevel()
        {
            int row = 9;
            Vector2i blockSize = new Vector2i(50, 15);
            int offsetX = ((int)window.Size.X - (blockSize.X * row)) / (row + 1);
            int offsetY = (int)(offsetX / 1.8f);
            Block[] blocks = new Block[row * row];
            for (int i = 0; i < row * row; i++)
            {
                blocks[i].size = blockSize;
                blocks[i].position = new Vector2i(offsetX + (blockSize.X + offsetX) * (i % row), offsetY + (blockSize.Y + offsetY) * (i / row));
                blocks[i].HP = 1;
                blocks[i].lightBlockSprite = new Sprite(blockTexture)
                {
                    Position = (Vector2f)blocks[i].position
                };
                blocks[i].hardBlockSprite = new Sprite(hardBlockTexture)
                {
                    Position = (Vector2f)blocks[i].position
                };
            }
            return blocks;
        }

        private static Racket InitRacket()
        {
            Vector2i racketSize = new Vector2i(100, 20);
            Racket racket = new Racket()
            {
                size = racketSize,
                position = new Vector2i((int)window.Size.X / 2 - racketSize.X / 2, (int)window.Size.Y - 50),
                sprite = new Sprite(racketTexture)
            };
            racket.sprite.Position = (Vector2f)racket.position;
            return racket;
        }

        private static void RacketProcessing(ref Racket racket)
        {
            if (Mouse.GetPosition(window).X <= racket.size.X / 2)
            {
                racket.position.X = 0;
                racket.sprite.Position = (Vector2f)racket.position;
            }
            else if (Mouse.GetPosition(window).X >= window.Size.X - racket.size.X / 2)
            {
                racket.position.X = (int)window.Size.X - racket.size.X;
                racket.sprite.Position = (Vector2f)racket.position;
            }
            else
            {
                racket.position.X = Mouse.GetPosition(window).X - racket.size.X / 2;
                racket.sprite.Position = (Vector2f)racket.position;
            }
        }
    }
}
