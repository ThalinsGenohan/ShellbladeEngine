using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public class FilledBox : Box
	{
		private readonly RectangleShape _rectangle;

		public override Vector2i GlobalPosition
		{
			get => base.GlobalPosition;
			set
			{
				base.GlobalPosition       = value;
				_rectangle.Position = (Vector2f)value;
			}
		}

		public override Vector2i Size
		{
			get => base.Size;
			set
			{
				base.Size       = value;
				_rectangle.Size = (Vector2f)value;
			}
		}

		public Color Color
		{
			get => _rectangle.FillColor;
			set => _rectangle.FillColor = value;
		}

		public FilledBox()
		{
			_rectangle = new RectangleShape(new Vector2f(0f, 0f))
			{
				FillColor = Color.White,
				Position  = (Vector2f)GlobalPosition,
				Size      = (Vector2f)Size,
			};
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_rectangle, states);
			base.Draw(target, states);
		}
	}
}
