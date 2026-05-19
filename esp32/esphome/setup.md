# Setup

## Virtualenv

```shell
cd PROJECT_DIR
virtualenv --python=/usr/local/bin/python3.14 venv --system-site-packages
#
# Use
source venv/bin/activate
source venv/bin/deactivate
#
pip install esphome
```

## RUN

```shell
# D:\Projects\ESP\ESPHome\ESPhome\my_proj
ls /mnt/d/Projects/ESP/ESPHome/
ls /mnt/d/Projects/ESP/ESPHome/ESPhome/my_proj/
esphome dashboard --port=8080 --address=127.0.0.1 /mnt/d/Projects/ESP/ESPHome/ESPhome/my_proj/
```

## Problems and fixes

On Linux distros without user interactive desktop features: MUST install this:

- <https://wiki.archlinux.org/title/XDG_user_directories>

`subprocess.check_output(["xdg-user-dir", "DOCUMENTS"])`
`PermissionError: [Errno 13] Permission denied: 'xdg-user-dir'`

FIX:

```shell
xdg-user-dirs-update
xdg-user-dirs-update
# 
# /home/user/Desktop was removed, reassigning DESKTOP to homedir
# /home/user/Downloads was removed, reassigning DOWNLOAD to homedir
# /home/user/Templates was removed, reassigning TEMPLATES to homedir
# /home/user/Public was removed, reassigning PUBLICSHARE to homedir
# /home/user/Documents was removed, reassigning DOCUMENTS to homedir
# /home/user/Music was removed, reassigning MUSIC to homedir
# /home/user/Pictures was removed, reassigning PICTURES to homedir
# /home/user/Videos was removed, reassigning VIDEOS to homedir
```