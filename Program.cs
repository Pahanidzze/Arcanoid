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
    class Program
    {
        private static RenderWindow window;

        private static void Main()
        {
            window = new RenderWindow(new VideoMode(800, 600), "Arcanoid");
            window.SetFramerateLimit(60);
            window.Closed += Window_Closed;
            int difficulty = 0;
            for (Menu.OpenMenu(window, ref difficulty); window.IsOpen && difficulty != -1; Menu.OpenMenu(window, ref difficulty))
            {
                difficulty = 0;
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
