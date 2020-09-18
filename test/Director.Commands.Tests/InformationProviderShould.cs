using Xunit;
using Xunit.Abstractions;

namespace Director.Commands.Tests
{
	public class InformationProviderShould
	{
		private readonly ITestOutputHelper _testOutputHelper;
		private readonly InformationProvider _informationProvider = new InformationProvider();

		public InformationProviderShould(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void GetLastBootUpTime()
		{
			string result = _informationProvider.GetLastBootUpTime();

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	
		[Fact]
		public void GetOsName()
		{
			string result = _informationProvider.GetOsName();

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	
		[Fact]
		public void GetOsVersion()
		{
			string result = _informationProvider.GetOsVersion();

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	
		[Fact]
		public void GetOsServicePack()
		{
			string result = _informationProvider.GetOsServicePack();

			Assert.NotNull(result);
			_testOutputHelper.WriteLine(result);
		}
	
		[Fact]
		public void GetIpAddress()
		{
			string result = _informationProvider.GetIpAddress();

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	
		[Fact]
		public void GetMmcVersion()
		{
			string result = _informationProvider.GetMmcVersion();

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	
		[Fact]
		public void GetPowershellVersion()
		{
			string result = _informationProvider.GetPowershellVersion();

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	
		[Theory]
		[InlineData("Data Source=.;Initial Catalog=master;Integrated Security=true;")]
		public void GetMssqlInformation(string connectionString)
		{
			string result = _informationProvider.GetMssqlInformation(connectionString);

			Assert.NotNull(result);
			Assert.True(!string.IsNullOrWhiteSpace(result));
			_testOutputHelper.WriteLine(result);
		}
	}
}
