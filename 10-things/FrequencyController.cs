using System;
using System.Linq;
using System.Threading;

namespace _10_things
{
	class FrequencyController
	{
		private readonly WordManager wordManager;
		private readonly WordFilter filter;
		private readonly WordFrequencyManager frequencyManager;

		public FrequencyController(WordManager wordManager, WordFilter filter, WordFrequencyManager frequencyManager)
		{
			this.wordManager = wordManager;
			this.filter = filter;
			this.frequencyManager = frequencyManager;
		}

		public void Run()
		{
			foreach (var word in this.wordManager.GetWords())
			{
				var currentWord = word.ToLowerInvariant();
				if (this.filter.Include(currentWord))
				{
					this.frequencyManager.AddWord(currentWord);
				}
			}

			foreach (var wordFrequency in this.frequencyManager.GetSorted().Take(25))
			{
				Console.WriteLine(wordFrequency.Key + "  -  " + wordFrequency.Value);
			}
		}
	}
}
