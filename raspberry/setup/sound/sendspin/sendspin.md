# sendspin

-<https://github.com/Sendspin/sendspin-cli?tab=readme-ov-file#install-as-daemon-systemd-linux>


TLDR: It's raw and does not work as fast and sync as Snapcast.

## See devices

Each USB sound card should have a separate daemon.

```shell
cat /proc/asound/cards
aplay -l

**** List of PLAYBACK Hardware Devices ****
card 0: USBCardKitchen [USB Audio Device], device 0: USB Audio [USB Audio]
  Subdevices: 0/1
  Subdevice #0: subdevice #0
card 1: vc4hdmi0 [vc4-hdmi-0], device 0: MAI PCM i2s-hifi-0 [MAI PCM i2s-hifi-0]
  Subdevices: 1/1
  Subdevice #0: subdevice #0
card 2: USBCardShower [USB Audio Device], device 0: USB Audio [USB Audio]
  Subdevices: 1/1
  Subdevice #0: subdevice #0
card 3: vc4hdmi1 [vc4-hdmi-1], device 0: MAI PCM i2s-hifi-0 [MAI PCM i2s-hifi-0]
  Subdevices: 1/1
  Subdevice #0: subdevice #0
card 4: USBCardBedroom [USB Audio Device], device 0: USB Audio [USB Audio]
  Subdevices: 1/1
  Subdevice #0: subdevice #0

cat /proc/asound/cards
 0 [USBCardKitchen ]: USB-Audio - USB Audio Device
                      C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.1, full speed
 1 [vc4hdmi0       ]: vc4-hdmi - vc4-hdmi-0
                      vc4-hdmi-0
 2 [USBCardShower  ]: USB-Audio - USB Audio Device
                      C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.2, full speed
 3 [vc4hdmi1       ]: vc4-hdmi - vc4-hdmi-1
                      vc4-hdmi-1
 4 [USBCardBedroom ]: USB-Audio - USB Audio Device
                      C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.3, full speed


sudo alsamixer
sudo alsactl store
sudo alsactl restore
```

### Daemonize

-<https://github.com/Sendspin/sendspin-cli?tab=readme-ov-file#install-as-daemon-systemd-linux>
-<https://gist.github.com/wiking-at/d7789afb9107c3f6a16501e1914bbe41>

Use [asound conf](../asound.conf)

#### Disable snapcast

```shell
sudo systemctl stop snapserver snapclient_bathroom snapclient_bedroom snapclient_kitchen
sudo systemctl disable snapserver snapclient_bathroom snapclient_bedroom snapclient_kitchen
```

#### Setup sendspin

```shell
mkdir ~/Downloads/sendspin
cd ~/Downloads/sendspin

# Install
curl -fsSL https://raw.githubusercontent.com/Sendspin/sendspin-cli/refs/heads/main/scripts/systemd/install-systemd.sh | sudo bash

# Manage
sudo systemctl start sendspin    # Start the service
sudo systemctl stop sendspin     # Stop the service
sudo systemctl status sendspin   # Check status
journalctl -u sendspin -f        # View logs

# Do not use default one
sudo systemctl disable sendspin

sudo /home/sendspin/.local/bin/sendspin --list-audio-devices
sudo /home/sendspin/.local/bin/sendspin --list-servers
#  ws://192.168.1.15:8927/sendspin
```

Do not start it yet, duplicate service config for all sound cards.

See: <https://github.com/Sendspin/sendspin-cli/blob/main/scripts/systemd/install-systemd.sh>

