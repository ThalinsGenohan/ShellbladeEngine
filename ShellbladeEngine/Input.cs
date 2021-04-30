using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using Shellblade.Graphics.UI;

namespace Shellblade
{
	public class Input
	{
		private Game _game;

		public uint              JoystickId { get; set; } = 0;
		public UIContainer       UI         { get; set; } = new UIContainer();
		public List<ButtonInput> Buttons    { get; set; } = new List<ButtonInput>();

		public void DoHoldInputs()
		{
			foreach (ButtonInput jsEvent in Buttons.Where(b => b.IsPressed))
				jsEvent.OnHold();

			foreach (ButtonInput kbEvent in Buttons.Where(b => b.IsPressed))
				kbEvent.OnHold();
		}

		internal void Install(Game game)
		{
			_game = game;
			RenderWindow rw = _game.Window;

			rw.KeyPressed  += OnKeyPressed;
			rw.KeyReleased += OnKeyReleased;

			rw.JoystickButtonPressed  += OnJoystickButtonPressed;
			rw.JoystickButtonReleased += OnJoystickButtonReleased;

			rw.MouseButtonPressed += UI.OnMouseButtonPressed;
			rw.MouseMoved         += UI.OnMouseMoved;
		}

		internal void Uninstall()
		{
			RenderWindow rw = _game.Window;

			rw.KeyPressed  -= OnKeyPressed;
			rw.KeyReleased -= OnKeyReleased;

			rw.JoystickButtonPressed  -= OnJoystickButtonPressed;
			rw.JoystickButtonReleased -= OnJoystickButtonReleased;

			rw.MouseButtonPressed -= UI.OnMouseButtonPressed;
			rw.MouseMoved         -= UI.OnMouseMoved;
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
			if ((button = Buttons.FirstOrDefault(b => b.JoystickButton == args.Button)) == default ||
			    button.IsPressed) return;

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

		public class ButtonInput
		{
			public Keyboard.Key Key            { get; set; }
			public uint?        JoystickButton { get; set; } = null;

			public Action OnPress   { get; set; } = () => { };
			public Action OnHold    { get; set; } = () => { };
			public Action OnRelease { get; set; } = () => { };

			public bool IsPressed { get; set; } = false;

			public ButtonInput(Keyboard.Key key, uint? button = null)
			{
				Key            = key;
				JoystickButton = button;
			}
		}
	}
}
