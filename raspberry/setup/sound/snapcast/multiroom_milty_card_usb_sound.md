# Setup SnapServer and two clients

# Assign USB Sound card

```shell
:~ $ find /sys/devices/ -name id | grep sound
/sys/devices/platform/soc@107c000000/107c706400.hdmi/sound/card1/id
/sys/devices/platform/soc@107c000000/107c706400.hdmi/sound/card1/input4/id
/sys/devices/platform/soc@107c000000/107c701400.hdmi/sound/card0/id
/sys/devices/platform/soc@107c000000/107c701400.hdmi/sound/card0/input2/id
/sys/devices/platform/snd_aloop.0/sound/card2/id
/sys/devices/platform/axi/1000120000.pcie/1f00200000.usb/xhci-hcd.0/usb1/1-2/1-2:1.0/sound/card4/id
/sys/devices/platform/axi/1000120000.pcie/1f00300000.usb/xhci-hcd.1/usb3/3-2/3-2:1.0/sound/card5/id
/sys/devices/platform/axi/1000120000.pcie/1f00300000.usb/xhci-hcd.1/usb3/3-1/3-1:1.0/sound/card3/id
```


```
# This is a sample ude rules file to staticaly assign names to sound cards (in this case USB) that have the exact
# same product, vendor and serial number. This normally creates card names in pulseaudio that are a combination of this
# attributes plus an auto incrementing numbering, the problem is that the cards will get their names depending on the 
# order the cards are plugged in.
# This udev rules fixes that issues by assigning a name to any card that is plugged in the same USB port, I don't know
# any other way.
#
# Name this file something like /etc/udev/rules.d/95-identical-cards-names.rules
# The list of cards should be changed to match your system, the only lines you are supposed to change/remove/add 
# are DEVPATHs, look a the comments comments to see where to get the soundcard path.
#
#
# For alsa card naming (check with `cat /proc/asound/cards`)
#
SUBSYSTEM!="sound", GOTO="alsa_naming_end"
ACTION!="add", GOTO="alsa_naming_end"

# DEVPATH can be obtained by looking at `udevadm monitor --subsystem=sound` and while pluging in the sound card.
# Do one card at a time, the "?" char on card should stay as it matches any card number that may pop on that USB port.
DEVPATH=="/devices/platform/axi/1000120000.pcie/1f00300000.usb/xhci-hcd.1/usb3/3-2/3-2:1.0/sound/card?", ATTR{id}="USBCard1"
DEVPATH=="/devices/platform/axi/1000120000.pcie/1f00200000.usb/xhci-hcd.0/usb1/1-1/1-1:1.0/sound/card?", ATTR{id}="USBCard2"
DEVPATH=="/devices/platform/axi/1000120000.pcie/1f00300000.usb/xhci-hcd.1/usb3/3-1/3-1:1.0/sound/card?", ATTR{id}="USBCard3"

LABEL="alsa_naming_end"
```

## Use names

Later use names: USBCard1,2,3 instead of IDs.

snapclient --instance=3 --Soundcard=USBCard1 --hostID=Test
snapclient --instance=3 --Soundcard=USBCard2 --hostID=Test
snapclient --instance=3 --Soundcard=USBCard3 --hostID=Test

## Problems:

Reboot will mix USB sound cards all over

Get list of devices:

- `cat /proc/asound/cards`
- `snapclient -l`

Change default system device:

- `alsamixer`
- `sudo alsactl store`

# Test devices one after another

## 1 Bathroom

snapclient --instance=3 --Soundcard=7 --hostID=Test
snapclient --instance=3 --Soundcard=11 --hostID=Test
snapclient --instance=3 --Soundcard=39 --hostID=Test

```
39: hw:CARD=Device,DEV=0
USB Audio Device, USB Audio
Direct hardware device without any conversions

40: plughw:CARD=Device,DEV=0
USB Audio Device, USB Audio
Hardware device with all software conversions

41: sysdefault:CARD=Device
USB Audio Device, USB Audio
Default Audio Device

42: front:CARD=Device,DEV=0
USB Audio Device, USB Audio
Front output / input
```

