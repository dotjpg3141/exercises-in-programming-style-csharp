using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _11_letterbox
{
	class DataStorageManager : DispatcherBase
	{
		private string text;

		public override object Dispatch(DispatchMethod method, object argument)
		{
			switch (method)
			{
				case DispatchMethod.Initialise:
					this.Initialize((string)argument);
					return null;

				case DispatchMethod.Words:
					return GetWords();

				default:
					return base.Dispatch(method, argument);
			}
		}

		private void Initialize(string filePath)
		{
			this.text = File.ReadAllText(filePath);
		}

		private IEnumerable<string> GetWords()
		{
			return new Regex("[^0-9a-zA-Z]+").Split(this.text)
				.Select(w => w.ToLowerInvariant())
				.Where(w => w.Length >= 2);
		}
	}
}
