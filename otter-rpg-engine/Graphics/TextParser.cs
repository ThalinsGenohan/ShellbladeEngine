using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics
{
	public class TextParser
	{
		private string             _fontId = "regular";
		public  List<List<Action>> CommandQueue;
		public  Color              Color { get; set; } = Color.White;
		private List<string>       FormattedText;
		private int                _tracking;

		public static Dictionary<string, string> Strings { get; } = new Dictionary<string, string>();
		public static Dictionary<string, Font>   Fonts   { get; } = new Dictionary<string, Font>();

		public Vector2i Size        { get; set; }
		public int      LineSpacing { get; set; }

		public int Tracking
		{
			get => _tracking + CurrentFont.TrackingOffset;
			set => _tracking = value;
		}

		public Font CurrentFont => Fonts[_fontId];

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
					CommandQueue[page].Add(() => _fontId = args);
					return "\ufffc";
				case "c":
				case "color":
					CommandQueue[page].Add(args switch
					{
						"black"   => () => Color = new Color(0x00, 0x00, 0x00, Color.A),
						"red"     => () => Color = new Color(0xff, 0x00, 0x00, Color.A),
						"green"   => () => Color = new Color(0x00, 0xff, 0x00, Color.A),
						"blue"    => () => Color = new Color(0x00, 0x00, 0xff, Color.A),
						"yellow"  => () => Color = new Color(0xff, 0xff, 0x00, Color.A),
						"magenta" => () => Color = new Color(0xff, 0x00, 0xff, Color.A),
						"cyan"    => () => Color = new Color(0x00, 0xff, 0xff, Color.A),
						"white"   => () => Color = new Color(0xff, 0xff, 0xff, Color.A),
						_         => () => Console.WriteLine("ERROR: [Text Command] Unrecognized color!"),
					});
					return "\ufffc";
				case "alpha":
					CommandQueue[page].Add(() => Color = new Color(Color.R, Color.G, Color.B, (byte)(255f / 100f * Convert.ToSingle(args))));
					return "\ufffc";
				case "reset":
					CommandQueue[page].Add(() => { Color = Color.White; });
					return "\ufffc";

				case "player":
				case "playername":
					return Strings["player.name"];
			}

			return "lol not really get trolled";
		}

		public List<string> ParseText(string inString)
		{
			string text = inString + " ";

			FormattedText = new List<string> { "" };
			CommandQueue  = new List<List<Action>> { new List<Action>() };

			var xPos       = 0;
			var yPos       = 0;
			int lineHeight = CurrentFont.Size.Y;
			var page       = 0;

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

						if (CurrentFont.Size.Y > lineHeight || xPos == 0) lineHeight = CurrentFont.Size.Y;

						continue;
					}

					commandBuf += c;
					continue;
				}

				if (char.IsWhiteSpace(c) || c == '\f')
				{
					if (xPos + CurrentFont.SpaceSize + Tracking + wordWidth > Size.X)
					{
						if (wordWidth > Size.X) throw new Exception($"\"{wordBuf}\" is too long for the textbox!");

						yPos += lineHeight + LineSpacing;
						xPos =  wordWidth;
						lineHeight = CurrentFont.Size.Y;
						if (yPos + lineHeight > Size.Y)
						{
							yPos = 0;
							page++;
							FormattedText.Add(wordBuf);
							CommandQueue.Add(new List<Action>());
						}
						else FormattedText[page] += "\n" + wordBuf;
					}
					else
					{
						if (xPos > 0)
						{
							FormattedText[page] += " ";
							xPos                += CurrentFont.SpaceSize + Tracking;
						}

						FormattedText[page] += wordBuf;
						xPos                += wordWidth;
					}

					wordBuf   = "";
					wordWidth = 0;

					switch (c)
					{
						case '\n':
						{
							yPos       += lineHeight + LineSpacing;
							xPos       =  0;
							lineHeight =  CurrentFont.Size.Y;
								if (yPos + lineHeight > Size.Y)
							{
								yPos = 0;
								page++;
								FormattedText.Add("");
								CommandQueue.Add(new List<Action>());
							}
							else FormattedText[page] += "\n";

							break;
						}
						case '\f':
							page++;
							lineHeight = CurrentFont.Size.Y;
							xPos       = 0;
							yPos       = 0;
							FormattedText.Add("");
							CommandQueue.Add(new List<Action>());
							break;
					}

					continue;
				}

				switch (c)
				{
					case '{':
						command      = true;
						commandEnter = i;
						continue;
					case '\ufffc':
						wordBuf += c;
						continue;
					default:
						wordBuf   += c;
						wordWidth += CurrentFont.Characters[c].Width + Tracking;
						break;
				}
			}

			_fontId = "regular";

			return FormattedText;
		}
	}
}
