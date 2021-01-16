using System;
using System.Collections.Generic;
using System.Text;

namespace OtterRPG
{
	class Player : Character
	{
		public Player() : base() {}

		public Player(string name, uint level, uint str, uint inte, uint agi, uint vit, uint fort)
			: base(name, level, str, inte, agi, vit, fort)
		{
		}
	}
}
