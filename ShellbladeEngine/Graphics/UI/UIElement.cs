using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public abstract class UIElement : Drawable
	{
		private UIElement _parent;

		public Dictionary<string, UIElement> Children { get; set; } = new Dictionary<string, UIElement>();

		public bool Hovered { get; internal set; } = false;
		public bool Visible { get; set; }          = true;

		public Action OnClick     { get; set; } = () => { };
		public Action OnMouseOver { get; set; } = () => { };
		public Action OnMouseOff  { get; set; } = () => { };

		public virtual Vector2i LocalPosition { get; set; }

		public virtual Vector2i Size { get; set; }

		public int ChildCount => 1 + Children.Values.Sum(c => c.ChildCount);

		public IntRect BoundingBox => new IntRect(GlobalPosition, Size);

		public UIElement Parent
		{
			get => _parent;
			set
			{
				_parent        = value;
				GlobalPosition = LocalPosition + value.GlobalPosition;
			}
		}

		public virtual Vector2i GlobalPosition
		{
			get => LocalPosition + (Parent?.GlobalPosition ?? new Vector2i(0, 0));
			set => LocalPosition = value - (Parent?.GlobalPosition ?? new Vector2i(0, 0));
		}

		public bool Contains(int x, int y) => BoundingBox.Contains(x,     y);
		public bool Contains(Vector2i vec) => BoundingBox.Contains(vec.X, vec.Y);

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
