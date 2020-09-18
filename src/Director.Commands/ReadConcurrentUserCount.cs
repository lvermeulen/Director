using System.Collections.Generic;
using System.Threading.Tasks;
using Director.Abstractions;
using Microsoft.Extensions.Logging;

namespace Director.Commands
{
    public class ReadConcurrentUserCount : IJobProvider
    {
	    private readonly ILogger<ReadConcurrentUserCount> _logger;

	    public ReadConcurrentUserCount(ILogger<ReadConcurrentUserCount> logger)
	    {
		    _logger = logger;
	    }

        public Task<string> GetScriptAsync(IDictionary<string, string> parameters)
        {
            if (!parameters.ContainsKeys("esentAssemblyLocation", "esentSummaryLocation"))
            {
	            _logger.LogError("ReadConcurrentUserCount GetScript failed: didn't receive all needed parameters");
                return Task.FromResult("");
            }

            string esentAssemblyLocation = parameters["esentAssemblyLocation"];
            _logger.LogTrace($"SendMail ReadConcurrentUserCount: esentAssemblyLocation: {esentAssemblyLocation}");
            string esentSummaryLocation = parameters["esentSummaryLocation"];
            _logger.LogTrace($"SendMail ReadConcurrentUserCount: esentSummaryLocation: {esentSummaryLocation}");

            string script = @"
[Reflection.Assembly]::LoadFrom('{0}')
$a = 'Microsoft.Isam.Esent.Collections.Generic.PersistentDictionary`2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]'
$out = @()

$storage = New-Object $a '{1}'

foreach($key in $storage.Keys)
{{
    $obj = New-Object psobject

    $application = $key.Split('|')[0]
    $domain = $key.Split('|')[1]
    $maxcount = $key.Split('|')[2]
    $datetime = $storage[$key].Split('|')[0]
    $counter = $storage[$key].Split('|')[1]

    $obj | Add-Member noteproperty Application $application
    $obj | Add-Member noteproperty Domain $domain
    $obj | Add-Member noteproperty MaxCount $maxcount
    $obj | Add-Member noteproperty DateTime $datetime
    $obj | Add-Member noteproperty Counter $counter
    
    $out += $obj
}}

$storage.Dispose();

$out";
            script = string.Format(script, esentAssemblyLocation, esentSummaryLocation);

            return Task.FromResult(script);
        }
    }
}
