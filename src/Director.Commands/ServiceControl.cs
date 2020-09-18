using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Director.Abstractions;
using Microsoft.Extensions.Logging;

namespace ICDAgent
{
	public class ServiceControl : IJobProvider
	{
		private readonly ILogger<ServiceControl> _logger;

		public ServiceControl(ILogger<ServiceControl> logger)
		{
			_logger = logger;
		}

		public Task<string> GetScriptAsync(IDictionary<string, string> parameters)
		{
			if (!parameters.ContainsKeys("serviceControlAction", "serviceName"))
			{
				_logger.LogError("ServiceControl GetScript failed: didn't receive all needed parameters");
				return Task.FromResult("");
			}

			string serviceControlAction = parameters["serviceControlAction"];
			_logger.LogTrace($"ServiceControl GetScript: serviceControlAction: {serviceControlAction}");
			string serviceName = parameters["serviceName"];
			_logger.LogTrace($"ServiceControl GetScript: serviceName: {serviceName}");

			string script;
			switch (serviceControlAction)
			{
				case "Start":
					script = $"Start-Service {serviceName}";
					break;
				case "Stop":
					script = $"Stop-Service {serviceName}";
					break;
				case "Restart":
					script = $"Restart-Service {serviceName}";
					break;
				default:
					throw new ArgumentException($"Invalid argument: {serviceControlAction}");
			}

			return Task.FromResult(script);
		}
	}
}