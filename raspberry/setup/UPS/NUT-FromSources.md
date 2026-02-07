# Overall update

Current NUT version 2.8.0 had a bug where CPU usage goes up to 100% on battery power, the fix is at the master branch.
Better install NUT from sources to get this and other fixes:
- https://networkupstools.org/docs/user-manual.chunked/_installation_instructions.html


# NOTE

No longer create SYSMEMD scripts:

```yaml
cat /etc/os-release
PRETTY_NAME="Debian GNU/Linux 13 (trixie)"
NAME="Debian GNU/Linux"
VERSION_ID="13"
VERSION="13 (trixie)"
VERSION_CODENAME=trixie
DEBIAN_VERSION_FULL=13.3
ID=debian
HOME_URL="https://www.debian.org/"
SUPPORT_URL="https://www.debian.org/support"
BUG_REPORT_URL="https://bugs.debian.org/"


Failed to restart nut-driver-enumerator.service: Unit nut-driver-enumerator.service not found.
Failed to restart nut-monitor.service: Unit nut-monitor.service not found.
Failed to restart nut-server.service: Unit nut-server.service not found.

```


# Download

```shell
mkdir NUT_Sources && cd NUT_Sources
git clone https://github.com/networkupstools/nut.git

# ALT
mkdir nut_2_8_4
cd nut_2_8_4/
wget https://github.com/networkupstools/nut/archive/refs/tags/v2.8.4.zip
unzip v2.8.4.zip
cd nut-2.8.4/
```

# Configure

- to build and install USB drivers, add `--with-usb` **(note that you need to install libusb development package or files).**
- to build and install CGI scripts, add `--with-cgi`


```shell
./autogen.sh
# In case of error see Issues
# conf help:
./configure --help
# Run, in case of USB driver or libgd related error, see below
./configure --with-user=nut --with-group=nut --with-usb --with-cgi

./configure --with-user=nut --with-group=nut --with-usb --with-cgi --htmldir=/usr/local/ups/html
```

## Issues:

1. `configure: error: cannot find required auxiliary files: ar-lib missing install-sh config.guess config.sub`
   1. https://askubuntu.com/a/27679

Probable fix:

```shell
sudo apt-get install autogen libtool shtool
```


2. `configure: error: USB drivers requested, but libusb not found.`
   1. https://askubuntu.com/questions/225382/install-usblib-package-ubuntu

Probable fix:

```shell
sudo apt-get install libusb-1.0-0-dev
```

3. `configure: error: libgd not found, required for CGI build`
   1. https://metacpan.org/dist/GD
   2. https://howtoinstall.co/package/libgd-dev

```shell
# Works
sudo apt-get install libgd-dev
# OR
sudo apt-get install libgd-gd2-perl
```

# Make & install

- https://networkupstools.org/docs/user-manual.chunked/_installation_instructions.html#replacing-a-systemd-enabled-nut-deployment
- https://networkupstools.org/docs/user-manual.chunked/_installation_instructions.html#replacing-a-systemd-enabled-nut-deployment


```shell
make
make install
# Or
make -j 4 all && make -j 4 check && \
    { sudo systemctl stop nut-monitor nut-server || true ; } && \
    { sudo systemctl stop nut-driver.service || true ; } && \
    { sudo systemctl stop nut-driver.target || true ; } && \
    { sudo systemctl stop nut.target || true ; } && \
    sudo make install && \
    sudo systemctl daemon-reload && \
    sudo systemd-tmpfiles --create && \
    sudo systemctl disable nut.target nut-driver.target \
        nut-monitor nut-server nut-driver-enumerator.path \
        nut-driver-enumerator.service && \
    sudo systemctl enable nut.target nut-driver.target \
        nut-monitor nut-server nut-driver-enumerator.path \
        nut-driver-enumerator.service && \
    { sudo systemctl restart udev || true ; } && \
    sudo systemctl restart nut-driver-enumerator.service \
        nut-monitor nut-server


sudo systemctl restart nut-driver-enumerator.service nut-monitor nut-server


# 
./configure --enable-inplace-runtime --with-user=nut --with-group=nut --with-usb --with-cgi \
    --with-libsystemd \
    --with-systemdsystemunitdir=/home/sanek/Downloads/nut-md \
    --with-systemdsystempresetdir=/home/sanek/Downloads/nut-md \
    --with-systemdshutdowndir=/home/sanek/Downloads/nut-md \
    --with-systemdtmpfilesdir=/home/sanek/Downloads/nut-md



./configure --enable-inplace-runtime --with-libsystemd --with-user=nut --with-group=nut --with-usb --with-cgi
make -j 4 all && make -j 4 check && \
    { sudo systemctl stop nut-monitor nut-server || true ; } && \
    { sudo systemctl stop nut-driver.service || true ; } && \
    { sudo systemctl stop nut-driver.target || true ; } && \
    { sudo systemctl stop nut.target || true ; } && \
    sudo make install && \
    sudo systemctl daemon-reload && \
    sudo systemd-tmpfiles --create && \
    sudo systemctl disable nut.target nut-driver.target \
        nut-monitor nut-server nut-driver-enumerator.path \
        nut-driver-enumerator.service && \
    sudo systemctl enable nut.target nut-driver.target \
        nut-monitor nut-server nut-driver-enumerator.path \
        nut-driver-enumerator.service && \
    { sudo systemctl restart udev || true ; } && \
    sudo systemctl restart nut-driver-enumerator.service \
        nut-monitor nut-server

# Start over
make clean && make distclean
```

