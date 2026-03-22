# Remote Syslog

DOCs:
- https://linux.how2shout.com/how-to-setup-syslog-server-on-ubuntu-server-24-04/
- https://www.cyberciti.biz/faq/how-to-set-up-ufw-firewall-on-ubuntu-24-04-lts-in-5-minutes/
- https://kifarunix.com/setup-rsyslog-server-on-ubuntu-20-04/
- https://www.rsyslog.com/doc/configuration/global/options/rsconf1_umask.html

```shell
sudo apt install rsyslog
sudo systemctl status rsyslog
sudo systemctl restart rsyslog
sudo vi /etc/rsyslog.conf

```

```
#Check ports
sudo netstat -tulnp | grep 514
```

Logs are 
`$WorkDirectory /var/spool/rsyslog`

```conf
#
# Set the default permissions for all log files.
#
$FileOwner nobody
$FileGroup nogroup
$dirOwner nobody
$dirGroup nogroup
$FileCreateMode 0776
$DirCreateMode 0776
$Umask 0776

```


## Modify formats

`sudo vi /etc/rsyslog.conf`

```conf
#Custom template to generate the log filename dynamically based on the client's IP address.
# $template RemInputLogs, "/var/log/remotelogs/%FROMHOST-IP%/%PROGRAMNAME%_%APP-NAME%.log"
$template RemInputLogs, "/var/log/remotelogs/%FROMHOST-IP%/%PROGRAMNAME%.log"
*.* ?RemInputLogs
```

## Or mounted disk

```conf
#Custom template to generate the log filename dynamically based on the client's IP address.
# $template RemInputLogs, "/var/log/remotelogs/%FROMHOST-IP%/%PROGRAMNAME%_%APP-NAME%.log"
$template RemInputLogs, "/mnt/share/HA_Share_1Tb/logs/%FROMHOST-IP%/%PROGRAMNAME%.log"
*.* ?RemInputLogs
```


Check: 
`rsyslogd -f /etc/rsyslog.conf -N1`

Dir:
```shell
mkdir -p /var/log/remotelogs/
sudo chown -R syslog: /var/log/remotelogs
# Share
sudo chown -R nobody:nogroup /mnt/share/HA_Share_1Tb/logs/
sudo chmod a+rwx -R /mnt/share/HA_Share_1Tb/logs/
# ??
sudo chmod u+rwx -R /mnt/share/HA_Share_1Tb/logs/
sudo chmod g+rwx -R /mnt/share/HA_Share_1Tb/logs/
```

Restart:

`sudo systemctl restart rsyslog`

## Log files from client:


Change:

```shell
sudo vi /etc/rsyslog.conf
```

# Logrotate

DOC: 
- https://betterstack.com/community/guides/logging/how-to-manage-log-files-with-logrotate-on-ubuntu-20-04/


Add new conf for new app:

`/etc/logrotate.d/mikrotik`

```shell
sudo vi "/etc/logrotate.conf"
sudo vi "/etc/logrotate.d/mikrotik"
sudo vi "/etc/logrotate.d/local-new"
sudo vi "/etc/logrotate.d/home-assistant"
sudo vi "/etc/logrotate.d/truenas"
# Test
sudo logrotate -d /etc/logrotate.d/mikrotik
sudo logrotate -d /etc/logrotate.d/local-new
sudo logrotate -d /etc/logrotate.d/home-assistant
# Force
sudo logrotate -f /etc/logrotate.d/mikrotik
sudo logrotate -f /etc/logrotate.d/local-new
sudo logrotate -f /etc/logrotate.d/home-assistant
sudo logrotate -f /etc/logrotate.d/truenas
```


```conf
/var/log/remotelogs/IP_ADDR/*.log {
    daily
    missingok
    notifempty
    copytruncate
    rotate 60
    size 10M
    dateext
    dateformat -%d_%m_%Y
}
```

OR 

```conf
# Local
/mnt/share/HA_Share_1Tb/logs/127.0.0.1/*.log {
    daily
    missingok
    notifempty
    copytruncate
    rotate 60
    su nobody nogroup
    create 0776 nobody nogroup
    size 10M
    dateext
    dateformat -%d_%m_%Y
    postrotate
        chmod -R a+rwx /mnt/share/HA_Share_1Tb/logs/
    endscript
}

## Mikrotik
/mnt/share/HA_Share_1Tb/logs/IP_ADDR/*.log {
    daily
    missingok
    notifempty
    copytruncate
    rotate 60
    su nobody nogroup
    create 0776 nobody nogroup
    size 10M
    dateext
    dateformat -%d_%m_%Y
    postrotate
       chmod -R a+rwx /mnt/share/HA_Share_1Tb/logs/
    endscript
}

## HomeAssistant
/mnt/share/HA_Share_1Tb/logs/IP_ADDR/*.log {
    daily
    missingok
    notifempty
    copytruncate
    rotate 60
    su nobody nogroup
    create 0776 nobody nogroup
    size 10M
    dateext
    dateformat -%d_%m_%Y
    postrotate
       chmod -R a+rwx /mnt/share/HA_Share_1Tb/logs/
    endscript
}

## Truenas
/mnt/share/HA_Share_1Tb/logs/TRUE_NAS/*.log {
    daily
    missingok
    notifempty
    copytruncate
    rotate 30
    su nobody nogroup
    create 0776 nobody nogroup
    size 2M
    dateext
    dateformat -%d_%m_%Y
    postrotate
       chmod -R a+rwx /mnt/share/HA_Share_1Tb/logs/
    endscript
}
```