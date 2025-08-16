# Setup SnapServer and two clients

## Problems:

Reboot will mix USB sound cards all over


Get list of devices:

cat /proc/asound/cards
snapclient -l

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


Test again
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