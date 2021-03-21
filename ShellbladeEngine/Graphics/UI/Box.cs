using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace Shellblade.Graphics.UI
{
	public class Box : UIElement
	{
		internal RectangleShape rectangle;

		public override Vector2i GlobalPosition
		{
			get => base.GlobalPosition;
			set
			{
				base.GlobalPosition = value;
				rectangle.Position = (Vector2f)value;
			}
		}

		public override Vector2i Size
		{
			get => base.Size;
			set
			{
				base.Size = value;
				rectangle.Size = (Vector2f)value;
			}
		}

		public Box()
        {
			rectangle = new RectangleShape(new Vector2f(0f, 0f))
			{
				Position = (Vector2f)GlobalPosition,
				Size = (Vector2f)Size
			};
        }

		public override void Draw(RenderTarget target, RenderStates states)
		{
			//THIS CLASS IS FOR TRANSPARENT BUT FUNCTIONAL "WRAPPERS", AND WONT BE DRAWN
		}
	}
}
