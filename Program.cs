using System;

namespace CharacterGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var a = new Character();
			Console.WriteLine($"{a.FirstName} {a.LastName}");
			Console.WriteLine($"Role: {a.PrimaryOccupation["Role"]}");
			Console.WriteLine($"Specialty: {a.PrimaryOccupation["Specialty"]}");
			Console.WriteLine($"Rank: {a.PrimaryOccupation["Rank"]}");
			Console.WriteLine("Physical Traits:");

			foreach (var item in a.PhysicalTraits)
			{
				Console.WriteLine($"{item.Key}: {item.Value}");
			}
		}
	}
}