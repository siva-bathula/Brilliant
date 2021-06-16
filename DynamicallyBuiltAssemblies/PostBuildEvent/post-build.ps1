
Start-Process -FilePath "D:\Work\Brilliant\AppRunner\bin\x64\Debug\AppRunner.exe" "PostBuild"

$proc = New-Object System.Diagnostics.Process
$proc.StartInfo.WindowStyle = "Hidden"
$proc.StartInfo.UseShellExecute = $false # Necessary to capture stderr/stdout.
$proc.StartInfo.RedirectStandardOutput = $true
$proc.StartInfo.RedirectStandardError = $true
$proc.StartInfo.FileName = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\xsd.exe"
$proc.StartInfo.Arguments = 'D:\Work\Brilliant\DynamicallyBuiltAssemblies\ProblemSets.dll', '/out:D:\Work\Brilliant\AppRunner'

# Start the process, read all output and errors,
# then wait for the process to end.
$proc.Start() | Out-Null
$output = $proc.StandardOutput.ReadToEnd()
$outputErr = $proc.StandardError.ReadToEnd()
$proc.WaitForExit()

move-item -path D:\Work\Brilliant\AppRunner\schema0.xsd D:\Work\Brilliant\AppRunner\ProblemsSets.xsd -Force

exit