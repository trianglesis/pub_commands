# Report RPi hardware states to Home Assistant


## Probably the best and simplest:

1. Cron and script via REST
   1. https://community.home-assistant.io/t/monitoring-a-server-via-mqtt/564054/4

Helps:
- https://www.cyberciti.biz/faq/linux-find-out-raspberry-pi-gpu-and-arm-cpu-temperature-command/
- https://bc-robotics.com/tutorials/setting-cron-job-raspberry-pi/

Better use webhook, since later you can specify each sensor!


```bash
sudo vi simple_rest_ha_hardware_report.sh
```

Commands may be useful:
```sh
# NVMe lifespan left in %
# EXAMPLE: "available_spare                         : 100%
#           available_spare_threshold               : 10%"
sudo nvme smart-log /dev/nvme0n1 | grep -i "available_spare\s" | awk -F'[^0-9]*' '{print $2}'
# Data unit reads\written: bytes 
# EXAMPLE: "Data Units Written                      : 117,197,668 (60.01 TB)"
sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Read" | awk '{print $5}'
sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Written" | awk '{print $5}'
# Optional, cut only TB:
sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Read" | awk -F'[^0-9]*' '{print $5}'
sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Written" | awk -F'[^0-9]*' '{print $5}'
# Disk space /
# EXAMPLE:"df -h" "Filesystem      Size  Used Avail Use% Mounted on
#                  /dev/nvme0n1p2  491G  7.2G  459G   2% /"
# Size G
df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $5}'
# Used G 
df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $6}'
# Avail G
df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $7}'
# Used %
df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $8}'
# NVMe temps
# EXAMPLE: "Temperature Sensor 1           : 40°C (313 Kelvin)
#           Temperature Sensor 2           : 41°C (314 Kelvin)"
sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 1" | awk -F'[^0-9]*' '{print $3}'
sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 2" | awk -F'[^0-9]*' '{print $3}'
# GPU? Temp:
# EXAMPLE: "temp=51.0'C"
vcgencmd measure_temp | awk -F'[^0-9]*' '{print $2}'
# CPU Temp:
cpu=$(</sys/class/thermal/thermal_zone0/temp) && echo "$((cpu/1000))"
# OR SOC temp:
# EXAMPLE: "50700" = 50.7 C
cat /sys/devices/virtual/thermal/thermal_zone0/temp
# RAM: free -h
# Example:                total        used        free      shared  buff/cache   available
#          Mem:           7.9Gi       557Mi       5.8Gi        38Mi       1.6Gi       7.3Gi
#          Swap:           99Mi          0B        99Mi
# Total G
free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $2}'
# Used G
free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $3}'
# Free G
free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $4}'
# CPU Freq:
cpu_freq=$(sudo cat /sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_cur_freq) && echo "$((cpu_freq/1000))"
# CPU Loads
cat /proc/loadavg | cut -d\  -f1
cat /proc/loadavg | cut -d\  -f2
cat /proc/loadavg | cut -d\  -f3
```

Compose variables:
```sh
export NVME_SPARE=`sudo nvme smart-log /dev/nvme0n1 | grep -i "available_spare\s" | awk -F'[^0-9]*' '{print $2}'`
export NVME_UNITS_READ=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Read" | awk -F'[^0-9]*' '{print $5}'`
export NVME_UNITS_WRITTEN=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Written" | awk -F'[^0-9]*' '{print $5}'`
export DISK_SIZE=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $5}'`
export DISK_SPACE_USED=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $6}'`
export DISK_SPACE_AVAILABLE=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $7}'`
export DISK_SPACE_USED_PERCENT=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $8}'`
export NVME_TEMP_1=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 1" | awk -F'[^0-9]*' '{print $3}'`
export NVME_TEMP_2=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 2" | awk -F'[^0-9]*' '{print $3}'`
export GPU_TEMP=`vcgencmd measure_temp | awk -F'[^0-9]*' '{print $2}'`
export CPU_TEMP=`cpu=$(</sys/class/thermal/thermal_zone0/temp) && echo "$((cpu/1000))"`
export RAM_TOTAL=`free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $2}'`
export RAM_USED=`free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $3}'`
export RAM_FREE=`free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $4}'`
export CPU_FREQ=`cpu_freq=$(sudo cat /sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_cur_freq) && echo "$((cpu_freq/1000))"`
export CPU_LOAD_1=`cat /proc/loadavg | cut -d\  -f1`
export CPU_LOAD_5=`cat /proc/loadavg | cut -d\  -f2`
export CPU_LOAD_15=`cat /proc/loadavg | cut -d\  -f3`
```

```sh
#!/bin/bash

