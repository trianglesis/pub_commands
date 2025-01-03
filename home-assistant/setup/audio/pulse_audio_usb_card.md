# Produce sound, TTS, play music locally from HA


- https://community.home-assistant.io/t/select-default-audio-device/420811/7


HA VLC Addon keeps volume level at 50% always. 
Use system volume level to lower the max volume for the system.

```shell
ha audio info

# Set default
ha audio default output --name alsa_output.usb-C-Media_Electronics_Inc._USB_Audio_Device-00.analog-stereo

# Change volume
ha audio volume output --index 0 --volume 55
```