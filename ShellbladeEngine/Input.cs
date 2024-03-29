﻿using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using Shellblade.Graphics.UI;

namespace Shellblade
{
	public class Input
	{
		public uint              JoystickId { get; set; } = 0;
		public UIState       UI         { get; set; } = new();
		public List<ButtonInput> Buttons    { get; set; } = new();

		public void DoHoldInputs()
		{
			foreach (ButtonInput jsEvent in Buttons.Where(b => b.IsPressed))
				jsEvent.OnHold();

			foreach (ButtonInput kbEvent in Buttons.Where(b => b.IsPressed))
				kbEvent.OnHold();
		}

		internal void Install()
		{
			RenderWindow rw = Game.Window;

			rw.KeyPressed  += OnKeyPressed;
			rw.KeyReleased += OnKeyReleased;

			rw.JoystickButtonPressed  += OnJoystickButtonPressed;
			rw.JoystickButtonReleased += OnJoystickButtonReleased;
		}

		internal void Uninstall()
		{
			RenderWindow rw = Game.Window;

			rw.KeyPressed  -= OnKeyPressed;
			rw.KeyReleased -= OnKeyReleased;

			rw.JoystickButtonPressed  -= OnJoystickButtonPressed;
			rw.JoystickButtonReleased -= OnJoystickButtonReleased;
		}

		private void OnKeyPressed(object sender, KeyEventArgs args)
		{
			ButtonInput button;
			if ((button = Buttons.FirstOrDefault(b => b.Key == args.Code)) == default
			|| button.IsPressed)
			return;

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
