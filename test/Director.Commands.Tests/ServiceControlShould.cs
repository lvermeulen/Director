using System.Collections.Generic;
using System.Threading.Tasks;
using Director.Abstractions;
using ICDAgent;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Director.Commands.Tests
{
	public class ServiceControlShould
	{
		private readonly ITestOutputHelper _testOutputHelper;
		private readonly IJobProvider _jobProvider = new ServiceControl(NullLogger<ServiceControl>.Instance);

		public ServiceControlShould(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public async Task StartServiceAsync()
		{
			string result = await _jobProvider.GetScriptAsync(new Dictionary<string, string>
			{
				["serviceControlAction"] = "Start",
				["serviceName"] = nameof(StartServiceAsync)
			}).ConfigureAwait(false);

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}

		[Fact]
		public async Task StopServiceAsync()
		{
			string result = await _jobProvider.GetScriptAsync(new Dictionary<string, string>
			{
				["serviceControlAction"] = "Stop",
				["serviceName"] = nameof(StopServiceAsync)
			}).ConfigureAwait(false);

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}

		[Fact]
		public async Task RestartServiceAsync()
		{
			string result = await _jobProvider.GetScriptAsync(new Dictionary<string, string>
			{
				["serviceControlAction"] = "Restart",
				["serviceName"] = nameof(RestartServiceAsync)
			}).ConfigureAwait(false);

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	}
}
