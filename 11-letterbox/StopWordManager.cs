using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_letterbox
{
	class StopWordManager : DispatcherBase
	{
		private HashSet<string> words;

		public override object Dispatch(DispatchMethod method, object argument)
		{
			switch (method)
			{
				case DispatchMethod.Initialise:
					Initialize((string)argument);
					return null;

				case DispatchMethod.IsStopWord:
					return IsStopWord((string)argument);

				default:
					return base.Dispatch(method, argument);
			}
		}

		private void Initialize(string filePath)
		{
			var dataStorage = new DataStorageManager();
			dataStorage.Dispatch(DispatchMethod.Initialise, filePath);

			this.words = new HashSet<string>((IEnumerable<string>)dataStorage.Dispatch(DispatchMethod.Words));
		}

		private object IsStopWord(string word)
		{
			return this.words.Contains(word);
		}
	}
}
