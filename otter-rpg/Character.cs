using System;
using System.Collections.Generic;
using System.Text;

namespace OtterRPG
{
	public class Character
	{
		public string Name         { get; set; }
		public uint   Level        { get; set; }
		public uint   HP           { get; set; }
		public uint   MaxHP        { get; set; }
		public uint   Defense      { get; set; }
		public uint   MagicDefense { get; set; }
		public uint   Actions      { get; set; }

		public Stat Strength  { get; } // STR Physical attack power
		public Stat Intellect { get; } // INT Magical attack power
		public Stat Agility   { get; } // AGI Actions per turn
		public Stat Vitality  { get; } // VIT Physical defense
		public Stat Fortitude { get; } // FOR Magical defense

		// Job
		// Equips
		// Skills
		// Passives
		// Resistances

		public Character()
		{
			Name         = "";
			Level        = 0;
			HP           = 0;
			MaxHP        = 0;
			Defense      = 0;
			MagicDefense = 0;
			Actions      = 0;

			Strength  = new Stat(0);
			Intellect = new Stat(0);
			Agility = new Stat(0);
			Vitality = new Stat(0);
			Fortitude = new Stat(0);
		}

		public Character(string name, uint level, uint str, uint inte, uint agi, uint vit, uint fort)
		{
			Name         = name;
			Level        = level;
			HP           = 0;
			MaxHP        = 0;
			Defense      = 0;
			MagicDefense = 0;
			Actions      = 0;

			Strength  = new Stat(str);
			Intellect = new Stat(inte);
			Agility   = new Stat(agi);
			Vitality  = new Stat(vit);
			Fortitude = new Stat(fort);
		}

		public class Stat
		{
			public uint  Base;
			public float JobBonus;
			public float EquipBonus;
			public float Buffs;

			public uint Total => (uint)MathF.Round(Base * JobBonus * EquipBonus * Buffs);

			public Stat(uint value)
			{
				Base       = value;
				JobBonus   = 1f;
				EquipBonus = 1f;
				Buffs      = 1f;
			}
		}
	}
}
