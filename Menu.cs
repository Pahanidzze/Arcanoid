using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arcanoid
{
    class Menu
    {
        private static RenderWindow window;
        private static readonly Font arial = new Font("arial.ttf");
        private struct Button
        {
            public RectangleShape shape;
            public Text text;
        }

        public static void OpenMenu(RenderWindow win, ref int mode)
        {
            window = win;
            window.Clear();
            Button buttonEasy = InitializeButton(250, 165, 300, 50, "Легко", 108, 4);
            Button buttonNormal = InitializeButton(250, 250, 300, 50, "Нормально", 60, 4);
            Button buttonHard = InitializeButton(250, 335, 300, 50, "Сложно", 89, 4);
            Button buttonExit = InitializeButton(250, 450, 300, 50, "Выход", 100, 4);
            FillMenu(buttonEasy, buttonNormal, buttonHard, buttonExit);
            window.Display();
            while (window.IsOpen && mode == 0)
            {
                window.DispatchEvents();
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    Vector2i mousePosition = Mouse.GetPosition(window);
                    if (mousePosition.X >= buttonEasy.shape.Position.X && mousePosition.X <= buttonEasy.shape.Position.X + buttonEasy.shape.Size.X &&
                        mousePosition.Y >= buttonEasy.shape.Position.Y && mousePosition.Y <= buttonEasy.shape.Position.Y + buttonEasy.shape.Size.Y)
                    {
                        mode = 1;
                    }
                    else if (mousePosition.X >= buttonNormal.shape.Position.X && mousePosition.X <= buttonNormal.shape.Position.X + buttonNormal.shape.Size.X && 
                             mousePosition.Y >= buttonNormal.shape.Position.Y && mousePosition.Y <= buttonNormal.shape.Position.Y + buttonNormal.shape.Size.Y)
                    {
                        mode = 2;
                    }
                    else if (mousePosition.X >= buttonHard.shape.Position.X && mousePosition.X <= buttonHard.shape.Position.X + buttonHard.shape.Size.X &&
                             mousePosition.Y >= buttonHard.shape.Position.Y && mousePosition.Y <= buttonHard.shape.Position.Y + buttonHard.shape.Size.Y)
                    {
                        mode = 3;
                    }
                    else if (mousePosition.X >= buttonExit.shape.Position.X && mousePosition.X <= buttonExit.shape.Position.X + buttonExit.shape.Size.X &&
                             mousePosition.Y >= buttonExit.shape.Position.Y && mousePosition.Y <= buttonExit.shape.Position.Y + buttonExit.shape.Size.Y)
                    {
                        mode = -1;
                    }
                }
            }
        }

        private static void FillMenu(Button buttonEasy, Button buttonNormal, Button buttonHard, Button buttonExit)
        {
            Text header = new Text("Arcanoid", arial, 48)
            {
                Position = new Vector2f(300, 50)
            };
            window.Draw(header);
            DrawButton(buttonEasy);
            DrawButton(buttonNormal);
            DrawButton(buttonHard);
            DrawButton(buttonExit);
        }

        private static Button InitializeButton(int positionX, int positionY, int sizeX, int sizeY, string text = "", int textPositionX = 0, int textPositionY = 0)
        {
            Button button;
            button.shape = new RectangleShape()
            {
                Position = new Vector2f(positionX, positionY),
                Size = new Vector2f(sizeX, sizeY)
            };
            button.text = new Text(text, arial, 34)
            {
                Position = new Vector2f(positionX + textPositionX, positionY + textPositionY),
                FillColor = Color.Black
            };
            return button;
        }

        private static void DrawButton(Button button)
        {
            window.Draw(button.shape);
            window.Draw(button.text);
        }
    }
}