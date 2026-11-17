# Uplaod to local gallery from google

DOC:
- https://github.com/simulot/immich-go
- https://github.com/simulot/immich-go/blob/main/docs/commands/upload.md


```shell
immich-go.exe upload from-google-photos --client-timeout=60m --concurrent-tasks=6 --on-errors=continue --server=http://1.1.1.1:30041 --api-key=USER_API_KEY --admin-api-key=ADMIN_API_KEY "\\NAS\Data\DIR\takeout-20251119T171946Z-3-001.zip" --log-level=ERROR --log-type=TEXT --log-file=dasha_1.log 

immich-go.exe upload from-google-photos --client-timeout=60m --concurrent-tasks=6 --on-errors=continue --server=http://1.1.1.1:30041 --api-key=USER_API_KEY --admin-api-key=ADMIN_API_KEY "\\NAS\Data\DIR\takeout-20251119T171946Z-3-002.zip" --log-level=ERROR --log-type=TEXT --log-file=dasha_2.log 

immich-go.exe upload from-google-photos --client-timeout=60m --concurrent-tasks=6 --on-errors=continue --server=http://1.1.1.1:30041 --api-key=USER_API_KEY --admin-api-key=ADMIN_API_KEY "\\NAS\Data\DIR\takeout-20251119T171946Z-3-003.zip" --log-level=ERROR --log-type=TEXT --log-file=dasha_3.log 


immich-go.exe upload from-google-photos --server=http://1.1.1.1:30041 --api-key=USER_API_KEY --admin-api-key=ADMIN_API_KEY O:\\GooglePhotosExport\\takeout-20251021T090015Z-1-004.zip
```

Defaults:

```text
--sync-albums

```

```
middlewared.service_exception.CallError: [EPERM] Filesystem permissions on path /mnt/Main/Data prevent access for user "apps" to the path /mnt/Main/Data/SyncPhotos. This may be fixed by granting the aforementioned user execute permissions on the path: /mnt/Main/Data.
```


### Upgrade issues


- <https://github.com/truenas/apps/issues/4628>

```
middlewared.service_exception.ValidationErrors: [EINVAL] values.immich.postgres_image_selector: Invalid choice: vectorchord_15_image

    stop container
    edit ix_values file sudo nano /mnt/.ix-apps/app_configs/immich/versions/1.13.6/ix_values.yaml
    edit app and set pg18
    wait for migration
    stop app
    unedit ix_values file sudo nano /mnt/.ix-apps/app_configs/immich/versions/1.13.6/ix_values.yaml
    start app and update to version 1.14.0

```

#### Immich update to v1.14.69

App Version:
v3.4.1
Version:
v1.14.69 

The issue: __database files are incompatible with server__
- `The data directory was initialized by PostgreSQL version 15, which is not compatible with this version 18.4 (Debian 18.4-1.pgdg12+1).`

```log
2026-11-17 19:03:19.646719+00:00🎉 All permissions configured successfully!

2026-11-17 19:03:20.148462+00:00[ix-postgres-main]      - [2026-11-17 21:03:20] - Target version directory already exists with correct version [18]. Upgrade already completed.

2026-11-17 18:56:37.859276+00:00[immich-docker-entrypoint] LOG:  Using SSD storage
2026-11-17 18:56:37.885579+00:002026-11-17T18:56:37.885579223Z
2026-11-17 18:56:37.885642+00:00PostgreSQL Database directory appears to contain a database; Skipping initialization
2026-11-17 18:56:37.885667+00:002026-11-17T18:56:37.885667503Z
2026-11-17 18:56:37.909545+00:002026-11-17 18:56:37.909 GMT [1] LOG:  skipping missing configuration file "/etc/postgresql/postgresql.override.conf"
2026-11-17 18:56:37.909664+00:002026-11-17 18:56:37.909 GMT [1] LOG:  skipping missing configuration file "/var/lib/postgresql/15/docker/postgresql.override.conf"
2026-11-17 18:56:37.909804+00:002026-11-17 18:56:37.909 GMT [1] LOG:  skipping missing configuration file "/etc/postgresql/postgresql.override.conf"
2026-11-17 18:56:37.909860+00:002026-11-17 18:56:37.909 GMT [1] LOG:  skipping missing configuration file "/var/lib/postgresql/15/docker/postgresql.override.conf"
2026-11-17 18:56:37.910550+00:002026-11-17 20:56:37.910 EET [1] FATAL:  database files are incompatible with server
2026-11-17 18:56:37.910594+00:002026-11-17 20:56:37.910 EET [1] DETAIL:  The data directory was initialized by PostgreSQL version 15, which is not compatible with this version 18.4 (Debian 18.4-1.pgdg12+1).


2026/11/17 21:03:22] (ERROR) app_lifecycle.compose_action():56 - Failed 'up' action for 'immich' app:  Network ix-internal-immich-immich-net  Creating
...
 Container ix-immich-pgvecto-1  Error
 dependency failed to start: container ix-immich-pgvecto-1 is unhealthy
 \n
```

