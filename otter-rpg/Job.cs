using System.Collections.Generic;

namespace OtterRPG
{
	public class Job
	{
		public Dictionary<Character.StatWeight, float> StatWeights { get; }
		public List<Weapon.WeaponType>                 WeaponTypes { get; }

		public string Name        { get; set; }
		public string Description { get; set; }

		public Job(string name, string desc, Dictionary<Character.StatWeight, float> stats, List<Weapon.WeaponType> weapons)
		{
			Name        = name;
			Description = desc;
			StatWeights = stats;
			WeaponTypes = weapons;
		}
	}
}
