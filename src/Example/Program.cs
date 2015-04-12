using System;
using CacheIO;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			Cache cache = new Cache("../../cache/");

			Console.WriteLine("Cache contains " + cache.IndexList.Length + " indexes.");

			Console.ReadLine(); // Pause
		}
	}
}