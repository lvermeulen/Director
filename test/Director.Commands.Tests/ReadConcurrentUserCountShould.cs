using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Director.Abstractions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Director.Commands.Tests
{
	public class ReadConcurrentUserCountShould
	{
		private readonly ITestOutputHelper _testOutputHelper;
		private readonly IJobProvider _jobProvider = new ReadConcurrentUserCount(NullLogger<ReadConcurrentUserCount>.Instance);

		public ReadConcurrentUserCountShould(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public async Task ReadConcurrentUserCountAsync()
		{
			string result = await _jobProvider.GetScriptAsync(new Dictionary<string, string>
			{
				["esentAssemblyLocation"] = @"C:\Windows\System32\esent.dll",
				["esentSummaryLocation"] = Path.GetTempFileName()
			}).ConfigureAwait(false);

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	}
}
