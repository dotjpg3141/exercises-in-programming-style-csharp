using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _06_code_golf
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Constraints:
			    As few lines of code as possible
			 */

			var stopWords = new HashSet<string>(File.ReadAllText(args[2]).Split(','));
			File.WriteAllText(args[1], string.Join("", new Regex("[^0-9a-zA-Z]+").Split(File.ReadAllText(args[0])).Select(w => w.ToLowerInvariant()).Where(w => w.Length >= 2 && !stopWords.Contains(w)).GroupBy(w => w).OrderByDescending(g => g.Count()).Take(25).Select(g => $"{g.Count()}  -  {g.Key}\n")));
		}
	}
}
