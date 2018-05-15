using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_letterbox
{
	class WordFrequencyController : DispatcherBase
	{
		private DataStorageManager storageManager;
		private StopWordManager stopWordManager;
		private WordFrequencyManager wordFrequencyManager;

		public override object Dispatch(DispatchMethod method, object argument)
		{
			switch (method)
			{
				case DispatchMethod.Initialise:
					(string inputPath, string stopWordPath) = (ValueTuple<string, string>)argument;
					this.Initialize(inputPath, stopWordPath);
					return null;

				case DispatchMethod.Run:
					this.Run();
					return null;

				default:
					return base.Dispatch(method, argument);
			}
		}

		private void Initialize(string inputPath, string stopWordPath)
		{
			this.storageManager = new DataStorageManager();
			this.stopWordManager = new StopWordManager();
			this.wordFrequencyManager = new WordFrequencyManager();

			this.storageManager.Dispatch(DispatchMethod.Initialise, inputPath);
			this.stopWordManager.Dispatch(DispatchMethod.Initialise, stopWordPath);
		}


		private void Run()
		{
			foreach (var word in (IEnumerable<string>)this.storageManager.Dispatch(DispatchMethod.Words))
			{
				var isStopWord = (bool)this.stopWordManager.Dispatch(DispatchMethod.IsStopWord, word);
				if (!isStopWord)
				{
					this.wordFrequencyManager.Dispatch(DispatchMethod.AddWord, word);
				}
			}

			var freqs = (IEnumerable<KeyValuePair<string, int>>)this.wordFrequencyManager.Dispatch(DispatchMethod.Sorted);
			foreach (var kvp in freqs.Take(25))
			{
				Console.WriteLine(kvp.Key + "  -  " + kvp.Value);
			}
		}
	}
}
