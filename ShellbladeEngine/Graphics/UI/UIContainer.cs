using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade.Graphics.UI
{
	public class UIContainer : Drawable
	{

		public int ElementCount => Elements.Values.Sum(c => c.ChildCount);

		public Dictionary<string, UIElement> Elements { get; set; } = new Dictionary<string, UIElement>();

		public bool Visible { get; set; } = true;

		public void OnMouseButtonPressed(object sender, MouseButtonEventArgs args)
		{
			var rw    = (RenderWindow)sender;
			var click = (Vector2i)rw.MapPixelToCoords(new Vector2i(args.X, args.Y), rw.GetView());

			foreach (UIElement element in Elements.Values)
			{
				if (element.Contains(click)) element.OnClick();

				foreach (UIElement child in element.Children.Values.Where(e => e.Contains(click)))
					child.OnClick();
			}
		}

		public void OnMouseMoved(object sender, MouseMoveEventArgs args)
		{
			var rw  = (RenderWindow)sender;
			var pos = (Vector2i)rw.MapPixelToCoords(new Vector2i(args.X, args.Y), rw.GetView());


			foreach (UIElement element in Elements.Values)
			{
				DoMouseHover(element, pos);
				foreach (UIElement child in element.Children.Values)
					DoMouseHover(child, pos);
			}
		}

		private static void DoMouseHover(UIElement element, Vector2i mousePos)
		{
			switch (element.Hovered)
			{
				case false when element.Contains(mousePos):
					element.Hovered = true;
					element.OnMouseOver();
					break;
				case true when !element.Contains(mousePos):
					element.Hovered = false;
					element.OnMouseOff();
					break;
			}
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			if (!Visible) return;
			foreach (UIElement element in Elements.Values.Where(e => e.Visible)) target.Draw(element, states);
		}
	}
}
