using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _04_cookbook
{
	static class Program
	{
		private static readonly List<string> words = new List<string>();
		private static readonly HashSet<string> stopWords = new HashSet<string>();
		private static readonly Dictionary<string, int> wordFrequencies = new Dictionary<string, int>();

		static void Main(string[] args)
		{
			/*
			 Constraints:
				Larger problem decomposed in procedural abstractions
				Larger problem solved as a sequence of commands, each corresponding to a procedure
			 */

			ReadWords(args[0]);
			NormalizeWords();

			ReadStopWords(args[1]);
			RemoveStopWordsAndSmallWords();

			CountWords();
			PrintResult();
		}

		static void ReadWords(string path)
		{
			var text = File.ReadAllText(path);
			words.AddRange(new Regex(@"[^0-9A-Za-z]+").Split(text));
		}

		static void ReadStopWords(string path)
		{
			var text = File.ReadAllText(path);
			stopWords.UnionWith(text.Split(','));
		}

		static void NormalizeWords()
		{
			for (int i = 0; i < words.Count; i++)
			{
				words[i] = words[i].ToLowerInvariant();
			}
		}

		static void RemoveStopWordsAndSmallWords()
		{
			int index = words.Count - 1;
			while (index >= 0)
			{
				var word = words[index];
				if (word.Length < 2 || stopWords.Contains(word))
				{
					words.RemoveAt(index);
				}
				index--;
			}
		}

		static void CountWords()
		{
			foreach (var word in words)
			{
				wordFrequencies.TryGetValue(word, out int count);
				wordFrequencies[word] = count + 1;
			}
		}

		static void PrintResult()
		{
			var topWords = wordFrequencies.OrderByDescending(kvp => kvp.Value).Take(25);
			foreach (var kvp in topWords)
			{
				Console.WriteLine(kvp.Key + "  -  " + kvp.Value);
			}
		}
	}
}
