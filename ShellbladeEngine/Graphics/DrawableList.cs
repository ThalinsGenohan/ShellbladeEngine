using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace Shellblade.Graphics
{
	public class DrawableList
	{
		internal List<DrawableItem> List { get; } = new List<DrawableItem>();

		public int Count => List.Count;

		public DrawableItem this[string id]
		{
			get => List.FirstOrDefault(item => item.Id == id);
			set
			{
				int i = List.FindIndex(item => item.Id == id);
				if (i == -1)
					List.Add(new DrawableItem(id, value.Drawable, value.Priority, value.Background));
				else
					List[i] = value;

				List.Sort((x, y) => x.Priority.CompareTo(y.Priority));
			}
		}

		internal DrawableItem this[int index] => List[index];

		public void Add(DrawableItem item)
		{
			this[item.Id] = item;
		}

		public void Add(string id, Drawable item, int priority = 0, bool background = false) =>
			Add(new DrawableItem(id, item, priority, background));

		public void Remove(string id)
		{
			List.Remove(List.Find(item => item.Id == id));
		}

		public class DrawableItem : Drawable
		{
			public string   Id         { get; }
			public Drawable Drawable   { get; set; }
			public int      Priority   { get; set; }
			public bool     Background { get; set; }

			public DrawableItem(string id, Drawable drawable, int priority, bool background)
			{
				Id         = id;
				Drawable   = drawable;
				Priority   = priority;
				Background = background;
			}

			public void Draw(RenderTarget target, RenderStates states)
			{
				target.Draw(Drawable, states);
			}
		}
	}
}
