namespace OtterRPG
{
	public class Weapon : Equip
	{
		public enum WeaponType
		{
			Broadsword,
			Greatsword,
			Dagger,
			Staff,
			Bow,
			Shield,
		}

		public int BaseDamage { get; }

		public bool TwoHanded   { get; }
		public bool OffHandOnly { get; }
	}
}
