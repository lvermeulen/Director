using System;
using System.Collections;
using System.Collections.Generic;

namespace Director.Commands
{
    public class InformationProvider
    {
	    private string RunScriptHandler(string script, Func<ICollection, string> resultsHandler) => resultsHandler?.Invoke(PowershellCommand.RunScript(script).Value);

	    private string RunScript(string script, Func<string, string> additionalProcessing = null) => RunScriptHandler(script, x =>
	    {
		    foreach (var item in x)
		    {
			    string result = item.ToString().Split('=')[1].Trim();
			    if (additionalProcessing != null)
			    {
				    result = additionalProcessing.Invoke(result);
			    }
			    return result?.Replace("}", "");
		    }

		    return "";
	    });

	    public string GetLastBootUpTime()
        {
            const string script = @"Get-CimInstance -query ""SELECT LastBootUpTime FROM Win32_OperatingSystem"" | SELECT LastBootUpTime";
            return RunScript(script);
        }

        public string GetOsName()
        {
            const string script = @"Get-CimInstance -query ""SELECT Name FROM Win32_OperatingSystem"" | SELECT Name";
            return RunScript(script, x => x.Split('|')[0].Trim());
        }

        public string GetOsVersion()
        {
            const string script = @"Get-CimInstance -query ""SELECT Version FROM Win32_OperatingSystem"" | SELECT Version";
            return RunScript(script);
        }

        public string GetOsServicePack()
        {
            const string script = @"Get-CimInstance -query ""SELECT CSDVersion FROM Win32_OperatingSystem"" | SELECT CSDVersion";
            return RunScript(script);
        }

        public string GetIpAddress()
        {
            const string script = @"Get-CimInstance -query ""SELECT IPAddress FROM Win32_NetworkAdapterConfiguration"" | SELECT IPAddress | format-list | out-string";
            return RunScriptHandler(script, x =>
            {
	            var output = new List<string>();
	            foreach (var item in x)
	            {
		            var strings = item.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		            foreach (string s in strings)
		            {
			            string ipAddress = "";
			            if (s.Contains("{") && s.Contains(","))
			            {
				            ipAddress = s.Split('{')[1].Split(',')[0].Trim();
			            }
			            else if (s.Contains("{") && s.Contains("}"))
			            {
				            ipAddress = s.Split('{')[1].Split('}')[0].Trim();
			            }

			            if (!string.IsNullOrEmpty(ipAddress))
			            {
				            output.Add(ipAddress);
			            }
		            }
	            }

	            return string.Join(", ", output);
            });
        }

        public string GetMmcVersion()
        {
            const string script = @"Get-CimInstance -query ""SELECT Version from CIM_DataFile WHERE name = 'C:\\Windows\\System32\\mmc.exe'"" | SELECT Version";
            return RunScript(script);
        }

        public string GetPowershellVersion()
        {
            const string script = @"get-host | SELECT Version";
            return RunScript(script);
        }

        public string GetMssqlInformation(string connectionString)
        {
            const string query = @"
                SELECT 
	                SERVERPROPERTY ('productversion') as ProductVersion, 
	                SERVERPROPERTY ('productlevel') as ProductLevel, 
	                SERVERPROPERTY ('edition') as Edition,
	                SYSTEM_USER as 'User'";

            string script = $@"
                    $SqlConnection = New-Object System.Data.SqlClient.SqlConnection;
                    $SqlConnection.ConnectionString = '{connectionString}';
                    $SqlCmd = New-Object System.Data.SqlClient.SqlCommand;
                    $SqlCmd.CommandText = ""{query}"";
                    $SqlCmd.Connection = $SqlConnection;
                    $SqlAdapter = New-Object System.Data.SqlClient.SqlDataAdapter;
                    $SqlAdapter.SelectCommand = $SqlCmd;
                    $DataSet = New-Object System.Data.DataSet;
                    $SqlAdapter.Fill($DataSet);
                    $SqlConnection.Close();
                    $DataSet.Tables[0] | format-list | out-string";

            return RunScriptHandler(script, x =>
            {
	            foreach (var item in x)
	            {
		            string s = item.ToString();
		            if (s.Contains(":"))
		            {
			            return s;
		            }
	            }

	            return "";

            });
        }
    }
}
