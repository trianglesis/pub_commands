

root@OpenWrt:~# cat /etc/opkg.conf
dest root /
dest ram /tmp
lists_dir ext  /usr/lib/opkg/lists
option overlay_root /overlay
option check_signature


root@OpenWrt:~#  grep -e /overlay /etc/mtab
/dev/sda1 /overlay ext4 rw,relatime 0 0
overlayfs:/overlay / overlay rw,noatime,lowerdir=/,upperdir=/overlay/upper,workdir=/overlay/work 0 0

root@OpenWrt:~# df /overlay /
Filesystem           1K-blocks      Used Available Use% Mounted on
/dev/sda1              7346668     12076   6939996   0% /overlay
overlayfs:/overlay     7346668     12076   6939996   0% /


root@OpenWrt:~# ls -l /sys/block
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock0 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd0/mtdblock0
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock1 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtdblock1
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock2 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtd2/mtdblock2
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock3 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtd3/mtdblock3
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock4 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtd4/mtdblock4
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock5 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd5/mtdblock5
lrwxrwxrwx    1 root     root             0 Jun  7 16:04 sda -> ../devices/platform/ahb/1b000000.usb/usb1/1-1/1-1.1/1-1.1:1.0/host0/target0:0:0/0:0:0:0/block/sda



WEB UI:

![UI Mounts](openwrt/setup/PICs/UI_Mounts.png)