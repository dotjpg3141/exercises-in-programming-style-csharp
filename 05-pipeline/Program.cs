using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _05_pipeline
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Constraints:
				Larger problem decomposed in functional abstractions. Functions, according to Mathematics, are relations from inputs to outputs.
				Larger problem solved as a pipeline of function applications
			 */

			PrintFrequencies(CountWordFrequencies(FilterStopWords(args[1], NormalizeWords(ExtractWords(ReadFile(args[0]))))));
		}

		private static string ReadFile(string path)
		{
			return File.ReadAllText(path);
		}

		private static IEnumerable<string> ExtractWords(string text)
		{
			return new Regex("[^0-9a-zA-Z]+").Split(text);
		}

		private static IEnumerable<string> NormalizeWords(IEnumerable<string> words)
		{
			return words.Where(w => w.Length >= 2).Select(w => w.ToLowerInvariant());
		}

		private static IEnumerable<string> FilterStopWords(string path, IEnumerable<string> words)
		{
			var stopWords = new HashSet<string>(ReadFile(path).Split(','));
			return words.Where(w => !stopWords.Contains(w));
		}

		private static Dictionary<string, int> CountWordFrequencies(IEnumerable<string> words)
		{
			var result = new Dictionary<string, int>();
			foreach (var word in words)
			{
				result.TryGetValue(word, out var count);
				result[word] = count + 1;
			}
			return result;
		}

		private static void PrintFrequencies(Dictionary<string, int> wordFrequencies)
		{
			foreach (var frequencies in wordFrequencies.OrderByDescending(w => w.Value).Take(25))
			{
				Console.WriteLine(frequencies.Key + "  -  " + frequencies.Value);
			}
		}
	}
}
