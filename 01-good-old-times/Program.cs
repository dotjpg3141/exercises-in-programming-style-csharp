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

			/*
				Constraints:
					Very small amount of primary memory, typically orders of magnitude smaller than the data that needs to be processed/generated.
					No labels -- i.e. no variable names or tagged memory addresses. All we have is memory that is addressable with numbers.
					No memory allocation apart from primary memory and IO streams
			 */

			//var input = args[0];
			//var output = args[1];
			//var stop = args[2];

			// word count 25
			// max word length 15

			var data = new int[1024 * 128];

			/*
				Memory layout
					000..009               variables
					010..034               current word
					035..600               stop words
					600+(n*16)             frequency of word n
					601+(n*16)..615+(n*16) word n
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

			// NOTE(jpg): read words, filter stop words and count frequency
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
							// filter stop words
							data[8] = 35;
							while (data[8] < data[0])
							{
								for (data[9] = 0; data[9] < data[2]; data[9]++)
								{
									if (data[data[8]] == ',')
										break; // end of current stop word

									if (data[data[8]] != data[10 + data[9]])
										break; // stop word does not match current word

									data[8]++;
								}

								if (data[data[8]] == ',' && data[9] == data[2])
								{
									// current word matches stop word
									data[8] = int.MaxValue;
								}
								else
								{
									// go to next stop word
									while (data[data[8]] != ',')
									{
										data[8]++;
									}
									data[8]++;
								}
							}

							// increase word counter if no stop word
							if (data[8] != int.MaxValue)
							{
								// foreach already saved word
								for (data[8] = 0; data[8] < data[3]; data[8]++)
								{
									// foreach character in word
									for (data[9] = 0; data[9] < 15; data[9]++)
									{
										if (data[601 + data[9] + 16 * data[8]] == 0)
											break; // end of word

										if (data[10 + data[9]] != data[601 + data[9] + 16 * data[8]])
										{
											data[9] = -1;
											break;
										}
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
						}

						data[2] = 0;

						if (data[1] == -1)
							break;
					}
				}
			}

			using (var outputFs = new FileStream(args[1], FileMode.Create))
			using (var output = new StreamWriter(outputFs))
			{
				////NOTE(jpg): print word frequencys
				//for (data[8] = 0; data[8] < data[3]; data[8]++)
				//{
				//	output.Write(data[600 + 16 * data[8]]);
				//	output.Write(": ");

				//	// foreach character in word
				//	for (data[9] = 0; data[9] < 15; data[9]++)
				//	{
				//		if (data[601 + data[9] + 16 * data[8]] == 0) break;
				//		output.Write((char)data[601 + data[9] + 16 * data[8]]);
				//	}
				//	output.WriteLine();
				//}

				data[2] = int.MaxValue; // last printed word
				data[7] = int.MaxValue; // max count required

				// NOTE(jpg): print N=25 words with most count
				for (data[10] = 0; data[10] < 25 && data[10] < data[3]; data[10]++)
				{
					data[4] = 0; // current best word
					data[5] = 0; // current count
					data[6] = 0; // min count required

					for (data[8] = 0; data[8] < data[3]; data[8]++)
					{
						data[5] = data[600 + 16 * data[8]];

						if (data[6] < data[5] && (data[5] < data[7] || (data[5] == data[7] && data[8] > data[2])))
						{
							data[6] = data[5];
							data[4] = data[8];
						}
					}

					// foreach character in word
					for (data[9] = 0; data[9] < 15; data[9]++)
					{
						if (data[601 + data[9] + 16 * data[4]] == 0) break;
						output.Write((char)data[601 + data[9] + 16 * data[4]]);
					}

					output.Write("  -  ");
					output.Write(data[600 + 16 * data[4]]);
					output.WriteLine();

					data[2] = data[4];
					data[7] = data[6];
				}
			}
		}
	}
}
