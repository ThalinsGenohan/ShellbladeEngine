using System;
using System.Collections.Generic;
using System.Numerics;
using SFML.Graphics;
using SFML.System;
using Shellblade.Graphics.UI;
using Text = Shellblade.Graphics.UI.Text;

namespace Shellblade.Graphics
{
	public class Textbox : UIElement
	{
		public static Dictionary<string, Func<string>> Strings { get; set; } = new Dictionary<string, Func<string>>();

		private readonly Box _background;
		private readonly Text           _text;

		private ulong _timer = 0;

		public uint TextDelay { get; set; } = 50;

		public string Text
		{
			get => _text.String;
			set => _text.String = value;
		}

		private Vector2i Inside => Size - new Vector2i(16, 16);

		public Textbox(Vector2i pos, Vector2i size)
		{
			GlobalPosition = pos;
			Size     = size;

			_text = new Text
			{
				GlobalPosition = GlobalPosition + new Vector2i(8, 8),
				Size           = Inside,
				LineSpacing    = 1,
				Instant        = false,
			};

			_background = new GradientBox
			{
				Colors = new[]
				{
					new Color(0x5C1AE1e5),
					new Color(0x00000099),
					new Color(0x5C1AE1e5),
					new Color(0x00000099),
				},
				GlobalPosition = GlobalPosition,
				Size = Size,
			};
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_background, states);
			target.Draw(_text,       states);
		}

		public void Next()
		{
			if (_text.Paused) _text.Paused = false;

			if (!_text.PageDone)
			{
				_text.DrawIndex = _text.CurrentPage.Count - 1;
				return;
			}

			if (_text.LastPage) return;

			_text.PageIndex++;
		}

		public void UpdateScroll(int ms)
		{
			if (_text.PageDone) return;

			_timer += (ulong)ms;
			while (_timer >= TextDelay)
			{
				_timer -= TextDelay;
				_text.DrawIndex++;
			}
		}
	}
}
