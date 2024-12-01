## Piper

- https://github.com/rhasspy/piper

```shell
# https://askubuntu.com/a/1306980
amixer set Master 80% >> /dev/null

# Intonation experiments
# Clean
echo "Робота від батареї" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_no_power.wav
echo "Рівень заряду десять відсотків. Система буде вимкнена за п'ять хвилин." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_10_percent_will_poweroff.wav
echo "Увага, система вимикається через низький рівень заряду акумулятора." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_poweroff.wav
echo "Електропостачання відновлено." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_power_back.wav
echo "Увага, батарея потребує заміни." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_battery_change.wav

echo "Увага, робота від батареї, рівень заряду п'ятдесят відсотків" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_50_percent.wav
echo "Рівень заряду тридцять відсотків" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_30_percent.wav
echo "Рівень заряду двадцять відсотків" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_20_percent.wav
# Air alert
echo "Увага! Повітряна Тривога У Київській Області" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file air_alert_raise.wav && /
echo "Відбій! Повітряної Тривоги У Київській Області" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file air_alert_release.wav

# Grid Power off
echo "Міське Електропостачання Відсутнє" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file city_grid_power_off.wav && /
echo "Міське Електропостачання Відновлено" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file city_grid_power_on.wav

# Forced pauses
echo "У вага. Робота від батареї... Рівень за.ряду. П'ятдесят відсотків." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output_file UPS_50_percent_2.wav

# Warning
sleep 4 && amixer set Master 60% >> /dev/null && echo "ей, увага, повітряна тривога у київській області." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 0 --output-raw | aplay -r 22050 -f S16_LE -t raw - && amixer set Master 10% >> /dev/null
sleep 4 && amixer set Master 60% >> /dev/null && echo "ей, увага, повітряна тривога у київській області." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output-raw | aplay -r 22050 -f S16_LE -t raw - && amixer set Master 10% >> /dev/null
sleep 4 && amixer set Master 60% >> /dev/null && echo "ей, увага, повітряна тривога у київській області." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 2 --output-raw | aplay -r 22050 -f S16_LE -t raw - && amixer set Master 10% >> /dev/null

# UPS Low Batt
amixer set Master 40% >> /dev/null && echo "Увага. Робота від батареї. Електропостачання відсутнє." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output-raw | aplay -r 22050 -f S16_LE -t raw - && amixer set Master 10% >> /dev/null
amixer set Master 40% >> /dev/null && echo "Увага. Робота від батареї. Електропостачання відсутнє. Заряд на рівні п'ятидесяти відсотків." | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/ukr/uk_UA-ukrainian_tts-medium.onnx --speaker 1 --output-raw | aplay -r 22050 -f S16_LE -t raw - && amixer set Master 10% >> /dev/null

# Eng
echo "Hey, Warning there is an air alert in Kyiv oblast" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/eng/en_GB-northern_english_male-medium.onnx --output-raw | aplay -r 22050 -f S16_LE -t raw -

# Pause
sleep 4 && amixer set Master 50% >> /dev/null && echo "Hey, Hey, Warning there is an air alert in Kyiv oblast" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/eng/en_GB-northern_english_male-medium.onnx --output-raw | aplay -r 22050 -f S16_LE -t raw - && amixer set Master 10% >> /dev/null

sleep 4 && amixer set Master 50% >> /dev/null && echo "Hey, Hey, Warning there is an air alert in Kyiv oblast" | /home/USER/text_to_speech/piper/piper --model /home/USER/text_to_speech/eng/alan_medium/en_GB-alan-medium.onnx --output-raw | aplay -r 22050 -f S16_LE -t raw - && amixer set Master 10% >> /dev/null

amixer set Master 10% >> /dev/null
```


## Issues:

- https://forums.raspberrypi.com/viewtopic.php?t=194411

Example:

```shell
aplay --device="dmix:CARD=Audio,DEV=0" /home/USER/Music/notifications/generated/UPS_no_power.wav
Playing WAVE '/home/USER/Music/notifications/generated/UPS_no_power.wav' : Signed 16 bit Little Endian, Rate 22050 Hz, Mono
aplay: set_params:1358: Channels count non available
```

## Try different devices for MONO prerecorded WAV from piper:

```shell
# List devices
aplay -L
# I only have USB card:
hw:CARD=Audio,DEV=0
     USB Audio, USB Audio
    Direct hardware device without any conversions
plughw:CARD=Audio,DEV=0
     USB Audio, USB Audio
    Hardware device with all software conversions
sysdefault:CARD=Audio
     USB Audio, USB Audio
    Default Audio Device
front:CARD=Audio,DEV=0
     USB Audio, USB Audio
    Front output / input
iec958:CARD=Audio,DEV=0
     USB Audio, USB Audio
    IEC958 (S/PDIF) Digital Audio Output
dmix:CARD=Audio,DEV=0
     USB Audio, USB Audio
    Direct sample mixing device
usbstream:CARD=Audio
     USB Audio
    USB Stream Output
# 
# Playing:
sleep 5 && aplay --device="plughw:CARD=Audio,DEV=0" /home/USER/Music/notifications/generated/UPS_no_power.wav 
sleep 5 && aplay --device="sysdefault:CARD=Audio" /home/USER/Music/notifications/generated/UPS_no_power.wav
# Not playing
aplay --device="hw:CARD=Audio,DEV=0" /home/USER/Music/notifications/generated/UPS_no_power.wav
aplay --device="front:CARD=Audio,DEV=0" /home/USER/Music/notifications/generated/UPS_no_power.wav
aplay --device="iec958:CARD=Audio,DEV=0" /home/USER/Music/notifications/generated/UPS_no_power.wav
aplay --device="dmix:CARD=Audio,DEV=0" /home/USER/Music/notifications/generated/UPS_no_power.wav
aplay --device="usbstream:CARD=Audio" /home/USER/Music/notifications/generated/UPS_no_power.wav
# Test ddownloaded WAV and prerecorded
sleep 10 && aplay --device="plughw:CARD=Audio,DEV=0" /home/USER/Music/notifications/generated/UPS_no_power.wav && aplay --device="plughw:CARD=Audio,DEV=0" /home/USER/Music/notifications/stop.wav
```