#!/bin/bash
# Play notification sound at givel volume with given file
# Use aplay -L to show devices

PATH_TO_NOTIFICATIONS="/home/USER/Music/notifications"
PATH_TO_DING="/home/USER/Music/notifications/stop.wav"

verbose=$3
sound_volume=$1
file_to_play=$2

# Tune volume at evenings: 
# https://stackoverflow.com/a/2899142/4915733
# https://stackoverflow.com/a/74904869/4915733

current_hour=`date +%H | bc`
tune_volume(){
  if $verbose ; then echo "DEBUG: Current volume is: $sound_volume and hour: $current_hour"; fi
  # 10:00 18:00
  if (( $current_hour >= 10 )) && (( $current_hour < 18 )); then
    if $verbose ; then echo "DEBUG: Not changing volume at working hours: $sound_volume hour: $current_hour"; fi
  # 18:00 21:00
  elif (( $current_hour >= 18 )) && (( $current_hour < 21 )); then
    sound_volume=35
    if $verbose ; then echo "DEBUG: Adjusting lower volume at evenings: $sound_volume hour: $current_hour"; fi
  # 21:00 23:00
  elif (( $current_hour >= 21 )) && (( $current_hour <= 23 )); then
    sound_volume=15
    if $verbose ; then echo "DEBUG: Adjusting lower volume at evenings: $sound_volume hour: $current_hour"; fi
  # 23:00 10:00
  elif (( $current_hour < 10 )); then
    sound_volume=10
    if $verbose ; then echo "DEBUG: Adjusting lower volume at night: $sound_volume hour: $current_hour"; fi
  else
    if $verbose ; then echo "DEBUG: Else Not changing volume: $sound_volume hour: $current_hour"; fi
  fi
}

# List devices: aplay -L
DEVICE="plughw:CARD=Audio,DEV=0"
DEVICE_ALT="dmix:CARD=Audio,DEV=0"

play_sound_notification(){
  # No verbose by default, and verbose if any arg!
  if [ -z "$verbose" ]; then
    verbose=false
  else
    verbose=true
    echo "DEBUG: mode at any arg: $verbose"
  fi

  # Volume check if integer:
  if [[ "$sound_volume" =~ ^[0-9]+$ || "$sound_volume" =~ ^[-][0-9]+$  ]]; then
    # Volume check if GTE 101 or LT 1:
    if [[ $sound_volume -ge 101 ]] || [[ $sound_volume -lt 0 ]]; then
      echo "ERROR: Volume is incorrect not 1-100: $sound_volume set default = 90, and later set down dased on daytime."
      $sound_volume = 90
    fi
    # All correct:
    if $verbose ; then echo "DEBUG: volume is OK: $sound_volume"; fi
  else
    echo "ERROR: Volume is incorrect, not an integer: $sound_volume set default = 90, and later set down dased on daytime."
    $sound_volume = 90
  fi
  
  # Tune volume based on daytime
  tune_volume

  # Check if file exsists:
  file_full_path="$PATH_TO_NOTIFICATIONS/$file_to_play"
  if [ -f $file_full_path ]; then
      if $verbose ; then echo "DEBUG: file exists: $file_full_path"; fi
  else
    echo "File: $file_to_play"
    echo "ERROR: The file does not exist at path with this name: $file_full_path"
    return 0
  fi

  # Set volume:
  if $verbose; then
    echo "============================"
    echo "Volume controls are:"
    /usr/bin/amixer
    /usr/bin/amixer scontrols
    echo "============================"
    echo "Set volume at level: $sound_volume"
    echo "Run CMD: /usr/bin/amixer set Master $sound_volume%"
    echo "============================"
    /usr/bin/amixer set Master "$sound_volume%"
    echo "============================"
    echo "Play initial ding to wake up USB Sound device: $PATH_TO_DING"
    /usr/bin/aplay --device=$DEVICE -q --nonblock --duration=1 $PATH_TO_DING
    echo "Now play requested notification: $file_full_path"
    /usr/bin/aplay --device=$DEVICE -q $file_full_path
    echo "============================"
    echo "Set volume back at level 10%"
    echo "============================"
    /usr/bin/amixer set Master "10%"
    echo "============================"
  else
    /usr/bin/amixer set Master "$sound_volume%" >> /dev/null
    /usr/bin/aplay --device=$DEVICE -q --nonblock --duration=1 $PATH_TO_DING
    /usr/bin/aplay --device=$DEVICE -q $file_full_path
    /usr/bin/amixer set Master "10%" >> /dev/null
  fi
  # Play notification:

  # End and exit
  return 0
}

play_sound_notification $sound_volume $file_to_play $verbose


