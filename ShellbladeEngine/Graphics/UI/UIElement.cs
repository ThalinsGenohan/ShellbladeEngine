using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public abstract class UIElement : Drawable
	{
		private UIElement? _parent = null;

		public List<UIElement> Children { get; set; } = new();

		public string ID { get; set; }
		
		public bool Visible { get; set; }          = true;

		public virtual Vector2i LocalPosition { get; set; }
		public virtual Vector2i Size          { get; set; }

		public int TreeSize => 1 + Children.Sum(c => c.TreeSize);

		public IntRect BoundingBox => new(GlobalPosition, Size);

		public UIElement? Parent
		{
			get => _parent;
			set
			{
				_parent        = value;
				if (_parent != null)
					GlobalPosition = LocalPosition + _parent.GlobalPosition;
			}
		}

		public virtual Vector2i GlobalPosition
		{
			get => LocalPosition + (Parent?.GlobalPosition ?? new Vector2i(0, 0));
			set => LocalPosition = value - (Parent?.GlobalPosition ?? new Vector2i(0, 0));
		}

		public UIElement? GetChild(string id)
		{
			return Children.FirstOrDefault(t => t.ID == id);
		}

		public bool Contains(int x, int y) => BoundingBox.Contains(x,     y);
		public bool Contains(Vector2i vec) => BoundingBox.Contains(vec.X, vec.Y);

		public void AddChild(string id, UIElement child)
		{
			Children.Add(child);
			child.Parent = this;
			child.ID     = id;
		}

		public virtual void Draw(RenderTarget target, RenderStates states)
		{
			if (!Visible) return;

			foreach (UIElement child in Children)
				target.Draw(child, states);
		}
	}
}
