using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace Shellblade
{
	public class Input
	{
		private Graphics.Window _window;

		public uint JoystickId { get; set; } = 0;

		internal void Install(Graphics.Window window)
		{
			_window = window;
			RenderWindow rw = _window.RenderWindow;

			rw.KeyPressed  += OnKeyPressed;
			rw.KeyReleased += OnKeyReleased;

			rw.JoystickButtonPressed  += OnJoystickButtonPressed;
			rw.JoystickButtonReleased += OnJoystickButtonReleased;
		}

		internal void Uninstall()
		{
			RenderWindow rw = _window.RenderWindow;

			rw.KeyPressed  -= OnKeyPressed;
			rw.KeyReleased -= OnKeyReleased;

			rw.JoystickButtonPressed  -= OnJoystickButtonPressed;
			rw.JoystickButtonReleased -= OnJoystickButtonReleased;
		}

		private void OnKeyPressed(object sender, KeyEventArgs args)
		{
			ButtonInput button;
			if ((button = Buttons.FirstOrDefault(b => b.Key == args.Code)) == default || button.IsPressed) return;

			button.IsPressed = true;
			button.OnPress();
		}

		private void OnKeyReleased(object sender, KeyEventArgs args)
		{
			ButtonInput button;
			if ((button = Buttons.FirstOrDefault(b => b.Key == args.Code)) == default) return;

			button.IsPressed = false;
			button.OnRelease();
		}

		private void OnJoystickButtonPressed(object sender, JoystickButtonEventArgs args)
		{
			if (args.JoystickId != JoystickId) return;

			ButtonInput button;
			if ((button = Buttons.FirstOrDefault(b => b.JoystickButton == args.Button)) == default || button.IsPressed) return;

			button.IsPressed = true;
			button.OnPress();
		}

		private void OnJoystickButtonReleased(object sender, JoystickButtonEventArgs args)
		{
			if (args.JoystickId != JoystickId) return;

			ButtonInput button;
			if ((button = Buttons.FirstOrDefault(b => b.JoystickButton == args.Button)) == default) return;

			button.IsPressed = false;
			button.OnRelease();
		}

		public List<ButtonInput> Buttons { get; set; }

		public class ButtonInput
		{
			public Keyboard.Key Key            { get; set; }
			public uint?        JoystickButton { get; set; } = null;

			public Action OnPress   { get; set; } = () => { };
			public Action OnHold    { get; set; } = () => { };
			public Action OnRelease { get; set; } = () => { };

			public bool IsPressed  { get; set; } = false;

			public ButtonInput(Keyboard.Key key, uint? button = null)
			{
				Key            = key;
				JoystickButton = button;
			}
		}
	}
}
