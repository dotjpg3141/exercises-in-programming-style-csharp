using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_go_forth
{
	internal static class Program
	{
		private static readonly Stack<object> Stack = new Stack<object>();
		private static readonly Dictionary<string, object> Heap = new Dictionary<string, object>();

		static void Push(object value) => Stack.Push(value);
		static void PushRange<T>(IEnumerable<T> value) => value.ToList().ForEach(t => Push(t));
		static T Pop<T>() => (T)Stack.Pop();
		static void Store(string name) => Heap[name] = Stack.Pop();
		static T Load<T>(string name) => (T)Heap[name];

		static void Dup()
		{
			var tmp = Stack.Pop();
			Push(tmp);
			Push(tmp);
		}

		static void Main(string[] args)
		{
			/*
			 Constraints:
				Existence of an all-important data stack. All operations (conditionals, arithmetic, etc.) are done over data on the stack
				Existence of a heap for storing data that's needed for later operations. The heap data can be associated with names (i.e. variables). As said above, all operations are done over data on the stack, so any heap data that needs to be operated upon needs to be moved first to the stack and eventually back to the heap
				Abstraction in the form of user-defined "procedures" (i.e. names bound to a set of instructions), which may be called something else entirely
			*/

			Push(args[0]);
			ReadFile();
			TokenizeWords();

			Push(args[2]);
			ReadFile();
			TokenizeStopWords();

			FrequencyCount();
			FrequencyCountList();
			Push(args[1]);
			PrintFrequencies();

			Console.WriteLine(string.Join("\n", Stack));
		}

		static void ReadFile()
		{
			Push(File.ReadAllText(Pop<string>()));
		}

		static void TokenizeWords()
		{
			Store("text");

			Push(0);
			Store("start");

			Push(0);
			Store("end");

			while (Load<int>("end") < Load<string>("text").Length)
			{
				Push(Load<string>("text")[Load<int>("end")]);
				if (char.IsLetterOrDigit(Pop<char>()))
				{
					Push(Load<int>("end") + 1);
					Store("end");
				}
				else
				{
					if (Load<int>("end") - Load<int>("start") >= 2)
					{
						Push(Load<string>("text").Substring(Load<int>("start"), Load<int>("end") - Load<int>("start")));
						Push(Pop<string>().ToLowerInvariant());
					}

					Push(Load<int>("end") + 1);
					Dup();
					Store("start");
					Store("end");
				}
			}
		}

		static void TokenizeStopWords()
		{
			Push(Stack.Count);
			Store("stack_start");

			Push(new HashSet<string>());
			Store("stop_words");

			TokenizeWords();

			while (Load<int>("stack_start") != Stack.Count)
			{
				Load<HashSet<string>>("stop_words").Add(Pop<string>());
			}
		}

		static void FrequencyCount()
		{
			Push(new Dictionary<string, int>());
			Store("word_freq");

			while (Stack.Count != 0)
			{
				Dup();
				if (Load<HashSet<string>>("stop_words").Contains(Pop<string>()))
				{
					Pop<string>();
				}
				else
				{
					Load<Dictionary<string, int>>("word_freq").Inc();
				}
			}
		}

		static void FrequencyCountList()
		{
			PushRange(Load<Dictionary<string, int>>("word_freq").OrderByDescending(k => k.Value).Take(25).Reverse());
		}

		static void PrintFrequencies()
		{
			using (var outputFs = new FileStream(Pop<string>(), FileMode.Create))
			using (var output = new StreamWriter(outputFs))
			{
				while (Stack.Count != 0)
				{
					Dup();
					output.Write(Pop<KeyValuePair<string, int>>().Key);
					output.Write("  -  ");
					output.Write(Pop<KeyValuePair<string, int>>().Value);
					output.WriteLine();
				}
			}
		}

		static void Inc<T>(this Dictionary<T, int> source)
		{
			Dup();
			source.TryGetValue(Pop<T>(), out int value);
			source[Pop<T>()] = value + 1;
		}
	}
}
