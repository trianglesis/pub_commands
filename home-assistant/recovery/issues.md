# Issues

# No GUI but ping OK

Useful commands from CLI:
- https://www.home-assistant.io/common-tasks/os/#home-assistant-via-the-command-line


## Logs check

- https://www.home-assistant.io/integrations/logger/
- https://www.home-assistant.io/more-info/unsupported/systemd_journal/
- https://developers.home-assistant.io/docs/operating-system/debugging/#checking-the-logs
- https://community.home-assistant.io/t/how-to-get-to-your-log-after-restart-restore/387662

Log into HA OS
- https://community.home-assistant.io/t/how-to-get-access-at-damn-host-system/96549/86
- https://developers.home-assistant.io/docs/operating-system/debugging/#accessing-the-container-bash
- https://community.home-assistant.io/t/howto-how-to-access-the-home-assistant-os-host-itself-over-ssh/263352

```shell
cat /config/home-assistant.log
cat /config/home-assistant.log.1
cat /config/home-assistant.log.fault

# Addons specific:
ha addons logs 77b2833f_timescaledb

# Tail
tail -f -n50 /config/home-assistant.log

# Sys log ONLY at direct CLI and after `login` into small linux
journalctl -n 50 --no-pager -f
```

Nothing critical:
- [HA log](logs/home-assistant.log)
- [HA log 1](logs/home-assistant.log.1)
- Empty: `home-assistant.log.fault`

#### Extras:

Escape docker with HA SSH addon `Protection mode Off`

```shell
docker exec -it homeassistant /bin/bash
su root
```


### Tune logs level

```yaml
logger:
  default: critical
  logs:
    # log level for HA core
    homeassistant.core: info
    homeassistant.loader: info
    # log level for MQTT integration
    homeassistant.components.mqtt: info
    # Other
    homeassistant.components.light: info
    # CUSTOM
    homeassistant.custom_components.yasno_outages: info
    homeassistant.custom_components.localtuya: info
```

### SSH component for easy access

Initialize linux console first by `login` cmd.

```shell
login
ha adons | grep ssh
ha addons start a0d7b954_ssh
```

Example

```shell
➜  ~ ha addons | grep ssh
  slug: a0d7b954_ssh
  url: https://github.com/hassio-addons/addon-ssh
```


### Restart

```shell
ha core restart
ha supervisor restart

# OR
ha core start
```

Does not help

```shell
➜  ~ ha core restart
Processing... Done.

Error: Another job is running for job group home_assistant_core
➜  ~ ha supervisor restart
Error: 'Supervisor.restart' blocked from execution, system is not running - startup

### Extraordinary
➜  ~ ha core start
Processing... Done.B

Post "http://supervisor/core/start": context deadline exceeded (Client.Timeout exceeded while awaiting headers)

```

### Safe mode

Stop `core` by force if needed, intrerrupt and stop again.
The UI should work!

```shell
ha core stop
# start save after interruption and forced stop!
ha core restart --safe-mode
```

Output:

```shell
➜  ~ ha core stop
Processing... Done.

Command completed successfully.

➜  ~ ha core restart --safe-mode
Processing... Done.

Command completed successfully.

```

Does not help: 

- Try kill `core` by stopping it multiple times!

`Error: Another job is running for job group home_assistant_core`

See jobs:

```shell
Error: Another job is running for job group home_assistant_core
➜  ~ ha jobs info
ignore_conditions: []
jobs:
- child_jobs: []
  done: false
  errors: []
  name: home_assistant_core_start
  progress: 0
  reference: null
  stage: null
  uuid: 029906881c704a2689198470736040f1
```



### Revert to older version

- https://www.home-assistant.io/blog/categories/release-notes/
- https://community.home-assistant.io/t/error-another-job-is-running-for-job-group-home-assistant-core/619612/9

Usual way

```shell
ha core check
ha core rebuild
ha core update --version 2024.12.1
```

Extraordinary

```shell
# Loop force! use 1st terminal
watch -n0 “/usr/bin/ha core update --version 2024.12.1”
# restart core in the second:
ha supervisor restart
```

Outputs

```shell
➜  ~ ha core check
Processing... Done.

Command completed successfully.

➜  ~ ha core rebuild
Processing... Done.

Error: Another job is running for job group home_assistant_core

➜  ~ ha core update --version 2024.12.1
Processing... Done.

Error: Another job is running for job group home_assistant_core

```

