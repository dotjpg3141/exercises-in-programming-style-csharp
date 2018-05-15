using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_letterbox
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 Constraints:
				The larger problem is decomposed into 'things' that make sense for the problem domain
				Each 'thing' is a capsule of data that exposes one single procedure, namely the ability to receive and dispatch messages that are sent to it
				Message dispatch can result in sending the message to another capsule
			 */

			var controller = new WordFrequencyController();
			controller.Dispatch(DispatchMethod.Initialise, (args[0], args[1]));
			controller.Dispatch(DispatchMethod.Run);
		}
	}
}
