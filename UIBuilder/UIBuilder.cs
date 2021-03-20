using UIBuilder.Scenes;
using Shellblade;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;

namespace UIBuilder
{
    internal class UIBuilder
    {
        private const uint _resX = 320;
        private const uint _resY = 240;
        private const uint _scale = 4;
        private const string _fontDir = "assets/fonts/";

        private static uint _windowX => _resX * _scale; //maybe restructure so Window_ only gets recalculated
        private static uint _windowY => _resY * _scale; //when Res_ changes? sfml probably has something for this

        private static void Main(string[] args)
        {
            var window = new Game(_windowX, _windowY, _resX, _resY, "UI Builder");
            var main = new Main(window);

            window.LoadScene(main);
            window.Run();
        }
    }
}
