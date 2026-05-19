# WSL

-<https://learn.microsoft.com/en-us/windows/wsl/basic-commands>

```shell
# Install ubuntu as default
wsl --install
wsl --install OracleLinux_9_5

# Show available
wsl --list --online

# Show installed
wsl --list --verbose

# Update WSL
wsl --update

# Version, status
wsl --status
wsl --version
```


## Change WSL versions

```shell
wsl --set-version <distribution name> <versionNumber>
```