export HALLT="TOKEN"
export HA_IP="IP"
export HA_PORT="PORT"

export NVME_SPARE=`sudo nvme smart-log /dev/nvme0n1 | grep -i "available_spare\s" | awk -F'[^0-9]*' '{print $2}'`
export NVME_UNITS_READ=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Read" | awk -F'[^0-9]*' '{print $5}'`
export NVME_UNITS_WRITTEN=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Written" | awk -F'[^0-9]*' '{print $5}'`
export DISK_SIZE=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $5}'`
export DISK_SPACE_USED=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $6}'`
export DISK_SPACE_AVAILABLE=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $7}'`
export DISK_SPACE_USED_PERCENT=`df -h / | grep "/dev/nvme0n1p2" | awk -F'[^0-9.]*' '{print $8}'`
export NVME_TEMP_1=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 1" | awk -F'[^0-9]*' '{print $3}'`
export NVME_TEMP_2=`sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 2" | awk -F'[^0-9]*' '{print $3}'`
export GPU_TEMP=`vcgencmd measure_temp | awk -F'[^0-9]*' '{print $2}'`
export CPU_TEMP=`cpu=$(</sys/class/thermal/thermal_zone0/temp) && echo "$((cpu/1000))"`
export RAM_TOTAL=`free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $2}'`
export RAM_USED=`free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $3}'`
export RAM_FREE=`free -h | grep "Mem:" | awk -F'[^0-9,.]*' '{print $4}'`
export CPU_FREQ=`cpu_freq=$(sudo cat /sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_cur_freq) && echo "$((cpu_freq/1000))"`
export CPU_LOAD_1=`cat /proc/loadavg | cut -d\  -f1`
export CPU_LOAD_5=`cat /proc/loadavg | cut -d\  -f2`
export CPU_LOAD_15=`cat /proc/loadavg | cut -d\  -f3`
export TS="`date -Iseconds`"

curl -k -X POST -H "Authorization: Bearer $HALLT" -H "Content-Type: application/json" \
     -d "{\"NVME_SPARE\": $NVME_SPARE, \
         \"NVME_UNITS_READ\": $NVME_UNITS_READ, \
         \"NVME_UNITS_WRITTEN\": $NVME_UNITS_WRITTEN, \
         \"DISK_SIZE\": $DISK_SIZE, \
         \"DISK_SPACE_USED\": $DISK_SPACE_USED, \
         \"DISK_SPACE_AVAILABLE\": $DISK_SPACE_AVAILABLE, \
         \"DISK_SPACE_USED_PERCENT\": $DISK_SPACE_USED_PERCENT, \
         \"NVME_TEMP_1\": $NVME_TEMP_1, \
         \"NVME_TEMP_2\": $NVME_TEMP_2, \
         \"GPU_TEMP\": $GPU_TEMP, \
         \"CPU_TEMP\": $CPU_TEMP, \
         \"RAM_TOTAL\": $RAM_TOTAL, \
         \"RAM_USED\": $RAM_USED, \
         \"RAM_FREE\": $RAM_FREE, \
         \"CPU_FREQ\": $CPU_FREQ, \
         \"CPU_LOAD_1\": $CPU_LOAD_1, \
         \"CPU_LOAD_5\": $CPU_LOAD_5, \
         \"CPU_LOAD_15\": $CPU_LOAD_15, \
         \"updated\": \"$TS\"}" \
     http://$HA_IP:$HA_PORT/api/webhook/rpi-webhook-hardware-mon