```shell
ls /etc/default/sendspin
ls /lib/systemd/system/

Config:  /home/sendspin/.config/sendspin/settings-daemon.json
Service: systemctl {start|stop|status} sendspin
Logs:    journalctl -u sendspin -f

# Conf, but do not use it
sudo cat /home/sendspin/.config/sendspin/settings-daemon.json
sudo systemctl stop sendspin.service
sudo systemctl disable sendspin.service

# Reuse this conf
sudo cat /etc/systemd/system/sendspin.service
sudo cp /etc/systemd/system/sendspin.service /etc/systemd/system/sendspin_kitchen.service
sudo cp /etc/systemd/system/sendspin.service /etc/systemd/system/sendspin_bathroom.service
sudo cp /etc/systemd/system/sendspin.service /etc/systemd/system/sendspin_bedroom.service

sudo vi /etc/systemd/system/sendspin_kitchen.service
sudo vi /etc/systemd/system/sendspin_bathroom.service
sudo vi /etc/systemd/system/sendspin_bedroom.service

sudo systemctl daemon-reload

sudo systemctl start sendspin_kitchen.service sendspin_bathroom.service sendspin_bedroom.service
sudo systemctl restart sendspin_kitchen.service sendspin_bathroom.service sendspin_bedroom.service
sudo systemctl status sendspin_kitchen.service sendspin_bathroom.service sendspin_bedroom.service
sudo systemctl stop sendspin_kitchen.service sendspin_bathroom.service sendspin_bedroom.service
sudo systemctl disable sendspin_kitchen.service sendspin_bathroom.service sendspin_bedroom.service

journalctl -u sendspin_kitchen -f
journalctl -u sendspin_bathroom -f
journalctl -u sendspin_bedroom -f

```

```shell
usb_card_shower
usb_card_bedroom
usb_card_kitchen

# Modify exec
/home/sendspin/.local/bin/sendspin daemon --hardware-volume true --static-delay-ms -100 --audio-format flac:48000:24:2 --id kitchen --name "Kitchen" --url ws://192.168.1.15:8927/sendspin --audio-device "usb_card_kitchen"
/home/sendspin/.local/bin/sendspin daemon --hardware-volume true --static-delay-ms -100 --audio-format flac:48000:24:1 --id bathroom --name "Bathroom" --url ws://192.168.1.15:8927/sendspin --audio-device "usb_card_shower"
/home/sendspin/.local/bin/sendspin daemon --hardware-volume true --static-delay-ms -100 --audio-format flac:48000:24:1 --id bedroom --name "Bedroom" --url ws://192.168.1.15:8927/sendspin --audio-device "usb_card_bedroom"

# Test directly
sudo /home/sendspin/.local/bin/sendspin daemon --hardware-volume true --static-delay-ms -100 --audio-format flac:48000:24:1 --id kitchen --name "Kitchen" --url ws://192.168.1.15:8927/sendspin --audio-device usb_card_kitchen
sudo /home/sendspin/.local/bin/sendspin daemon --hardware-volume true --static-delay-ms -100 --audio-format flac:48000:24:1 --id bathroom --name "Bathroom" --url ws://192.168.1.15:8927/sendspin --audio-device usb_card_shower
sudo /home/sendspin/.local/bin/sendspin daemon --hardware-volume true --static-delay-ms -100 --audio-format flac:48000:24:1 --id bedroom --name "Bedroom" --url ws://192.168.1.15:8927/sendspin --audio-device usb_card_bedroom
```

#### Log

Install

