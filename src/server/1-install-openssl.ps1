function Install-OpenSSL {
    <#
    .SYNOPSIS
        Installs the latest version of OpenSSL
    .DESCRIPTION
        Cross-platform script to install OpenSSL if it is not already installed. But you can use -Force to re-install :)
        OpenSSL is a software library that provides secure communications over computer networks
        using the Transport Layer Security (TLS) or the Secure Sockets Layer (SSL) protocols.
        It is widely used by Internet servers, including the majority of HTTPS websites.
        https://en.wikipedia.org/wiki/OpenSSL
    .EXAMPLE
        Install-Script -Name Install-OpenSSL -Repository PSGallery -Scope CurrentUser; Install-OpenSSL

        # A one-liner to install OpenSSL using the latest installer fuction from the PSGallery script :)
    .EXAMPLE
        . ([scriptblock]::Create((Invoke-RestMethod -Verbose:$false -Method Get https://api.github.com/gists/afe67e3c7b2d6fce2770e6c57314b113).files.'Install-OpenSSL.ps1'.content)); Install-OpenSSL

        # A one-liner to install OpenSSL using the latest installer fuction from the gist :)
    .LINK
        https://gist.github.com/
    .LINK
        https://www.openssl.org/
    #>
    [CmdletBinding(SupportsShouldProcess = $true)]
    param (
        # Forces the installer to use the system PackageManager, Installing that PM first if not found.
        [switch]$UsesystemPackageManager,
        [switch]$Force
    )

    begin {
        $Host_OS = if ($(Get-Variable IsWindows -Value)) { "Windows" } elseif ($(Get-Variable IsLinux -Value)) { "Linux" } elseif ($(Get-Variable IsMacOS -Value)) { "macOS" } else { "UNKNOWN" }
    }

    process {
        if ($Host_OS -eq "Windows") {
            if (!(Get-Module psyml -ListAvailable)) { Install-Module -Name psyml -Repository PSGallery -AllowClobber }
            $openssl = [IO.FileInfo](Get-Command openssl -ErrorAction Ignore).Source; $IsInstalled = $openssl -as [bool]
            $Get_PkgInfo = [scriptblock]::Create({
                    param($_Pkg_Id) # (todo: Use pkgname as param then use winget api to get _Pkg_Id instead)
                    $pkgyaml = $(Invoke-WebRequest -Verbose:$false -Uri "https://github.com/microsoft/winget-pkgs/blob/master/manifests/s/$($_Pkg_Id.Replace('.', '/'))").Links.where({ $_.title -and $_.title -as 'version' -is [version] }) | Select-Object href, @{l = 'version'; e = { $_.title } } | Sort-Object -Property version -Descending | Select-Object @{l = 'href'; e = { 'https://github.com' + $_.href } }, version -First 1
                    $pkg_msi = $pkgyaml | Select-Object @{l = 'Info'; e = {
                            $tmp = [IO.path]::GetTempFileName(); [System.Net.WebClient]::New().DownloadFile([uri]::new($($_.href.Replace('tree', 'raw') + "/$_Pkg_Id.installer.yaml")), $tmp)
                            $yml = [IO.File]::ReadAllText($tmp) | ConvertFrom-Yaml
                            $yml.Installers.Where({ $_.InstallerType -eq 'wix' -and $_.Architecture -eq $(if ([System.Environment]::Is64BitOperatingSystem) { 'x64' } else { 'x86' }) }) | Select-Object @{l = 'url'; e = { $_.InstallerUrl } }, @{l = 'Sha256'; e = { $_.InstallerSha256 } }
                        }
                    }, version;
                    return [PSCustomObject]@{
                        Name    = $_Pkg_Id
                        version = $pkg_msi.version
                        Sha256  = $pkg_msi.Info.Sha256
                        url     = $pkg_msi.Info.url
                    }
                }
            )
            if (!$IsInstalled) {
                # huh! maybe its not added to Path; let's check
                $OPENSSLDIR = $(("C:\Program Files\OpenSSL-Win64\bin\", [IO.Path]::Combine(([IO.FileInfo](Get-Command git -ErrorAction Ignore).Source).Directory.Parent.FullName, 'usr', 'bin')) | Select-Object @{l = 'File'; e = { [IO.FileInfo][IO.Path]::Combine([IO.DirectoryInfo]::New($_).FullName, 'openssl.exe') } }).Where({ $_.File.Exists });
                if ($OPENSSLDIR.count -gt 1) {
                    Write-Warning "Found Multiple ($($OPENSSLDIR.count)) openssl executables!"
                }; $IsInstalled = $OPENSSLDIR.count -gt 0
                $usr_bin = $OPENSSLDIR[0].File.directory
                $PATH = [Environment]::GetEnvironmentVariable('PATH', 'Machine')
                if ($PATH -notlike "*" + $usr_bin.FullName + "*" ) {
                    Write-Verbose "Added $usr_bin to PATH" -Verbose
                    [Environment]::SetEnvironmentVariable("PATH", ($PATH + $([IO.Path]::PathSeparator) + $usr_bin.FullName), 'Machine')
                    Write-Verbose "Restart terminal or refreshEnv to take effect" -Verbose
                } else {
                    Write-Host $usr_bin.FullName -NoNewline -ForegroundColor Green; Write-Host " was already added to path."
                }
            }
            if (!$IsInstalled -or $Force.IsPresent) {
                $pkg = $Get_PkgInfo.Invoke('ShiningLight.OpenSSL.Light');
                if (!$UsesystemPackageManager.IsPresent) {
                    $msi = [IO.Path]::Combine([IO.Path]::GetTempPath(), ($pkg.url | Split-Path -Leaf))
                    Write-Verbose "Downloading installer $($pkg.url) ..."
                    [System.Net.WebClient]::New().DownloadFile([uri]::new($pkg.url), $msi)
                    if ((Get-FileHash -Path $msi).Hash -ne $pkg.Sha256) { throw [System.Exception]::New('InvalidHash') }
                    Write-Host "Hash verification competed successfuly" -f Green
                    $log = [IO.Path]::GetTempFileName()
                    try {
                        if ($PSCmdlet.ShouldProcess("Installing OpenSSL version $($pkg.version)", '', '')) {
                            Start-Process -FilePath (Get-Command msiexec).Source -ArgumentList ('/i', $msi, '/quiet', '/qn', '/norestart', '/log', $log) -Wait
                        }
                        Remove-Item $log
                    } catch {
                        Write-Host $_.Exception.Message -ForegroundColor Red
                        Write-Host "View Install log at: $log" -ForegroundColor Red
                    }
                } else {
                    if (!(Get-Command winget -ErrorAction Ignore)) {
                        $URL = "https://api.github.com/repos/microsoft/winget-cli/releases/latest"
                        $URL = (Invoke-WebRequest -Uri $URL -Verbose:$false).Content | ConvertFrom-Json |
                            Select-Object -ExpandProperty "assets" |
                            Where-Object "browser_download_url" -Match '.msixbundle' |
                            Select-Object -ExpandProperty "browser_download_url"
                        $msix = [IO.Path]::Combine([IO.Path]::GetTempPath(), ($URL | Split-Path -Leaf))
                        Write-Verbose "Downloading winget installer to $msix ..."
                        [System.Net.WebClient]::New().DownloadFile([uri]::new($URL), $msix)
                        Write-Host "Installing winget" -f Green
                        Add-AppxPackage -Path $msix
                        Remove-Item $msix
                    }
                    winget install ShiningLight.OpenSSL.Light
                }
            } else {
                Write-Host "OpenSSL is already Installed :)" -ForegroundColor Green
            }
        } elseif ($Host_OS -eq "Linux") {
            if (-not (Get-Command openssl -ErrorAction SilentlyContinue)) {
                # Install OpenSSL on Linux using the system package manager
                Write-Host "OpenSSL not found. Installing OpenSSL using the system package manager..."
                try {
                    if (Test-Path /etc/debian_version) {
                        # Debian-based systems (e.g. Ubuntu)
                        sudo apt-get update
                        sudo apt-get install -y openssl
                    } elseif (Test-Path /etc/redhat-release) {
                        # RedHat-based systems (e.g. CentOS)
                        sudo yum install -y openssl
                    } else {
                        Write-Host "Unsupported Linux distribution. Please install OpenSSL manually."
                        exit 1
                    }
                } catch {
                    Write-Host "Failed to install OpenSSL using the system package manager."
                    exit 1
                }
            }
        } elseif ($Host_OS -eq "macOS") {
            if (-not (Get-Command openssl -ErrorAction SilentlyContinue)) {
                # Install OpenSSL on macOS using Homebrew package manager
                Write-Host "OpenSSL not found. Installing OpenSSL using Homebrew package manager..."
                try {
                    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"
                    brew install openssl
                } catch {
                    Write-Host "Failed to install OpenSSL using Homebrew package manager."
                    exit 1
                }
            }
        } else {
            Write-Warning "Unsupported operating system. Please install OpenSSL manually."
        }
    }
}