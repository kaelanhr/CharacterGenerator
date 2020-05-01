using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CharacterGenerator
{
	public class Character
	{
		public string FirstName { get; private set; }
		public string LastName { get; private set; }

		// refactor later to have list of 'character traits' which are flaws/strengths
		public string[] Flaws { get; private set; }
		public string[] Strengths { get; private set; }

		public Dictionary<string,string> PrimaryOccupation { get; private set; }
		public string SecondaryOccupation { get; private set; }

		public Dictionary<string, string> PhysicalTraits { get; private set; }

		public static string[] GetLastNamesList()
		{
			return File.ReadAllLines("Resources/Last-Name-List.txt");
		}

		public static string[] GetFirstNamesList()
		{
			return File.ReadAllLines("Resources/First-Name-List.txt");
		}

		public static JArray GetJobs()
		{
			using StreamReader r = new StreamReader("Resources/Jobs-List.json");
			string json = r.ReadToEnd();
			return JsonConvert.DeserializeObject<JArray>(json);
		}

		private static readonly Random random = new Random();

		public Character()
		{
			FirstName = GetFirstNamesList()[random.Next(0, GetFirstNamesList().Length)];
			LastName = GetLastNamesList()[random.Next(0, GetLastNamesList().Length)];
			PrimaryOccupation = SelectRandomJob();
		}

		private Dictionary<string, string> SelectRandomJob()
		{
			// randomly select a job
			var job = random.Next(0, GetJobs().Count);

			// randomly select a Specialty
			var specialty = (string[]) GetJobs()[job]["Specialty"].ToObject(typeof(string[]));

			var rankString = "N/A";
			try
			{
				// randomly select a rank if applicable
				var rank = (string[])GetJobs()[job]["Rank"].ToObject(typeof(string[]));
				rankString += $"{rank[random.Next(0, rank.Length)]}";
			}
			catch (NullReferenceException) { }

			return new Dictionary<string, string>
			{
				{ "Role", GetJobs()[job]["Role"].ToString() },
				{"Specialty", specialty[random.Next(0, specialty.Length)] },
				{"Rank", rankString }
			};
		}
	}
}