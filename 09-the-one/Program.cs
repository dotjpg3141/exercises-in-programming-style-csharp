using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _09_the_one
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Constraints:
				Existence of an abstraction to which values can be converted.
				This abstraction provides operations to (1) wrap around values, so that they become the abstraction; (2) bind itself to functions, so to establish sequences of functions; and (3) unwrap the value, so to examine the final result.
				Larger problem is solved as a pipeline of functions bound together, with unwrapping happening at the end.
				Particularly for The One style, the bind operation simply calls the given function, giving it the value that it holds, and holds on to the returned value.
			 */

			var stopWords = new TheOne<string>(args[2])
				.Bind(ReadText)
				.Bind(TokenizeWords)
				.Unwrap();

			var frequencies = new TheOne<string>(args[0])
				.Bind(ReadText)
				.Bind(TokenizeWords)
				.Bind(NormalizeWords)
				.Bind(words => FilterWords(words, stopWords))
				.Bind(CountWords)
				.Bind(LimitWords)
				.Unwrap();

			PrintResult(frequencies, args[1]);
		}

		static string ReadText(string path)
		{
			return File.ReadAllText(path);
		}

		static string[] TokenizeWords(string text)
		{
			return new Regex("[^0-9a-zA-Z]+").Split(text);
		}

		static string[] NormalizeWords(string[] words)
		{
			return words.Select(w => w.ToLowerInvariant()).ToArray();
		}

		static string[] FilterWords(string[] words, string[] stopWords)
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
			return result.ToArray();
		}

		static Dictionary<string, int> CountWords(string[] words)
		{
			var frequencies = new Dictionary<string, int>();
			foreach (var word in words)
			{
				frequencies.TryGetValue(word, out var count);
				frequencies[word] = count + 1;
			}
			return frequencies;
		}

		static KeyValuePair<string, int>[] LimitWords(Dictionary<string, int> words)
		{
			return words.OrderByDescending(kvp => kvp.Value).Take(25).ToArray();
		}

		private static void PrintResult(KeyValuePair<string, int>[] frequencies, string path)
		{
			using (var writer = new StreamWriter(path))
			{
				foreach (var kvp in frequencies)
				{
					writer.WriteLine(kvp.Key + "  -  " + kvp.Value);
				}
			}
		}
	}

	struct TheOne<T>
	{
		private readonly T value;

		public TheOne(T value)
		{
			this.value = value;
		}

		public TheOne<U> Bind<U>(Func<T, U> func)
		{
			return new TheOne<U>(func(this.value));
		}

		public T Unwrap()
		{
			return this.value;
		}
	}
}
