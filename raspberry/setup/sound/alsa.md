# ALSA

- https://techie-show.com/alsamixer-not-saving-settings/
- https://raspberrypi.stackexchange.com/questions/13666/alsa-volume-lower-after-reboot/13674#13674
- https://unix.stackexchange.com/questions/714500/sound-volume-settings-dont-persist-through-reboots
- https://panther.kapsi.fi/posts/2016-04-25_asound
- https://linuxaudiofoundation.org/category/alsa/
- https://forum.libreelec.tv/thread/28211-saving-and-restoring-alsa-mixer-state/ !!!!


```shell
# Volume:
sudo alsamixer
amixer controls


sudo modprobe snd-bcm2835                      # load module for single boot
echo "snd-bcm2835" | sudo tee -a /etc/modules  # load module for persistance

# NOICE!
speaker-test

# Save forever
sudo alsactl store
sudo alsactl --no-ucm store
sudo alsactl store 0 && sudo alsactl store 1 && sudo alsactl store 2 && sudo alsactl store 3 && sudo alsactl store 4

sudo alsactl restore
# Fix failed to import hw:2 use case configuration 
sudo alsactl --no-ucm restore

# Clean
sudo alsactl clean
sudo alsactl restore -P
sudo rm /var/lib/alsa/asound.state

sudo systemctl enable alsa-restore.service
sudo systemctl enable alsa-state.service
sudo systemctl enable alsa-utils.service

sudo systemctl start alsa-restore.service && sudo systemctl start alsa-state.service && sudo systemctl start alsa-utils.service

sudo systemctl status alsa-restore.service 
sudo systemctl status alsa-state.service 
sudo systemctl status alsa-utils.service

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

amixer cset numid=3 70%
```

# Setup ALSA devices

- https://superuser.com/a/1081754
- https://wiki.archlinux.org/title/Advanced_Linux_Sound_Architecture/Configuration_examples
- https://www.tinkerboy.xyz/raspberry-pi-downmixing-from-stereo-to-mono-sound-output/

- https://forums.raspberrypi.com/viewtopic.php?t=33431

```
 Which combines left and right on the left channel (good), but on full volume for each channel (bad). To fix this, and prevent the signal from being sent out at 200%, the simple fix is this: 
```


`vi cat `
`sudo vi /etc/asound.conf`

Current setup is probably surviving reboots with volume set
- no, only for default (one of three) USB device
- reinstall alsa everything! - does not help


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

## Settings

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
/usr/bin/amixer set Master 80
amixer set Master 80% >> /dev/null

```

Work but need a user login

```shell
sudo alsactl --file ~/.config/asound.state store
sudo vi ~/.bashrc
sudo vi /root/.bashrc
sudo alsactl --file ~/.config/asound.state restore
```


## Workarounds


- https://linuxaudiofoundation.org/category/alsa/

```shell
sudo dpkg -l alsa*
sudo apt --purge remove alsa-tools alsa-topology-conf alsa-ucm-conf alsa-utils

sudo apt --purge remove alsa-base alsa-firmware-loaders alsa-oss alsa-source alsa-tools alsa-tools-gui alsa-topology-conf alsa-ucm-conf alsamixergui alsa-utils

sudo apt-get install alsa-base alsa-utils alsa-tools libasound2

sudo apt-get install alsa-utils alsa-tools
sudo apt-get install alsa-ucm-conf

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


## Pusleaudio

- https://www.freedesktop.org/software/pulseaudio/pavucontrol/?__goaway_challenge=meta-refresh&__goaway_id=bb3d98019ab369cd8f7ab4db831735c5&__goaway_referer=https%3A%2F%2Fduckduckgo.com%2F
- https://forums.raspberrypi.com/viewtopic.php?t=62851


```shell
sudo apt update
sudo apt install pulseaudio-utils pavucontrol

# remove
sudo apt -y purge "pavucontrol"
sudo apt -y purge "pulseaudio"
sudo apt -y purge "pulseaudio-utils"
```