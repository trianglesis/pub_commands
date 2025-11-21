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