## 2 Bedroom
snapclient --instance=3 --Soundcard=52 --hostID=Test

```
52: hw:CARD=Device_1,DEV=0
USB Audio Device, USB Audio
Direct hardware device without any conversions

53: plughw:CARD=Device_1,DEV=0
USB Audio Device, USB Audio
Hardware device with all software conversions

54: sysdefault:CARD=Device_1
USB Audio Device, USB Audio
Default Audio Device

55: front:CARD=Device_1,DEV=0
USB Audio Device, USB Audio
Front output / input
```


# Try to run two separate instances:

- https://github.com/badaix/snapcast/issues/809

Re-use

## First service

`$ cat /lib/systemd/system/snapclient.service`

```conf
[Unit]
Description=Snapcast client
Documentation=man:snapclient(1)
Wants=avahi-daemon.service
After=network-online.target time-sync.target sound.target avahi-daemon.service

[Service]
EnvironmentFile=-/etc/default/snapclient
ExecStart=/usr/bin/snapclient --logsink=system $SNAPCLIENT_OPTS
User=snapclient
Group=snapclient
Restart=on-failure

[Install]
WantedBy=multi-user.target
```

## First conf

```conf
# Start the client, used only by the init.d script
START_SNAPCLIENT=true

# Additional command line options that will be passed to snapclient
# note that user/group should be configured in the init.d script or the systemd unit file
# For a list of available options, invoke "snapclient --help"

# Bathroom
SNAPCLIENT_OPTS="--instance=1 --Soundcard=39 --hostID=Bathroom"
```


Additional instance by copying the defaut one:

`sudo cp /lib/systemd/system/snapclient.service /lib/systemd/system/snapclient_2.service`
`sudo cp /etc/default/snapclient /etc/default/snapclient_2`


Modify copied:

`sudo vi /lib/systemd/system/snapclient_2.service`
`sudo vi /etc/default/snapclient_2`

## Second Service

```conf
[Unit]
Description=Snapcast client 2
Documentation=man:snapclient(1)
Wants=avahi-daemon.service
After=network-online.target time-sync.target sound.target avahi-daemon.service

[Service]
EnvironmentFile=-/etc/default/snapclient_2
ExecStart=/usr/bin/snapclient --logsink=system $SNAPCLIENT_OPTS
User=snapclient
Group=snapclient
Restart=on-failure

[Install]
WantedBy=multi-user.target
```

## Second config file

```conf
# Start the client, used only by the init.d script
START_SNAPCLIENT=true

# Additional command line options that will be passed to snapclient
# note that user/group should be configured in the init.d script or the systemd unit file
# For a list of available options, invoke "snapclient --help"

# Bedroom
SNAPCLIENT_OPTS="--instance=2 --Soundcard=52 --hostID=Bedroom"
```

# Start Restart Reload

```shell
sudo systemctl daemon-reload
sudo systemctl restart snapclient
sudo systemctl restart snapclient_2
```

# Etc

- https://github.com/badaix/snapcast/issues/1358
- https://www.alsa-project.org/main/index.php/Asoundrc


# Test again
```
Kitchen:
75: iec958:CARD=Device_2,DEV=0

Bathroom
49: iec958:CARD=Device,DEV=0

Bedroom
62: iec958:CARD=Device_1,DEV=0

```

Add more

```shell
sudo cp /lib/systemd/system/snapclient.service /lib/systemd/system/snapclient_3.service
sudo cp /etc/default/snapclient /etc/default/snapclient_3
sudo vi /lib/systemd/system/snapclient_3.service
sudo vi /etc/default/snapclient_3

sudo systemctl enable snapclient_3
sudo systemctl restart snapclient_3
```


# Drift USB devices after reboot

```shell
snapclient -l

sudo vi /etc/default/snapclient
sudo vi /etc/default/snapclient_2
sudo vi /etc/default/snapclient_3

snapclient --instance=3 --Soundcard=USBCard1
snapclient --instance=3 --Soundcard=USBCard2
snapclient --instance=3 --Soundcard=USBCard3


sudo systemctl enable snapclient snapclient_2 snapclient_3
sudo systemctl restart snapclient snapclient_2 snapclient_3
```