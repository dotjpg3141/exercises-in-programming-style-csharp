using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_things
{
	public class WordFilter
	{
		private readonly HashSet<string> words;

		public WordFilter(WordManager wordManager)
		{
			this.words = new HashSet<string>(wordManager.GetWords());
		}

		public bool Include(string word)
		{
			return word.Length >= 2 && !this.words.Contains(word);
		}
	}
}