```log

~/Downloads/sendspin $ curl -fsSL https://raw.githubusercontent.com/Sendspin/sendspin-cli/refs/heads/main/scripts/systemd/install-systemd.sh | sudo bash

Sendspin Service Installation

User Setup
You can run sendspin as a dedicated 'sendspin' user (recommended)
or as your current user (sanek).

Use dedicated 'sendspin' user? [Y/n] Y
Creating sendspin system user...
✓ Created sendspin system user
✓ Linger enabled for sendspin
Daemon will run as: sendspin

Checking dependencies...
Missing: libportaudio2
Install now? (apt-get install -y libportaudio2) [Y/n] Y
Reading package lists... Done
Building dependency tree... Done
Reading state information... Done
The following NEW packages will be installed:
  libportaudio2
0 upgraded, 1 newly installed, 0 to remove and 17 not upgraded.
Need to get 58.8 kB of archives.
After this operation, 221 kB of additional disk space will be used.
Get:1 http://deb.debian.org/debian trixie/main arm64 libportaudio2 arm64 19.6.0-1.2+b3 [58.8 kB]
Fetched 58.8 kB in 0s (393 kB/s)
Selecting previously unselected package libportaudio2:arm64.
(Reading database ... 179849 files and directories currently installed.)
Preparing to unpack .../libportaudio2_19.6.0-1.2+b3_arm64.deb ...
Unpacking libportaudio2:arm64 (19.6.0-1.2+b3) ...
Setting up libportaudio2:arm64 (19.6.0-1.2+b3) ...
Processing triggers for libc-bin (2.41-12+rpt1+deb13u1) ...
Missing: libopenblas0
Install now? (apt-get install -y libopenblas0) [Y/n] y
Reading package lists... Done
Building dependency tree... Done
Reading state information... Done
The following additional packages will be installed:
  libopenblas0-pthread
The following NEW packages will be installed:
  libopenblas0 libopenblas0-pthread
0 upgraded, 2 newly installed, 0 to remove and 17 not upgraded.
Need to get 3,599 kB of archives.
After this operation, 25.6 MB of additional disk space will be used.
Get:1 http://deb.debian.org/debian trixie/main arm64 libopenblas0-pthread arm64 0.3.29+ds-3 [3,557 kB]
Get:2 http://deb.debian.org/debian trixie/main arm64 libopenblas0 arm64 0.3.29+ds-3 [42.7 kB]
Fetched 3,599 kB in 0s (12.8 MB/s)
Selecting previously unselected package libopenblas0-pthread:arm64.
(Reading database ... 179856 files and directories currently installed.)
Preparing to unpack .../libopenblas0-pthread_0.3.29+ds-3_arm64.deb ...
Unpacking libopenblas0-pthread:arm64 (0.3.29+ds-3) ...
Selecting previously unselected package libopenblas0:arm64.
Preparing to unpack .../libopenblas0_0.3.29+ds-3_arm64.deb ...
Unpacking libopenblas0:arm64 (0.3.29+ds-3) ...
Setting up libopenblas0-pthread:arm64 (0.3.29+ds-3) ...
update-alternatives: using /usr/lib/aarch64-linux-gnu/openblas-pthread/libblas.so.3 to provide /usr/lib/aarch64-linux-gnu/libblas.so.3 (libblas.so.3-aarch64-linux-gnu) in auto mode
update-alternatives: using /usr/lib/aarch64-linux-gnu/openblas-pthread/liblapack.so.3 to provide /usr/lib/aarch64-linux-gnu/liblapack.so.3 (liblapack.so.3-aarch64-linux-gnu) in auto mode
update-alternatives: using /usr/lib/aarch64-linux-gnu/openblas-pthread/libopenblas.so.0 to provide /usr/lib/aarch64-linux-gnu/libopenblas.so.0 (libopenblas.so.0-aarch64-linux-gnu) in auto mode
Setting up libopenblas0:arm64 (0.3.29+ds-3) ...
Processing triggers for libc-bin (2.41-12+rpt1+deb13u1) ...
Missing: uv
Install now? (curl -LsSf https://astral.sh/uv/install.sh | sh) [Y/n] y
downloading uv 0.11.2 aarch64-unknown-linux-gnu
installing to /home/sendspin/.local/bin
  uv
  uvx
everything's installed!

To add $HOME/.local/bin to your PATH, either restart your shell or run:

    source $HOME/.local/bin/env (sh, bash, zsh)
    source $HOME/.local/bin/env.fish (fish)
✓ uv installed

Installing Sendspin...
Resolved 35 packages in 3.30s
      Built mpris-api==1.3.0
      Built tunit==1.7.2
Prepared 35 packages in 1.48s
Installed 35 packages in 25ms
 + aiohappyeyeballs==2.6.1
 + aiohttp==3.13.3
 + aiosendspin==4.4.0
 + aiosendspin-mpris==2.1.1
 + aiosignal==1.4.0
 + attrs==26.1.0
 + av==17.0.0
 + cffi==2.0.0
 + dbus-next==0.2.3
 + frozenlist==1.8.0
 + idna==3.11
 + ifaddr==0.2.0
 + markdown-it-py==4.0.0
 + mashumaro==3.20
 + mdurl==0.1.2
 + mpris-api==1.3.0
 + multidict==6.7.1
 + numpy==2.4.3
 + orjson==3.11.7
 + pillow==12.1.1
 + propcache==0.4.1
 + pulsectl==24.11.0
 + pulsectl-asyncio==1.2.2
 + pycparser==3.0
 + pygments==2.19.2
 + qrcode==8.2
 + readchar==4.2.1
 + rich==14.3.3
 + sendspin==5.9.0
 + sounddevice==0.5.5
 + tunit==1.7.2
 + typing-extensions==4.15.0
 + unidecode==1.4.0
 + yarl==1.23.0
 + zeroconf==0.148.0
Installed 1 executable: sendspin

Configuration
Client friendly name (shown in the UI) 

Configuration
Client friendly name (shown in the UI) [raspberrypi]: rasp5
Client ID (used in settings and scripts) [rasp5]: 1

Audio Devices
Available audio output devices:

  [0] USB Audio Device: - (hw:0,0)
       Channels: 2, Sample rate: 44100.0 Hz
  [1] USB Audio Device: - (hw:2,0)
       Channels: 2, Sample rate: 44100.0 Hz
  [2] USB Audio Device: - (hw:4,0)
       Channels: 2, Sample rate: 44100.0 Hz
  [3] sysdefault
       Channels: 128, Sample rate: 48000.0 Hz
  [4] front
       Channels: 2, Sample rate: 44100.0 Hz
  [5] surround40
       Channels: 2, Sample rate: 44100.0 Hz
  [6] iec958
       Channels: 2, Sample rate: 44100.0 Hz
  [7] spdif
       Channels: 2, Sample rate: 44100.0 Hz
  [8] lavrate
       Channels: 128, Sample rate: 44100.0 Hz
  [9] samplerate
       Channels: 128, Sample rate: 44100.0 Hz
  [10] speexrate
       Channels: 128, Sample rate: 44100.0 Hz
  [11] a52
       Channels: 6, Sample rate: 48000.0 Hz
  [12] speex
       Channels: 1, Sample rate: 44100.0 Hz
  [13] upmix
       Channels: 8, Sample rate: 44100.0 Hz
  [14] vdownmix
       Channels: 6, Sample rate: 44100.0 Hz
  [15] default (default)
       Channels: 128, Sample rate: 48000.0 Hz
  [16] dmix
       Channels: 2, Sample rate: 48000.0 Hz

To select an audio device:
  sendspin --audio-device 0

ALSA devices (use by name with --audio-device):

  null
       Discard all samples (playback) or generate zero samples (capture)
  default
       Default Audio Device
  sysdefault
       Default Audio Device
  lavrate
       Rate Converter Plugin Using Libav/FFmpeg Library
  samplerate
       Rate Converter Plugin Using Samplerate Library
  speexrate
       Rate Converter Plugin Using Speex Resampler
  jack
       JACK Audio Connection Kit
  oss
       Open Sound System
  pulse
       PulseAudio Sound Server
  speex
       Plugin using Speex DSP (resample, agc, denoise, echo, dereverb)
  upmix
       Plugin for channel upmix (4,6,8)
  vdownmix
       Plugin for channel downmix (stereo) with a simple spacialization
  usb_card_1
  usb_card_2
  usb_card_3
  bathroom_mono
  bedroom_mono
  hw:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  plughw:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  default:CARD=USBCardKitchen
       USB Audio Device, USB Audio
  sysdefault:CARD=USBCardKitchen
       USB Audio Device, USB Audio
  front:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround21:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround40:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround41:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround50:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround51:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround71:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  iec958:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  dmix:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  usbstream:CARD=USBCardKitchen
       USB Audio Device
  hw:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  plughw:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  default:CARD=vc4hdmi0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  sysdefault:CARD=vc4hdmi0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  hdmi:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  dmix:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  usbstream:CARD=vc4hdmi0
       vc4-hdmi-0
  hw:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  plughw:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  default:CARD=USBCardShower
       USB Audio Device, USB Audio
  sysdefault:CARD=USBCardShower
       USB Audio Device, USB Audio
  front:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround21:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround40:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround41:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround50:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround51:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround71:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  iec958:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  dmix:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  usbstream:CARD=USBCardShower
       USB Audio Device
  hw:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  plughw:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  default:CARD=vc4hdmi1
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  sysdefault:CARD=vc4hdmi1
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  hdmi:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  dmix:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  usbstream:CARD=vc4hdmi1
       vc4-hdmi-1
  hw:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  plughw:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  default:CARD=USBCardBedroom
       USB Audio Device, USB Audio
  sysdefault:CARD=USBCardBedroom
       USB Audio Device, USB Audio
  front:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround21:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround40:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround41:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround50:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround51:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround71:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  iec958:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  dmix:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
Audio device [default]: usb_card_1
✓ Config written to /home/sendspin/.config/sendspin/settings-daemon.json

Service Setup
Enable on boot? [Y/n] n
Start now? [Y/n] n

Installation Complete!

Config:  /home/sendspin/.config/sendspin/settings-daemon.json
Service: systemctl {start|stop|status} sendspin
Logs:    journalctl -u sendspin -f

```

