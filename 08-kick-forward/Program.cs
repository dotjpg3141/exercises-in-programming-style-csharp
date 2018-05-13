using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _08_kick_forward
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Variation of the pipeline style, with the following additional constraints:
				Each function takes an additional parameter, usually the last, which is another function
				That function parameter is applied at the end of the current function
				That function parameter is given as input what would be the output of the current function
				Larger problem is solved as a pipeline of functions, but where the next function to be applied is given as parameter to the current function
			 */

			ReadFile(args[0], wordsText =>
			{
				FindWords(wordsText, words =>
				{
					NormalizeWords(words, () =>
					{
						ReadFile(args[2], stopWordText =>
						{
							FindWords(stopWordText, stopwWords =>
							{
								FilterWords(words, stopwWords, filteredWords =>
								{
									CountWords(filteredWords, freqeuncies => PrintWords(freqeuncies, args[1]));
								});
							});
						});
					});
				});
			});
		}

		private static void ReadFile(string path, Action<string> callback)
		{
			var text = File.ReadAllText(path);

			callback(text);
		}

		private static void FindWords(string text, Action<string[]> callback)
		{
			var words = new Regex("[^0-9a-zA-Z]+").Split(text);

			callback(words);
		}

		private static void NormalizeWords(string[] words, Action callback)
		{
			for (int i = 0; i < words.Length; i++)
			{
				words[i] = words[i].ToLowerInvariant();
			}

			callback();
		}

		private static void FilterWords(string[] words, string[] stopWords, Action<string[]> callback)
		{
			var set = new HashSet<string>(stopWords);
			var result = new List<string>();
			foreach (var word in words)
			{
				if (word.Length >= 2 && !set.Contains(word))
				{
					result.Add(word);
				}
			}

			callback(result.ToArray());
		}

		private static void CountWords(string[] words, Action<Dictionary<string, int>> callback)
		{
			var frequencies = new Dictionary<string, int>();
			foreach (var word in words)
			{
				frequencies.TryGetValue(word, out var count);
				frequencies[word] = count + 1;
			}

			callback(frequencies);
		}

		private static void PrintWords(Dictionary<string, int> words, string path)
		{
			var top = words.OrderByDescending(kvp => kvp.Value).Take(25);

			using (var writer = new StreamWriter(path))
			{
				foreach (var kvp in top)
				{
					writer.WriteLine(kvp.Key + "  -  " + kvp.Value);
				}
			}
		}
	}
}
