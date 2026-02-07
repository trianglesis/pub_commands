# Disk usage:

```shell
du -h PATH | sort -hr | head -n 20
du -h / | sort -hr | head -n 20
du -h /var/ | sort -hr | head -n 40
du -h /home/user/ | sort -hr | head -n 20


du -h -d 4 / | sort -hr | head -n 20
du --exclude "*/share/*" | sort -hr | head -n 20
du --exclude "*/share/*" --exclude "*/media/*" | sort -hr | head -n 20
```