Start

```log
INFO:sendspin.audio_devices:Using audio device 1: USB Audio Device: - (hw:2,0)
INFO:sendspin.cli:Using ALSA mixer volume control: card 2, element 'Speaker'
INFO:sendspin.cli:Using preferred audio format: flac:48000:24:1
INFO:sendspin.daemon.daemon:Starting Sendspin daemon: bathroom
INFO:sendspin.audio_devices:Detected 16 supported audio formats (FLAC + PCM)
INFO:aiosendspin.client.client:Set static playback delay to -100.0 ms
INFO:mpris_api:PatchedMprisService started.
INFO:aiosendspin_mpris.mpris:MPRIS interface started
WARNING:aiosendspin_mpris.mpris_service:MPRIS not available: DBus address error
INFO:aiosendspin.client.client:Connecting to Sendspin server at ws://192.168.1.15:8927/sendspin
INFO:mpris_api:PatchedMprisService stopped.
INFO:aiosendspin.client.client:Connected to server 'Music Assistant' (e6af760739df4bc8a7c3168cfb6f3fcd) version 1
INFO:aiosendspin.client.client:Handshake with server complete
```

Show devices:

```log

sudo /home/sendspin/.local/bin/sendspin --list-audio-devices
Available audio output devices:

  [0] USB Audio Device: - (hw:0,0)
       Channels: 2, Sample rate: 44100.0 Hz
  [1] USB Audio Device: - (hw:1,0)
       Channels: 2, Sample rate: 44100.0 Hz
  [2] USB Audio Device: - (hw:2,0)
       Channels: 2, Sample rate: 44100.0 Hz
  [3] sysdefault
       Channels: 128, Sample rate: 48000.0 Hz
  [4] front
       Channels: 2, Sample rate: 44100.0 Hz
  [5] surround40
       Channels: 2, Sample rate: 44100.0 Hz
  [6] iec958
       Channels: 2, Sample rate: 44100.0 Hz
  [7] spdif
       Channels: 2, Sample rate: 44100.0 Hz
  [8] lavrate
       Channels: 128, Sample rate: 44100.0 Hz
  [9] samplerate
       Channels: 128, Sample rate: 44100.0 Hz
  [10] speexrate
       Channels: 128, Sample rate: 44100.0 Hz
  [11] a52
       Channels: 6, Sample rate: 48000.0 Hz
  [12] speex
       Channels: 1, Sample rate: 44100.0 Hz
  [13] upmix
       Channels: 8, Sample rate: 44100.0 Hz
  [14] vdownmix
       Channels: 6, Sample rate: 44100.0 Hz
  [15] usb_card_shower
       Channels: 2, Sample rate: 44100.0 Hz
  [16] usb_card_kitchen
       Channels: 2, Sample rate: 44100.0 Hz
  [17] usb_card_bedroom
       Channels: 2, Sample rate: 44100.0 Hz
  [18] default (default)
       Channels: 128, Sample rate: 48000.0 Hz
  [19] dmix
       Channels: 2, Sample rate: 48000.0 Hz

To select an audio device:
  sendspin --audio-device 0

ALSA devices (use by name with --audio-device):

  null
       Discard all samples (playback) or generate zero samples (capture)
  default
       Default Audio Device
  sysdefault
       Default Audio Device
  lavrate
       Rate Converter Plugin Using Libav/FFmpeg Library
  samplerate
       Rate Converter Plugin Using Samplerate Library
  speexrate
       Rate Converter Plugin Using Speex Resampler
  jack
       JACK Audio Connection Kit
  oss
       Open Sound System
  pulse
       PulseAudio Sound Server
  speex
       Plugin using Speex DSP (resample, agc, denoise, echo, dereverb)
  upmix
       Plugin for channel upmix (4,6,8)
  vdownmix
       Plugin for channel downmix (stereo) with a simple spacialization
  usb_card_shower
  usb_card_kitchen
  usb_card_bedroom
  hw:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  plughw:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  default:CARD=USBCardKitchen
       USB Audio Device, USB Audio
  sysdefault:CARD=USBCardKitchen
       USB Audio Device, USB Audio
  front:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround21:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround40:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround41:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround50:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround51:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  surround71:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  iec958:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  dmix:CARD=USBCardKitchen,DEV=0
       USB Audio Device, USB Audio
  usbstream:CARD=USBCardKitchen
       USB Audio Device
  hw:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  plughw:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  default:CARD=USBCardShower
       USB Audio Device, USB Audio
  sysdefault:CARD=USBCardShower
       USB Audio Device, USB Audio
  front:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround21:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround40:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround41:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround50:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround51:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  surround71:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  iec958:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  dmix:CARD=USBCardShower,DEV=0
       USB Audio Device, USB Audio
  usbstream:CARD=USBCardShower
       USB Audio Device
  hw:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  plughw:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  default:CARD=USBCardBedroom
       USB Audio Device, USB Audio
  sysdefault:CARD=USBCardBedroom
       USB Audio Device, USB Audio
  front:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround21:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround40:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround41:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround50:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround51:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  surround71:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  iec958:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  dmix:CARD=USBCardBedroom,DEV=0
       USB Audio Device, USB Audio
  usbstream:CARD=USBCardBedroom
       USB Audio Device
  hw:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  plughw:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  default:CARD=vc4hdmi0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  sysdefault:CARD=vc4hdmi0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  hdmi:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  dmix:CARD=vc4hdmi0,DEV=0
       vc4-hdmi-0, MAI PCM i2s-hifi-0
  usbstream:CARD=vc4hdmi0
       vc4-hdmi-0
  hw:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  plughw:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  default:CARD=vc4hdmi1
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  sysdefault:CARD=vc4hdmi1
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  hdmi:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  dmix:CARD=vc4hdmi1,DEV=0
       vc4-hdmi-1, MAI PCM i2s-hifi-0
  usbstream:CARD=vc4hdmi1
       vc4-hdmi-1

```


