# Disk size operatrions

Docs:
- https://packetpushers.net/blog/ubuntu-extend-your-default-lvm-space/
- https://www.truenas.com/community/threads/expanding-vm-disk-size.104040/


Utils

```shell
sudo lsblk
sudo vgdisplay
# Nice extend\shrink text ui
sudo cfdisk
pvresize /dev/sda3

sudo lvextend -l +100%FREE /dev/ubuntu-vg/ubuntu-lv
sudo resize2fs /dev/mapper/ubuntu--vg-ubuntu--lv
```