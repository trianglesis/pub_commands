https://www.libe.net/en/openwrt-hass
https://openwrt.org/docs/guide-user/perf_and_log/statistic.collectd?s[]=way
https://community.home-assistant.io/t/collectd-plugin-openwrt-mqtt/445987

# Setup

Options:

```shell
# Or add directly to:
vi /etc/collectd.conf

# Change base dir to: ??
/etc/collectd/

# Directory for sub-configurations
/etc/collectd/conf.d/*.conf
ls -lah /etc/collectd/conf.d/
mkdir -p /etc/collectd/conf.d/
# Separate file?
vi /etc/collectd/conf.d/mqtt.conf
cat /etc/collectd/conf.d/mqtt.conf
```

Conf directly into conf file

```conf
# collectd-mod-mqtt
LoadPlugin mqtt
<Plugin "mqtt">
  <Publish "OpenWRT">
    Host "192.168.1.15"
    Port "1883"
    User "USER"
    Password "PASS"
    ClientId "OpenWRT"
    Prefix "collectd"
    Retain true
  </Publish>
</Plugin>
```

## Errors

```log
Wed Sep  2 18:43:05 2026 daemon.err collectd[8316]: Parse error in file `/etc/collectd/conf.d/mqtt.conf', line 71 near `<newline>': block not closed

```

From doc

```shell

```

Check

```shell
ls -lah /overlay/RRD_Logs/OpenWrt/cpu-0/
ls -lah /overlay/RRD_Logs/OpenWrt/uptime/

# Delete old
rm -rf /overlay/RRD_Logs/OpenWrt/*

/etc/init.d/luci_statistics restart
/etc/init.d/collectd restart
/etc/init.d/rpcd restart

/etc/init.d/collectd status

# Check logs
logread -f | grep collectd
```

## start over

```shell
collectd-mod-dhcpleases
# All install
opkg install collectd collectd-mod-rrdtool collectd-mod-iptables collectd-mod-netlink collectd-mod-rrdtool collectd-mod-iptables collectd-mod-netlink luci-app-statistics collectd-mod-ping collectd-mod-mqtt

# All install refresh
opkg install collectd collectd-mod-rrdtool collectd-mod-iptables collectd-mod-netlink collectd-mod-rrdtool collectd-mod-iptables collectd-mod-netlink luci-app-statistics collectd-mod-ping collectd-mod-mqtt --force-maintainer --force-reinstall --force-overwrite

opkg install collectd-mod-rrdtool --force-maintainer --force-reinstall --force-overwrite
opkg install collectd-mod-iptables --force-maintainer --force-reinstall --force-overwrite
opkg install collectd-mod-netlink --force-maintainer --force-reinstall --force-overwrite
opkg install collectd-mod-wireless --force-maintainer --force-reinstall --force-overwrite

# ???
opkg install collectd-mod-sensors --force-maintainer --force-reinstall --force-overwrite
```