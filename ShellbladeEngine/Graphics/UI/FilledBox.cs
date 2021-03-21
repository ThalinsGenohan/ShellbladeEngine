using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public class FilledBox : Box
	{
		public Color Color
		{
			get => rectangle.FillColor;
			set => rectangle.FillColor = value;
		}

		public FilledBox() : base()
		{
			rectangle.FillColor = Color.White;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(rectangle, states);
			base.Draw(target, states);
		}
	}
}
