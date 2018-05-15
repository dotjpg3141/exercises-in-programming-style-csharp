using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_letterbox
{
	abstract class DispatcherBase
	{
		public object Dispatch(DispatchMethod method)
		{
			return Dispatch(method, null);
		}

		public virtual object Dispatch(DispatchMethod method, object argument)
		{
			throw new Exception($"Invalid dispatch method {method}");
		}
	}
}
