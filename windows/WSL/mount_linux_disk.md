- https://learn.microsoft.com/en-us/windows/wsl/wsl2-mount-disk


# List disks

```shell
PS C:\Users\user> Get-CimInstance -Query "SELECT * from Win32_DiskDrive"

DeviceID           Caption                    Partitions Size          Model
--------           -------                    ---------- ----          -----
\\.\PHYSICALDRIVE3 MSI M390  500GB USB Device 8          500105249280  MSI M390  500GB USB Device
\\.\PHYSICALDRIVE2 AAAAAAAAAAAAAAAAAAAAAAA    4          2000396321280 BBBBBBBBBBBBBBBBBBBBBBBBB
\\.\PHYSICALDRIVE0 AAAAAAAAAAAAAAAAAAAAAAA    1          6001172513280 BBBBBBBBBBBBBBBBBBBBBBBBB
\\.\PHYSICALDRIVE1 AAAAAAAAAAAAAAAAAAAAAAA    1          2000396321280 BBBBBBBBBBBBBBBBBBBBBBBBB

```


# Mount

```shell
# not partitioned
PS C:\Users\user> wsl.exe --mount \\.\PHYSICALDRIVE3
PS C:\Users\user> wsl.exe --unmount \\.\PHYSICALDRIVE3

# For partitioned!
PS C:\Users\user> wsl.exe --mount \\.\PHYSICALDRIVE3 --bare
```


# Check

in WSL

```shell
[user@user-pc ~]$ lsblk
NAME   MAJ:MIN RM   SIZE RO TYPE MOUNTPOINTS
sda      8:0    0 388.6M  1 disk
sdb      8:16   0   186M  1 disk
sdc      8:32   0     8G  0 disk [SWAP]
sdd      8:48   0     1T  0 disk /mnt/wslg/distro
                                 /
sde      8:64   0 465.8G  0 disk
├─sde1   8:65   0    64M  0 part
├─sde2   8:66   0    24M  0 part
├─sde3   8:67   0   256M  0 part
├─sde4   8:68   0    24M  0 part
├─sde5   8:69   0   256M  0 part
├─sde6   8:70   0     8M  0 part
├─sde7   8:71   0    96M  0 part
└─sde8   8:72   0   465G  0 part
```


# Access

```
sudo mkdir -p /mnt/test_mount
sudo chown user:user -R /mnt/test_mount
sudo mount /dev/sde8 /mnt/test_mount
```

```shell
[user@user-pc ~]$ df -h
Filesystem      Size  Used Avail Use% Mounted on

C:\             454G  290G  165G  64% /mnt/c
/dev/sde8       458G   91G  349G  21% /mnt/test_mount

[user@user-pc ~]$ ls /mnt/test_mount
docker  logs  lost+found  supervisor  swapfile
```