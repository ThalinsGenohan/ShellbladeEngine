using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class Textbox : Drawable
	{
		private readonly RectangleShape _background;
		private          bool           _parsed;
		private          List<Sprite>   _characters;
		private readonly List<Action>   _commandQueue;
		private          int            _currentCommand;
		private          int            _currentIndex;
		private          int            _tracking;

		private Color _color;

		public Vector2f     Position      { get; set; }
		public Vector2f     Size          { get; set; }
		public Font         Font          { get; set; }
		public string       Text          { get; set; }
		public List<string> FormattedText { get; set; }
		public int          CurrentBox    { get; private set; }

		public int Tracking
		{
			get => _tracking + Font.TrackingOffset;
			set => _tracking = value;
		}

		public static Dictionary<string, Func<string>> Strings { get; set; } = new Dictionary<string, Func<string>>();

		private Vector2i _inside => (Vector2i)Size - new Vector2i(16, 16);
		private int      _lines  => (int)Math.Floor(_inside.Y / (float)Font.Size.Y);


		public Textbox(Vector2i pos, Vector2i size)
		{
			Position = (Vector2f)pos;
			Size     = (Vector2f)size;

			_background = new RectangleShape(Size)
			{
				Position = Position,
				Texture = new Texture(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\testbox.png")
				{
					Repeated = true,
					Smooth   = false,
				},
				TextureRect      = new IntRect(0, 0, size.X, size.Y),
				OutlineColor     = Color.White,
				OutlineThickness = -6f,
				FillColor        = new Color(0xffffff55),
			};

			_commandQueue = new List<Action>();

			_color = Color.White;

			CurrentBox = 0;
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			if (!_parsed)
			{
				_parsed = true;
				ParseText();
				PrintText();
			}

			target.Draw(_background, states);

			foreach (Sprite c in _characters) target.Draw(c, states);
		}

		private void DoColor(uint color)
		{
			_color = new Color((color << 8) | _color.A);
		}

		private string ParseCommand(string command)
		{
			int    splitIndex = command.IndexOf(':');
			string comm       = splitIndex > 0 ? command.Remove(splitIndex) : command;
			string args       = splitIndex > 0 ? command.Substring(splitIndex + 1) : "";

			switch (comm)
			{
				case "color":
					switch (args)
					{
						case "red":
							_commandQueue.Add(() => DoColor(0xff0000));
							break;
						case "yellow":
							_commandQueue.Add(() => DoColor(0xffff00));
							break;
					}

					return "\ufffc";

				case "player":
				case "playername":
					return Strings["player.name"]();
			}

			return "lol not really get trolled";
		}

		private void ParseText()
		{
			string text = Text + " ";

			FormattedText = new List<string> { "" };

			var xPos = 0;
			var line = 0;
			var box  = 0;

			var command      = false;
			var commandBuf   = "";
			var commandEnter = 0;

			var wordBuf   = "";
			var wordWidth = 0;

			for (var i = 0; i < text.Length; i++)
			{
				char c = text[i];

				if (command)
				{
					if (c == '}')
					{
						text = text.Remove(commandEnter, i - commandEnter + 1).Insert(commandEnter, ParseCommand(commandBuf));

						command    = false;
						i          = commandEnter - 1;
						commandBuf = "";
						continue;
					}

					commandBuf += c;
					continue;
				}

				if (c == '\ufffc')
				{
					FormattedText[box] += c;
					continue;
				}

				if (char.IsWhiteSpace(c))
				{
					if (xPos + Font.SpaceSize + wordWidth >= _inside.X)
					{
						if (wordWidth >= _inside.X) throw new Exception($"\"{wordBuf}\" is too long for the textbox!");

						line++;
						xPos = wordWidth;
						if (line >= _lines)
						{
							box++;
							line = 0;
							FormattedText.Add(wordBuf);
						}
						else
						{
							FormattedText[box] += "\n" + wordBuf;
						}
					}
					else
					{
						if (xPos > 0)
						{
							FormattedText[box] += " " + wordBuf;

							xPos += Font.SpaceSize + wordWidth;
						}
						else
						{
							FormattedText[box] += wordBuf;

							xPos += wordWidth;
						}
					}

					wordBuf   = "";
					wordWidth = 0;

					if (c == '\n')
					{
						line++;
						if (line >= _lines)
						{
							box++;
							line = 0;
							FormattedText.Add("");
						}

						xPos = 0;

						FormattedText[box] += "\n";
					}

					continue;
				}

				if (c == '{')
				{
					command      = true;
					commandEnter = i;
					continue;
				}

				wordBuf   += c;
				wordWidth += Font.VariableWidth ? Font.Characters[c].Width + Tracking : Font.Size.X;
			}
		}

		public List<Sprite> PrintText()
		{
			var sprites = new List<Sprite>();
			var pos     = new Vector2f(8f, 8f);

			foreach (char c in FormattedText[CurrentBox])
			{
				if (c == '\ufffc')
				{
					_commandQueue[_currentCommand]();
					_currentCommand++;
					continue;
				}

				if (c == '\n')
				{
					pos.X =  8;
					pos.Y += Font.Size.Y;
					continue;
				}

				Sprite s = Font.Characters[c].Sprite;
				s.Position = Position + pos;
				s.Color    = _color;

				pos.X += Font.VariableWidth ? c == ' ' ? Font.SpaceSize : Font.Characters[c].Width + Tracking : Font.Size.X;

				sprites.Add(s);
				_currentIndex++;
			}

			_characters = sprites;

			return sprites;
		}

		public void Next()
		{
			if (CurrentBox >= FormattedText.Count - 1) return;

			CurrentBox++;
			PrintText();
		}
	}
}
