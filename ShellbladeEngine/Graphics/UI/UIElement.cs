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

		public List<UIElement> Children { get; set; } = new List<UIElement>();

		public string ID { get; set; }

		public bool Hovered { get; internal set; } = false;
		public bool Visible { get; set; }          = true;

		public Action OnClick     { get; set; } = () => { };
		public Action OnMouseOver { get; set; } = () => { };
		public Action OnMouseOff  { get; set; } = () => { };

		public virtual Vector2i LocalPosition { get; set; }
		public virtual Vector2i Size          { get; set; }

		internal bool HoveredNow { get; set; } = false;

		public int TreeSize => 1 + Children.Sum(c => c.TreeSize);

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

		public UIElement GetChild(string id)
		{
			for (var i = 0; i < Children.Count; i++)
			{
				if (Children[i].ID == id)
					return Children[i];
			}

			return null;
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

			for (var i = 0; i < Children.Count; i++)
				target.Draw(Children[i], states);
		}
	}
}
