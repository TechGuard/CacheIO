using System;
using CacheIO;
using CacheIO.IO;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			Cache cache = new Cache("../../cache/");

			Random rand = new Random();
			int start = rand.Next(0, 23112 - 20);

			for(int i = start; i < start + 20; i++)
			{
				readItem(cache, i);
			}

			Console.ReadLine(); // Pause
		}

		private static void readItem(Cache cache, int id)
		{
			ItemDefinition item = new ItemDefinition(id);
			item.Load(cache);

			Console.WriteLine(id + ": " + item.name + " (" + item.modelId + ")");
		}
	}
}