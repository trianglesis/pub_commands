# Use NUT 

Docs:
- https://networkupstools.org/

Initial:
- https://pimylifeup.com/raspberry-pi-nut-server/

More:
- https://community.openhab.org/t/beginners-guide-to-network-ups-tools-nut-on-a-raspberry-pi/78443
- https://melgrubb.com/2016/12/11/rphs-v2-ups/
- https://forums.unraid.net/topic/93341-tutorial-networked-nut-for-cyberpower-ups/
- https://www.networkshinobi.com/nut-email-notifications/
- https://www.ipfire.org/docs/addons/nut/detailed
- https://forums.gentoo.org/viewtopic-t-1034618-start-0.html

# Overall update

Current NUT version 2.8.0 had a bug where CPU usage goes up to 100% on battery power, the fix is at the master branch.
Better install NUT from sources to get this and other fixes:

- https://networkupstools.org/docs/user-manual.chunked/_installation_instructions.html

- [My walkthrough](NUT-FromSources.md)


# Setup and install

## Install

```shell
sudo apt install nut
```

## Check USB devices:

```shell
~ $ lsusb
Bus 004 Device 001: ID 1d6b:0003 Linux Foundation 3.0 root hub
Bus 003 Device 109: ID 0d9f:0004 Powercom Co., Ltd HID UPS Battery
Bus 003 Device 001: ID 1d6b:0002 Linux Foundation 2.0 root hub
Bus 002 Device 001: ID 1d6b:0003 Linux Foundation 3.0 root hub
Bus 001 Device 001: ID 1d6b:0002 Linux Foundation 2.0 root hub
```

## Add UPS to config:

```shell
sudo vi /etc/nut/ups.conf
```

Bus 001 Device 003: ID 0665:5161


Use 0d9f:0004 as vendorid\productid
Read about overriding options above at web.

- **override**
  - https://networkupstools.org/docs/developer-guide.chunked/apas02.html
  - Use with caution! This will only change the appearance of the variable to the outside world, internally in the UPS the original value is used.

- **ignorelb**
  - https://networkupstools.org/docs/man/ups.conf.html


```text
[Powercom800W]
    driver = usbhid-ups
    desc = "Powercom Co 800W"
    port = auto
    vendorid = 0d9f
    productid = 0004
    pollfreq = 5
    pollinterval = 2
    ignorelb
    override.ups.delay.start = 2
    override.battery.charge.low = 55
    override.battery.runtime.low = 45
    override.battery.charge.warning = 65
    override.ups.beeper.status = disabled
    override.battery.mfr.date = 2025/10/31
    override.battery.date = 31/10/25
    override.battery.date.maintenance = 31/10/25
```

For HA

```yaml
users:
  - username: nut-ups
    password: NutUpsSanek
    instcmds:
      - all
    actions: []
devices:
  - name: PowerCom
    driver: usbhid-ups
    port: auto
    config:
      - vendorid = 0d9f
      - productid = 0004
      - desc = "Powercom 800W"
     - pollfreq = 5
     - pollinterval = 2
     - ignorelb
     - override.battery.date = 2025/10/31
     - override.battery.mfr.date = 2025/10/31
     - override.battery.date.maintenance = 2026/10/31
     - override.ups.delay.start = 2
     - override.battery.charge.low = 55
     - override.battery.runtime.low = 45
     - override.battery.charge.warning = 65
     - override.ups.beeper.status = disabled
mode: netserver
shutdown_host: false
log_level: info
list_usb_devices: true
```


## Make accessible

```shell
sudo vi /etc/nut/upsd.conf
```

- Add address:
```text
LISTEN 0.0.0.0 3493
```


## Setup user

User: `upsmon`

```shell
sudo vi /etc/nut/upsd.users
```

At the end of file add:

```text
[upsmon]
    password = <PASSWORD>
    upsmon primary
```

### More users:

- [From here](https://blog.entek.org.uk/notes/2021/01/10/ups-monitoring-and-auto-shut-down-with-nut.html#:~:text=By%20default%2C%20NUT%20will%20signal,itself%20down%2015%20seconds%20later.)

```text
[admin]
    password = adminpass
    actions = SET
    actions = FSD
    instcmds = ALL
    upsmon master

[monmaster]
    password = masterpass
    upsmon master

[monslave]
    password = slavepass
    upsmon slave
```


## Configure monitoring:

```shell
sudo vi /etc/nut/upsmon.conf
```

### Possible options to look at:

```text
POLLFREQ 30

DEADTIME 120
```


- Add your UPS for internal monitoring:

```text
MONITOR Powercom800W@localhost 1 monmaster PASSWORD master
```

# Run as server

```shell
sudo vi /etc/nut/nut.conf
```

Change to:

```text
MODE=netserver
```

## Restart and check


```shell
sudo systemctl restart nut-server && sudo systemctl restart nut-monitor
```

```shell
upsc Powercom800W@localhost
```


# Install WEB Gui

```shell
sudo apt install nut-cgi
sudo vi /etc/nut/hosts.conf
MONITOR Powercom800W@localhost "Powercom800W_Raspb"
```

## I'm using lighttpd

This is because I have a Pi-Hole installed already:
Install modules:


```shell
sudo apt install nut-cgi -y sudo lighttpd-enable-mod cgi sudo service lighttpd force-reload
```

### Check CGI:

```shell
sudo vi /etc/lighttpd/conf-enabled/10-cgi.conf

### See:
server.modules += ( "mod_cgi" )

$HTTP["url"] =~ "^/cgi-bin/" {
        cgi.assign = ( "" => "" )
        alias.url += ( "/cgi-bin/" => "/usr/lib/cgi-bin/" )
}
```

### Create a new http conf for NUT:

```shell
sudo vi /etc/lighttpd/conf-enabled/nut-ups.conf
```

General HTTPD conf should include files: `line: 48 include "/etc/lighttpd/conf-enabled/*.conf"`
Pi-Hole will overwrite the general config file do not change anything there.

Conf content working fine for me for all three pages for my UPS, but index.html is not working at the main www dir, and it does not matter.

```shell
# Wotk with both, inxed and CGI
server.modules += (
    "mod_auth",
    "mod_cgi",
    "mod_expire",
    "mod_rewrite",
    "mod_fastcgi",
    "mod_setenv",
    "mod_status"
)
# Work CGI, doesnt work idex
$HTTP["url"] =~ "/nut/" {
    cgi.assign = ( "" => "" )
    alias.url += ( "/nut/cgi-bin" => "/usr/lib/cgi-bin/nut" )
    alias.url += ( "/cgi-bin/nut" => "/usr/lib/cgi-bin/nut" )
}
# Static HTML
$HTTP["url"] =~ "/nut-web/" { alias.url += ( "/nut-web" => "/usr/share/nut/www" ) }
```

### Optional CGI executable flag:

```shell
sudo vi /etc/nut/upsset.conf
```

Uncomment

```text
###
I_HAVE_SECURED_MY_CGI_DIRECTORY
###

```


### Restart

`sudo systemctl restart lighttpd.service`


See those pages:

### Views:

- http://YOUR_PI_HOST/cgi-bin/nut/upsstats.cgi?host=Powercom800W@localhost&treemode
- http://YOUR_PI_HOST/cgi-bin/nut/upsstats.cgi
- http://YOUR_PI_HOST/cgi-bin/nut/upsstats.cgi?host=Powercom800W@localhost



# Notifications and email

## Simple

- [Read](https://www.networkshinobi.com/nut-email-notifications/)
- [Email](Email.md)

```shell
sudo vi /etc/nut/upsmon.conf
```

- Add at the EOF:

```sh
# Email script for NOTIFYCMD
# Simple
NOTIFYCMD "/etc/nut/notifycmd.sh"

# Notification events
NOTIFYFLAG ONLINE     SYSLOG+WALL+EXEC
NOTIFYFLAG ONBATT     SYSLOG+WALL+EXEC
NOTIFYFLAG LOWBATT    SYSLOG+WALL+EXEC
NOTIFYFLAG FSD        SYSLOG+WALL+EXEC
NOTIFYFLAG COMMOK     SYSLOG+WALL+EXEC
NOTIFYFLAG COMMBAD    SYSLOG+WALL+EXEC
NOTIFYFLAG SHUTDOWN   SYSLOG+WALL+EXEC
NOTIFYFLAG REPLBATT   SYSLOG+WALL+EXEC
NOTIFYFLAG NOCOMM     SYSLOG+WALL+EXEC
NOTIFYFLAG NOPARENT   SYSLOG+WALL+EXEC
```

## Simple notification script:

```shell
sudo vi /etc/nut/notifycmd.sh
```

- Add notification script:

```shell
sudo tee -a /etc/nut/notifycmd.sh <<EOF
#!/bin/bash
EMAIL='to@YOYR_EMAIL.com'
echo -e "Subject: UPS ALERT Basic: $NOTIFYTYPE\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\n\nThis is a basic notification!" | msmtp $EMAIL
EOF
```

- Exec rights:


```shell
# Change group to nut
sudo chown :nut /etc/nut/notifycmd.sh
# Add execution
sudo chmod 774 /etc/nut/notifycmd.sh
# Restart the NUT services
sudo systemctl restart nut-server.service
sudo systemctl restart nut-driver.service
sudo systemctl restart nut-monitor.service


sudo systemctl disable nut-server.service
sudo systemctl disable nut-driver.service
sudo systemctl disable nut-monitor.service
```


## Extra notification script by Scheduler

- [Read](https://forums.unraid.net/topic/93341-tutorial-networked-nut-for-cyberpower-ups/)
- [Email](Email.md)

### Set signals

- At the end of the conf file:

```shell
sudo vi /etc/nut/upsmon.conf
```

- Add signals and script

```sh
# Extra notifications by scheduler
NOTIFYCMD /sbin/upssched
NOTIFYFLAG ONBATT   SYSLOG+WALL+EXEC
NOTIFYFLAG ONLINE   SYSLOG+WALL+EXEC
NOTIFYFLAG REPLBATT SYSLOG+WALL+EXEC
```

### Set schedule triggers

```shell
sudo vi /etc/nut/upssched.conf
```

- enable CMDSCRIPT

```sh
CMDSCRIPT /bin/upssched-cmd
```

- Add triggers  

```sh
# Command pipe and lock-file
PIPEFN /var/run/nut/upssched.pipe
LOCKFN /var/run/nut/upssched.lock

# Send alerts immediately on change in line power
AT ONBATT * EXECUTE onbatt
AT ONLINE * EXECUTE onpower

# (Optional) Silence the beeper after 1 minute
AT ONBATT * START-TIMER mute_beeper 60
AT ONLINE * CANCEL-TIMER mute_beeper

# Shutdown after 40 minutes on battery (40 * 60 = 1200)
AT ONBATT * START-TIMER onbatt_shutdown 2400

# Cancel timer if power's restored
AT ONLINE * CANCEL-TIMER onbatt_shutdown

# Battery replacement indicated by cron'd quick test
AT REPLBATT * EXECUTE replace_batt
```

### Set program, emails and logs

### Emails example

```sh
echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power outage, on battery\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\nActions:\n\n\t - UPS will be muted in 1 min\n\n\t - Intel NUC shutdown at 90% battery\n\n\t - System shutdown after 40 min on battery run" | msmtp $EMAIL
echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power restored\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE" | msmtp $EMAIL
echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power outage, on battery, mute beeper\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\nActions:\n\n\t - UPS will be muted in 1 min\n\n\t - Intel NUC shutdown at 90% battery\n\n\t - System shutdown after 40 min on battery run" | msmtp $EMAIL
echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power outage, shutdown system after 40 minutes\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\nActions:\n\n\t - System shutdown after 40 min on battery run" | msmtp $EMAIL
echo -e "Subject: UPS ALERT: $NOTIFYTYPE Quick self-test\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE" | msmtp $EMAIL
```

#### My addition to use emails:

- [Emailing from Pi](Email.md)
- [external doc](https://www.networkshinobi.com/nut-email-notifications/)
- [official doc](https://networkupstools.org/docs/user-manual.chunked/ar01s07.html)


### Now add this as new into the existing program file:

```shell
sudo vi /bin/upssched-cmd
```

```sh
# START: User-specific settings                            
#
UPS_USERNAME="admin"
UPS_PASSWORD="<PASSWORD>"
UPS_LINK="Powercom800W@localhost"
EMAIL="to@YOUR_DOMAIN.com"
# END  

case $1 in
    onbatt)
        # make sure beeper is enabled
        upscmd -u ${UPS_USERNAME} -p ${UPS_PASSWORD} ${UPS_LINK} beeper.enable
        # alert
        echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power outage, on battery\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\nActions:\n\n\t - UPS will be muted in 1 min\n\n\t - Intel NUC shutdown at 90% battery\n\n\t - System shutdown if battery level low at 10-20%" | msmtp $EMAIL
        message="Power outage, on battery"
        logger -t upssched-cmd "$message"
        ;;
    onpower)
        message="Power restored"
        echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power restored\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE" | msmtp $EMAIL
        logger -t upssched-cmd "$message"
        ;;
    mute_beeper)
         message="(1) minute limit exceeded, muting beeper"
         echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power outage, on battery, mute beeper\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\nActions:\n\n\t - UPS will be muted in 1 min\n\n\t - Intel NUC shutdown at 90% battery\n\n\t - System shutdown if battery level low at 10-20%" | msmtp $EMAIL
         upscmd -u ${UPS_USERNAME} -p ${UPS_PASSWORD} ${UPS_LINK} beeper.mute
         ;;
    onbatt_shutdown)
        message="Triggering shutdown on battery charge level 10%-20%"
        echo -e "Subject: UPS ALERT: $NOTIFYTYPE Power outage, shutdown system after 40 minutes\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\nActions:\n\n\t - System shutdown if battery level low at 10-20%" | msmtp $EMAIL
        logger -t upssched-cmd "$message"
        /sbin/upsmon -c fsd
        ;;
    replace_batt)
        message="Quick self-test indicates battery requires replacement"
        echo -e "Subject: UPS ALERT: $NOTIFYTYPE Quick self-test\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE" | msmtp $EMAIL
        logger -t upssched-cmd "$message"
        ;;
    *)
        logger -t upssched-cmd "Unrecognized command: $1"
        ;;
esac
```

## More options:

- [Read](https://www.ipfire.org/docs/addons/nut/detailed)

### NOTIFY FLAGS and NOTIFYMSG

- About [NOTIFY FLAGS](https://networkupstools.org/docs/man/upsmon.html)

More monstrous config:

`sudo vi /etc/nut/upsmon.conf`

```shell
# Full config:
NOTIFYCMD /sbin/upssched

# Events with battery
NOTIFYFLAG ONLINE     SYSLOG+WALL+EXEC
NOTIFYFLAG ONBATT     SYSLOG+WALL+EXEC
NOTIFYFLAG LOWBATT    SYSLOG+WALL+EXEC
NOTIFYFLAG REPLBATT   SYSLOG+WALL+EXEC
NOTIFYFLAG SHUTDOWN   SYSLOG+WALL+EXEC
# Events with UPS
NOTIFYFLAG FSD        SYSLOG+WALL+EXEC
NOTIFYFLAG COMMOK     SYSLOG+WALL+EXEC
NOTIFYFLAG COMMBAD    SYSLOG+WALL+EXEC
NOTIFYFLAG NOCOMM     SYSLOG+WALL+EXEC
NOTIFYFLAG NOPARENT   SYSLOG+WALL+EXEC
# Messages with battery
NOTIFYMSG ONLINE    "UPS %s on line power"
NOTIFYMSG ONBATT    "UPS %s on battery"
NOTIFYMSG LOWBATT   "UPS %s battery is low"
NOTIFYMSG REPLBATT  "UPS %s battery needs to be replaced"
NOTIFYMSG SHUTDOWN  "Auto logout and shutdown proceeding"
# Messages with UPS
NOTIFYMSG FSD       "UPS %s: forced shutdown in progress"
NOTIFYMSG COMMOK    "Communications with UPS %s established"
NOTIFYMSG COMMBAD   "Communications with UPS %s lost"
NOTIFYMSG NOCOMM    "UPS %s is unavailable"
NOTIFYMSG NOPARENT  "upsmon parent process died - shutdown impossible"
```

### Scheduler program

#### Possible error at service:

- `Failed to connect to parent and failed to create parent`: see **PIPEFN/LOCKFN**
  - Create a path: `sudo mkdir -p /var/run/nut/`
  - Make 770: `sudo chmod 770 -R /var/run/nut/`
  - Add user 770: `sudo chown :nut -R /var/run/nut/`


#### Extended program

AT <**EVENT**> * **ACTION** Do an action or cancel an action.

```shell
sudo vi /etc/nut/upssched.conf
```

```sh
CMDSCRIPT /etc/nut/upssched-cmd
PIPEFN /var/run/nut/upssched.pipe
LOCKFN /var/run/nut/upssched.lock
# ============================================================================
# BAD communications warnings, wait 30 seconds before send, cancel if becomes OK in 60 sec!
AT COMMBAD * START-TIMER commbad 30
AT COMMOK * CANCEL-TIMER commbad commok
AT NOCOMM * EXECUTE commbad # No communication - send warning!
# Battery power, send email update on intervals: 1 min, 15 min, 1 hour, 1,5 hours, 2 hours.
AT ONBATT * START-TIMER onbatt 60  # 1 min
AT ONBATT * START-TIMER onbatt 900 # 15 min
AT ONBATT * START-TIMER onbatt 1800 # 30 min
AT ONBATT * START-TIMER onbatt 3600 # 1 Hour
AT ONBATT * START-TIMER onbatt 4800 # 1.5 Hours
AT ONBATT * START-TIMER onbatt 7200 # 2 Hours
AT ONBATT * START-TIMER onbatt 10800 # 3 Hours
AT ONLINE * CANCEL-TIMER onbatt online
# Low batterwy warning and command execute immediately!
AT LOWBATT * EXECUTE onbatt
# When battery is bad - execute immediately!
AT REPLBATT * EXECUTE badbatt
# Shutdown at low batt execute immediately!
AT SHUTDOWN * EXECUTE powerdown
AT FSD * EXECUTE powerdownforced
# Service died:
AT NOPARENT * EXECUTE servicedown
```

#### Check statuses

- [From here](https://www.ipfire.org/docs/addons/nut/detailed)
- [Skip SSL cert error message](https://askubuntu.com/a/565740)


### Extended Program file


```shell
sudo vi /etc/nut/upssched-cmd
```

```sh
#!/bin/sh
UPS_USERNAME="admin"
UPS_PASSWORD="PASSWD"
UPS_LINK="Powercom800W@localhost"
EMAIL="it@EMAIL"

# Variables:
set_variables () {
    DATUM=`/bin/date`
    STAT=`upsc Powercom800W@localhost ups.status 2>&1 | grep -v '^Init SSL'`
    BATT=`upsc Powercom800W@localhost battery.charge 2>&1 | grep -v '^Init SSL'`
    BATT_t=`upsc Powercom800W@localhost battery.temperature 2>&1 | grep -v '^Init SSL'`
    BATT_LOW=`upsc Powercom800W@localhost battery.charge.low 2>&1 | grep -v '^Init SSL'`
    BATT_WARN=`upsc Powercom800W@localhost battery.charge.warning	 2>&1 | grep -v '^Init SSL'`
    RUNTIME=`upsc Powercom800W@localhost battery.runtime 2>&1 | grep -v '^Init SSL'`
    BEEPER=`upsc Powercom800W@localhost ups.beeper.status 2>&1 | grep -v '^Init SSL'`
    TESTRES=`upsc Powercom800W@localhost ups.test.result 2>&1 | grep -v '^Init SSL'`
    INP_FREQ=`upsc Powercom800W@localhost input.frequency 2>&1 | grep -v '^Init SSL'`
    INP_V=`upsc Powercom800W@localhost input.voltage 2>&1 | grep -v '^Init SSL'`
    INP_V_Nom=`upsc Powercom800W@localhost input.voltage.nominal 2>&1 | grep -v '^Init SSL'`
    OUT_FREQ=`upsc Powercom800W@localhost output.frequency 2>&1 | grep -v '^Init SSL'`
    OUT_V=`upsc Powercom800W@localhost output.voltage 2>&1 | grep -v '^Init SSL'`
    OUT_V_Nom=`upsc Powercom800W@localhost output.voltage.nominal 2>&1 | grep -v '^Init SSL'`
    # UPSLOG=`cat /var/log/messages | grep ups | tail -50`
    BASIC_INFO="
Date:                   $DATUM
Status:                 $STAT (OL online)
Charge level:           $BATT | Warning: $BATT_WARN Low: $BATT_LOW (shutdown at low!)
Runtime EST:            $RUNTIME

==================================================================================
##################################################################################
Input Freq:             $INP_FREQ
Input Voltage:          $INP_V
Input Voltage nominal:  $INP_V_Nom
==================================================================================
Input Freq:             $OUT_FREQ
Input Voltage:          $OUT_V
Input Voltage nominal:  $OUT_V_Nom
==================================================================================
Sound signal:           $BEEPER | (enabled\disabled)
Battery temp:           $BATT_t
Battery low:            $BATT_LOW - set at /etc/nut/ups.conf
Battery warn:           $BATT_WARN - set at /etc/nut/ups.conf
Battery test state:     $TESTRES - at CRON
"
}

disable_beep () {
    # Disable BEEP
    # But my model is beeping even on disabling, better not touch it.
    # /bin/upscmd -u ${UPS_USERNAME} -p ${UPS_PASSWORD} ${UPS_LINK} beeper.toggle
    /bin/upscmd -u ${UPS_USERNAME} -p ${UPS_PASSWORD} ${UPS_LINK} beeper.disable
}

wake_on_lan () {
    # Wake media box device:
    sudo wakeonlan -i 192.168.1.9 -p 4343 1C:69:7A:AB:54:E6
}

# Programms
case $1 in
    onbatt)
        disable_beep
        set_variables
        message="Power outage, on battery"
        logger -t upssched-cmd "$message"
        echo "Subject: UPS ALERT $NOTIFYTYPE power is down!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    badbatt)
        set_variables
        message="Bad battery, replace!"
        logger -t upssched-cmd "$message" 
        echo "Subject: UPS ALERT $NOTIFYTYPE bad battery!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    online)
        set_variables
        message="Power is back!"
        logger -t upssched-cmd "$message" 
        echo "Subject: UPS ALERT $NOTIFYTYPE power is back!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL
        wake_on_lan ;;
    commbad)
        set_variables
        message="Communication is bad with UPS!"
        logger -t upssched-cmd "$message" 
        echo "Subject: UPS ALERT $NOTIFYTYPE communications is down!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    commok)
        set_variables
        message="Comminication with UPS restored!"
        logger -t upssched-cmd "$message" 
        echo "Subject: UPS ALERT $NOTIFYTYPE communications restored!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    powerdown)
        set_variables
        message="Shut down the system"
        logger -t upssched-cmd "$message" 
        echo "Subject: UPS ALERT $NOTIFYTYPE system shutdown initiated!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    powerdownforced)
        set_variables
        message="Shutdown the system FORCED"
        logger -t upssched-cmd "$message" 
        echo "Subject: UPS ALERT $NOTIFYTYPE forced system shutdown!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    servicedown)
        set_variables
        message="UPS Service died!"
        logger -t upssched-cmd "$message" 
        echo "Subject: UPS ALERT $NOTIFYTYPE service died, need restart!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    test_email)
        echo "Setting variables"
        set_variables
        echo "sending test email: $BASIC_INFO"
        echo "Subject: UPS ALERT $NOTIFYTYPE test email!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    *)
        echo "wrong parameter" ;;
esac

exit 0
```

#### Check permissions:

- [At the config file too](NUT_UPS.md#possible-error-at-service)
  - Check all paths and access by NUT user

```shell
# Change group to nut
sudo chown :nut /etc/nut/upssched-cmd
# Add execution
sudo chmod 774 /etc/nut/upssched-cmd
# Restart the NUT services
sudo systemctl restart nut-server.service && sudo systemctl restart nut-monitor.service
sudo systemctl status nut-server.service && sudo systemctl status nut-monitor.service
```


#### Test script:

```shell
sudo /usr/bin/upssched-cmd onbatt
sudo /usr/bin/upssched-cmd online
sudo /usr/bin/upssched-cmd badbatt
sudo /usr/bin/upssched-cmd test_email
```

### Check for issues:

See logs:

```shell
journalctl -n 200 -f | grep nut
```

See `exec_cmd(/etc/nut/upssched-cmd onbatt) returned 2`

Fix: check the program for typos and run it manually in a console.


#### Bad comm for this device:

See: [About this device](https://networkupstools.org/ddl/Powercom/RPT-600AP.html)

Example for BADCOMM workaround:

```sh
#!/bin/bash
/usr/bin/logger -t NOTIFYCMD "${NOTIFYTYPE} ${UPSNAME}"
if [ "${NOTIFYTYPE}" = "COMMBAD" ]; then
  /usr/bin/logger -t NOTIFYCMD "Restarting upsdrvctl..."
  /sbin/upsdrvctl stop
  /sbin/upsdrvctl -D start
fi
```


### Test exec:

Use this template to send a test email and change the body as you want:

```sh
#!/bin/sh

NOTIFYTYPE='NOTIFYTYPE TEST'
UPSNAME='UPSNAME TEST'

EMAIL="it@DOMAIN.com"

# Variables:
set_variables () {
    DATUM=`/bin/date`
    STAT=`upsc Powercom800W@localhost ups.status 2>&1 | grep -v '^Init SSL'`
    BATT=`upsc Powercom800W@localhost battery.charge 2>&1 | grep -v '^Init SSL'`
    BATT_t=`upsc Powercom800W@localhost battery.temperature 2>&1 | grep -v '^Init SSL'`
    BATT_LOW=`upsc Powercom800W@localhost battery.charge.low 2>&1 | grep -v '^Init SSL'`
    BATT_WARN=`upsc Powercom800W@localhost battery.charge.warning	 2>&1 | grep -v '^Init SSL'`
    RUNTIME=`upsc Powercom800W@localhost battery.runtime 2>&1 | grep -v '^Init SSL'`
    BEEPER=`upsc Powercom800W@localhost ups.beeper.status 2>&1 | grep -v '^Init SSL'`
    TESTRES=`upsc Powercom800W@localhost ups.test.result 2>&1 | grep -v '^Init SSL'`
    INP_FREQ=`upsc Powercom800W@localhost input.frequency 2>&1 | grep -v '^Init SSL'`
    INP_V=`upsc Powercom800W@localhost input.voltage 2>&1 | grep -v '^Init SSL'`
    INP_V_Nom=`upsc Powercom800W@localhost input.voltage.nominal 2>&1 | grep -v '^Init SSL'`
    OUT_FREQ=`upsc Powercom800W@localhost output.frequency 2>&1 | grep -v '^Init SSL'`
    OUT_V=`upsc Powercom800W@localhost output.voltage 2>&1 | grep -v '^Init SSL'`
    OUT_V_Nom=`upsc Powercom800W@localhost output.voltage.nominal 2>&1 | grep -v '^Init SSL'`
    # UPSLOG=`cat /var/log/messages | grep ups | tail -50`
    BASIC_INFO="
Date:                   $DATUM
Status:                 $STAT (OL online)
Charge level:           $BATT | Warning: $BATT_WARN Low: $BATT_LOW (shutdown at low!)
Runtime EST:            $RUNTIME

==================================================================================
##################################################################################
Input Freq:             $INP_FREQ
Input Voltage:          $INP_V
Input Voltage nominal:  $INP_V_Nom
==================================================================================
Input Freq:             $OUT_FREQ
Input Voltage:          $OUT_V
Input Voltage nominal:  $OUT_V_Nom
==================================================================================
Sound signal:           $BEEPER | (enabled\disabled)
Battery temp:           $BATT_t
Battery low:            $BATT_LOW - set at /etc/nut/ups.conf
Battery warn:           $BATT_WARN - set at /etc/nut/ups.conf
Battery test state:     $TESTRES - at CRON
"
}

disable_beep () {
    # Disable BEEP
    /bin/upscmd -u ${UPS_USERNAME} -p ${UPS_PASSWORD} ${UPS_LINK} beeper.disable
}

set_variables
disable_beep
echo "Subject: UPS ALERT $NOTIFYTYPE test!\r\n\nUPS $UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL

exit 0
```


### Setup SSL

- [SSL May be useful](https://networkupstools.org/docs/user-manual.chunked/ar01s10.html#_configuring_ssl)

Use doc from above, do not set certs in a usual Linux dir at pi, use as described in the official doc, and let it be.

Note: it finished with `invalid directive CERTFILE /usr/local/ups/etc/upsd.pem` - so do not bother to use this, no SSL then.

Path to open ssl: `/usr/bin/openssl`

```shell
openssl req -new -x509 -nodes -out upsd.crt -keyout upsd.key
...
openssl x509 -hash -noout -in upsd.crt

# Add certs
sudo mkdir -p /usr/local/ups/etc/certs
sudo chmod 0755 /usr/local/ups/etc/certs
sudo cp upsd.crt /usr/local/ups/etc/certs/f0626ddf.0

# Create the combined file for upsd
sudo cat upsd.crt upsd.key > upsd.pem
sudo chown root:nut upsd.pem
sudo chmod 0640 upsd.pem

# Server-side
sudo mv upsd.pem /usr/local/ups/etc/upsd.pem

# Add line 375+
sudo vi /etc/nut/upsmon.conf
CERTFILE /usr/local/ups/etc/upsd.pem


# Clean:
rm -f upsd.crt upsd.key

# Point upsmon at the certificates
sudo vi /etc/nut/upsmon.conf

# Path, line 373
CERTPATH /usr/local/ups/etc/certs

# Line 400+
CERTVERIFY 1
FORCESSL 1
```

Restart the client and see:

```shell
sudo service nut-client restart && sudo service nut-client status
Mar 07 14:30:15 raspberrypi nut-monitor[1295564]: /etc/nut/upsmon.conf line 376: invalid directive CERTFILE /usr/local/ups/etc/upsd.pem

# OR

Mar 07 14:33:53 raspberrypi nut-monitor[1297153]: Init SSL with cerificate database located at /usr/local/ups/etc/certs
Mar 07 14:33:53 raspberrypi nut-monitor[1297153]: Can not initialize SSL context
```

Do not use SSL now.


## Restart the NUT services

```shell
sudo systemctl restart nut-server.service && sudo systemctl restart nut-monitor.service
# ???
sudo systemctl restart nut-driver.service
```

### Check

```shell
sudo service nut-server status && sudo service nut-client status
```

### Cron self-test

- [Cron](https://en.wikipedia.org/wiki/Cron)
- [UPS CMD](https://networkupstools.org/docs/man/upscmd.html)

```shell
sudo crontab -e
```

Add

```text
# ┌───────────── minute (0–59)
# │ ┌───────────── hour (0–23)
# │ │ ┌───────────── day of the month (1–31)
# │ │ │ ┌───────────── month (1–12)
# │ │ │ │ ┌───────────── day of the week (0–6) (Sunday to Saturday;
# │ │ │ │ │                                   7 is also Sunday on some systems)
# │ │ │ │ │
# │ │ │ │ │
# * * * * * <command to execute>

# Montly each 1st day at 10AM UPS self-test
0  11  1   *   *       /usr/local/ups/bin/upscmd -u admin -p sanek_ups_admin Powercom800W@localhost test.battery.start.quick
```

# CMD Control:

 - [Read](https://www.trojanc.co.za/2023/04/28/guide-network-ups-tools-commands/)
 - [CMD Variable set: upsrw](https://networkupstools.org/docs/man/upsrw.html)
 - [UPS CMD: upscmd](https://networkupstools.org/docs/man/upscmd.html)


### Check supported upscmd:

```shell
upscmd -l Powercom800W@localhost
```

Any non-listed command would return: `Unexpected response from upsd: ERR CMD-NOT-SUPPORTED`

#### Out:

```text
Instant commands supported on UPS [Powercom800W]:

beeper.disable - Disable the UPS beeper
beeper.enable - Enable the UPS beeper
beeper.toggle - Toggle the UPS beeper
load.on - Turn on the load immediately
load.on.delay - Turn on the load with a delay (seconds)
shutdown.return - Turn off the load and return when power is back
shutdown.stayoff - Turn off the load and remain off
test.battery.start.quick - Start a quick battery test
```

#### Test:

```shell
/bin/upscmd -u admin -p PASSWD Powercom800W@localhost beeper.toggle
/bin/upscmd -u admin -p PASSWD Powercom800W@localhost beeper.enable
/bin/upscmd -u admin -p PASSWD Powercom800W@localhost beeper.disable
# Will not work:
/bin/upscmd -u admin -p PASSWD Powercom800W@localhost battery.charge.low=10
# 
upsc Powercom800W@localhost ups.beeper.status 2>&1 | grep -v '^Init SSL'
upsc Powercom800W@localhost battery.charge.low 2>&1 | grep -v '^Init SSL'
```

# Set variables: upsrw

```shell
# Show supported
/bin/upsrw -u admin -p PASSWD Powercom800W@localhost

# Should not work
/bin/upsrw -s battery.charge.low=11 -u admin -p PASSWD Powercom800W@localhost

# Supported only
/bin/upsrw -s ups.delay.shutdown=120 -u admin -p PASSWD Powercom800W@localhost
```

# Listen to UPS from the local network


You can now listen to your UPS from another machine and shut it down if needed.
Usecase for windows.

- https://github.com/nutdotnet/WinNUT-Client

Install, open and configure it according to your previous setup.

![NUT WIndows](image.png)


![NUT Connected](image-1.png)



# More:

- [Logging periodical](https://networkupstools.org/docs/man/upslog.html)
  - upslog is a daemon that will poll a UPS at periodic intervals, fetch the variables that interest you, format them, and write them to a file.
  - Separate dir: `sudo mkdir -p /var/log/ups/`
  - Access to dir: `sudo chmod 776 -R /var/log/ups/`
  - Access to dir: `sudo chown user:user -R /var/log/ups/`
  - run as user 
    - `upslog -s Powercom800W@localhost -f "%TIME @Y-@m-@d @H:@M:@S% -- Status: [%VAR ups.status%] | Charge: %VAR battery.charge% | Runtime: %VAR battery.runtime% sec | Load: %VAR ups.load% | t: %VAR battery.temperature% | Input V: %VAR input.voltage% Hz: %VAR input.frequency%" -l /var/log/ups/UPS_Powercom800W.log`
  - Daemonize if needed
    - [System daemon](systemd-deamon.md)

- [SIMULATING POWER FAILURES](https://networkupstools.org/docs/man/upsmon.html)
  - Run: `upsmon -c fsd`

- Install WIN client and listen to this UPS from a related device:
  - https://github.com/nutdotnet/WinNUT-Client