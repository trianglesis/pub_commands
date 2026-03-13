# Copy files


```shell
rsync --progress --human-readable --times --recursive --checksum --log-file=rsync_$(date +%F_%R).log  postgres/  /mnt/o/Proj/HA_pg_db/postgres/ --delete-after
```