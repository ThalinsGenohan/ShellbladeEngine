using System;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public abstract class UIElement : Drawable
	{
		public IntRect BoundingBox { get; set; } = new IntRect();

		public bool Hovered { get; set; } = false;

		public Action OnClick     { get; set; } = () => { };
		public Action OnMouseOver { get; set; } = () => { };
		public Action OnMouseOff  { get; set; } = () => { };

		public Vector2i Position
		{
			get => new Vector2i(BoundingBox.Left, BoundingBox.Top);
			set => BoundingBox = new IntRect(value, Size);
		}

		public Vector2i Size
		{
			get => new Vector2i(BoundingBox.Width, BoundingBox.Height);
			set => BoundingBox = new IntRect(Position, value);
		}

		public bool Contains(int x, int y) => BoundingBox.Contains(x,     y);
		public bool Contains(Vector2i vec) => BoundingBox.Contains(vec.X, vec.Y);

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