```

### Test:

```sh
# Add this into the script and run to see if JSON is ok
echo "{\"NVME_SPARE\": $NVME_SPARE, \
\"NVME_UNITS_READ\": $NVME_UNITS_READ, \
\"NVME_UNITS_WRITTEN\": $NVME_UNITS_WRITTEN, \
\"DISK_SIZE\": $DISK_SIZE, \
\"DISK_SPACE_USED\": $DISK_SPACE_USED, \
\"DISK_SPACE_AVAILABLE\": $DISK_SPACE_AVAILABLE, \
\"DISK_SPACE_USED_PERCENT\": $DISK_SPACE_USED_PERCENT, \
\"NVME_TEMP_1\": $NVME_TEMP_1, \
\"NVME_TEMP_2\": $NVME_TEMP_2, \
\"GPU_TEMP\": $GPU_TEMP, \
\"CPU_TEMP\": $CPU_TEMP, \
\"RAM_TOTAL\": $RAM_TOTAL, \
\"RAM_USED\": $RAM_USED, \
\"RAM_FREE\": $RAM_FREE, \
\"CPU_FREQ\": $CPU_FREQ, \
\"CPU_LOAD_1\": $CPU_LOAD_1, \
\"CPU_LOAD_5\": $CPU_LOAD_5, \
\"CPU_LOAD_15\": $CPU_LOAD_15, \
\"updated\": \"$TS\"}"

# Test URL
echo "http://$HA_IP:$HA_PORT/api/webhook/rpi-webhook-hardware-mon"
```

## Result in dev tools:


![alt text](image-2.png)


## Debug message:

```text
./simple_rest_ha_hardware_report.sh
{"state": "RPi-Cron", "attributes": {"NVME_SPARE": "100", "NVME_UNITS_READ": "56", "NVME_UNITS_WRITTEN": "60", "DISK_SIZE": "457", "DISK_SPACE_USED": "6.7", "DISK_SPACE_AVAILABLE": "427", "DISK_SPACE_USED_PERCENT": "2", "NVME_TEMP_1": "40", "NVME_TEMP_2": "45", "GPU_TEMP": "51", "CPU_TEMP": "50", "RAM_TOTAL": "7.9", "RAM_USED": "559", "RAM_FREE": "5.8", "CPU_FREQ": "1700", "CPU_LOAD_1": "0.08", "CPU_LOAD_5": "0.04", "CPU_LOAD_15": "0.00", "updated": "2024-03-15T13:52:10+02:00" }}
{"entity_id":"sensor.rpi_monitor_script","state":"RPi-Cron","attributes":{"NVME_SPARE":"100","NVME_UNITS_READ":"56","NVME_UNITS_WRITTEN":"60","DISK_SIZE":"457","DISK_SPACE_USED":"6.7","DISK_SPACE_AVAILABLE":"427","DISK_SPACE_USED_PERCENT":"2","NVME_TEMP_1":"40","NVME_TEMP_2":"45","GPU_TEMP":"51","CPU_TEMP":"50","RAM_TOTAL":"7.9","RAM_USED":"559","RAM_FREE":"5.8","CPU_FREQ":"1700","CPU_LOAD_1":"0.08","CPU_LOAD_5":"0.04","CPU_LOAD_15":"0.00","updated":"2024-03-15T13:52:10+02:00"},"last_changed":"2024-03-15T11:52:10.204196+00:00","last_updated":"2024-03-15T11:52:10.204196+00:00","context":{"id":"AABBCC","parent_id":null,"user_id":"AABBCC"}}
```

## Add to CRON

```sh
crontab -e
```

```shell
# Every 1 min report hardware status variables to Home Assistant via REST
1 * * * * /bin/sh -c '/home/sanek/Documents/Scripts/simple_rest_ha_hardware_report.sh'
```


# Add to HA

- https://www.home-assistant.io/integrations/template/#trigger-based-sensor-and-binary-sensor-storing-webhook-information

Works in `config/configuration.yaml`

```yaml
template:
  - trigger:
      - platform: webhook
        webhook_id: rpi-webhook-hardware-mon
    sensor:
      - name: "Webhook NVME_SPARE"
        state: "{{ trigger.json.NVME_SPARE }}"
        unit_of_measurement: "%"
      - name: "Webhook NVME_UNITS_READ"
        state: "{{ trigger.json.NVME_UNITS_READ }}"
        unit_of_measurement: Tb
      - name: "Webhook NVME_UNITS_WRITTEN"
        state: "{{ trigger.json.NVME_UNITS_WRITTEN }}"
        unit_of_measurement: Tb
