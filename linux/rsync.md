# Copy files


```shell
rsync -a --progress --prune-empty-dirs --log-file=/usr/tideway/OCTOPUS_sync_python_testutils_$(date +%F_%R).log /usr/tideway/OCTOPUS/testutils/ /usr/tideway/python/testutils/ --delete-after


rsync --progress --human-readable --times --recursive --checksum --log-file=rsync_$(date +%F_%R).log  postgres/  /mnt/o/Proj/HA_pg_db/postgres/ --delete-after
```