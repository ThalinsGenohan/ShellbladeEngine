using System;
using System.Collections.Generic;
using System.Text;

namespace OtterRPG
{
	public class Item : BaseItem
	{
		public ItemCategory Category    { get; set; }


		public enum ItemCategory
		{
			Restorative,
		}
	}
}
