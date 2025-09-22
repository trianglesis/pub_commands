# Config snapcast



## Conf file

`sudo vi /etc/snapserver.conf`

https://github.com/badaix/snapcast/blob/develop/doc/player_setup.md#streams


Choose test file for setup, delete later.

```
source = pipe:///tmp/snapfifo?name=default
# This is for test, disable once set everything up
# source = pipe:///tmp/snapfifo?name=Radio&sampleformat=48000:16:2&codec=flac
# source = file:///home/share/Media/friend-request-14878.wav?name=FileTest

```

## Home assistant setup

Disable all other integrations and players:

`sudo vi /etc/mpd.conf`


```
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

```
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