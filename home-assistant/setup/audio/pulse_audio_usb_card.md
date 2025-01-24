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

# ➜  ~ ha audio volume output --index 6 --volume 65
# Command completed successfully.
```



Info 

```text
➜  ~ ha audio info
audio:
  application: []
  card:
  - driver: module-alsa-card.c
    index: 3
    name: alsa_card.usb-C-Media_Electronics_Inc._USB_Audio_Device-00
    profiles:
    - active: false
      description: Mono Input
      name: input:mono-fallback
    - active: false
      description: Analog Stereo Output
      name: output:analog-stereo
    - active: true
      description: Analog Stereo Output + Mono Input
      name: output:analog-stereo+input:mono-fallback
    - active: false
      description: Digital Stereo (IEC958) Output
      name: output:iec958-stereo
    - active: false
      description: Digital Stereo (IEC958) Output + Mono Input
      name: output:iec958-stereo+input:mono-fallback
    - active: false
      description: "Off"
      name: "off"
  input:
  - applications: []
    card: 3
    default: true
    description: USB Audio Device Mono
    index: 10
    mute: false
    name: alsa_input.usb-C-Media_Electronics_Inc._USB_Audio_Device-00.mono-fallback
    volume: 0.261016845703125
  output:
  - applications: []
    card: 3
    default: true
    description: USB Audio Device Analog Stereo
    index: 6
    mute: false
    name: alsa_output.usb-C-Media_Electronics_Inc._USB_Audio_Device-00.analog-stereo
    volume: 0.5500030517578125
host: 172.30.32.4
update_available: false
version: 2023.12.0
version_latest: 2023.12.0
```
