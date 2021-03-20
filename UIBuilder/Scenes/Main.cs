using System;
using System.Drawing;
using SFML.System;
using SFML.Window;
using Shellblade;

namespace UIBuilder.Scenes
{
    internal class Main : Scene
    {
        public Main(Game window) : base(window)
        {
            //Set default cursor
            SetCursor(@"assets/arrow_cursor.png", 0, 0, window);
        }


        public int SetCursor(string imagePath, uint hotspotX, uint hotspotY, Game window)
        {
            Bitmap image;
            try
            {
                image = new Bitmap(imagePath);
            }
            catch
            {
                return 1;
            }

            uint width = (uint)image.Width;
            uint height = (uint)image.Height;

            byte[] pixels = new byte[width * height * 4];
            Vector2u size = new Vector2u(width, height);
            Vector2u hotspot = new Vector2u(hotspotX, hotspotY);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = 4 * (x + image.Width * y);

                    Color pixel = image.GetPixel(x, y);

                    pixels[index] = pixel.R;
                    pixels[index + 1] = pixel.G;
                    pixels[index + 2] = pixel.B;
                    pixels[index + 3] = pixel.A;
                }
            }

            Cursor cursor = new Cursor(pixels, size, hotspot);

            window.UpdateCursor(cursor);

            return 0;
        }


        public override void Loop(Time dt)
        {
            
        }
    }
}