ROLLBACK TO Version: `v1.14.67`

```log
2026-11-17 19:07:08.415103+00:00PostgreSQL Database directory appears to contain a database; Skipping initialization
2026-11-17 19:07:08.415115+00:002026-11-17T19:07:08.415115101Z
2026-11-17 19:07:08.439769+00:002026-11-17 19:07:08.439 GMT [1] LOG:  skipping missing configuration file "/etc/postgresql/postgresql.override.conf"
2026-11-17 19:07:08.439834+00:002026-11-17 19:07:08.439 GMT [1] LOG:  skipping missing configuration file "/var/lib/postgresql/18/docker/postgresql.override.conf"
2026-11-17 19:07:08.440131+00:002026-11-17 19:07:08.440 GMT [1] LOG:  skipping missing configuration file "/etc/postgresql/postgresql.override.conf"
2026-11-17 19:07:08.440182+00:002026-11-17 19:07:08.440 GMT [1] LOG:  skipping missing configuration file "/var/lib/postgresql/18/docker/postgresql.override.conf"
2026-11-17 19:07:08.486058+00:002026-11-17 21:07:08.485 EET [1] LOG:  starting PostgreSQL 18.4 (Debian 18.4-1.pgdg12+1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 12.2.0-14+deb12u1) 12.2.0, 64-bit
2026-11-17 19:07:08.486469+00:002026-11-17 21:07:08.486 EET [1] LOG:  listening on IPv4 address "0.0.0.0", port 5432
2026-11-17 19:07:08.486507+00:002026-11-17 21:07:08.486 EET [1] LOG:  listening on IPv6 address "::", port 5432
2026-11-17 19:07:08.489032+00:002026-11-17 21:07:08.488 EET [1] LOG:  listening on Unix socket "/var/run/postgresql/.s.PGSQL.5432"
2026-11-17 19:07:08.498423+00:002026-11-17 21:07:08.498 EET [25] LOG:  database system was shut down at 2026-11-17 20:55:36 EET
2026-11-17 19:07:08.506334+00:002026-11-17 21:07:08.506 EET [1] LOG:  database system is ready to accept connections
2026-11-17 19:07:10.471493+00:002026-11-17 21:07:10.471 EET [35] WARNING:  database "immich" has a collation version mismatch
```

Check Using PG18 since april

```shell
-rw-------+ 1 netdata docker  723M Apr  1  2026 pre-upgrade-15-to-18-20260401155731.tar.zst
drwx------+ 21 netdata docker  28 Nov 17 21:07 docker

[~]$ sudo ls -lah /mnt/DISK/Immich/pgData/18/docker
total 238K
drwx------+ 21 netdata docker   28 Nov 17 21:07 .
drwxrwxr-x+  3 netdata docker    3 Apr  1  2026 ..
-rw-rw-r--+  1 netdata docker    3 Apr  1  2026 PG_VERSION
drwx------+  7 netdata docker    7 Apr  1  2026 base
drwx------+  2 netdata docker   63 Nov 17 21:07 global
...
drwxrwxr-x+  2 netdata docker    5 Apr  1  2026 pgsql_tmp_vchord_sampling
-rw-------+  1 netdata docker   88 Apr  1  2026 postgresql.auto.conf
-rw-------+  1 netdata docker  32K Apr  1  2026 postgresql.conf
-rw-rw-r--+  1 netdata docker  114 Nov 17 21:07 postmaster.opts
-rw-------+  1 netdata docker   99 Nov 17 21:07 postmaster.pid

[~]$ sudo ls -lah /mnt/DISK/Immich/pgData/15/docker
total 234K
drwx------+ 21 netdata docker   27 Apr  1  2026 .
drwxrwxr-x+  3 netdata docker    3 Dec  9  2025 ..
-rwxrwxr-x+  1 netdata docker    3 Oct 21  2025 PG_VERSION
drwxrwxr-x+  7 netdata docker    7 Oct 21  2025 base
drwxrwxr-x+  2 netdata docker   63 Apr  1  2026 global
...
drwxrwxr-x+  2 netdata docker    5 Dec  9  2025 pgsql_tmp_vchord_sampling
-rwxrwxr-x+  1 netdata docker   88 Oct 21  2025 postgresql.auto.conf
-rwxrwxr-x+  1 netdata docker  29K Oct 21  2025 postgresql.conf
-rwxrwxr-x+  1 netdata docker  242 Apr  1  2026 postmaster.opts
```

Solution:

```shell
sudo mv /mnt/DISK/Immich/pgData/15 /mnt/Main/Data/Backups/Manual/Immich/old_pg15/
```

Update and start OK.
