using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class Textbox : Drawable
	{
		private readonly RectangleShape     _background;
		private          bool               _parsed;
		private          List<Sprite>       _characters;
		private          List<List<Action>> _commandQueue;
		private          int                _currentCommand;
		private          int                _currentIndex;
		private          int                _tracking;

		private Color _color;

		public Vector2f     Position      { get; set; }
		public Vector2f     Size          { get; set; }
		public Font         Font          { get; set; }
		public string       Text          { get; set; }
		public List<string> FormattedText { get; set; }
		public int          CurrentPage    { get; private set; }

		public int Tracking
		{
			get => _tracking + Font.TrackingOffset;
			set => _tracking = value;
		}

		public static Dictionary<string, Func<string>> Strings { get; set; } = new Dictionary<string, Func<string>>();

		private Vector2i _inside => (Vector2i)Size - new Vector2i(16, 16);
		private int      _lines  => (int)Math.Floor(_inside.Y / (float)Font.Size.Y);
		private List<Action> _currentQueue
		{
			get => _commandQueue[CurrentPage];
			set => _commandQueue[CurrentPage] = value;
		}


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

			_commandQueue = new List<List<Action>>();

			_color = Color.White;

			CurrentPage = 0;

			_currentIndex = 0;

			TextDelay = 50;
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

			for (var i = 0; i <= Math.Min(_characters.Count - 1, _currentIndex); i++)
			{
				target.Draw(_characters[i], states);
			}
		}

		private void DoColor(byte r, byte g, byte b)
		{
			_color = new Color(r, g, b, _color.A);
		}

		private string ParseCommand(string command, int page)
		{
			int    splitIndex = command.IndexOf(':');
			string comm       = splitIndex > 0 ? command.Remove(splitIndex) : command;
			string args       = splitIndex > 0 ? command.Substring(splitIndex + 1) : "";

			switch (comm)
			{
				case "c":
				case "color":
					_commandQueue[page].Add(args switch
					{
						"black"   => () => DoColor(0x00, 0x00, 0x00),
						"red"     => () => DoColor(0xff, 0x00, 0x00),
						"green"   => () => DoColor(0x00, 0xff, 0x00),
						"blue"    => () => DoColor(0x00, 0x00, 0xff),
						"yellow"  => () => DoColor(0xff, 0xff, 0x00),
						"magenta" => () => DoColor(0xff, 0x00, 0xff),
						"cyan"    => () => DoColor(0x00, 0xff, 0xff),
						"white"   => () => DoColor(0xff, 0xff, 0xff),
						_         => () => Console.WriteLine("ERROR: [Text Command] Unrecognized color!"),
					});
					return "\ufffc";
				case "alpha":
					_commandQueue[page].Add(() => _color.A = (byte)(255f / 100f * Convert.ToSingle(args)));
					return "\ufffc";
				case "reset":
					_commandQueue[page].Add(() =>
					{
						_color = Color.White;
					});
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
			_commandQueue = new List<List<Action>>() { new List<Action>() };

			var xPos = 0;
			var line = 0;
			var page  = 0;

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
						string parsed = ParseCommand(commandBuf, page);

						text = text.Remove(commandEnter, i - commandEnter + 1).Insert(commandEnter, parsed);

						command    = false;
						i          = commandEnter - 1;
						commandBuf = "";
						continue;
					}

					commandBuf += c;
					continue;
				}

				if (char.IsWhiteSpace(c) || c == '\f')
				{
					if (xPos + Font.SpaceSize + wordWidth >= _inside.X)
					{
						if (wordWidth >= _inside.X) throw new Exception($"\"{wordBuf}\" is too long for the textbox!");

						line++;
						xPos = wordWidth;
						if (line >= _lines)
						{
							page++;
							line = 0;
							FormattedText.Add(wordBuf);
							_commandQueue.Add(new List<Action>());
						}
						else
						{
							FormattedText[page] += "\n" + wordBuf;
						}
					}
					else
					{
						if (xPos > 0)
						{
							FormattedText[page] += " " + wordBuf;

							xPos += Font.SpaceSize + wordWidth;
						}
						else
						{
							FormattedText[page] += wordBuf;

							xPos += wordWidth;
						}
					}

					wordBuf   = "";
					wordWidth = 0;

					switch (c)
					{
						case '\n':
						{
							line++;
							xPos = 0;
							if (line >= _lines)
							{
								page++;
								line = 0;
								FormattedText.Add("");
								_commandQueue.Add(new List<Action>());
							}
							else
							{
								FormattedText[page] += "\n";
							}

							break;
						}
						case '\f':
							page++;
							line = 0;
							xPos = 0;
							FormattedText.Add("");
							_commandQueue.Add(new List<Action>());
							break;
					}

					continue;
				}

				if (c == '{')
				{
					command      = true;
					commandEnter = i;
					continue;
				}

				if (c == '\ufffc')
				{
					wordBuf += c;
					continue;
				}

				wordBuf   += c;
				wordWidth += Font.VariableWidth ? Font.Characters[c].Width + Tracking : Font.Size.X;
			}
		}

		public void PrintText()
		{
			_characters = new List<Sprite>();

			var pos = new Vector2f(8f, 8f);

			foreach (char c in FormattedText[CurrentPage])
			{
				switch (c)
				{
					case '\ufffc':
						_currentQueue[_currentCommand]();
						_currentCommand++;
						continue;
					case '\n':
						pos.X =  8;
						pos.Y += Font.Size.Y;
						continue;
				}

				Sprite s = Font.Characters[c].Sprite;
				s.Position = Position + pos;
				s.Color    = _color;

				pos.X += Font.VariableWidth ? c == ' ' ? Font.SpaceSize : Font.Characters[c].Width + Tracking : Font.Size.X;

				_characters.Add(s);
			}
		}

		public void ChangeFont(Font font)
		{
			Font = font;

			_color          = Color.White;
			_currentCommand = 0;

			ParseText();
			PrintText();
		}

		public void Next()
		{
			if (CurrentPage >= FormattedText.Count - 1) return;

			CurrentPage++;
			_currentCommand = 0;
			PrintText();
		}

		private ulong _timer;
		public  bool  Done      { get; private set; }
		public  uint TextDelay { get; set; }

		public void UpdateScroll(int ms)
		{
			if (Done) return;

			_timer += (ulong)ms;
			while (_timer >= TextDelay)
			{
				_timer -= TextDelay;
				_currentIndex++;

				if (_currentIndex < _characters.Count) continue;

				Done = true;
				return;
			}
		}
	}
}
