# Network File Share

https://pimylifeup.com/raspberry-pi-exfat/


```shell
mkdir -p /mnt/share/500Gb
mkdir -p /mnt/share/HA_Share_1Tb

sudo apt install samba -y
sudo apt install nfs-kernel-server -y

# DO NOT USE EXFAT!
sudo apt install exfat-fuse -y
sudo apt install exfatprogs -y

# Format to ext4
sudo mkfs.ext4 /dev/sda1
sudo chmod 777 /mnt/share/HA_Share_1Tb

sudo blkid
# /dev/sda1: UUID="a07431e7-678d-4368-b67f-2b2793f9e9ef" BLOCK_SIZE="4096" TYPE="ext4" PARTUUID="c4ae1980-01"

sudo mount -t ext4 /dev/sda1 /mnt/share/HA_Share_1Tb

sudo vi /etc/fstab
# UUID="a07431e7-678d-4368-b67f-2b2793f9e9ef"   /mnt/share/HA_Share_1Tb     ext4    defaults,auto,umask=000,users,rw 0 0
sudo systemctl daemon-reload
sudo mount -a
/dev/sda1       916G   28K  870G   1% /mnt/share/HA_Share_1Tb

sudo vi /etc/exports
# /mnt/share/HA_Share_1Tb           192.168.1.0/24(rw,sync,no_subtree_check)
# /mnt/share/HA_Share_1Tb/           *(rw,all_squash,insecure,async,no_subtree_check,anonuid=1000,anongid=1000)

sudo systemctl restart nfs-server nfs-kernel-server

sudo exportfs -ra
# exportfs: /mnt/share/HA_Share_1Tb does not support NFS export

sudo vi /etc/samba/smb.conf
sudo systemctl restart samba
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
```

```
\\192.168.1.11\mnt\share\HA_Share_1Tb
```