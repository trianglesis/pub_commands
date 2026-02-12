
# Set default sound settings

https://projects-raspberry.com/getting-audio-out-working-on-the-raspberry-pi/
https://gist.github.com/rnagarajanmca/63badce0fe0e2ad126041c7c139970ea

Install?

sudo apt-get install alsa-utils
sudo apt-get install mpg321
sudo apt-get install lame

sudo apt-get remove pulseaudio
sudo apt-get install pulseaudio



```shell
# Enable audio (loads snd_bcm2835)
sudo vi /boot/firmware/config.txt
dtparam=audio=on 

# Enable sound module?
sudo modprobe snd-bcm2835

cat /proc/asound/cards
#  0 [USBCard2       ]: USB-Audio - USB Audio Device
#                       C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.1, full speed
#  1 [vc4hdmi0       ]: vc4-hdmi - vc4-hdmi-0
#                       vc4-hdmi-0
#  2 [USBCard1       ]: USB-Audio - USB Audio Device
#                       C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.2, full speed
#  3 [vc4hdmi1       ]: vc4-hdmi - vc4-hdmi-1
#                       vc4-hdmi-1
#  4 [USBCard3       ]: USB-Audio - USB Audio Device
#                       C-Media Electronics Inc. USB Audio Device at usb-xhci-hcd.0-1.3, full speed
aplay -l

# Set non HDMI
amixer cset numid=3 1

# amixer cset numid=3 1
# numid=3,iface=MIXER,name='Mic Playback Switch'
#   ; type=BOOLEAN,access=rw------,values=1
#   : values=on

```

`sudo vi /etc/mpd.conf`
`sudo rm -rf /etc/mpd.conf`


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


`vi ~/.asoundrc`
AND
`sudo vi /etc/asound.conf`
`sudo rm /etc/asound.conf`

```conf
pcm.!default {
       type hw
       card 0
}

ctl.!default {
       type hw
       card 0
}
```


# Problems

sudo raspi-config - `No internal audio devices found`
sudo raspi-config - `There was an error running option S2 Audio`

Re-install pulseaudio