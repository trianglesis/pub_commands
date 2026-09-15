# Thingino

- <https://thingino.com/installation>
- Camera <https://thingino.com/cameras/42>

## Install

- <https://github.com/wltechblog/thingino-installers>

Use SD card installed method. Write zip as boot image with PRI imager.

## SSH

Update: <https://github.com/gtxaspec/thingino-wiki/blob/master/docs/Updating.md>
Sysupgrade:  <https://github.com/themactep/thingino-firmware/blob/master/docs/firmware/sysupgrade.md>

## TTS

- <https://github.com/themactep/thingino-firmware/blob/master/scripts/tts/README.md>
- <https://github.com/themactep/thingino-firmware/tree/master/package/thingino-esphome/plugins/thingino_media_player>

### Play sound

Upload some media files at camera FS

```shell
root@box-cam ~# sleep 10 && play /mnt/mmcblk0p1/fog-horn-3-183969.mp3 -v 100
```