## ERRORS


Failed to play

```log
Mar 29 19:21:48 raspberrypi sendspin[93380]: Expression 'alsa_snd_pcm_prepare( stream->playback.pcm )' failed in 'src/hostapi/alsa/pa_linux_alsa.c', line: 2920
Mar 29 19:21:48 raspberrypi sendspin[93380]: Expression 'AlsaStart( stream, 0 )' failed in 'src/hostapi/alsa/pa_linux_alsa.c', line: 3246
Mar 29 19:21:48 raspberrypi sendspin[93380]: Expression 'AlsaRestart( self )' failed in 'src/hostapi/alsa/pa_linux_alsa.c', line: 3313
Mar 29 19:21:48 raspberrypi sendspin[93380]: Expression 'PaAlsaStream_HandleXrun( self )' failed in 'src/hostapi/alsa/pa_linux_alsa.c', line: 3950
Mar 29 19:21:48 raspberrypi sendspin[93380]: Expression 'PaAlsaStream_WaitForFrames( stream, &framesAvail, &xrun )' failed in 'src/hostapi/alsa/pa_linux_alsa.c', line: 4285
Mar 29 19:21:48 raspberrypi sendspin[93380]: Expression 'alsa_snd_pcm_drop( stream->playback.pcm )' failed in 'src/hostapi/alsa/pa_linux_alsa.c', line: 3042

```


Alt config for alsa:


```log
Found hardware: "USB-Audio" "USB Mixer" "USB0d8c:0014" "" ""
Hardware is initialized using a generic method
Found hardware: "USB-Audio" "USB Mixer" "USB0d8c:0014" "" ""
Hardware is initialized using a generic method
Found hardware: "USB-Audio" "USB Mixer" "USB0d8c:0014" "" ""
Hardware is initialized using a generic method


```