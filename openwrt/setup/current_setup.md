
# Main

root@OpenWrt:~# cat /etc/opkg.conf
dest root /
dest ram /tmp
lists_dir ext  /usr/lib/opkg/lists
option overlay_root /overlay
option check_signature


root@OpenWrt:~#  grep -e /overlay /etc/mtab
/dev/sda1 /overlay ext4 rw,relatime 0 0
overlayfs:/overlay / overlay rw,noatime,lowerdir=/,upperdir=/overlay/upper,workdir=/overlay/work 0 0

root@OpenWrt:~# df /overlay /
Filesystem           1K-blocks      Used Available Use% Mounted on
/dev/sda1              7346668     12076   6939996   0% /overlay
overlayfs:/overlay     7346668     12076   6939996   0% /


root@OpenWrt:~# ls -l /sys/block
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock0 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd0/mtdblock0
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock1 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtdblock1
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock2 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtd2/mtdblock2
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock3 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtd3/mtdblock3
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock4 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd1/mtd4/mtdblock4
lrwxrwxrwx    1 root     root             0 Jun  9 13:59 mtdblock5 -> ../devices/platform/ahb/1f000000.spi/spi_master/spi0/spi0.0/mtd/mtd5/mtdblock5
lrwxrwxrwx    1 root     root             0 Jun  7 16:04 sda -> ../devices/platform/ahb/1b000000.usb/usb1/1-1/1-1.1/1-1.1:1.0/host0/target0:0:0/0:0:0:0/block/sda

WEB UI:

![UI Mounts](openwrt/setup/PICs/UI_Mounts.png)


## Speed test


## Defauilt

- <https://iperf3serverlist.net/>
- <https://community.home-assistant.io/t/how-to-setup-iperf-integration/310575/12>

```shell
iperf3 -c speed.cosmonova.net -p 5201-5209
iperf3 -c 192.168.1.11 -p 5201-5209
iperf3 -c 192.168.1.7 -p 5201-5209

# As daemon
iperf3 --bind=192.168.1.11 --port=5201 --format=M --server
iperf3 --bind=192.168.1.7 --port=5201 --format=M --affinity=0 --idle-timeout=1800 --server --daemon
```

```shell
cat > /etc/init.d/iperf3 << EOL
#!/bin/sh /etc/rc.common
START=99
STOP=10
start() {
    iperf3 --bind=192.168.1.7 --port=5201 --format=M --affinity=0 --idle-timeout=1800 --server --daemon &
}
stop() {
    killall iperf3
}
EOL
```

Check

```shell
cat /etc/init.d/iperf3
chmod +x /etc/init.d/iperf3
/etc/init.d/iperf3 enable
```


### Extra

- <https://forum.openwrt.org/t/speedtest-new-package-to-measure-network-performance/24647>


```shell
speedtest-netperf.sh -H speed.cosmonova.net -p 1.1.1.1 --time 30 --number 2 --sequential
speedtest-netperf.sh -H speed.cosmonova.net -p 1.1.1.1 --time 30 --number 2 --concurrent


speedtest-netperf.sh [-4 | -6] [-H netperf-server] [-t duration] [-p host-to-ping] [-n simultaneous-streams ] [-s | -c]

Options, if present, are:

-4 | -6:           Enable ipv4 or ipv6 testing (default - ipv4)
-H | --host:       DNS or Address of a netperf server (default - netperf.bufferbloat.net)  
                   Alternate servers are netperf-east (US, east coast),
                   netperf-west (US, California), and netperf-eu (Denmark).
-t | --time:       Duration for how long each direction's test should run - (default - 60 seconds)
-p | --ping:       Host to ping to measure latency (default - gstatic.com)
-n | --number:     Number of simultaneous sessions (default - 5 sessions)
-s | --sequential: Sequential download/upload (default - sequential)
-c | --concurrent: Concurrent download/upload
```

## Tests

- <https://openwrt.org/docs/guide-user/network/network_configuration>

install ethtool

```shell

swconfig list
Found: switch0 - mdio.0

ubus list network.interface.*
network.interface.WAN_Tethering
network.interface.guest
network.interface.lan
network.interface.loopback
network.interface.wan

ifstatus lan

ethtool eth0
ethtool -i eth0.1
ethtool -i wlan0
ethtool -i wlan1
ethtool -i br-lan

hwinfo --network --short
```

Output

```txt
Settings for eth0:
        Supported ports: [ TP    MII ]
        Supported link modes:   10baseT/Half 10baseT/Full
                                100baseT/Half 100baseT/Full
                                1000baseT/Full
        Supported pause frame use: Symmetric Receive-only
        Supports auto-negotiation: Yes
        Supported FEC modes: Not reported
        Advertised link modes:  10baseT/Half 10baseT/Full
                                100baseT/Half 100baseT/Full
                                1000baseT/Full
        Advertised pause frame use: Symmetric Receive-only
        Advertised auto-negotiation: Yes
        Advertised FEC modes: Not reported
        Speed: 1000Mb/s
        Duplex: Full
        Auto-negotiation: on
        Port: Twisted Pair
        PHYAD: 0
        Transceiver: external
        MDI-X: Unknown
        Current message level: 0x000000ff (255)
                               drv probe link timer ifdown ifup rx_err tx_err
        Link detected: yes

root@OpenWrt:~# ethtool -i eth0.1
driver: 802.1Q VLAN Support
version: 1.8
firmware-version: N/A
expansion-rom-version:
bus-info:
supports-statistics: no
supports-test: no
supports-eeprom-access: no
supports-register-dump: no
supports-priv-flags: no

root@OpenWrt:~# ethtool -i eth0.2
driver: 802.1Q VLAN Support
version: 1.8
firmware-version: N/A
expansion-rom-version:
bus-info:
supports-statistics: no
supports-test: no
supports-eeprom-access: no
supports-register-dump: no
supports-priv-flags: no


 hwinfo --network --short
network interface:
  eth0.2               Ethernet network interface
  wlan0                WLAN network interface
  lo                   Loopback network interface
  wlan0-2              WLAN network interface
  eth0                 Ethernet network interface
  wlan1                WLAN network interface
  eth0.1               Ethernet network interface
  wlan0-1              WLAN network interface
  br-guest             Ethernet network interface
  br-lan               Ethernet network interface

root@OpenWrt:~# ethtool -i wlan0
driver: ath9k
version: 5.10.146
firmware-version: N/A
expansion-rom-version:
bus-info: 18100000.wmac
supports-statistics: yes
supports-test: no
supports-eeprom-access: no
supports-register-dump: no
supports-priv-flags: no

ethtool -i br-lan
driver: bridge
version: 2.3
firmware-version: N/A
expansion-rom-version:
bus-info: N/A
supports-statistics: no
supports-test: no
supports-eeprom-access: no
supports-register-dump: no
supports-priv-flags: no

```