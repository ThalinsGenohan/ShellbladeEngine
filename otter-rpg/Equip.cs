using System;
using System.Collections.Generic;
using System.Text;

namespace OtterRPG
{
	public class Equip : BaseItem
	{
		public EquipCategory Category { get; set; }

		public enum EquipCategory
		{
			Head,
			Armor,
			Feet,
			Accessory,

			Weapon, // Split this later maybe?
		}
	}
}
