# NVMe with Geekwork HAT


```shell
sudo apt install nvme-cli
```

# Show info:

```shell
# Show device:
sudo nvme list

sudo nvme smart-log /dev/nvme0n1
# OR
sudo nvme smart-log /dev/nvme0

sudo nvme smart-log /dev/nvme0n1 | grep -i '^temperature'
```