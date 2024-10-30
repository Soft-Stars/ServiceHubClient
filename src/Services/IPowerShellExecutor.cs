namespace ServiceHubClient.Services
{
    public interface IPowerShellExecutor
    {
        string ExecuteScript(string scriptPath, string scriptArguments);
    }
}
