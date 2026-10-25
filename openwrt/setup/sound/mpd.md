# USB Audio

- <https://openwrt.org/docs/guide-user/hardware/audio/usb.audio>
- <https://forum.archive.openwrt.org/viewtopic.php?id=28099&p=1#p125196>

## Install

```shell
opkg install alsa-utils 
madplay
mpd-mini
```

## Setup

```shell
root@OpenWrt:~# lsusb
# Bus 001 Device 004: ID 08bb:2902 C-Media Electronics Inc.       USB PnP Sound Device
# Bus 001 Device 003: ID 13fe:4300          USB DISK 2.0
# Bus 001 Device 002: ID 05e3:0608  USB2.0 Hub
# Bus 001 Device 001: ID 1d6b:0002 Linux 5.10.146 ehci_hcd EHCI Host Controller

root@OpenWrt:~# aplay -L
# null
#     Discard all samples (playback) or generate zero samples (capture)
# default
#     Default Audio Device
# sysdefault
#     Default Audio Device
# default:CARD=Device
#     USB PnP Sound Device, USB Audio
#     Default Audio Device
# sysdefault:CARD=Device
#     USB PnP Sound Device, USB Audio
#     Default Audio Device
# front:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     Front output / input
# surround21:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     2.1 Surround output to Front and Subwoofer speakers
# surround40:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     4.0 Surround output to Front and Rear speakers
# surround41:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     4.1 Surround output to Front, Rear and Subwoofer speakers
# surround50:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     5.0 Surround output to Front, Center and Rear speakers
# surround51:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     5.1 Surround output to Front, Center, Rear and Subwoofer speakers
# surround71:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     7.1 Surround output to Front, Center, Side, Rear and Woofer speakers
# iec958:CARD=Device,DEV=0
#     USB PnP Sound Device, USB Audio
#     IEC958 (S/PDIF) Digital Audio Output
root@OpenWrt:~# cat /proc/asound/cards
#  0 [Device         ]: USB-Audio - USB PnP Sound Device
#                       C-Media Electronics Inc. USB PnP Sound Device at usb-1b000000.usb-1.2, full spe



alsactl init
# Found hardware: "USB-Audio" "USB Mixer" "USB08bb:2902" "" ""
# Hardware is initialized using a generic method

# Volume:
amixer scontrols
# Simple mixer control 'Speaker',0
# Simple mixer control 'Mic',0
# Simple mixer control 'Auto Gain Control',0

amixer sset Master unmute
amixer set Speaker 50

# Test stream
# https://cast.mediaonline.net.ua/prmfm320

wget -O - https://cast.mediaonline.net.ua/prmfm320 | madplay -
# Playing!

# Save config
mkdir /overlay/alsa
alsactl --file /overlay/alsa/asound.state store
alsactl --file /overlay/alsa/asound.state --no-ucm restore
```

[Reference for ALSA](../../../raspberry/setup/sound/alsa.md)

## MPD

[Reference for MPD](../../../raspberry/setup/sound/MPD/mpd.conf)

```shell
#Create a folder for MPD files at the USB storage:
mkdir /overlay/MPD

/etc/mpd.conf
```

### Config

```conf
# Config
log_file            "/overlay/MPD/mpd.log"
pid_file            "/overlay/MPD/pid"
state_file          "/overlay/MPD/state"

; bind_to_address     "any"
bind_to_address     "192.168.1.7"
port                "6600"

input {
                    plugin "curl"
#       proxy "proxy.isp.com:8080"
#       proxy_user "user"
#       proxy_password "password"
}

#############
## USB DAC ##
#############
audio_output {
    type            "alsa"
    name            "Device"
    device          "hw:CARD=Device,DEV=0"
    mixer_type      "software"
    format          "44100:16:2"
    buffer_time     "200000"
    period_time     "5084"
}
```

### Service

- <https://forum.archive.openwrt.org/viewtopic.php?id=28099&p=1#p125196>

Prepare firewall

```shell
# EXAMPLE!
iptables -t nat -A prerouting_wan -p tcp --dport 6600 -j ACCEPT
iptables        -A input_wan      -p tcp --dport 6600 -j ACCEPT

# Added by GUI: 6600
```

Test

```shell
/usr/bin/mpd
Oct 14 19:35 : server_socket: bind to '0.0.0.0:6600' failed (continuing anyway, because binding to '[::]:6600' succeeded): Failed to bind socket: Address in use

/usr/bin/mpd --systemd /etc/mpd.conf
# When conf is OK - no warnings.
# And log no messages too
tail -f -n10 /overlay/MPD/mpd.log
```

Copyied from RPI as reference.

But try using example from `iperf3` - [see](../current_setup.md#default)

```shell
cat > /etc/init.d/mpd << EOL
#!/bin/sh /etc/rc.common
START=99
STOP=10
start() {
    /usr/bin/mpd --systemd /etc/mpd.conf &
}
stop() {
    killall mpd
}
EOL
```

#### Refferences

From RPi

`/etc/default/mpd`

```conf
## Defaults for the MPD init script, sourced by /etc/init.d/mpd on Debian
## systems.  Uncomment (remove the leading '#') and change values as needed.

## If you don't want MPD to be started as a system service (for example, if
## you want to run it from a regular user account), disable it using the
## command 'update-rc.d mpd disable' (or 'systemctl disable mpd' for systemd).

## The configuration file location for mpd:
# MPDCONF=/etc/mpd.conf

```

`/lib/systemd/system/mpd.service`

```conf
[Unit]
Description=Music Player Daemon
Documentation=man:mpd(1) man:mpd.conf(5)
Documentation=file:///usr/share/doc/mpd/html/user.html
After=network.target sound.target

[Service]
Type=notify
EnvironmentFile=/etc/default/mpd
ExecStart=/usr/bin/mpd --systemd $MPDCONF

# Enable this setting to ask systemd to watch over MPD, see
# systemd.service(5).  This is disabled by default because it causes
# periodic wakeups which are unnecessary if MPD is not playing.
#WatchdogSec=120

# allow MPD to use real-time priority 40
LimitRTPRIO=40
LimitRTTIME=infinity

# for io_uring
LimitMEMLOCK=64M

# disallow writing to /usr, /bin, /sbin, ...
ProtectSystem=yes

# more paranoid security settings
NoNewPrivileges=yes
ProtectKernelTunables=yes
ProtectControlGroups=yes
ProtectKernelModules=yes
RestrictAddressFamilies=AF_INET AF_INET6 AF_UNIX AF_NETLINK
RestrictNamespaces=yes

[Install]
WantedBy=multi-user.target
Also=mpd.socket
```