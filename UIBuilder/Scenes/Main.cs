using System;
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

        private Window _window;
        private Game _game;

        private bool _menu;
        private bool _menuOpenable = true;
        private bool _rMouseDown;
        private Vector2i _menuPos;
        private int _scale;
        private static bool _mouseInMenu = false;

        private OutlinedBox menu = new OutlinedBox
        {
            FillColor = new Color(0x5C1AE1a5),
            Color = new Color(0x5C1AE1ff),
            OutlineThickness = 2f,
            Size = new Vector2i(60, 100)
        };


        public Main(Game window) : base(window)
        {
            //Set default cursor
            window.SetCursor(@"assets/cursor_arrow.png", 0, 0, window);

            _window = window.GetWindow();
            _game = window;
            _scale = (int)(window.WindowSize.X / window.Resolution.X);

            
            testButton = new Button(new Vector2i(16, 16),
                                    new Texture(@"assets/testbox.png",
                                    new IntRect(0, 0, 16, 16)),
                                    window,
                                    () => { Console.WriteLine("Clicked!"); },
                                    () => { Console.WriteLine("Hovered On!"); },
                                    () => { Console.WriteLine("Hovered Off!"); })
            {
                Color          = new Color(0xffffffff),
                GlobalPosition = new Vector2i(64, 64)
            };
            

            Input.UI = new UIContainer
            {
                Elements = new Dictionary<string, UIElement>
                {
                    { "testButton", testButton },
                },
            };
        }

        public override void Loop(Time dt)
        {
            var mousePos = Mouse.GetPosition(_window) / _scale;

            //Menu handling
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                _rMouseDown = true;

                if (mousePos != _menuPos)
                {
                    _menu = false;
                    _mouseInMenu = false;
                    Input.UI.Elements.Remove("menu");
                }
            }
            else if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                _menu = false;
                _mouseInMenu = false;
                _rMouseDown = false;
                Input.UI.Elements.Remove("menu");
            }

            if (_rMouseDown && !Mouse.IsButtonPressed(Mouse.Button.Right) && !_menu)
            {
                _rMouseDown = false;
                if (!Input.UI.Elements.ContainsKey("menu") && _menuOpenable)
                {
                    _menu = true;
                    _menuPos = mousePos;
                    menu.GlobalPosition = _menuPos;
                    Input.UI.Elements.Add("menu", menu);
                }
            }

            //Menu overlap handling
            _mouseInMenu = menu.Contains(mousePos);

            foreach(UIElement element in Input.UI.Elements.Values)
            {
                if(element is Button)
                {
                    if (!_menu)
                    {
                        _mouseInMenu = false;
                    }

                    ((Button)element).covered = _mouseInMenu;

                    if (element.Contains(mousePos))
                    {
                        _menuOpenable = false;
                    }
                    else
                    {
                        _menuOpenable = true;
                    }
                }
            }
        }
    }
}
