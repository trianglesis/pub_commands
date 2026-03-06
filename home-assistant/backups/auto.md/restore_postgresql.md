# timescaledb

https://github.com/expaso/hassos-addon-timescaledb

Get dump from a backup: `\automatic_backup_2026_2_2\77b2833f_timescaledb\data`

Copy it where HA can access:

```
ls /media/Services
backup_db.sql

```
77b2833f-timescaledb

Create a backup:

`docker exec addon_timescaledb_timescaledb su - postgres -c "pg_dumpall -U postgres --clean --if-exists -f /data/manual_backup_$(date +%Y%m%d).sql"`

Restore from a manual backup:

`docker exec addon_77b2833f_timescaledb su - postgres -c "psql -U postgres -f /media/Services/backup_db.sql -d postgres"`
`docker exec addon_timescaledb_timescaledb su - postgres -c "psql -U postgres -f /data/manual_backup_YYYYMMDD.sql -d postgres"`

Important Notes

    The SQL dump is only present during the backup process and is automatically cleaned up
    If you need to keep a copy of the backup SQL file, copy it before the backup completes
    The PostgreSQL data directory (/data/postgres/*) is excluded from backups to reduce backup size and improve reliability
    Restore is automatic - no manual intervention required when restoring from a Home Assistant backup
