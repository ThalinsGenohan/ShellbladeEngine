using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace Shellblade.Graphics
{
	public class DrawableList
	{
		private readonly List<DrawableItem> _list = new List<DrawableItem>();

		public DrawableItem this[string id]
		{
			get => _list.FirstOrDefault(item => item.Id == id);
			set
			{
				int i = _list.FindIndex(item => item.Id == id);
				if (i == -1)
					_list.Add(new DrawableItem(id, value.Drawable, value.Priority, value.Background));
				else
					_list[i] = value;

				_list.Sort((x, y) => x.Priority.CompareTo(y.Priority));
			}
		}

		internal DrawableItem this[int index] => _list[index];

		public void Add(DrawableItem item)
		{
			this[item.Id] = item;
		}

		public void Add(string id, Drawable item, int priority = 0, bool background = false) => Add(new DrawableItem(id, item, priority, background));

		public int Count => _list.Count;

		public void Remove(string id)
		{
			_list.Remove(_list.Find(item => item.Id == id));
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
