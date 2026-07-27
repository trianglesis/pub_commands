# Music Player Daemon 

Install in RPi and use in HA Music

- <https://mpd.readthedocs.io/en/stable/user.html>
- <https://sysadminsage.com/play-mp3-on-raspberry-pi/>
- <https://www.lesbonscomptes.com/pages/raspmpd.html#installmpd

I use initial setup from SNAPCAST with same USB DACs: [setup](..\snapcast\multiroom_milty_card_usb_sound.md), [simple setup](..\snapcast\simple_setup.md)

## Setup

Check USB DACs (previously configured dev rules/names)

```shell
cat /proc/asound/cards
 0 [USBCardKitchen ]: USB-Audio - USB Audio Device
                      C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.1, full speed
 1 [USBCardShower  ]: USB-Audio - USB Audio Device
                      C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.2, full speed
 2 [USBCardBedroom ]: USB-Audio - USB Audio Device
                      C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.3, full speed

# OR

aplay -l
**** List of PLAYBACK Hardware Devices ****
card 0: USBCardKitchen [USB Audio Device], device 0: USB Audio [USB Audio]
  Subdevices: 1/1
  Subdevice #0: subdevice #0
card 1: USBCardShower [USB Audio Device], device 0: USB Audio [USB Audio]
  Subdevices: 1/1
  Subdevice #0: subdevice #0
card 2: USBCardBedroom [USB Audio Device], device 0: USB Audio [USB Audio]
  Subdevices: 1/1
  Subdevice #0: subdevice #0

# OR
aplay -L | grep hw:CARD=USBCard
hw:CARD=USBCardKitchen,DEV=0
plughw:CARD=USBCardKitchen,DEV=0
hw:CARD=USBCardShower,DEV=0
plughw:CARD=USBCardShower,DEV=0
hw:CARD=USBCardBedroom,DEV=0
plughw:CARD=USBCardBedroom,DEV=0


```

```shell
sudo vi /etc/mpd.conf
```

My conf

```conf
music_directory         "/mnt/share/HA_Share_1Tb/HA_Music/Music/"
playlist_directory      "/mnt/share/HA_Share_1Tb/HA_Music/playlists/"

bind_to_address         "any"

# metadata_to_use        "artist,album,title,track,name,genre,date,composer"


###########################################################
##### USB DACs outputs for HA Music #######################
###########################################################

#############
## Kitchen ##
#############
audio_output {
    type            "alsa"
    name            "USBCardKitchen"
    device          "hw:CARD=USBCardKitchen,DEV=0"
    mixer_type      "software"
}

#############
## Shower ##
#############
audio_output {
    type            "alsa"
    name            "USBCardShower"
    device          "hw:CARD=USBCardShower,DEV=0"
    mixer_type      "software"
}

#############
## Bedroom ##
#############
audio_output {
    type            "alsa"
    name            "USBCardBedroom"
    device          "hw:CARD=USBCardBedroom,DEV=0"
    mixer_type      "software"
}

```

### Continue 


```shell
sudo systemctl restart mpd
```

### Extra

Try use three separate MPD instances one per each USB DAC:
- <https://mpd.readthedocs.io/en/stable/mpd.conf.5.html>
- <https://www.reddit.com/r/systemd/comments/g88sm8/trying_to_run_multiple_instances_of_mpd_with/>

Copy default conf into multiple instances

```
sudo cp /etc/mpd.conf /etc/mpd_kitchen.conf
sudo cp /etc/mpd.conf /etc/mpd_bathroom.conf
sudo cp /etc/mpd.conf /etc/mpd_bedroom.conf

sudo nano /etc/mpd_kitchen.conf
sudo nano /etc/mpd_bathroom.conf
sudo nano /etc/mpd_bedroom.conf
```

Change each and delete other outputs

```conf
db_file                 "/var/lib/mpd/tag_cache_kitchen"
state_file                      "/var/lib/mpd/state_kitchen"
port                           "6601"

db_file                 "/var/lib/mpd/tag_cache_bathroom"
state_file                      "/var/lib/mpd/state_bathroom"
port                           "6602"

db_file                 "/var/lib/mpd/tag_cache_bedroom"
state_file                      "/var/lib/mpd/state_bedroom"
port                           "6603"
```

Copy system conf

```shell
cat /etc/default/mpd
# is dummy

cat /lib/systemd/system/mpd.service
# is actual

sudo cp /etc/default/mpd /etc/default/mpd_kitchen
sudo cp /etc/default/mpd /etc/default/mpd_bathroom
sudo cp /etc/default/mpd /etc/default/mpd_bedroom

# Change path to conf to each separate instances
sudo nano /etc/default/mpd_kitchen
sudo nano /etc/default/mpd_bathroom
sudo nano /etc/default/mpd_bedroom

# Copy service
sudo cp /lib/systemd/system/mpd.service /lib/systemd/system/mpd_kitchen.service
sudo cp /lib/systemd/system/mpd.service /lib/systemd/system/mpd_bathroom.service
sudo cp /lib/systemd/system/mpd.service /lib/systemd/system/mpd_bedroom.service

# Change conf as mentioned below
sudo nano /lib/systemd/system/mpd_kitchen.service
sudo nano /lib/systemd/system/mpd_bathroom.service
sudo nano /lib/systemd/system/mpd_bedroom.service
```

Change: `/etc/default/mpd_kitchen`, `/etc/default/mpd_bathroom`, `/etc/default/mpd_bedroom`

```conf
# Change
MPDCONF=/etc/mpd_kitchen.conf
MPDCONF=/etc/mpd_bathroom.conf
MPDCONF=/etc/mpd_bedroom.conf
```

Change: `/lib/systemd/system/mpd_kitchen.service`, `/lib/systemd/system/mpd_bathroom.service`, `/lib/systemd/system/mpd_bedroom.service`

```conf
# Change
Description=Music Player Daemon Kitchen
EnvironmentFile=/etc/default/mpd_kitchen

Description=Music Player Daemon Bathroom
EnvironmentFile=/etc/default/mpd_bathroom

Description=Music Player Daemon Bedroom
EnvironmentFile=/etc/default/mpd_bedroom
```

#### Stop main, start copies

```shell
# Stop main?
sudo systemctl stop mpd
sudo systemctl disable mpd
# Copies
sudo systemctl start mpd_kitchen.service mpd_bathroom.service mpd_bedroom.service
sudo systemctl enable mpd_kitchen.service mpd_bathroom.service mpd_bedroom.service
sudo systemctl status mpd_kitchen.service mpd_bathroom.service mpd_bedroom.service

# Now try main
sudo systemctl start mpd
sudo systemctl enable mpd
sudo systemctl restart mpd.service mpd_kitchen.service mpd_bathroom.service mpd_bedroom.service

# DISABLE
sudo systemctl disable mpd_kitchen.service mpd_bathroom.service mpd_bedroom.service
```