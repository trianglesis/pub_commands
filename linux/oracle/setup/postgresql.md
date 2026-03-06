- https://www.sqliz.com/posts/install-postgresql-on-oracle-linux-8/
- https://cloudspinx.com/how-to-install-postgresql-17-on-oracle-linux/


OLD:
`sudo dnf install postgresql-server postgresql-contrib`

New

`sudo dnf install https://download.postgresql.org/pub/repos/yum/reporpms/EL-9-x86_64/pgdg-redhat-repo-latest.noarch.rpm`


```shell
sudo dnf -qy module disable postgresql
sudo dnf search postgresql17
sudo dnf install postgresql17 postgresql17-server
```

WSL:

```shell
sudo postgresql-setup --initdb
System has not been booted with systemd as init system (PID 1). Can't operate.
Failed to connect to bus: Host is down
System has not been booted with systemd as init system (PID 1). Can't operate.
Failed to connect to bus: Host is down
FATAL: no db datadir (PGDATA) configured for 'postgresql.service' unit

# OR
sudo /usr/pgsql-17/bin/postgresql-17-setup initdb

ls /usr/pgsql-17/bin/

# OLD
psql --version
#psql (PostgreSQL) 13.23

sudo passwd postgres
sudo -u postgres psql

# WSL not working
sudo service postgresql start
```

Check 17th

```shell
ls -lah /usr/pgsql-17/bin/
total 14M
drwxr-xr-x 2 root root 4.0K Mar  6 14:30 .
drwxr-xr-x 5 root root 4.0K Mar  6 14:30 ..
-rwxr-xr-x 1 root root  70K Feb 24 11:02 clusterdb
-rwxr-xr-x 1 root root  74K Feb 24 11:02 createdb
-rwxr-xr-x 1 root root  74K Feb 24 11:02 createuser
-rwxr-xr-x 1 root root  66K Feb 24 11:02 dropdb
-rwxr-xr-x 1 root root  66K Feb 24 11:02 dropuser
-rwxr-xr-x 1 root root 111K Feb 24 11:02 initdb
-rwxr-xr-x 1 root root  41K Feb 24 11:02 pg_archivecleanup
-rwxr-xr-x 1 root root 140K Feb 24 11:02 pg_basebackup
-rwxr-xr-x 1 root root 160K Feb 24 11:02 pgbench
-rwxr-xr-x 1 root root  49K Feb 24 11:02 pg_checksums
-rwxr-xr-x 1 root root 115K Feb 24 11:02 pg_combinebackup
-rwxr-xr-x 1 root root  36K Feb 24 11:02 pg_config
-rwxr-xr-x 1 root root  44K Feb 24 11:02 pg_controldata
-rwxr-xr-x 1 root root  70K Feb 24 11:02 pg_createsubscriber
-rwxr-xr-x 1 root root  61K Feb 24 11:02 pg_ctl
-rwxr-xr-x 1 root root 437K Feb 24 11:02 pg_dump
-rwxr-xr-x 1 root root 115K Feb 24 11:02 pg_dumpall
-rwxr-xr-x 1 root root  37K Feb 24 11:02 pg_isready
-rwxr-xr-x 1 root root  90K Feb 24 11:02 pg_receivewal
-rwxr-xr-x 1 root root  53K Feb 24 11:02 pg_resetwal
-rwxr-xr-x 1 root root 190K Feb 24 11:02 pg_restore
-rwxr-xr-x 1 root root 107K Feb 24 11:02 pg_rewind
-rwxr-xr-x 1 root root  41K Feb 24 11:02 pg_test_fsync
-rwxr-xr-x 1 root root  32K Feb 24 11:02 pg_test_timing
-rwxr-xr-x 1 root root 164K Feb 24 11:02 pg_upgrade
-rwxr-xr-x 1 root root  94K Feb 24 11:02 pg_verifybackup
-rwxr-xr-x 1 root root  94K Feb 24 11:02 pg_waldump
-rwxr-xr-x 1 root root  41K Feb 24 11:02 pg_walsummary
-rwxr-xr-x 1 root root  11M Feb 24 11:02 postgres
-rwxr-xr-x 1 root root 2.2K Feb 24 11:02 postgresql-17-check-db-dir
-rwxr-xr-x 1 root root 9.4K Feb 24 11:02 postgresql-17-setup
-rwxr-xr-x 1 root root 713K Feb 24 11:02 psql
-rwxr-xr-x 1 root root  82K Feb 24 11:02 reindexdb
-rwxr-xr-x 1 root root  87K Feb 24 11:02 vacuumdb
```


