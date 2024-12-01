# Using systemd to daemonize UPS Mon logger

- [Logging periodical](https://networkupstools.org/docs/man/upslog.html)

Updated according to this installation: [From sources](NUT-FromSources.md)

# CMD:

```sh
upslog -s Powercom800W@localhost -f "%TIME @Y-@m-@d @H:@M:@S% -- Status: [%VAR ups.status%] | Charge: %VAR battery.charge% | Runtime: %VAR battery.runtime% sec | Load: %VAR ups.load% | t: %VAR battery.temperature% | Input V: %VAR input.voltage% Hz: %VAR input.frequency%" -l /var/log/ups/UPS_Powercom800W.log
```

# Conf

`sudo vi /etc/default/upslog.conf`

```sh
# ========================================
# sudo vi /etc/default/upslog.conf
# sudo vi /etc/systemd/system/upslog.service
# sudo systemctl daemon-reload
# sudo systemctl restart upslog.service
# ========================================

UPS_NAME="Powercom800W@localhost"

# 5 Minutes is ok. 300
LOG_TIME="300"

LOG_PATH="/var/log/ups/UPS_Powercom800W.log"
```

# Service

NOTE: `%%` double sign allows to escape this sign to be used as expected by upslog

`sudo vi /etc/systemd/system/upslog.service`

```sh
# ========================================
# sudo vi /etc/default/upslog.conf
# sudo vi /etc/systemd/system/upslog.service
# sudo systemctl daemon-reload
# sudo systemctl restart upslog.service
# ========================================

[Unit]
Description=NUT UpsMon logger Service
After=network.target

[Service]
Type=simple

User=nut
Group=nut

EnvironmentFile=-/etc/default/upslog.conf
WorkingDirectory=/var/run/nut/

ExecStart=/bin/sh -c '/usr/local/ups/bin/upslog -u nut -s ${UPS_NAME} -i ${LOG_TIME} -l ${LOG_PATH} -f "%%TIME @Y-@m-@d @H:@M:@S%% -- Status: [%%VAR ups.status%%] | Charge: %%VAR battery.charge%% | Runtime: %%VAR battery.runtime%% sec | Load: %%VAR ups.load%% | t: %%VAR battery.temperature%% | Input V: %%VAR input.voltage%% Hz: %%VAR input.frequency%%" '

ExecStop=/bin/systemctl kill upslog.service

RemainAfterExit=yes
Restart=on-failure
RestartSec=3

[Install]
WantedBy=multi-user.target
```

# Usage 

## Assign access:

```shell
sudo chown :nut -R /var/log/ups/
sudo chmod ug+rw -R /var/log/ups/
```

## Start

```shell
sudo systemctl daemon-reload && sudo systemctl start upslog.service; sudo systemctl status upslog.service
```

## Restart after changes

```shell
sudo systemctl daemon-reload && sudo systemctl restart upslog.service; sudo systemctl status upslog.service
```

## Troubleshoot

```shell
journalctl -xeu upslog.service
```