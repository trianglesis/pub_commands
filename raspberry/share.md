# Network File Share

https://pimylifeup.com/raspberry-pi-exfat/


```shell
sudo mkdir -p /mnt/share/500Gb
sudo mkdir -p /mnt/share/HA_Share_1Tb
sudo chmod 777 /mnt/share/HA_Share_1Tb

sudo apt install nfs-kernel-server samba -y

# DO NOT USE EXFAT!
sudo apt install exfat-fuse -y
sudo apt install exfatprogs -y

# Format to ext4
sudo mkfs.ext4 /dev/sda1

sudo blkid
# /dev/sda1: UUID="a07431e7-678d-4368-b67f-2b2793f9e9ef" BLOCK_SIZE="4096" TYPE="ext4" PARTUUID="c4ae1980-01"

sudo mount -t ext4 /dev/sda1 /mnt/share/HA_Share_1Tb

sudo vi /etc/fstab
# /dev/sdc1    /home/user/aaa/bbb/      ext4    defaults     0   0
# UUID="a07431e7-678d-4368-b67f-2b2793f9e9ef"   /mnt/share/HA_Share_1Tb     ext4    defaults     0   0
sudo systemctl daemon-reload
sudo mount -a
/dev/sda1       916G   28K  870G   1% /mnt/share/HA_Share_1Tb

sudo vi /etc/exports
# /mnt/share/HA_Share_1Tb           192.168.1.0/24(rw,sync,no_subtree_check)
# /mnt/share/HA_Share_1Tb/          *(rw,sync,no_subtree_check)

sudo systemctl restart nfs-server nfs-kernel-server

sudo exportfs -ra
# exportfs: /mnt/share/HA_Share_1Tb does not support NFS export
showmount --exports

sudo vi /etc/samba/smb.conf
sudo systemctl restart samba

sudo useradd homeassistant
sudo smbpasswd -a homeassistant
```


```conf
[HA_Share]
    workgroup = WORKGROUP
    netbios name = rpi5
    server string = rpi5
    # specify shared directory
    path = /mnt/share/HA_Share_1Tb
    # allow writing
    writable = yes
    # allow guest user (nobody)
    guest ok = yes
    read only = no
    browsable = yes
    # set permission [777] when file created
    force create mode = 0777
    # set permission [777] when folder created
    force directory mode = 0777
    public = yes
    wins support = yes
    local master = yes
    preferred master = yes

[HA-Music-rpi5]
    path = /mnt/share/HA_Share_1Tb/HA_Music/Music/
    writeable = yes
    browseable = yes
    public = no
```

```
\\192.168.1.11\mnt\share\HA_Share_1Tb
```