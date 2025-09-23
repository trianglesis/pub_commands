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