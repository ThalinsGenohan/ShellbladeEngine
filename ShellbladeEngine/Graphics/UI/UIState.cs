using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace Shellblade.Graphics.UI
{
	public class UIState : Drawable
	{
		public List<UIElement> Elements { get; set; } = new();

		public bool Visible { get; set; } = true;

		public int TotalElementCount => Elements.Sum(c => c.TreeSize);
		
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
		
		public void Draw(RenderTarget target, RenderStates states)
		{
			if (!Visible) return;

			foreach (UIElement element in Elements)
				target.Draw(element);
		}
	}
}
