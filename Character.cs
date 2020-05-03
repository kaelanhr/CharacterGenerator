using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

		public static string[] GetLastNamesList() => File.ReadAllLines("Resources/Last-Name-List.txt");
		public static string[] GetFirstNamesList() => File.ReadAllLines("Resources/First-Name-List.txt");

		public static JArray GetJobs() => ReadJArrayFromFile("Resources/Jobs-List.json");
		public static JArray GetPhysicalTraits() => ReadJArrayFromFile("Resources/Physical-Traits-List.json");

		private static JArray ReadJArrayFromFile(string filePath)
		{
			using StreamReader r = new StreamReader(filePath);
			string json = r.ReadToEnd();
			return JsonConvert.DeserializeObject<JArray>(json);
		}

		private static readonly Random random = new Random();

		public Character()
		{
			FirstName = GetFirstNamesList()[random.Next(0, GetFirstNamesList().Length)];
			LastName = GetLastNamesList()[random.Next(0, GetLastNamesList().Length)];
			PrimaryOccupation = SelectRandomJob();
			PhysicalTraits = SelectPhysicalTraits();
		}

		private Dictionary<string, string> SelectRandomJob()
		{
			// randomly select a job
			var job = GetJobs()[random.Next(0, GetJobs().Count)];
			var specialty = job["Specialty"].ToObject<string[]>();
			var rank = job["Rank"]?.ToObject<string[]>();

			var rankString = rank != null ? GetRandomItem(rank) : "N/A";

			return new Dictionary<string, string>
			{
				{ "Role", job["Role"].ToString() },
				{"Specialty", GetRandomItem(specialty) },
				{"Rank", rankString }
			};
		}

		private Dictionary<string, string> SelectPhysicalTraits()
		{
			var traitDictionary = new Dictionary<string, string>();
			foreach(var trait in GetPhysicalTraits())
			{
				var traitList = trait["Types"].ToObject<string[]>();

				// if there is a chance and the role fails it will not show up in the physcal traits lsit
				if(trait["Chance"] != null)
				{
					var roll = random.Next(0, 100);
					if (roll > trait["Chance"].ToObject<int>())
					{
						continue;
					}
				}
				traitDictionary.Add(trait["Attribute"].ToString(), GetRandomItem(traitList));
			}
			return traitDictionary;
		}

		private static string GetRandomItem(string[] collection)
		{
			var index = random.Next(0, collection.Length);
			return collection[index];
		}
	}
}