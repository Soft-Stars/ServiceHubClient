# ServiceHubClient

This project is a BackgroundService for Windows Service

Check more details about it [https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service]

In this Windows service I created a SignalR Client which connect to a hub and waiting for messages

If message is synchonrize than it will execute Synchonrize function from local agent service

If message is export than it will execute export function from local agent service

Before running it please Set the correct values in appsettings.json