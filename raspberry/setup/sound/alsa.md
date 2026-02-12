# ALSA

- https://techie-show.com/alsamixer-not-saving-settings/
- https://raspberrypi.stackexchange.com/questions/13666/alsa-volume-lower-after-reboot/13674#13674
- https://unix.stackexchange.com/questions/714500/sound-volume-settings-dont-persist-through-reboots
- https://panther.kapsi.fi/posts/2016-04-25_asound
- https://linuxaudiofoundation.org/category/alsa/


```shell
# Volume:
sudo alsamixer
amixer controls

# NOICE!
speaker-test

# Save forever
sudo alsactl store
sudo alsactl --no-ucm store

sudo alsactl store 0 && sudo alsactl store 1 && sudo alsactl store 2 && sudo alsactl store 3 && sudo alsactl store 4

sudo alsactl restore
# Fix failed to import hw:2 use case configuration 
sudo alsactl --no-ucm restore


sudo amixer controls
# numid=3,iface=MIXER,name='Mic Playback Switch'
# numid=4,iface=MIXER,name='Mic Playback Volume'
# numid=7,iface=MIXER,name='Mic Capture Switch'
# numid=8,iface=MIXER,name='Mic Capture Volume'
# numid=9,iface=MIXER,name='Auto Gain Control'
# numid=5,iface=MIXER,name='Speaker Playback Switch'
# numid=6,iface=MIXER,name='Speaker Playback Volume'
# numid=2,iface=PCM,name='Capture Channel Map'
# numid=1,iface=PCM,name='Playback Channel Map'

amixer cget numid=3
# numid=3,iface=MIXER,name='Master Playback Volume'
#   ; type=INTEGER,access=rw------,values=2,min=0,max=65536,step=1
#   : values=16462,16462

sudo amixer cset numid=3 1
```

# Setup ALSA devices

- https://superuser.com/a/1081754
- https://wiki.archlinux.org/title/Advanced_Linux_Sound_Architecture/Configuration_examples
- https://www.tinkerboy.xyz/raspberry-pi-downmixing-from-stereo-to-mono-sound-output/

- https://forums.raspberrypi.com/viewtopic.php?t=33431

```
 Which combines left and right on the left channel (good), but on full volume for each channel (bad). To fix this, and prevent the signal from being sent out at 200%, the simple fix is this: 
```


`vi ~/.asoundrc`
`sudo vi /etc/asound.conf`

Current setup is probably surviving reboots with volume set
- no, only for default (one of three) USB device

```conf
# Bathroom
pcm.usb_card_1 {
    type hw
    card USBCard1
}

# Kitchen
pcm.usb_card_2 {
    type hw
    card USBCard2
}

# Bedroom
pcm.usb_card_3 {
    type hw
    card USBCard3
}


# slave.pcm "hw:USBCard1"
pcm.bathroom_mono{
  slave {
        pcm "usb_card_1"
    }
  slave.channels 2
  type route
  ttable {
    # Copy both input channels to output channel 0 (Left).
    0.0 0.5
    1.0 0.5
    # Send nothing to output channel 1 (Right).
    0.1 0
    1.1 0
  }
}

# slave.pcm "hw:USBCard3"
pcm.bedroom_mono{
  slave {
        pcm "usb_card_3"
    }
  slave.channels 2
  type route
  ttable {
    # Copy both input channels to output channel 0 (Left).
    0.0 0.5
    1.0 0.5
    # Send nothing to output channel 1 (Right).
    0.1 0
    1.1 0
  }
}

```

```
defaults.pcm.!card USBCard1
defaults.ctl.!card USBCard1
defaults.pcm.!device 0
defaults.ctl.!device 0

```

# Settings

```shell
/usr/bin/amixer scontrols

amixer
# Simple mixer control 'Master',0
#   Capabilities: pvolume pswitch pswitch-joined
#   Playback channels: Front Left - Front Right
#   Limits: Playback 0 - 65536
#   Mono:
#   Front Left: Playback 26214 [40%] [on]
#   Front Right: Playback 26214 [40%] [on]
# Simple mixer control 'Capture',0
#   Capabilities: cvolume cvolume-joined cswitch cswitch-joined
#   Capture channels: Mono
#   Limits: Capture 0 - 65536
#   Mono: Capture 65536 [100%] [on]

# Now it saves master volume at ONE default Sound device (of 3)
/usr/bin/amixer set Master 75
amixer set Master 80% >> /dev/null

```

Does not work, volume still can stuck at small % and only be set after user login.

```shell
sudo alsactl --file ~/.config/asound.state store
sudo vi ~/.bashrc
alsactl --file ~/.config/asound.state restore
```


# Workarounds

When\if volume level cannot be saved between reboots - reinstall alsa everything!

- https://linuxaudiofoundation.org/category/alsa/

```shell
sudo dpkg -l alsa*
sudo apt --purge remove alsa-tools alsa-topology-conf alsa-ucm-conf alsa-utils

sudo apt --purge remove alsa-base alsa-firmware-loaders alsa-oss alsa-source alsa-tools alsa-tools-gui alsa-topology-conf alsa-ucm-conf alsamixergui alsa-utils

sudo apt-get install alsa-base alsa-utils alsa-tools libasound2

sudo apt-get install alsa-utils alsa-tools

sudo reboot

# check alsa status
sudo systemctl status alsa-utils.service

# if alsa service is masked unmask it
sudo rm /lib/systemd/system/alsa-utils.service
sudo systemctl daemon-reload
sudo systemctl enable alsa-utils
sudo systemctl start alsa-utils

# check the status again everything should be okay
sudo systemctl status alsa-utils.service

```