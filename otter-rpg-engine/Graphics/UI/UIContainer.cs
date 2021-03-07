using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade.Graphics.UI
{
	public class UIContainer : Drawable
	{
		public Dictionary<string, UIElement> Elements { get; set; } = new Dictionary<string, UIElement>();

		public void OnMouseButtonPressed(object sender, MouseButtonEventArgs args)
		{
			var rw    = (RenderWindow)sender;
			var click = (Vector2i)rw.MapPixelToCoords(new Vector2i(args.X, args.Y), rw.GetView());

			foreach (UIElement element in Elements.Values.Where(e => e.Contains(click)))
				element.OnClick();
		}

		public void OnMouseMoved(object sender, MouseMoveEventArgs args)
		{
			var rw  = (RenderWindow)sender;
			var pos = (Vector2i)rw.MapPixelToCoords(new Vector2i(args.X, args.Y), rw.GetView());

			foreach (UIElement element in Elements.Values)
			{
				switch (element.Hovered)
				{
					case false when element.Contains(pos):
						element.Hovered = true;
						element.OnMouseOver();
						break;
					case true when !element.Contains(pos):
						element.Hovered = false;
						element.OnMouseOff();
						break;
				}
			}
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			foreach (UIElement element in Elements.Values) target.Draw(element, states);
		}
	}
}
