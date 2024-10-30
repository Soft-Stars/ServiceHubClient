using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServiceHubClient.Services
{
    public class PowerShellExecutor : IPowerShellExecutor
    {
        public string ExecuteScript(string script, string scriptArguments)
        {
            string report = "";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-Command {script}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                report += output;
                if (!string.IsNullOrEmpty(error))
                {
                    report += error;
                }
            }
            return report;
        }
    }
}
