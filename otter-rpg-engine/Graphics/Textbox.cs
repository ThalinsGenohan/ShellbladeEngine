using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class Textbox : Drawable
	{
		public static Dictionary<string, Func<string>> Strings { get; set; } = new Dictionary<string, Func<string>>();

		private readonly RectangleShape _background;

		private readonly TextParser _parser;

		private bool         _parsed         = false;
		private List<Sprite> _characters     = new List<Sprite>();
		private int          _currentCommand = 0;
		private int          _currentIndex   = 0;
		private Color        _color          = Color.White;
		private ulong        _timer          = 0;
		private List<string> _formattedText  = new List<string>();
		private int          _currentPage    = 0;

		public Vector2f Position  { get; set; }
		public Vector2f Size      { get; set; }
		public string   Text      { get; set; }
		public uint     TextDelay { get; set; }         = 50;
		public bool     PageDone  { get; private set; } = false;

		private List<List<Action>> CommandQueue => _parser.CommandQueue;
		private Font               CurrentFont  => _parser.CurrentFont;
		private Vector2i           Inside       => (Vector2i)Size - new Vector2i(16, 16);

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

			_parser = new TextParser
			{
				LineSpacing = 1,
				Size        = Inside,
			};
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			if (!_parsed)
			{
				_parsed        = true;
				_formattedText = _parser.ParseText(Text);
				PrintText();
			}

			target.Draw(_background, states);

			for (var i = 0; i <= Math.Min(_characters.Count - 1, _currentIndex); i++)
				target.Draw(_characters[i], states);
		}

		public void PrintText()
		{
			_characters = new List<Sprite>();

			var pos        = new Vector2i(8, 8);
			int lineHeight = CurrentFont.Size.Y;

			foreach (char c in _formattedText[_currentPage])
			{
				switch (c)
				{
					case '\ufffc':
						CommandQueue[_currentPage][_currentCommand]();
						_currentCommand++;
						if (CurrentFont.Size.Y > lineHeight || pos.X == 8) lineHeight = CurrentFont.Size.Y;
						continue;
					case '\n':
						pos.X      =  8;
						pos.Y      += lineHeight + _parser.LineSpacing;
						lineHeight =  CurrentFont.Size.Y;
						continue;
				}

				Sprite s = CurrentFont.Characters[c].Sprite;
				s.Position = Position + (Vector2f)pos;
				s.Color    = _parser.Color;

				pos.X += CurrentFont.Characters[c].Width + _parser.Tracking;

				_characters.Add(s);
			}
		}

		public void Next()
		{
			if (_currentPage >= _formattedText.Count - 1) return;

			_currentIndex   = 0;
			_currentCommand = 0;
			_currentPage++;
			PageDone = false;
			PrintText();
		}

		public void UpdateScroll(int ms)
		{
			if (PageDone) return;

			_timer += (ulong)ms;
			while (_timer >= TextDelay)
			{
				_timer -= TextDelay;
				_currentIndex++;

				if (_currentIndex < _characters.Count) continue;

				PageDone = true;
				return;
			}
		}

		private void DoColor(byte r, byte g, byte b)
		{
			_color = new Color(r, g, b, _color.A);
		}

		/*
		private string ParseCommand(string command, int page)
		{
			int    splitIndex = command.IndexOf(':');
			string comm       = splitIndex > 0 ? command.Remove(splitIndex) : command;
			string args       = splitIndex > 0 ? command.Substring(splitIndex + 1) : "";

			switch (comm)
			{
				case "f":
				case "font":
					_fontId = args;
					_commandQueue[page].Add(() => _fontId = args);
					return "\ufffc";
				case "c":
				case "color":
					_commandQueue[page].Add(args switch
					{
						"black"   => () => _color = new Color(0x00, 0x00, 0x00, _color.A),
						"red"     => () => _color = new Color(0xff, 0x00, 0x00, _color.A),
						"green"   => () => _color = new Color(0x00, 0xff, 0x00, _color.A),
						"blue"    => () => _color = new Color(0x00, 0x00, 0xff, _color.A),
						"yellow"  => () => _color = new Color(0xff, 0xff, 0x00, _color.A),
						"magenta" => () => _color = new Color(0xff, 0x00, 0xff, _color.A),
						"cyan"    => () => _color = new Color(0x00, 0xff, 0xff, _color.A),
						"white"   => () => _color = new Color(0xff, 0xff, 0xff, _color.A),
						_         => () => Console.WriteLine("ERROR: [Text Command] Unrecognized color!"),
					});
					return "\ufffc";
				case "alpha":
					_commandQueue[page].Add(() => _color.A = (byte)(255f / 100f * Convert.ToSingle(args)));
					return "\ufffc";
				case "reset":
					_commandQueue[page].Add(() => { _color = Color.White; });
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

			_formattedText = new List<string> { "" };
			_commandQueue  = new List<List<Action>> { new List<Action>() };

			var xPos = 0;
			var line = 0;
			var page = 0;

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
					if (xPos + CurrentFont.SpaceSize + wordWidth >= Inside.X)
					{
						if (wordWidth >= Inside.X) throw new Exception($"\"{wordBuf}\" is too long for the textbox!");

						line++;
						xPos = wordWidth;
						if (line >= Lines)
						{
							page++;
							line = 0;
							_formattedText.Add(wordBuf);
							_commandQueue.Add(new List<Action>());
						}
						else
						{
							_formattedText[page] += "\n" + wordBuf;
						}
					}
					else
					{
						if (xPos > 0)
						{
							_formattedText[page] += " " + wordBuf;

							xPos += CurrentFont.SpaceSize + wordWidth;
						}
						else
						{
							_formattedText[page] += wordBuf;

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
							if (line >= Lines)
							{
								page++;
								line = 0;
								_formattedText.Add("");
								_commandQueue.Add(new List<Action>());
							}
							else
							{
								_formattedText[page] += "\n";
							}

							break;
						}
						case '\f':
							page++;
							line = 0;
							xPos = 0;
							_formattedText.Add("");
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
				wordWidth += CurrentFont.VariableWidth ? CurrentFont.Characters[c].Width + Tracking : CurrentFont.Size.X;
			}

			_fontId = "regular";
		}
		*/
	}
}
