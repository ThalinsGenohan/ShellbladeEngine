namespace OtterRPG
{
	public class Equip : BaseItem
	{
		public enum EquipCategory
		{
			Head,
			Armor,
			Feet,
			Accessory,

			Weapon, // Split this later maybe?
		}

		public EquipCategory Category { get; set; }
	}
}