## Remove installed previously

```shell
sudo apt remove nut
```

# Daemon

Should be generated in config + make install

- https://github.com/networkupstools/nut/tree/master/scripts/systemd

Pre setup:
- UPS Config See: [ups.conf](NUT_UPS.md#add-ups-to-config)
- UPS User setup See: [ups.conf](NUT_UPS.md#setup-user)
- UPS conf mon See: [ups.conf](NUT_UPS.md#configure-monitoring)
- UPS Scheduler conf See: [ups.conf](NUT_UPS.md#set-schedule-triggers)

# Config migration from NUT install

Use old NUT config files from the previous installation, but copy files into a new dir created by the config script. Next time config script will only replace sample files.

Everything should be the same, all options are compatible!

Bin files are on a new path too.

```shell
# Paths, check and remove old NUT:
ls /lib/systemd/system/
ls -lah /etc/systemd/system/
# Check after 'make install':
ls -lah /lib/systemd/system/ | grep nut
```

## Now copy old working config files at the new destination

```shell
# Configs, copy installed config from system etc here:
sudo cp /etc/nut/* /usr/local/ups/etc/
ls -lah /usr/local/ups/etc/
# Rights for user nut!
sudo chown -R root:nut /usr/local/ups/
# If you want - delete old configs now from /etc
sudo rm -r /etc/nut
```

## Check if everything is set as we expect from the main doc file.

```shell
sudo vi /usr/local/ups/etc/hosts.conf
# MONITOR powerwalker@localhost "RaspberryPi"
sudo vi /usr/local/ups/etc/nut.conf
# MODE=netserver
sudo vi /usr/local/ups/etc/ups.conf
# See "UPS Config"
sudo vi /usr/local/ups/etc/upsd.conf
# LISTEN 0.0.0.0 3493
sudo vi /usr/local/ups/etc/upsd.users
# See "UPS User setup"
sudo vi /usr/local/ups/etc/upssched.conf
# See "UPS Scheduler conf"
# Maybe change: CMDSCRIPT /usr/local/ups/etc/upssched-cmd
# sudo chown -R root:nut /usr/local/ups/
# Maybe change: PIPEFN /var/state/ups/upssched/upssched.pipe
# Maybe change: LOCKFN /var/state/ups/upssched/upssched.lock
sudo vi /usr/local/ups/etc/upsmon.conf
# Check and change:
# NOTIFYCMD /usr/local/ups/sbin/upssched
sudo vi /usr/local/ups/etc/upsset.conf
# Uncomment: I_HAVE_SECURED_MY_CGI_DIRECTORY
```

## Extended Program file

Use one from [Extended Program file](NUT_UPS.md#extended-program-file)

And use all new paths for each bin in there:

```shell
sudo vi /usr/local/ups/etc/upssched-cmd
```

### The Program:


```sh
#!/bin/sh
UPS_USERNAME="admin"
UPS_PASSWORD="PASSWD"
UPS_LINK="powerwalker@localhost"
EMAIL="it@MAIL.COM"

# Variables:
set_variables () {
    DATUM=`/bin/date -R`
    DATE_LOG=`/bin/date +"%Y-%m-%d %H:%M:%S"`
    STAT=`/usr/local/ups/bin/upsc powerwalker@localhost ups.status`
    LOAD=`/usr/local/ups/bin/upsc powerwalker@localhost ups.load`
    BATT=`/usr/local/ups/bin/upsc powerwalker@localhost battery.charge`
    BATT_t=`/usr/local/ups/bin/upsc powerwalker@localhost battery.temperature`
    BATT_LOW=`/usr/local/ups/bin/upsc powerwalker@localhost battery.charge.low`
    BATT_WARN=`/usr/local/ups/bin/upsc powerwalker@localhost battery.charge.warning`
    RUNTIME=`/usr/local/ups/bin/upsc powerwalker@localhost battery.runtime`
    BEEPER=`/usr/local/ups/bin/upsc powerwalker@localhost ups.beeper.status`
    TESTRES=`/usr/local/ups/bin/upsc powerwalker@localhost ups.test.result`
    INP_FREQ=`/usr/local/ups/bin/upsc powerwalker@localhost input.frequency`
    INP_V=`/usr/local/ups/bin/upsc powerwalker@localhost input.voltage`
    INP_V_Nom=`/usr/local/ups/bin/upsc powerwalker@localhost input.voltage.nominal`
    OUT_FREQ=`/usr/local/ups/bin/upsc powerwalker@localhost output.frequency`
    OUT_V=`/usr/local/ups/bin/upsc powerwalker@localhost output.voltage`
    OUT_V_Nom=`/usr/local/ups/bin/upsc powerwalker@localhost output.voltage.nominal`
    # UPSLOG=`cat /var/log/messages | grep ups | tail -50`
    BASIC_INFO="
Date:                   $DATUM
Status:                 $STAT (OL online)
Charge level:           $BATT | Warning: $BATT_WARN Low: $BATT_LOW (shutdown at low!)
Runtime EST:            $RUNTIME
Notify type:            $NOTIFYTYPE

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
Sound signal:           $BEEPER | (enabled / disabled)
Battery temp:           $BATT_t
Battery low:            $BATT_LOW - set at /usr/local/ups/etc/ups.conf
Battery warn:           $BATT_WARN - set at /usr/local/ups/etc/ups.conf
Battery test state:     $TESTRES - at CRON
"
    LOG_LOCAL="$DATE_LOG -- Mail Status: [$STAT] | Charge: $BATT| Runtime: $RUNTIME sec | Load: $LOAD | t: $BATT_t | Input V: $INP_V Hz: $INP_FREQ"
}


update_local_log () {
    echo "$LOG_LOCAL" >> /var/log/ups/UPS_powerwalker.log
}

disable_beep () {
    # Disable BEEP
    # But my model is beeping even on disabling, better not touch it.
    # /usr/local/ups/bin/upscmd -u ${UPS_USERNAME} -p ${UPS_PASSWORD} ${UPS_LINK} beeper.toggle
    /usr/local/ups/bin/upscmd -u ${UPS_USERNAME} -p ${UPS_PASSWORD} ${UPS_LINK} beeper.disable >> /dev/null 2>&1
}

wake_on_lan () {
    # Wake media box device:
    sudo wakeonlan -i 192.168.1.9 -p 4343 1C:69:7A:AB:54:E6 >> /var/log/ups/UPS_powerwalker.log >> /dev/null 2>&1
}

disable_beep

# Programms
# /home/sanek/play_sound_notification.sh $volume $file
case $1 in
    onbatt)
        set_variables
        update_local_log
        message="Power outage, on battery"
        logger -t upssched-cmd "$message"
        # /bin/bash /home/sanek/play_sound_notification.sh 15 generated/UPS_no_power.wav
        echo "Subject: UPS ALERT: power is down!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    badbatt)
        set_variables
        update_local_log
        message="Bad battery, replace!"
        logger -t upssched-cmd "$message"
        # /bin/bash /home/sanek/play_sound_notification.sh 30 generated/UPS_battery_change.wav
        echo "Subject: UPS ALERT: bad battery!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    online)
        set_variables
        update_local_log
        message="Power is back!"
        logger -t upssched-cmd "$message"
        # /bin/bash /home/sanek/play_sound_notification.sh 30 generated/UPS_power_back.wav
        echo "Subject: UPS ALERT: power is back!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL
        wake_on_lan ;;
    commbad)
        set_variables
        update_local_log
        message="Communication is bad with UPS!"
        logger -t upssched-cmd "$message"
        echo "Subject: UPS ALERT: communications is down!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL
        /usr/local/ups/sbin/upsdrvctl stop
        /usr/local/ups/sbin/upsdrvctl -D start ;;
    commok)
        set_variables
        update_local_log
        message="Comminication with UPS restored!"
        logger -t upssched-cmd "$message"
        echo "Subject: UPS ALERT: communications restored!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    powerdown)
        set_variables
        update_local_log
        message="Shut down the system"
        logger -t upssched-cmd "$message"
        # /bin/bash /home/sanek/play_sound_notification.sh 30 generated/UPS_10_percent_will_poweroff.wav
        echo "Subject: UPS ALERT: system shutdown initiated!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    powerdownforced)
        set_variables
        update_local_log
        message="Shutdown the system FORCED"
        logger -t upssched-cmd "$message"
        # /bin/bash /home/sanek/play_sound_notification.sh 30 generated/UPS_poweroff.wav
        echo "Subject: UPS ALERT: forced system shutdown!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    servicedown)
        set_variables
        update_local_log
        message="UPS Service died!"
        logger -t upssched-cmd "$message"
        echo "Subject: UPS ALERT: service died, need restart!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    test_email)
        echo "Setting variables"
        set_variables
        echo "sending test email: $BASIC_INFO"
        #. /home/sanek/.bashrc && /home/sanek/play_sound_notification.sh 60 generated/UPS_no_power.wav 1
        echo "Subject: UPS ALERT: test email!\r\n\n$UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL ;;
    *)
        echo "wrong parameter" ;;
esac

exit 0


```

```shell
sudo chmod a+rwx /usr/local/ups/etc/upssched-cmd
sudo /usr/local/ups/etc/upssched-cmd test_email
```


## A New place for html:

Change here:

Old: `/usr/share/nut/www` new: `/usr/local/ups/html`

```shell
sudo vi /etc/lighttpd/conf-enabled/nut-ups.conf
# Rights for lighttpd: www-data oe nut?
sudo chown www-data:www-data -R /usr/local/ups/html
sudo systemctl restart lighttpd.service
# Check:
journalctl -n 100 -f | grep http
# lighttpd[545374]: /usr/local/ups/html/index.html: Permission denied
```


## New places for bin files:

```shell
# Bin:
/usr/local/ups/bin/NUT-Monitor
/usr/local/ups/bin/nut-scanner
/usr/local/ups/bin/usbhid-ups
/usr/local/ups/bin/powercom
# sbin
/usr/local/ups/sbin/upsdrvctl
# Runs
/usr/local/ups/bin/upssched-cmd
/usr/local/ups/bin/upslog
/usr/local/ups/bin/upscmd
/usr/local/ups/bin/upsc
```

## New places for cmds too:

```shell
# Commands:
/usr/local/ups/bin/upsc powerwalker@localhost
/usr/local/ups/bin/upsc powerwalker@localhost battery.charge.low
/usr/local/ups/bin/upsc powerwalker@localhost ups.beeper.status
/usr/local/ups/bin/upsc powerwalker@localhost test.battery.start.quick

# Test
/usr/local/ups/bin/upscmd -l powerwalker@localhost
/usr/local/ups/bin/upscmd -u admin -p passwd powerwalker@localhost beeper.toggle
/usr/local/ups/bin/upscmd -u admin -p passwd powerwalker@localhost beeper.enable
/usr/local/ups/bin/upscmd -u admin -p PASSWD powerwalker@localhost beeper.disable
/usr/local/ups/bin/upscmd -u admin -p PASSWD powerwalker@localhost battery.charge.low=10

# Show supported
/usr/local/ups/bin/upsrw -u admin -p passwd powerwalker@localhost
```

## Start restart:

You should not forget to use an extra long `make install` option from the step Make & Install

- https://networkupstools.org/docs/user-manual.chunked/_installation_instructions.html#Installing_inplace

```shell
sudo systemctl restart nut-driver-enumerator.service nut-monitor nut-server
```

## Test your script:

New place for your script same as conf files: `sudo vi /usr/local/ups/etc/upssched-cmd`

```shell
sudo vi /usr/local/ups/etc/upssched-cmd
# Test
sudo /usr/local/ups/etc/upssched-cmd onbatt
sudo /usr/local/ups/etc/upssched-cmd online
sudo /usr/local/ups/etc/upssched-cmd badbatt
sudo /usr/local/ups/etc/upssched-cmd test_email
```

## Other integrations

### RPI Monitor

Change path to bin and cmd:

- old: `upsc powerwalker@localhost battery.charge.low 2>&1 | grep -v '^Init SSL'`
- new `/usr/local/ups/bin/upsc powerwalker@localhost battery.charge.low`

# Outputs:

## On successful config:

```text
NUT Configuration summary:
==========================

* configured version:   2.8.1.1 (v2.8.1-962-g7b225f529)
* build serial drivers: yes
* build USB drivers:    yes (libusb-1.0)
* build neon based XML driver:  no
* enable Avahi support: no
* build Powerman PDU client driver:     no
* build Modbus driver:  no
* build IPMI driver:    no
* build GPIO driver:    no
* build Mac OS X meta-driver:   no
* build i2c based drivers:      no
* enable SSL support:   yes (OpenSSL)
* enable libwrap (tcp-wrappers) support:        no
* enable libltdl (Libtool dlopen abstraction) support:  yes
* build nut-scanner:    yes
* build CGI programs:   yes
* install NUT-Monitor desktop application:      yes
* install PyNUT binding module: yes
* use default  Python  interpreter:     /usr/bin/python
* use specific Python2 interpreter:
* use specific Python3 interpreter:     /usr/bin/python3.11
* build and install documentation:      man=auto
* build specific documentation format(s):       no
* build and install the development files:      no
* consider basic SMF support:   no
* consider basic systemd support:       yes
* build with tighter systemd support:   no
* build C++11 codebase (client library, etc.):  yes
* build C++ tests with CPPUNIT: no
* build and install the nutconf tool (experimental, may lack support for recent NUT options):   yes
* build SNMP drivers:   no
* build SNMP drivers with statically linked lib(net)snmp:       no
* User to run as:       nut
* Group of user to run as:      nut

NUT Paths:
----------

* Default installation prefix path:     /usr/local/ups
* State file path:      /var/state/ups
* Unprivileged PID file path:   /var/state/ups
* Privileged PID file path:     /run
* Driver program path:  /usr/local/ups/bin
* CGI program path:     /usr/local/ups/cgi-bin
* HTML file path:       /usr/local/ups/html
* Config file path:     /usr/local/ups/etc
* Data file path:       /usr/local/ups/share
* Tool program path:    /usr/local/ups/bin
* System program path:  /usr/local/ups/sbin
* System library path:  /usr/local/ups/lib
* System exec-library path:     /usr/local/ups/libexec

NUT Paths for third-party integrations:
---------------------------------------

* Default  Python  interpreter site-packages:   /usr/local/lib/python3.11/dist-packages
* Specific Python2 interpreter site-packages:
* Specific Python3 interpreter site-packages:   /usr/local/lib/python3.11/dist-packages
* pkg-config *.pc directory:    ${libdir}/pkgconfig => /usr/local/ups/lib/pkgconfig
* Service units for systemd:    /lib/systemd/system
* Shutdown hooks for systemd:   /lib/systemd/system-shutdown
* Systemd-tmpfiles configs:     /usr/lib/tmpfiles.d
* Augeas lenses directory:      /usr/share/augeas/lenses
* Udev rules directory: /lib/udev

NUT Build/Target system info:
-----------------------------

* Compact version of C compiler:        gcc (Debian 12.2.0-14) 12.2.0
* Compact version of C++ compiler:      g++ (Debian 12.2.0-14) 12.2.0
* Compact version of C preprocessor:    gcc (Debian 12.2.0-14) 12.2.0
* host env spec we run on:      aarch64-unknown-linux-gnu
* host env spec we built on:    aarch64-unknown-linux-gnu
* host env spec we built for:   aarch64-unknown-linux-gnu
* host OS short spec we run on: aarch64-linux-gnu
* host OS short spec we built on:       aarch64-linux-gnu
* host OS short spec we built for:      aarch64-linux-gnu
* host multiarch spec we build for (as suggested by compiler being used):       aarch64-linux-gnu
* ccache namespace tag (if ccache is used and new enough):      nut:aarch64-linux-gnu

NUT Compiler settings:
----------------------

* CC            : gcc
* CFLAGS        : -isystem /usr/local/include -g -O2 -Wno-reserved-identifier -Wno-unknown-warning-option -std=gnu99 -Wno-system-headers -Wall -Wextra -Wsign-compare -pedantic -Wno-error
* CXX           : g++
* CXXFLAGS      : -isystem /usr/local/include -g -O2 -Wno-reserved-identifier -Wno-unknown-warning-option -std=gnu++11 -Wno-system-headers -Wall -Wextra -Wno-error
* CPP           : gcc -E
* CPPFLAGS      :
* CONFIG_FLAGS  : --with-user=nut --with-group=nut --with-usb --with-cgi
```

## Errors

Check the logs:

```shell
journalctl -n 200 -f | grep nut
# Restart and check:
sudo systemctl restart nut-driver-enumerator.service nut-monitor nut-server
sudo systemctl status nut-driver-enumerator.service nut-monitor nut-server
```

Usually may have rights issues: `connect failed: Connection failure: Connection refused`

see logs: 
`nut-server[496034]: Can't open /usr/local/ups/etc/ups.conf: Can't open /usr/local/ups/etc/ups.conf: Permission denied`

Fix path

### bin path issues:

See logs: `nut-monitor[501291]: sh: 1: /sbin/upssched: not found`