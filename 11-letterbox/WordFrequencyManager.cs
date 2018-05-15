using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_letterbox
{
	class WordFrequencyManager : DispatcherBase
	{
		private readonly Dictionary<string, int> wordFrequencies = new Dictionary<string, int>();

		public override object Dispatch(DispatchMethod method, object argument)
		{
			switch (method)
			{
				case DispatchMethod.AddWord:
					AddWord((string)argument);
					return null;

				case DispatchMethod.Sorted:
					return Sorted();

				default:
					return base.Dispatch(method, argument);
			}
		}

		private void AddWord(string word)
		{
			this.wordFrequencies.TryGetValue(word, out var count);
			this.wordFrequencies[word] = count + 1;
		}

		private object Sorted()
		{
			return this.wordFrequencies.OrderByDescending(kvp => kvp.Value);
		}
	}
}
