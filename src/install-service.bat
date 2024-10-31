@echo off
setlocal

:: Set the service name
set "serviceName=Pharmacy001"

:: Set the path to appsettings.json and the executable
set "jsonFile=appsettings.json"
set "batFolder=%~dp0"
set "exePath=%batFolder%ServiceHubClient.exe"

:: Update ClientId in appsettings.json using PowerShell
powershell -Command "((Get-Content -path '%jsonFile%' -Raw | ConvertFrom-Json).SignalRSetting.ClientId = '%serviceName%') | ConvertTo-Json -Depth 32 | Set-Content -Path '%jsonFile%'"

:: Create the service
sc create "%serviceName%" binPath= "\"%exePath%\""

:: Set the service description
sc description "%serviceName%" "Service created from batch script"

:: Start the service
sc start "%serviceName%"

:: Check if the service started successfully
sc query "%serviceName%" | findstr /i "RUNNING"
if %errorlevel% equ 0 (
    echo Service %serviceName% started successfully.
) else (
    echo Failed to start service %serviceName%.
)

endlocal
