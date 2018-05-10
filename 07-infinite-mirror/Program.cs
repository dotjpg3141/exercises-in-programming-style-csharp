using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace _07_infinite_mirror
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Constraints:
				All, or a significant part, of the problem is modelled by induction. That is, specify the base case (n_0) and then the n+1 rule
			 */

			int maxStackSize = 1024 * 1024 * 32;
			var thread = new Thread(() => Run(args), maxStackSize);
			thread.Start();
			thread.Join();
		}

		static void Run(string[] args)
		{
			var words = TokenizeWordFile(args[0]);
			var stopWords = new HashSet<string>(TokenizeWordFile(args[2]));

			var frequencies = CountFrequencies(words, stopWords).ToArray();
			Sort(frequencies, (a, b) => b.Value - a.Value);

			using (var writer = new StreamWriter(args[1]))
				PrintFrequencies(frequencies, writer, 25);
		}

		static string[] TokenizeWordFile(string inputPath)
		{
			var text = File.ReadAllText(inputPath);
			return new Regex("[^0-9a-zA-Z]").Split(text);
		}

		static Dictionary<string, int> CountFrequencies(string[] words, HashSet<string> stopWords, int index = 0)
		{
			if (words.Length == index)
			{
				return new Dictionary<string, int>();
			}
			else
			{
				var result = CountFrequencies(words, stopWords, index + 1);

				var word = words[index].ToLowerInvariant();
				if (word.Length >= 2 && !stopWords.Contains(word))
				{
					result.TryGetValue(word, out int count);
					result[word] = count + 1;
				}

				return result;
			}
		}

		static void Sort<T>(T[] words, Func<T, T, int> comparer, int index = 0)
		{
			if (index == words.Length)
			{
				// noop
			}
			else
			{
				var minIndex = index;
				for (int i = index + 1; i < words.Length; i++)
				{
					if (comparer(words[i], words[minIndex]) < 0)
					{
						minIndex = i;
					}
				}

				var temp = words[index];
				words[index] = words[minIndex];
				words[minIndex] = temp;

				Sort(words, comparer, index + 1);
			}
		}

		static void PrintFrequencies(KeyValuePair<string, int>[] frequencies, StreamWriter writer, int count)
		{
			if (count == 0)
			{
				// noop
			}
			else
			{
				PrintFrequencies(frequencies, writer, count - 1);
				writer.WriteLine(frequencies[count - 1].Key + "  -  " + frequencies[count - 1].Value);
			}
		}
	}
}
