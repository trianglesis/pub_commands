# NFS


`vi /etc/fstab`

```conf
# Local HAS save logs
10.1.1.1:/mnt/Main/Services/    /home/pihole/SHARE/     nfs     auto,rw,noatime,nolock,bg,intr,tcp,actimeo=1800 0 0
10.1.1.1:/mnt/Main/Services/    /home/pihole/SHARE/     nfs     rsize=8192,wsize=8192,timeo=14,intr
10.1.1.1:/mnt/Main/Services/    /home/pihole/SHARE/     nfs     auto,nofail,noatime,nolock,intr,tcp,actimeo=1800 0 0
10.1.1.1:/mnt/Main/Services/    /home/pihole/SHARE/     nfs     defaults 0 0
# RPi
10.1.1.1:/mnt/Main/Services/    /home/sanek/SHARE/     nfs     auto,rw,noatime,nolock,bg,intr,tcp,actimeo=1800 0 0
```

```shell
sudo apt install nfs-common
sudo chmod -R 777 /home/pihole/SHARE/

# reload and mount
sudo systemctl daemon-reload && sudo mount -a
# Check
df -h
```

## Manual or for test

```shell
sudo mount -t nfs 10.1.1.1:/Main/Services/ /home/pihole/SHARE/
sudo mount -v -t nfs 10.1.1.1:/Main/Services/ /home/pihole/SHARE/

# See
/var/log/syslog
nfsstat -m

sanek@raspberrypi:~ $ sudo touch /home/sanek/SHARE/raspberry.txt
touch: cannot touch '/home/sanek/SHARE/raspberry.txt': Permission denied
```