### Backups

```shell
➜  ~ ha backup restore 5f35ed18
Processing... Done.

Error: 'BackupManager.do_restore_full' blocked from execution, system is not running - startup

```

### Disable integrations

See logs

```shell
vi /mnt/data/supervisor/homeassistant/.storage/core.config_entries
vi /config/.storage/core.config_entries
```


### Disable addons

Possible issues from logs: `core_mosquitto`

```shell

```


### PostgreSQL possible issue in logs

See:

```shell
ha addons logs 77b2833f_timescaledb
```

```log
2024-12-16 10:07:14.009 UTC [241] LOG:  starting PostgreSQL 16.3 on aarch64-alpine-linux-musl, compiled by gcc (Alpine 13.2.1_git20240309) 13.2.1 20240309, 64-bit
2024-12-16 10:07:14.009 UTC [241] LOG:  listening on IPv4 address "0.0.0.0", port 5432
2024-12-16 10:07:14.009 UTC [241] LOG:  listening on IPv6 address "::", port 5432
2024-12-16 10:07:14.015 UTC [241] LOG:  listening on Unix socket "/run/postgresql/.s.PGSQL.5432"
2024-12-16 10:07:14.023 UTC [252] LOG:  database system was shut down at 2024-12-16 10:03:29 UTC
2024-12-16 10:07:14.035 UTC [241] LOG:  database system is ready to accept connections
2024-12-16 10:07:14.039 UTC [255] LOG:  TimescaleDB background worker launcher connected to shared catalogs
[12:07:14] INFO: Create database if not exist: 'homeassistant'
[12:07:14] INFO: Starting PgAgent..
[12:07:14] INFO: done
[12:07:14] INFO: done
[12:07:14] NOTICE: TimescaleDb is running!
2024-12-16 10:07:15.004 UTC [285] ERROR:  duplicate key value violates unique constraint "pga_jobagent_pkey"
2024-12-16 10:07:15.004 UTC [285] DETAIL:  Key (jagpid)=(285) already exists.
2024-12-16 10:07:15.004 UTC [285] STATEMENT:  INSERT INTO pgagent.pga_jobagent (jagpid, jagstation) SELECT pg_backend_pid(), '77b2833f-timescaledb'
Mon Dec 16 12:07:15 2024 WARNING: pgAgent Query error: ERROR:  duplicate key value violates unique constraint "pga_jobagent_pkey"
DETAIL:  Key (jagpid)=(285) already exists.
 with status: PGRES_FATAL_ERROR Connection Error: ERROR:  duplicate key value violates unique constraint "pga_jobagent_pkey"
DETAIL:  Key (jagpid)=(285) already exists.
```




### Overall working way to get to working system in my case:

1. Downgrade
2. Safe mode
3. Exit

```shell
# Run twice
ha core update --version 2024.12.1
# Run twice
ha core stop
# Check core:
ha core stats
# Try restart supervisor if core stop runs too long
ha supervisor restart
# or reboot OS completely!
ha host reboot
# OR
sudo reboot now
# OR
sudo halt

# Extra mile
ha supervisor repair
ha core check

# start save after interruption and forced stop!
ha core restart --safe-mode
```


Output:

```shell
➜  ~ ha core update --version 2024.12.1
Processing... Done.

Error: Another job is running for job group home_assistant_core
➜  ~ ha core info
arch: aarch64
audio_input: null
audio_output: null
backups_exclude_database: false
boot: true
image: ghcr.io/home-assistant/raspberrypi5-64-homeassistant
ip_address: 172.30.32.1
machine: raspberrypi5-64
port: 8123
ssl: false
update_available: true
version: 2024.12.1
version_latest: 2024.12.3
watchdog: true

➜  ~ ha core stats
blk_read: 0
blk_write: 0
cpu_percent: 24.98
memory_limit: 8324440064
memory_percent: 4.38
memory_usage: 364920832
network_rx: 0
network_tx: 0


➜  ~ ha supervisor restart
Error: 'Supervisor.restart' blocked from execution, system is not running - startup
➜  ~ ha supervisor repair
Processing... Done.

Command completed successfully.

➜  ~ ha core check
Processing... Done.

Command completed successfully.

```

###