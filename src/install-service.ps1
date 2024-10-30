param (
    [string]$serviceName,
    [string]$clientId
)


# $env:SignalRSetting__ClientId = $clientId;

# Path to the executable file
$exePath = "C:\github\EY\ServiceHubClient\src\bin\Release\net8.0\win-x64\publish\ServiceHubClient.exe" 

# Check if the executable file exists
if (-Not (Test-Path $exePath)) {
    Write-Host "Error: The executable file '$exePath' does not exist."
    exit 1
}

# Create the service
try {
    Write-Host "Installing service '$serviceName'..."
    sc create $serviceName binPath= $exePath
    # New-Service -Name $serviceName -BinaryPathName $exePath -DisplayName $serviceName -StartupType Automatic -Description "Service for $serviceName"

    # Start the service
    Write-Host "Starting service '$serviceName'..."
    Start-Service -Name $serviceName

    Write-Host "Service '$serviceName' installed and started successfully."
} catch {
    Write-Host "Error: An exception occurred while installing the service. $_"
}


# sc delete SoftStartPharmacy001