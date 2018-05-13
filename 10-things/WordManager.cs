using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _10_things
{
	public class WordManager
	{
		private string path;

		public WordManager(string path)
		{
			this.path = path;
		}

		public string[] GetWords()
		{
			var text = File.ReadAllText(this.path);
			var words = new Regex("[^0-9a-zA-Z]+").Split(text);
			return words;
		}
	}
}
