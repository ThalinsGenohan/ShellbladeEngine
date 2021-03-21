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
            window.SetCursor(@"assets/cursor_arrow.png", 0, 0, window);

            testButton = new Button(new Vector2i(16, 16),
                                    new Texture(@"assets/testbox.png",
                                    new IntRect(0, 0, 16, 16)),
                                    window,
                                    () => { Console.WriteLine("Clicked!"); },
                                    () => { Console.WriteLine("Hovered On!"); },
                                    () => { Console.WriteLine("Hovered Off!"); })
            {
                Color          = new SFML.Graphics.Color(0xffffffff),
                GlobalPosition = new Vector2i(64, 64)
            };

            Input.UI = new UIContainer
            {
                Elements = new Dictionary<string, UIElement>
                {
                    {"testButton", testButton }
                }
            };
        }

        public override void Loop(Time dt)
        {
            
        }
    }
}
