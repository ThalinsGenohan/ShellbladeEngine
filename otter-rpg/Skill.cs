using System;
using System.Collections.Generic;
using System.Text;

namespace OtterRPG
{
	public class Skill
	{
		public string Name        { get; set; }
		public string Description { get; set; }

		public Element    Element   { get; set; }
		public int        BasePower { get; set; }
		public TargetMode Target    { get; set; }

		public List<EffectFunction> Effects { get; set; }




		public delegate void EffectFunction(List<Character> chars);

		public enum TargetMode
		{
			SingleEnemy,
			AllEnemies,
			SingleAlly,
			AllAllies,
			Self,
			All,
		}
	}
}
