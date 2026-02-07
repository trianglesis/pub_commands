

# Install

For HA

- https://github.com/hassio-addons/addon-nut

Check your device and see which driver to use:

- https://networkupstools.org/stable-hcl.html

  
List USB devices:

```shell
➜  / lsusb
Bus 003 Device 001: ID 1d6b:0002
Bus 001 Device 001: ID 1d6b:0002
Bus 001 Device 003: ID 0665:5161  #|-> this is upsN
Bus 004 Device 001: ID 1d6b:0003
Bus 002 Device 001: ID 1d6b:0003

#

Bus 003 Device 002: ID 0764:0601 1 1500
#  Bus 003 Device 002: ID 0764:0601 Cyber Power System, Inc. PR1500LCDRT2U UPS

```




# Update default conf:

Add IDs and different driver
- nutdrv_qx

Powerwalker

```yaml
users:
  - username: ha_ups
    password: NuT_UPS_HoM3_@$$
    instcmds:
      - all
    actions: []
devices:
  - name: PowerWalker
    driver: usbhid-ups
    port: auto
    config:
      - vendorid = 0764
      - productid = 0601
      - desc = "PowerWalker Basic VI 1500 STL"
mode: netserver
shutdown_host: false
log_level: info
list_usb_devices: true
```


# Check logs


```log
Poll UPS [myups@localhost] failed - Driver not connected
  15.513101	[D1] mainloop: UPS [myups] is now connected as FD -1
  17.515146	[D1] mainloop: UPS [myups] is not currently connected
  17.515254	[D1] mainloop: UPS [myups] is now connected as FD -1
  19.517299	[D1] mainloop: UPS [myups] is not currently connected
  19.517404	[D1] mainloop: UPS [myups] is now connected as FD -1
  20.513212	  20.005933	Poll UPS [myups@localhost] failed - Driver not connected
[D1] mainloop: UPS [myups] is not currently connected
  20.513376	[D1] mainloop: UPS [myups] is now connected as FD -1
```


# Connect at the integration to a hostname:

- See "Hostname" at the page of addon: "Network UPS Tools"

![note](image.png)