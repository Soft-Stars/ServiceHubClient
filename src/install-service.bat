@echo off
setlocal

:: Set the service name
set "serviceName=SoftStarsPharmacy003"

:: Set the path to appsettings.json and the executable
set "jsonFile=appsettings.json"
set "batFolder=%~dp0"
set "exePath=%batFolder%ServiceHubClient.exe"
setx MY_ENV_VAR "%serviceName%" /M
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
