# Remote Syslog

DOCs:
- https://linux.how2shout.com/how-to-setup-syslog-server-on-ubuntu-server-24-04/
- https://www.cyberciti.biz/faq/how-to-set-up-ufw-firewall-on-ubuntu-24-04-lts-in-5-minutes/
- https://kifarunix.com/setup-rsyslog-server-on-ubuntu-20-04/

```shell
sudo apt install rsyslog
sudo systemctl status rsyslog
sudo vi /etc/rsyslog.conf

#Check ports
sudo netstat -tulnp | grep 514
```

Logs are 
`$WorkDirectory /var/spool/rsyslog`

## Modify formats

`sudo vi /etc/rsyslog.conf`

```
#Custom template to generate the log filename dynamically based on the client's IP address.
# $template RemInputLogs, "/var/log/remotelogs/%FROMHOST-IP%/%PROGRAMNAME%_%APP-NAME%.log"
$template RemInputLogs, "/var/log/remotelogs/%FROMHOST-IP%/%PROGRAMNAME%.log"
*.* ?RemInputLogs
```

Check: 
`rsyslogd -f /etc/rsyslog.conf -N1`

Dir:
```shell
mkdir -p /var/log/remotelogs/
sudo chown -R syslog: /var/log/remotelogs
```

Restart:

`sudo  systemctl restart rsyslog`

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
vi "/etc/logrotate.conf"
vi "/etc/logrotate.d/mikrotik"
# Test
logrotate -d /etc/logrotate.d/mikrotik
# Force
logrotate -f /etc/logrotate.d/mikrotik
```


```conf
/var/log/remotelogs/192.168.1.1/*.log {
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