using SFML.System;
using SFML.Window;
using Shellblade;

namespace UIBuilder.Scenes
{
    internal class Main : Scene
    {
        private static byte[] _pixels = new byte[] { 255, 0, 0, 255 };
        private static Vector2u _size = new Vector2u(16, 16);
        private static Vector2u _hotspot = new Vector2u(0, 0);
        private Cursor _cursor = new Cursor(_pixels, _size, _hotspot);

        public Main(Game window) : base(window)
        {
            window.UpdateCursor(_cursor);
        }

        public override void Loop(Time dt)
        {
            
        }
    }
}
