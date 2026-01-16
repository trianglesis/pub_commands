# Next Cload as APP in TrueNAS

DOC:
- https://tduffinntu.github.io/posts/nextcloud-truenas-scale/
- https://computingondemand.com/truenas-scale-fix-nextcloud-database-missing-indices/
- https://forums.truenas.com/t/nextcloud-external-apps-need-docker-reference/22668
- https://forums.truenas.com/t/configuring-nextcloud-cron-jobs-in-truenas-scale/3755/16?u=user27
- https://www.truenas.com/community/threads/truenas-scale-nextcloud-cron-not-working.102870/post-707773
- https://help.nextcloud.com/t/some-jobs-have-not-been-executed-warning-despite-cron-running-correctly/215490
- https://doc.owncloud.com/server/next/admin_manual/configuration/server/background_jobs_configuration.html
- https://docs.nextcloud.com/server/stable/admin_manual/configuration_server/background_jobs_configuration.html
- https://github.com/nextcloud/server/issues/46149


## Fixes


### Truenas Scale Fix Nextcloud Database Missing Indices

```text

Log into the shell of your container from TrueNAS Scale by clicking on the app from the Apps page and looking for the shell icon
Choose the container with a string of digits that also displays “nextcloud” in the Containers*
at the command prompt type “whoami”
    This will likely return “root” and this is the wrong user for us
Type “su -l www-data -s /bin/bash”
    This will change you to the www-data user
    Verify by typing “whoami”
Type “php /var/www/html/occ db:add-missing-indices”

```



### AppAPI deploy daemon 

Set local IP for docker app from NAS


```text
Nextcloud Wiki:

You will need to set up a Deploy Daemon in the AppAPI admin settings. A Deploy Daemon is a way for Nextcloud to install and communicate with and control External Apps.

    You will need to setup a docker container called docker-socket-proxy that proxies access to docker for your Nextcloud instance
    Now you can connect your Nextcloud to the docker-socket-proxy by entering its details in the Deploy Daemon creation form in the AppAPI settings.

```

### CRON

Update True NAS app for NextCloud allowing CRON checkbox: `*/5 * * * *`

OR:

- "System settings->Advanced" add:

```shell

# My "ix-nextcloud-nextcloud-1"
sudo docker ps | grep "ix-nextcloud-nextcloud-1" | awk '{ print $1; }'
52804b32a26b

# Test
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php /var/www/html/cron.php
#OR
sudo docker ps | grep "ix-nextcloud-nextcloud-1" | awk '{ print $1; }' | xargs -I % sudo docker exec --user www-data "%" php -f /var/www/html/cron.php
# - should be no output

# Use wrong name to test
sudo docker exec -u www-data ix-nextcloud-1 php /var/www/html/cron.php
```

Extra run skipped jobs by hand:
Go to Apps and select shell for cron:

```shell
su -l www-data -s /bin/bash
cd html
php occ background-job:list
# See the table and run skipped at separate shell
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ background-job:execute --force-execute 8230

# Additions
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ maintenance:repair
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ db:add-missing-indices
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ db:add-missing-columns
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ db:add-missing-primary-keys

# ???
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ background:cron
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ background:queue:status --display-invalid-jobs
# From SSH
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ background-job:list

# Run all? - Really helped to resolve the issue!
sudo docker exec -u www-data ix-nextcloud-nextcloud-1 php occ background-job:worker
```


```shell
su -l www-data -s /bin/bash
cd html
php occ background-job:list

+------+-------------------------------------------------------------------+---------------------------+---------------------------------------------------------------------------------------------------------------------------+
| id   | class                                                             | last_run                  | argument                                                                                                                  |
+------+-------------------------------------------------------------------+---------------------------+---------------------------------------------------------------------------------------------------------------------------+
| 1    | OCA\Files_Trashbin\BackgroundJob\ExpireTrash                      | 2025-10-21T15:01:43+00:00 | null                                                                                                                      |
| 30   | OCA\OAuth2\BackgroundJob\CleanupExpiredAuthorizationCode          | 2025-10-11T16:17:50+00:00 | null                                                                                                                      |
| 14   | OCA\UpdateNotification\BackgroundJob\UpdateAvailableNotifications | 2025-10-21T15:10:47+00:00 | null                                                                                                                      |
| 20   | OCA\Files\BackgroundJob\ScanFiles                                 | 2025-10-21T15:10:02+00:00 | null                                                                                                                      |
| 34   | OCA\DAV\BackgroundJob\CleanupInvitationTokenJob                   | 2025-10-21T08:44:12+00:00 | null                                                                                                                      |
| 42   | OC\Preview\BackgroundCleanupJob                                   | 1970-01-01T00:00:00+00:00 | null                                                                                                                      |
|...
```

### ETC

```shell
      background-job:delete
      background-job:execute
      background-job:list
      background-job:worker
      background:ajax
      background:cron
      background:webcron
``



# Immich fix


- https://github.com/immich-app/immich/issues/24009#issuecomment-3556056053

```
For TrueNAS users it was kind of a pain for me to figure out how to run the command to fix this issue, so hopefully this saves someone a little suffering:

    Select the immich application
    Click the shell icon for the pgvecto container
    Login to pg with psql -U immich -d immich
    Run the fix command DELETE FROM system_metadata WHERE key = 'version-check-state';
    Exit with \q
    Profit

```

```shell

sudo docker ps | grep "immich"

# ix-immich-pgvecto-1
# ix-immich-server-1

sudo docker exec ix-immich-server-1  psql -U postgres -d immich -c "UPDATE system_metadata SET value = value::jsonb || '{\"newVersionCheck\": {\"enabled\": false}}'::jsonb WHERE key = 'system-config'; DELETE FROM system_metadata WHERE key = 'version-check-state';"
```



