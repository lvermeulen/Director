using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Director.Abstractions;

namespace Director.Commands
{
	public static class PowershellCommand
	{
		public static Result<ICollection> RunScript(string script, Dictionary<string, object> scriptParameters = null)
		{
			ICollection collection;
			using (var powershell = PowerShell.Create())
			{
				powershell.AddScript(script);
				if (scriptParameters != null)
				{
					powershell.AddParameters(scriptParameters);
				}
				collection = powershell.Invoke();

				if (powershell.HadErrors)
				{
					var errors = powershell.Streams.Error.ToList()
						.Select(x => x.ToString());
					return Result<ICollection>.Fail(errors);
				}
			}

			return Result<ICollection>.From(collection);
		}
	}
}