# ETC

```
whereis postgres
postgres: /usr/bin/postgres /usr/share/man/man1/postgres.1.gz

id              postgresql
major           12
data_default    /var/pgsql/data
package         postgresql-upgrade
engine          /usr/lib64/pgsql/postgresql-12/bin
description     "Upgrade data from system PostgreSQL version (PostgreSQL 12)"
redhat_sockets_hack no
```


## Init DB

```shell
mkdir test_pg_db
/usr/pgsql-17/bin/initdb -D ~/test_pg_db

The files belonging to this database system will be owned by user "user".
This user must also own the server process.

The database cluster will be initialized with locale "en_US.UTF-8".
The default database encoding has accordingly been set to "UTF8".
The default text search configuration will be set to "english".

Data page checksums are disabled.

fixing permissions on existing directory /home/user/test_pg_db ... ok
creating subdirectories ... ok
selecting dynamic shared memory implementation ... posix
selecting default "max_connections" ... 100
selecting default "shared_buffers" ... 128MB
selecting default time zone ... Europe/Kiev
creating configuration files ... ok
running bootstrap script ... ok
performing post-bootstrap initialization ... ok
syncing data to disk ... ok

initdb: warning: enabling "trust" authentication for local connections
initdb: hint: You can change this by editing pg_hba.conf or using the option -A, or --auth-local and --auth-host, the next time you run initdb.

Success. You can now start the database server using:

    /usr/pgsql-17/bin/pg_ctl -D /home/user/test_pg_db -l logfile start

# Looks ok
ls -lah ~/test_pg_db
total 128K
drwx------ 19 user user 4.0K Mar  6 14:34 .
drwx------ 36 user user 4.0K Mar  6 14:33 ..
drwx------  5 user user 4.0K Mar  6 14:34 base
drwx------  2 user user 4.0K Mar  6 14:34 global
drwx------  2 user user 4.0K Mar  6 14:34 pg_commit_ts
drwx------  2 user user 4.0K Mar  6 14:34 pg_dynshmem
-rw-------  1 user user 5.6K Mar  6 14:34 pg_hba.conf
-rw-------  1 user user 2.6K Mar  6 14:34 pg_ident.conf
drwx------  4 user user 4.0K Mar  6 14:34 pg_logical
drwx------  4 user user 4.0K Mar  6 14:34 pg_multixact
drwx------  2 user user 4.0K Mar  6 14:34 pg_notify
drwx------  2 user user 4.0K Mar  6 14:34 pg_replslot
drwx------  2 user user 4.0K Mar  6 14:34 pg_serial
drwx------  2 user user 4.0K Mar  6 14:34 pg_snapshots
drwx------  2 user user 4.0K Mar  6 14:34 pg_stat
drwx------  2 user user 4.0K Mar  6 14:34 pg_stat_tmp
drwx------  2 user user 4.0K Mar  6 14:34 pg_subtrans
drwx------  2 user user 4.0K Mar  6 14:34 pg_tblspc
drwx------  2 user user 4.0K Mar  6 14:34 pg_twophase
-rw-------  1 user user    3 Mar  6 14:34 PG_VERSION
drwx------  4 user user 4.0K Mar  6 14:34 pg_wal
drwx------  2 user user 4.0K Mar  6 14:34 pg_xact
-rw-------  1 user user   88 Mar  6 14:34 postgresql.auto.conf
-rw-------  1 user user  31K Mar  6 14:34 postgresql.conf
```

## Init existing DB

```shell
/usr/pgsql-17/bin/postgres --config-file=/mnt/test_mount/supervisor/addons/data/77b2833f_timescaledb/postgres/postgresql.conf --data_directory=/mnt/test_mount/supervisor/addons/data/77b2833f_timescaledb/postgres/ --hba_file=/mnt/test_mount/supervisor/addons/data/77b2833f_timescaledb/postgres/pg_hba.conf --ident_file=/mnt/test_mount/supervisor/addons/data/77b2833f_timescaledb/postgres/pg_ident.conf
```