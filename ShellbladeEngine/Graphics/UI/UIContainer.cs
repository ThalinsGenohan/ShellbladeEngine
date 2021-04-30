using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade.Graphics.UI
{
	public class UIContainer : Drawable
	{
		public List<UIElement> Elements { get; set; } = new List<UIElement>();

		public bool Visible { get; set; } = true;

		public int TotalElementCount => Elements.Sum(c => c.TreeSize);

		private static bool DoChildClicks(UIElement element, Vector2i click)
		{
			if (element.TreeSize > 1)
			{
				for (int i = element.Children.Count - 1; i >= 0; i--)
				{
					if (DoChildClicks(element.Children[i], click))
						return true;
				}

				return false;
			}

			if (!element.Contains(click))
				return false;

			element.OnClick();
			return true;
		}

		private static bool DoChildMouseOvers(UIElement element, Vector2i pos)
		{
			if (element.TreeSize > 1)
			{
				for (int i = element.Children.Count - 1; i >= 0; i--)
				{
					if (DoChildMouseOvers(element.Children[i], pos))
						return true;
				}

				element.HoveredNow = true;
				if (!element.Hovered)
					element.OnMouseOver();
				return true;
			}

			if (!element.Contains(pos))
				return false;

			element.HoveredNow = true;
			if (!element.Hovered)
				element.OnMouseOver();
			return true;
		}

		private static void DoChildMouseOffs(UIElement element)
		{
			if (element.TreeSize > 1)
				for (int i = element.Children.Count - 1; i >= 0; i--)
					DoChildMouseOffs(element.Children[i]);

			if (element.Hovered && !element.HoveredNow)
				element.OnMouseOff();

			element.Hovered    = element.HoveredNow;
			element.HoveredNow = false;
		}

		public void AddElement(string id, UIElement element)
		{
			Elements.Add(element);
			element.ID = id;
		}

		public void RemoveElement(string id)
		{
			for (var i = 0; i < Elements.Count; i++)
			{
				if (Elements[i].ID != id) continue;

				Elements.Remove(Elements[i]);
				return;
			}
		}

		public void OnMouseButtonPressed(object sender, MouseButtonEventArgs args)
		{
			var rw    = (RenderWindow)sender;
			var click = (Vector2i)rw.MapPixelToCoords(new Vector2i(args.X, args.Y), rw.GetView());

			for (int i = Elements.Count - 1; i >= 0; i--)
			{
				if (DoChildClicks(Elements[i], click))
					break;
			}
		}

		public void OnMouseMoved(object sender, MouseMoveEventArgs args)
		{
			var rw  = (RenderWindow)sender;
			var pos = (Vector2i)rw.MapPixelToCoords(new Vector2i(args.X, args.Y), rw.GetView());

			for (int i = Elements.Count - 1; i >= 0; i--)
			{
				if (DoChildMouseOvers(Elements[i], pos))
					break;
			}

			for (int i = Elements.Count - 1; i >= 0; i--) DoChildMouseOffs(Elements[i]);
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			if (!Visible) return;

			for (var i = 0; i < Elements.Count; i++)
				target.Draw(Elements[i]);
		}
	}
}
