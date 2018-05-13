using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _10_things
{
	class WordFrequencyManager
	{
		private readonly Dictionary<string, int> wordFrequencies = new Dictionary<string, int>();

		public void AddWord(string word)
		{
			this.wordFrequencies.TryGetValue(word, out var count);
			this.wordFrequencies[word] = count + 1;
		}

		public IEnumerable<KeyValuePair<string, int>> GetSorted()
		{
			return this.wordFrequencies.OrderByDescending(kvp => kvp.Value);
		}
	}
}
