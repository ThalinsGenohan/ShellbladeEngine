using System.Collections.Generic;

namespace OtterRPG
{
	public class Skill
	{
		public enum TargetMode
		{
			SingleEnemy,
			AllEnemies,
			SingleAlly,
			AllAllies,
			Self,
			All,
		}


		public delegate void EffectFunction(List<Character> chars);

		public string Name        { get; set; }
		public string Description { get; set; }

		public Element    Element   { get; set; }
		public int        BasePower { get; set; }
		public TargetMode Target    { get; set; }

		public List<EffectFunction> Effects { get; set; }
	}
}
