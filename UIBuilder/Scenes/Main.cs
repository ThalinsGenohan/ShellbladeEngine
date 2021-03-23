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
        private bool _selectorRect;
        private bool _lMouseDown;
        private Vector2i _selectorPosition = new Vector2i();

        private OutlinedBox menu = new OutlinedBox
        {
            FillColor = new Color(0x5C1AE1a5),
            Color = new Color(0x5C1AE1ff),
            OutlineThickness = 2f,
            Size = new Vector2i(60, 100)
        };

        private OutlinedBox selectorRect = new OutlinedBox
        {
            Color = new Color(0xD54CEDe5),
            OutlineThickness = 2f,
            Size = new Vector2i(1, 1)
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
                                    () => { Console.WriteLine("Hovered On!"); testButton.Opacity = .5f; },
                                    () => { Console.WriteLine("Hovered Off!"); testButton.Opacity = 1f; })
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

        public override void Loop(Time dt)  //[Bug] IMPORTANT! Mouse events OUTSIDE OF WINDOW need to be IGNORED!
        {
            var mousePos = Mouse.GetPosition(_window) / _scale;


            //[MENU HANDLING]//
            UIElement mousedElement = null;

            //If cursor is above a UI Element, prevent menu from opening
            _menuOpenable = true;
            foreach (UIElement element in Input.UI.Elements.Values)
            {
                if (element.Contains(mousePos) && element != menu)
                {
                    _menuOpenable = false;
                    mousedElement = element;
                    //Leaving this here commented out for now, but I think safe to remove, also hotspot is wrong
                    //if (element is Button) _game.SetCursor("assets/cursor_hand.png", 0, 0, _game);
                    break;
                }
                mousedElement = null;
            }

            //If RMB is pressed, try to close menu for reopening at other position
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                _rMouseDown = true;

                //Check if mouse is at position where it opened menu, if yes, close menu
                if (mousePos != _menuPos && _menuOpenable)
                {
                    _menu = false;
                    _mouseInMenu = false;
                    Input.UI.Elements.Remove("menu");
                }
            } //If LMB is pressed, close menu
            else if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var delta_menu = _menu;
                _menu = false;
                _mouseInMenu = false;
                _rMouseDown = false;
                Input.UI.Elements.Remove("menu");

                //Trigger OnMouseOver if mouse is on button
                if (delta_menu != _menu && !(mousedElement is null))
                {
                    if (mousedElement is Button)
                    {
                        if (((Button)mousedElement).covered)
                        {
                            ((Button)mousedElement).covered = false;
                            mousedElement.OnMouseOver();
                        }
                    }
                }
            }

            //If menu was closed by RMB press previously, try to reopen
            if (_rMouseDown && !Mouse.IsButtonPressed(Mouse.Button.Right) && !_menu)
            {
                _rMouseDown = false;

                //If menu isn't open, open menu
                if (!Input.UI.Elements.ContainsKey("menu") && _menuOpenable)
                {
                    _menu = true;
                    _menuPos = mousePos;
                    menu.GlobalPosition = _menuPos;
                    Input.UI.Elements.Add("menu", menu);
                }
            }

            //Menu overlap handling
            //If mouse is within menu and menu is open, set _mouseInMenu to true, and set arrow cursor
            var delta_mouseInMenu = _mouseInMenu;
            _mouseInMenu = menu.Contains(mousePos) && _menu;
            if (_mouseInMenu) _game.SetCursor("assets/cursor_arrow.png", 0, 0, _game);

            //If a button is covered by the menu, tell it it's covered (maybe do this with all UIElements [add covered to class?])
            //Trigger events that might not be detected through covering timing
            //Also, if selector rect is active, set all elements to covered
            foreach (UIElement element in Input.UI.Elements.Values)
            {
                if(element is Button)
                {
                    //Safety net (I'm too scared to remove this but since modification of _mouseInMenu logic it's probably safe to remove)
                    if (!_menu)
                    {
                        _mouseInMenu = false;
                    }

                    if (_selectorRect)
                    {
                        ((Button)element).covered = true;
                    }
                    else
                    {
                        ((Button)element).covered = false;
                        var trigger = (delta_mouseInMenu != _mouseInMenu) && element.Contains(mousePos);

                        if (_mouseInMenu && trigger) element.OnMouseOff();
                        ((Button)element).covered = _mouseInMenu;
                        if (!_mouseInMenu && trigger) element.OnMouseOver();
                    }
                }
            }


            //[SELECTOR HANDLING]//
            //Set up selector rectangle
            var delta_lMouseDown = _lMouseDown;
            _lMouseDown = Mouse.IsButtonPressed(Mouse.Button.Left);

            if (_lMouseDown)
            {
                //_mouseInMenu here because once UI in menu, i dont want selector to open on click in menu
                var _selectorRectOpenable = _menuOpenable && !_selectorRect && (_lMouseDown != delta_lMouseDown) && !_mouseInMenu;
                if (_selectorRectOpenable && !_menu)
                {
                    _selectorRect = true;
                }
            }
            else
            {
                _selectorRect = false;

                //The "is Button" IS the null check! Careful when opening this to other UIElements
                if (mousedElement is Button && mousedElement.Contains(mousePos) && !_menu)
                {
                    if (((Button)mousedElement).covered == true)
                    {
                        ((Button)mousedElement).covered = false;
                        mousedElement.OnMouseOver();
                    }
                }
            }

            //Open selector rectangle and adjust size
            if (_selectorRect)
            {
                if (!Input.UI.Elements.ContainsKey("selector_rect"))
                {
                    selectorRect.GlobalPosition = mousePos;
                    _selectorPosition = selectorRect.GlobalPosition;
                    Input.UI.Elements.Add("selector_rect", selectorRect);
                }

                selectorRect.Size = mousePos - _selectorPosition;
            }

            //Close selector rectangle
            if (!_selectorRect)
            {
                Input.UI.Elements.Remove("selector_rect");
            }
        }
    }
}
