using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_good_old_times
{
	class Program
	{
		static void Main(string[] args)
		{
			//var input = args[0];
			//var output = args[1];
			//var stop = args[2];

			// word count 25
			// max word length 15

			var data = new int[1024 * 96];

			/*
				000..009  variables
				010..034  current word
				035..600  stop words
				600       frequency word 1
				601..615  characters word 1
				616..631  word2
				...
			 */

			data[0] = 35;   // end of stop words
			data[1] = '\0'; // current char
			data[2] = 0;    // current word length
			data[3] = 0;    // words count

			// NOTE(jpg): read stop words
			using (var stopWords = new FileStream(args[2], FileMode.Open))
			{
				while (true)
				{
					data[data[0]] = stopWords.ReadByte();
					if (data[data[0]] == -1) break;
					data[0]++;
				}
			}

			// NOTE(jpg): read words and count frequency
			using (var input = new FileStream(args[0], FileMode.Open))
			{
				while (true)
				{
					data[1] = input.ReadByte();

					if (char.IsLetterOrDigit((char)data[1]))
					{
						if (data[2] < 15)
						{
							data[10 + data[2]] = char.ToLowerInvariant((char)data[1]);
							data[2]++;
						}
					}
					else
					{
						if (data[2] >= 2) // found word
						{
							// foreach already saved word
							for (data[8] = 0; data[8] < data[3]; data[8]++)
							{
								// foreach character in word
								for (data[9] = 0; data[9] < data[2]; data[9]++)
								{
									if (data[10 + data[9]] != data[601 + data[9] + 16 * data[8]])
										break;
								}

								if (data[9] == data[2]) // word already saved
								{
									data[600 + 16 * data[8]]++; // increase frequency
									break;
								}
							}

							// no match found, save word
							if (data[8] == data[3])
							{
								data[3]++;

								data[600 + 16 * data[8]] = 1;
								for (data[9] = 0; data[9] < data[2]; data[9]++)
								{
									data[601 + data[9] + 16 * data[8]] = data[10 + data[9]];
								}
							}

						}

						data[2] = 0;

						if (data[1] == -1)
							break;
					}
				}
			}

			for (data[8] = 0; data[8] < data[3]; data[8]++)
			{
				Console.Write(data[600 + 16 * data[8]]);
				Console.Write(": ");

				// foreach character in word
				for (data[9] = 0; data[9] < 15; data[9]++)
				{
					Console.Write((char)data[601 + data[9] + 16 * data[8]]);
				}
				Console.WriteLine();
			}
		}
	}
}
