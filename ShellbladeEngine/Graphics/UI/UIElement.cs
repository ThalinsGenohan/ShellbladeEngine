using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public abstract class UIElement : Drawable
	{
		private UIElement _parent;

		public UIElement Parent
		{
			get => _parent;
			set
			{
				_parent        = value;
				GlobalPosition = LocalPosition + value.GlobalPosition;
			}
		}

		public Dictionary<string, UIElement> Children { get; set; } = new Dictionary<string, UIElement>();

		public IntRect BoundingBox => new IntRect(GlobalPosition, Size);

		public bool Hovered { get; internal set; } = false;
		public bool Visible { get; set; }          = true;

		public Action OnClick     { get; set; } = () => { };
		public Action OnMouseOver { get; set; } = () => { };
		public Action OnMouseOff  { get; set; } = () => { };

		public virtual Vector2i GlobalPosition
		{
			get => LocalPosition + (Parent?.GlobalPosition ?? new Vector2i(0, 0));
			set => LocalPosition = value - (Parent?.GlobalPosition ?? new Vector2i(0, 0));
		}

		public virtual Vector2i LocalPosition { get; set; }

		public virtual Vector2i Size { get; set; }

		public bool Contains(int x, int y) => BoundingBox.Contains(x,     y);
		public bool Contains(Vector2i vec) => BoundingBox.Contains(vec.X, vec.Y);

		public void SetParent(UIElement parent)
		{

		}

		public void AddChild(string id, UIElement child)
		{
			Children.Add(id, child);
			child.Parent = this;
		}

		public virtual void Draw(RenderTarget target, RenderStates states)
		{
			foreach (UIElement e in Children.Values)
				target.Draw(e, states);
		}
	}
}
