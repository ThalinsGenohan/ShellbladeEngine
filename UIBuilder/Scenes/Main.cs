using System;
using System.Drawing;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using Shellblade;
using Shellblade.Graphics;
using Shellblade.Graphics.UI;

namespace UIBuilder.Scenes
{
    internal class Main : Scene
    {
        private Button testButton { get; }


        public Main(Game window) : base(window)
        {
            //Set default cursor
            SetCursor(@"assets/arrow_cursor.png", 0, 0, window);

            testButton = new Button(new Vector2i(16, 16),
                                    new Texture(@"assets/testbox.png",
                                    new IntRect(0, 0, 16, 16)))
            {
                Color          = new SFML.Graphics.Color(0xffffffff),
                OnClick        = () =>
                {
                    testButton.SetSize(new Vector2i(160, 120));
                },
                GlobalPosition = new Vector2i(64, 64)
            };
            testButton.OnMouseOver = () => { Console.WriteLine("Moused over"); };
            testButton.OnMouseOff  = () => { Console.WriteLine("Moused off"); };

            Input.UI = new UIContainer
            {
                Elements = new Dictionary<string, UIElement>
                {
                    {"testButton", testButton }
                }
            };
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

                    System.Drawing.Color pixel = image.GetPixel(x, y);

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
