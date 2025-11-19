# Config snapcast


Check prereq: 
- [snapuser](https://github.com/badaix/snapcast/issues/668#issuecomment-692732687)

## Conf file

`sudo vi /etc/snapserver.conf`

https://github.com/badaix/snapcast/blob/develop/doc/player_setup.md#streams

Choose test file for setup, delete later.

```conf
source = pipe:///tmp/snapfifo?name=default
# This is for test, disable once set everything up
# source = pipe:///tmp/snapfifo?name=Radio&sampleformat=48000:16:2&codec=flac
# source = file:///home/share/Media/friend-request-14878.wav?name=FileTest
```

### Tweak the source

Read [doc](https://github.com/badaix/snapcast/blob/master/doc/configuration.md)

Default: `source = pipe:///tmp/snapfifo?name=default`

This is a short version of a config I use.
I've discovered that buffer between 250-350ms is fine to play short TTS messages and notification sounds 1-3sec long.
I also deliberately lowered the codec and sample format, since my audio setup is pretty simple and not `audiophilistic` so it's better to keep resources low.

```conf
[server]
threads = 4
pidfile = /var/run/snapserver/pid
user = snapserver
group = snapserver

[http]
bind_to_address = 0.0.0.0
port = 1780
ssl_enabled = false
doc_root = /usr/share/snapserver/snapweb

[tcp]
enabled = true
bind_to_address = 0.0.0.0
port = 1705

[stream]
bind_to_address = 0.0.0.0
port = 1704
source = pipe:///tmp/snapfifo?name=default&mode=create&dryout_ms=200&sampleformat=44100:16:2&send_silence=false&idle_threshold=200&silence_threshold_percent=1.0
sampleformat = 44100:16:2
codec = ogg
chunk_ms = 20
buffer = 350
```



## Home assistant setup

Disable all other integrations and players, I only use HA Music as player.
I've deleted every player but VLC from rasp.

`sudo vi /etc/mpd.conf`


```conf
 # https://github.com/badaix/snapcast/blob/develop/doc/configuration.md#alsa

 audio_output_format            "48000:16:2"
 audio_output {
         type            "alsa"
         name            "My ALSA Device"
         device          "hw:0,0,0"      # optional
 #       auto_resample   "no"
 #       mixer_type      "hardware"      # optional
 #       mixer_device    "default"       # optional
 #       mixer_control   "PCM"           # optional
 #       mixer_index     "0"             # optional
 }

```


`sudo vi /etc/asound.conf`

```conf
#pcm.!default {
#       type hw
#       card 0
#}
#
#ctl.!default {
#       type hw
#       card 0
#}

```



# Not simple setup

 - https://github.com/badaix/snapcast/blob/develop/doc/build.md#raspberry-pi-cross-compile


```shell
# /home/USER/Snapcast_sources/snapcast

cd <snapcast dir>
mkdir build
cd build

# /usr/include/boost
cmake .. -DBOOST_ROOT=/usr/include/boost
cmake --build .
```

Binaries will be created in <snapcast dir>/bin:

<snapcast dir>/bin/snapclient
<snapcast dir>/bin/snapserver

`/home/$USER/Snapcast_sources/snapcast/bin/snapserver`
`/home/$USER/Snapcast_sources/snapcast/bin/snapclient`

Update conf files to use compiled binaries

```shell
# check
/home/$USER/Snapcast_sources/snapcast/bin/snapserver -V
/home/$USER/Snapcast_sources/snapcast/bin/snapclient -V

# delete installed snapserver, snapclient
# copy to /usr/bin
sudo cp Snapcast_sources/snapcast/bin/snapserver /usr/bin/
sudo cp Snapcast_sources/snapcast/bin/snapclient /usr/bin/

# Test
/usr/bin/snapserver --version
/usr/bin/snapclient --version

sudo cp /lib/systemd/system/snapserver.service /lib/systemd/system/snapserver.service_backup
sudo cp /lib/systemd/system/snapclient.service /lib/systemd/system/snapclient.service_backup
sudo cp /lib/systemd/system/snapclient_2.service /lib/systemd/system/snapclient_2.service_backup
sudo cp /lib/systemd/system/snapclient_3.service /lib/systemd/system/snapclient_3.service_backup

sudo vi /lib/systemd/system/snapserver.service
sudo vi /lib/systemd/system/snapclient.service
sudo vi /lib/systemd/system/snapclient_2.service
sudo vi /lib/systemd/system/snapclient_3.service

# Check args, new version arg --soundcard= is lowercase
sudo vi /etc/default/snapclient
sudo vi /etc/default/snapclient_2
sudo vi /etc/default/snapclient_3

# Conf changed - reload
sudo systemctl daemon-reload
sudo systemctl restart snapserver snapclient snapclient_2 snapclient_3

# Check
sudo systemctl restart snapserver
sudo systemctl status snapserver
sudo systemctl status snapclient
sudo systemctl status snapclient_2
sudo systemctl status snapclient_3

# Debug
journalctl -xeu snapserver.service
journalctl -xeu snapclient.service
journalctl -xeu snapclient_2.service
journalctl -xeu snapclient_3.service

# Stop
sudo systemctl stop snapserver snapclient snapclient_2 snapclient_3

# old configs moving
sudo rm /lib/systemd/system/snapserver.service
sudo rm /lib/systemd/system/snapclient.service
sudo rm /lib/systemd/system/snapclient_2.service
sudo rm /lib/systemd/system/snapclient_3.service

sudo cp /lib/systemd/system/snapserver.service_backup /lib/systemd/system/snapserver.service
sudo cp /lib/systemd/system/snapclient.service_backup /lib/systemd/system/snapclient.service
sudo cp /lib/systemd/system/snapclient_2.service_backup /lib/systemd/system/snapclient_2.service
sudo cp /lib/systemd/system/snapclient_3.service_backup /lib/systemd/system/snapclient_3.service
```


## Etc

Web:

Revive old path:
`sudo chown snapclient:snapclient -R /usr/share/snapserver/snapweb/`
Download compiled web zip: https://github.com/badaix/snapweb/releases
Copy: `sudo cp -r /home/$USER/Snapcast_sources/snapweb/* /usr/share/snapserver/snapweb/`
Change own: `sudo chown snapclient:snapclient -R /usr/share/snapserver/snapweb/`