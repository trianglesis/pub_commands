
# Diskpart

- https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/diskpart

```shell
diskpart

list disk

#   Disk ###  Status         Size     Free     Dyn  Gpt
#   --------  -------------  -------  -------  ---  ---
#   Disk 0    Online         5589 GB  1024 KB        *
#   Disk 1    Online         1863 GB      0 B
#   Disk 2    Online         1863 GB   186 GB        *
#   Disk 3    Online           14 GB     9 MB

select disk 3
list partition

#   Partition ###  Type              Size     Offset
#   -------------  ----------------  -------  -------
#   Partition 1    Primary             14 GB  4096 KB
#   Partition 2    Primary           6144 KB    14 GB

select partition 1
delete partition

create partition primary

detail disk

# Generic- SD/MMC USB Device
# Disk ID: A557D540
# Type   : USB
# Status : Online
# Path   : 0
# Target : 0
# LUN ID : 0
# Location Path : UNAVAILABLE
# Current Read-only State : No
# Read-only  : No
# Boot Disk  : No
# Pagefile Disk  : No
# Hibernation File Disk  : No
# Crashdump Disk  : No
# Clustered Disk  : No

#   Volume ###  Ltr  Label        Fs     Type        Size     Status     Info
#   ----------  ---  -----------  -----  ----------  -------  ---------  --------
# * Volume 6                             Removable     14 GB  Healthy    Offline

detail partition

# Partition 2
# Type  : 06
# Hidden: No
# Active: No
# Offset in Bytes: 1048576

#   Volume ###  Ltr  Label        Fs     Type        Size     Status     Info
#   ----------  ---  -----------  -----  ----------  -------  ---------  --------
# * Volume 6                             Removable     14 GB  Healthy    Offline


online disk
online volume

list volume

#   Volume ###  Ltr  Label        Fs     Type        Size     Status     Info
#   ----------  ---  -----------  -----  ----------  -------  ---------  --------
#   Volume 0     O   Big Data 6T  NTFS   Partition   5589 GB  Healthy
#   Volume 1     E   Data2Tb      NTFS   Partition   1863 GB  Healthy
#   Volume 2     C                NTFS   Partition    453 GB  Healthy    Boot
#   Volume 3     D   Data 1.4Tb   NTFS   Partition   1221 GB  Healthy
#   Volume 4                      FAT32  Partition    100 MB  Healthy    System
#   Volume 5                      NTFS   Partition    742 MB  Healthy    Hidden
#   Volume 6                             Removable     14 GB  Healthy    Offline

assign letter=X
format fs=ntfs label=Flash16Gb

automount
```