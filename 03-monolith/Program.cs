using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_monolith
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Constraints:
				No abstractions
				No use of library functions
			 */

			var inputPath = args[0];
			var outputPath = args[1];
			var stopWordPath = args[2];

			var stopWords = File.ReadAllText(stopWordPath).Split(',');

			var wordCount = new List<(int count, string word)>();

			using (var input = new StreamReader(inputPath))
			{
				string line;
				while ((line = input.ReadLine()) != null)
				{
					int start = 0;
					int end = 0;

					while (end <= line.Length)
					{
						var c = end != line.Length ? line[end] : '\n';
						if (char.IsLetterOrDigit(c))
						{
							end++;
						}
						else
						{
							var length = end - start;
							if (length >= 2)
							{
								var word = line.Substring(start, length).ToLowerInvariant();

								bool isStopWord = false;
								foreach (var stopWord in stopWords)
								{
									if (stopWord == word)
									{
										isStopWord = true;
										break;
									}
								}

								if (!isStopWord)
								{
									int index = -1;
									for (int i = 0; i < wordCount.Count; i++)
									{
										if (wordCount[i].word == word)
										{
											index = i;
											break;
										}
									}

									if (index == -1)
									{
										wordCount.Add((1, word));
									}
									else
									{
										var tuple = wordCount[index];
										tuple.count++;
										wordCount[index] = tuple;

										while (index > 0 && wordCount[index].count > wordCount[index - 1].count)
										{
											var temp = wordCount[index];
											wordCount[index] = wordCount[index - 1];
											wordCount[index - 1] = temp;
											index--;
										}
									}
								}
							}

							start = ++end;
						}
					}
				}

				using (var output = new StreamWriter(outputPath))
				{
					for (int i = 0; i < 25; i++)
					{
						output.WriteLine(wordCount[i].word + "  -  " + wordCount[i].count);
					}
				}
			}
		}
	}
}
