# Monitor RPi from HA


- https://github.com/Sennevds/system_sensors



# Virt Env



```shell
# python3 -m venv path/to/venv
python3 -m venv /home/$USER/HA-MQTT

cd /home/$USER/HA-MQTT
git clone https://github.com/Sennevds/system_sensors.git

source /home/$USER/HA-MQTT/bin/activate
source /home/$USER/HA-MQTT/bin/deactivate

# ls
# bin  include  lib  lib64  pyvenv.cfg  system_sensors

cd system_sensors
pip3 install -r requirements.txt


# Edit settings_example.yaml in "~/system_sensors/src" to reflect your setup and save as settings.yaml:
# tls:
    # ca_certs: ''
    # certfile: ''
    # keyfile: ''

mv src/settings_example.yaml src/settings.yaml
```

```shell
sudo vi /lib/systemd/system/ha-rpimon.service

sudo systemctl daemon-reload
sudo systemctl enable --now ha-rpimon.service
sudo systemctl status ha-rpimon.service
```

```conf
[Unit]
Description=Python based System Sensor Service for MQTT
After=multi-user.target

[Service]
User=_USER_
Type=idle
ExecStart=/home/_USER_/HA-MQTT/bin/python3 /home/_USER_/HA-MQTT/system_sensors/src/system_sensors.py /home/_USER_/HA-MQTT/system_sensors/src/settings.yaml

[Install]
WantedBy=multi-user.target
```