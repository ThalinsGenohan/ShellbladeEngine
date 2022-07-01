using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public class Text : UIElement
	{
		public static Dictionary<string, string> Strings { get; } = new();
		public static Dictionary<string, Font>   Fonts   { get; } = new();
		public static Dictionary<string, SoundBuffer>  Voices  { get; } = new();

		public static List<char> SilentChars { get; } = new()
		{
			' ',
			',',
			'.',
			'!',
			'?',
			'\'',
			'\"',
			':',
			';',
			'~',
			'(',
			')',
			'-',
		};

		private string _fontId     = "regular";
		private int    _tracking   = 0;
		private string _string     = "";
		private int    _pageIndex  = 0;
		private ulong  _delayTimer = 0;

		public uint                   Speed              { get; set; }         = 50;
		public uint                   Delay              { get; set; }         = 0;
		public string                 VoiceId            { get; set; }         = "silent";
		public int                    DrawIndex          { get; set; }         = 0;
		public bool                   Instant            { get; set; }         = true;
		public int                    LineSpacing        { get; set; }         = 0;
		public bool                   Paused             { get; set; }         = false;
		public bool                   VariableWidth      { get; set; }         = true;
		public Color                  Color              { get; set; }         = Color.White;
		public bool                   Shakey             { get; set; }         = false;
		public bool                   Wavey              { get; set; }         = false;
		public List<string>           FormattedText      { get; private set; } = new();
		public List<List<CharSprite>> RenderedCharacters { get; private set; } = new();
		public Vector2i               FormattedSize      { get; private set; } = new(0, 0);

		private List<List<Action>> CommandQueue { get; set; } = new();
		private Sound              VoicePlayer  { get; } = new();

		public bool             PageDone     => DrawIndex >= RenderedCharacters[PageIndex].Count - 1;
		public Font             CurrentFont  => Fonts[_fontId];
		public int              PageCount    => RenderedCharacters.Count;
		public bool             LastPage     => PageIndex >= PageCount - 1;
		public List<CharSprite> CurrentPage  => RenderedCharacters[PageIndex];

		public int PageIndex
		{
			get => _pageIndex;
			set
			{
				_pageIndex = Math.Min(value, PageCount - 1);
				DrawIndex  = 0;
			}
		}

		public override Vector2i GlobalPosition
		{
			get => base.GlobalPosition;
			set
			{
				base.GlobalPosition = value;
				RenderCharacters();
			}
		}

		public string String
		{
			get => _string;
			set
			{
				_string = value;
				ParseText(value);
				RenderCharacters();
			}
		}

		public int Tracking
		{
			get => _tracking + CurrentFont.TrackingOffset;
			set => _tracking = value;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			if (RenderedCharacters.Count == 0) return;

			if (!PageDone)
			{
				_delayTimer += (ulong)Game.DeltaTime.AsMilliseconds();
				while (!PageDone && _delayTimer >= Speed + RenderedCharacters[PageIndex][DrawIndex + 1].Delay)
				{
					DrawIndex++;
					CharSprite currentChar = RenderedCharacters[PageIndex][DrawIndex];

					_delayTimer -= Speed + currentChar.Delay;

					VoicePlayer.Stop();
					if (currentChar.VoiceId != "silent" && !SilentChars.Contains(currentChar.Character))
					{
						VoicePlayer.SoundBuffer = Voices[currentChar.VoiceId];
						VoicePlayer.Play();
					}
				}
			}

			int index = Instant ? RenderedCharacters[PageIndex].Count - 1 : DrawIndex;

			for (var i = 0; i <= index; i++)
				RenderedCharacters[PageIndex][i].Draw(target, states);

			base.Draw(target, states);
		}

		public List<List<CharSprite>> RenderCharacters()
		{
			RenderedCharacters = new List<List<CharSprite>>();

			for (var page = 0; page < FormattedText.Count; page++)
			{
				RenderedCharacters.Add(new List<CharSprite>());
				var pos        = new Vector2i(0, 0);
				int lineHeight = CurrentFont.Size.Y;
				var command    = 0;

				foreach (char c in FormattedText[page])
				{
					switch (c)
					{
						case '\ufffc':
							CommandQueue[page][command]();
							command++;
							if (CurrentFont.Size.Y > lineHeight || pos.X == 0) lineHeight = CurrentFont.Size.Y;
							continue;
						case '\n':
							pos.X      =  0;
							pos.Y      += lineHeight + LineSpacing;
							lineHeight =  CurrentFont.Size.Y;
							continue;
					}

					CharSprite s = CurrentFont.Characters[c].Sprite;
					s.Position = (Vector2f)(pos + GlobalPosition);
					s.Color    = Color;
					s.IsShaky  = Shakey;
					s.IsWavy   = Wavey;
					s.Delay    = Delay;
					s.VoiceId  = VoiceId;
					if (Delay > 0) Delay = 0;
					RenderedCharacters[page].Add(s);

					if (!VariableWidth) pos.X += CurrentFont.Size.X;
					else pos.X                += CurrentFont.Characters[c].Width + Tracking;

					if (pos.X > FormattedSize.X) FormattedSize = new Vector2i(pos.X,           FormattedSize.Y);
					if (pos.Y > FormattedSize.Y) FormattedSize = new Vector2i(FormattedSize.X, pos.Y);
				}
			}

			return RenderedCharacters;
		}

		public List<string> ParseText(string inString)
		{
			string text = inString + " ";

			FormattedText       = new List<string> { "" };
			CommandQueue        = new List<List<Action>> { new() };

			Vector2i size           = Size;
			if (size.X <= 0) size.X = int.MaxValue;
			if (size.Y <= 0) size.Y = int.MaxValue;

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
						string parsed = ParseCommand(commandBuf, page, commandEnter);

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
					if (xPos + CurrentFont.SpaceSize + Tracking + wordWidth > size.X)
					{
						if (wordWidth > size.X) throw new Exception($"\"{wordBuf}\" is too long for the textbox!");

						yPos       += lineHeight + LineSpacing;
						xPos       =  wordWidth;
						lineHeight =  CurrentFont.Size.Y;
						if (yPos + lineHeight > size.Y)
						{
							yPos = 0;
							page++;
							FormattedText.Add(wordBuf);
							CommandQueue.Add(new List<Action>());
						} else
						{
							FormattedText[page] += "\n" + wordBuf;
						}
					} else
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
							if (yPos + lineHeight > size.Y)
							{
								yPos = 0;
								page++;
								FormattedText.Add("");
								CommandQueue.Add(new List<Action>());
							} else
							{
								FormattedText[page] += "\n";
							}

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

		private string ParseCommand(string command, int page, int index)
		{
			int    splitIndex = command.IndexOf(':');
			string comm       = splitIndex > 0 ? command[..splitIndex] : command;
			string args       = splitIndex > 0 ? command[(splitIndex + 1)..] : "";

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
						_         => () => Console.WriteLine("ERROR: [String Command] Unrecognized color!"),
					});
					return "\ufffc";
				case "alpha":
					CommandQueue[page]
						.Add(() => Color = new Color(Color.R,
						                             Color.G,
						                             Color.B,
						                             (byte)(255f / 100f * Convert.ToSingle(args))));
					return "\ufffc";
				case "shake":
					CommandQueue[page].Add(() => Shakey = true);
					return "\ufffc";
				case "noshake":
					CommandQueue[page].Add(() => Shakey = false);
					return "\ufffc";
				case "wave":
					CommandQueue[page].Add(() => Wavey = true);
					return "\ufffc";
				case "nowave":
					CommandQueue[page].Add(() => Wavey = false);
					return "\ufffc";
				case "d":
				case "delay":
					CommandQueue[page].Add(() => Delay = uint.Parse(args));
					return "\ufffc";
				 case "s":
				 case "speed":
					CommandQueue[page].Add(() => Speed = uint.Parse(args));
					return "\ufffc";
				case "v":
				case "voice":
					CommandQueue[page].Add(() => VoiceId = args);
					return "\ufffc";
				case "reset":
					CommandQueue[page].Add(() =>
					{
						Color = Color.White;
					});
					return "\ufffc";

				case "player":
				case "playername":
					return Strings["player.name"];
			}

			return "[Unknown Command]";
		}
	}
}