```

Add to `template: !include templates.yaml`
Save the same indentation!

```yaml
  - trigger:
      - platform: webhook
        webhook_id: rpi-webhook-hardware-mon
    sensor:
      - name: "Webhook RPi NVME_SPARE"
        state: "{{ trigger.json.NVME_SPARE }}"
        unit_of_measurement: "%"
      - name: "Webhook RPi NVME_UNITS_READ"
        state: "{{ trigger.json.NVME_UNITS_READ }}"
        unit_of_measurement: Tb
      - name: "Webhook RPi NVME_UNITS_WRITTEN"
        state: "{{ trigger.json.NVME_UNITS_WRITTEN }}"
        unit_of_measurement: Tb
      - name: "Webhook RPi DISK_SIZE"
        state: "{{ trigger.json.DISK_SIZE }}"
        unit_of_measurement: Gb
      - name: "Webhook RPi DISK_SPACE_USED"
        state: "{{ trigger.json.DISK_SPACE_USED }}"
        unit_of_measurement: Gb
      - name: "Webhook RPi DISK_SPACE_AVAILABLE"
        state: "{{ trigger.json.DISK_SPACE_AVAILABLE }}"
        unit_of_measurement: Gb
      - name: "Webhook RPi DISK_SPACE_USED_PERCENT"
        state: "{{ trigger.json.DISK_SPACE_USED_PERCENT }}"
        unit_of_measurement: "%"
      - name: "Webhook RPi NVME_TEMP_1"
        state: "{{ trigger.json.NVME_TEMP_1 }}"
        unit_of_measurement: "°C"
      - name: "Webhook RPi NVME_TEMP_2"
        state: "{{ trigger.json.NVME_TEMP_2 }}"
        unit_of_measurement: "°C"
      - name: "Webhook RPi GPU_TEMP"
        state: "{{ trigger.json.GPU_TEMP }}"
        unit_of_measurement: "°C"
      - name: "Webhook RPi CPU_TEMP"
        state: "{{ trigger.json.CPU_TEMP }}"
        unit_of_measurement: "°C"
      - name: "Webhook RPi RAM_TOTAL"
        state: "{{ trigger.json.RAM_TOTAL }}"
        unit_of_measurement: Gb
      - name: "Webhook RPi RAM_USED"
        state: "{{ trigger.json.RAM_USED }}"
        unit_of_measurement: Mb
      - name: "Webhook RPi RAM_FREE"
        state: "{{ trigger.json.RAM_FREE }}"
        unit_of_measurement: Gb
      - name: "Webhook RPi CPU_FREQ"
        state: "{{ trigger.json.CPU_FREQ }}"
        unit_of_measurement: Hz
      - name: "Webhook RPi CPU_LOAD_1"
        state: "{{ trigger.json.CPU_LOAD_1 }}"
        unit_of_measurement: "%"
      - name: "Webhook RPi CPU_LOAD_5"
        state: "{{ trigger.json.CPU_LOAD_5 }}"
        unit_of_measurement: "%"
      - name: "Webhook RPi CPU_LOAD_15"
        state: "{{ trigger.json.CPU_LOAD_15 }}"
        unit_of_measurement: "%"
      - name: "Webhook RPi Updated"
        state: "{{ trigger.json.updated }}"
        unit_of_measurement: time
```