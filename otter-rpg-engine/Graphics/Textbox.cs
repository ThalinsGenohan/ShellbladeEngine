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

		public Vector2f     Position      { get; set; }
		public Vector2f     Size          { get; set; }
		public int          Kerning       { get; set; }
		public Font         Font          { get; set; }
		public string       Text          { get; set; }
		public List<string> FormattedText { get; set; }
		public int          CurrentBox    { get; private set; }

		private Vector2i _inside => (Vector2i)Size - new Vector2i(16, 16);

		private int _lines => (int)Math.Floor(_inside.Y / (float)Font.Size.Y);

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
				OutlineColor     = Color.Green,
				OutlineThickness = -8f,
			};

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

		private void ParseText()
		{
			Text += " ";

			FormattedText = new List<string> { "" };

			var xPos = 0;
			var line = 0;
			var box  = 0;

			var control    = false;
			var controlBuf = "";

			var wordBuf   = "";
			var wordWidth = 0;

			foreach (char c in Text)
			{
				if (control)
				{
					if (c == '}')
					{
						control = false;
						continue;
					}

					controlBuf += c;
					continue;
				}

				if (char.IsWhiteSpace(c))
				{
					if (xPos + Font.SpaceSize + wordWidth > _inside.X)
					{
						if (wordWidth > _inside.X) throw new Exception($"\"{wordBuf}\" is too long for the textbox!");

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
					control = true;
					continue;
				}

				wordBuf   += c;
				wordWidth += Font.VariableWidth ? Font.Characters[c].Width + Kerning : Font.Size.X;
			}
		}

		public List<Sprite> PrintText()
		{
			var sprites = new List<Sprite>();
			var pos     = new Vector2f(8f, 8f);

			foreach (char c in FormattedText[CurrentBox])
			{
				if (c == '\n')
				{
					pos.X =  8;
					pos.Y += Font.Size.Y;
					continue;
				}

				Sprite s = Font.Characters[c].Sprite;
				s.Position =  Position + pos;
				pos.X      += Font.VariableWidth ? Font.Characters[c].Width + Kerning : Font.Size.X;

				sprites.Add(s);
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
