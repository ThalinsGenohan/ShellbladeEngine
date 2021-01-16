using System;

namespace OtterRPG
{
	class Program
	{
		static void Main(string[] args)
		{
			Player p1 = new Player("Thalins", 1, 1, 1, 1, 1, 1);

			p1.HP = p1.MaxHP = 100;

			Console.WriteLine($"{p1.Name} Lv {p1.Level}\n" +
			                  $"- STR: {p1.Strength.Total}\n" +
			                  $"- INT: {p1.Intellect.Total}\n" +
			                  $"- AGI: {p1.Agility.Total}\n" +
			                  $"- VIT: {p1.Vitality.Total}\n" +
			                  $"- FOR: {p1.Fortitude.Total}\n");
		}
	}
}
