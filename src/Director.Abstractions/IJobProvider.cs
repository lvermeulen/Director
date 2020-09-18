using System.Collections.Generic;
using System.Threading.Tasks;

namespace Director.Abstractions
{
	public interface IJobProvider
	{
		Task<string> GetScriptAsync(IDictionary<string, string> parameters);
	}
}
