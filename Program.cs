using System;

namespace CharacterGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var a = new Character();
			Console.WriteLine($"{a.FirstName} {a.LastName}");
			Console.WriteLine($"{a.PrimaryOccupation}");
		}
	}
}