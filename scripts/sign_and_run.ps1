<#
PowerShell script to build, sign Assignment1_Adv.dll with a dev self-signed certificate, and run the project.
Run in PowerShell (CurrentUser) as a normal user. If signtool is available it will be used; otherwise Set-AuthenticodeSignature will be attempted.
#>
param(
	[string]$ProjectRoot = "C:\Users\LOQ\source\repos\Assignment1_Adv\Assignment1_Adv",
	[string]$Configuration = 'Debug',
	[string]$Framework = 'net10.0'
)

Set-StrictMode -Version Latest

$ErrorActionPreference = 'Stop'

try {
	Write-Output "Building project..."
	dotnet build $ProjectRoot -c $Configuration

	$dllPath = Join-Path $ProjectRoot "bin\$Configuration\$Framework\Assignment1_Adv.dll"
	if (-not (Test-Path $dllPath)) {
		throw "Built DLL not found at: $dllPath"
	}

	Write-Output "Creating a temporary code-signing certificate..."
	$guid = [guid]::NewGuid().ToString()
	$subject = "CN=DevCodeSign-$guid"
	$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject $subject -KeyExportPolicy Exportable -CertStoreLocation Cert:\CurrentUser\My -NotAfter (Get-Date).AddYears(5)
	$thumb = $cert.Thumbprint

	$tempDir = if (Test-Path C:\temp) { 'C:\temp' } else { [IO.Path]::GetTempPath() }
	$pfxPath = Join-Path $tempDir "devcodesign-$guid.pfx"
	$pwd = ConvertTo-SecureString -String "P@ssw0rd!" -Force -AsPlainText
	Export-PfxCertificate -Cert Cert:\CurrentUser\My\$thumb -FilePath $pfxPath -Password $pwd

	Write-Output "Importing certificate into CurrentUser\TrustedPublisher..."
	Import-PfxCertificate -FilePath $pfxPath -CertStoreLocation Cert:\CurrentUser\TrustedPublisher -Password $pwd | Out-Null

	# Try to find signtool
	Write-Output "Searching for signtool.exe..."
	$signtoolPaths = @(
		"$env:ProgramFiles(x86)\Windows Kits\10",
		"$env:ProgramFiles\Windows Kits\10",
		"$env:ProgramFiles(x86)\Windows Kits\8.1",
		"$env:ProgramFiles\Windows Kits\8.1"
	) | Where-Object { $_ -and (Test-Path $_) }

	$signtool = $null
	foreach ($base in $signtoolPaths) {
		$found = Get-ChildItem -Path $base -Filter signtool.exe -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1
		if ($found) { $signtool = $found.FullName; break }
	}

	if ($signtool) {
		Write-Output "Signing using signtool: $signtool"
		& $signtool sign /f $pfxPath /p P@ssw0rd! /fd SHA256 /tr http://timestamp.digicert.com /td SHA256 `"$dllPath`"
	}
	else {
		Write-Output "signtool not found. Attempting Set-AuthenticodeSignature using certificate from CurrentUser\\My..."
		$certFromStore = Get-ChildItem Cert:\CurrentUser\My | Where-Object Thumbprint -EQ $thumb
		if (-not $certFromStore) { throw 'Could not retrieve certificate from store.' }
		Set-AuthenticodeSignature -Certificate $certFromStore -HashAlgorithm 'SHA256' -FilePath $dllPath | Out-Null
	}

	Write-Output "Verifying signature..."
	if ($signtool) {
		& $signtool verify /pa /v `"$dllPath`"
	}
	else {
		Get-AuthenticodeSignature -FilePath $dllPath | Format-List
	}

	Write-Output "Running the project..."
	dotnet run --project $ProjectRoot -c $Configuration

	Write-Output "Done."
}
catch {
	Write-Error "Error: $_"
	exit 1
}
