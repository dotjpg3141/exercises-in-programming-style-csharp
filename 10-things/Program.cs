using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_things
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Constraints:
				The larger problem is decomposed into 'things' that make sense for the problem domain
				Each 'thing' is a capsule of data that exposes procedures to the rest of the world
				Data is never accessed directly, only through these procedures
				Capsules can reappropriate procedures defined in other capsules
			 */

			var wordManager = new WordManager(args[0]);
			var stopWordManager = new WordManager(args[1]);
			var frequencyManager = new WordFrequencyManager();
			var filter = new WordFilter(stopWordManager);

			var controller = new FrequencyController(wordManager, filter, frequencyManager);
			controller.Run();
		}
	}
}
