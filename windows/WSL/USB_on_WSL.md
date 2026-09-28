# USB

Passthrough USB from Win host to WSL

## DOC

- <https://gist.github.com/0xcafed00d/f25d042c33f3cac142b650985c12fd0b>

## Gerenal info!

You can use ESPHome WEB started from WSL at localhost (use Chrome) and it will list all your USB devices!

```shell
# project path: /home/user/projects/ESPhome
source /home/user/projects/ESPhome/venv/bin/activate
esphome dashboard --port=8080 --address=127.0.0.1 my_proj/
```

### Config

At WIN CMD:

```shell
winget install --interactive --exact dorssel.usbipd-win

# See output below
usbipd list

# Pass the device ADMIN CMD
usbipd bind --busid 12-2

# Replace 12-2 with your actual bus ID.
# You only need to do this once per device in most cases. The sharing state is persistent across reboots.
# You can verify the device is shared:

usbipd list

# Now open a regular, non-admin PowerShell window and run:
usbipd attach --wsl --busid 12-2

# Check in WSL
lsusb
# Bus 001 Device 001: ID 1d6b:0002 Linux Foundation 2.0 root hub
# Bus 001 Device 002: ID 303a:1001 Espressif USB JTAG/serial debug unit
# Bus 002 Device 001: ID 1d6b:0003 Linux Foundation 3.0 root hub

# From PowerShell, detach the device:
usbipd detach --busid 12-2
```

```text
Connected:
BUSID  VID:PID    DEVICE                                                        STATE
2-2    041e:3278  USB Input Device, USB Serial Device (COM3), Sound Blaster X4  Not shared
2-7    8087:0a2b  Intel(R) Wireless Bluetooth(R)                                Not shared
12-2   303a:1001  USB Serial Device (COM4), USB JTAG/serial debug unit          Not shared
17-3   1532:026c  USB Input Device, Razer Huntsman V2                           Not shared
17-4   046d:c52b  Logitech USB Input Device, USB Input Device                   Not shared
18-2   1462:3fa4  USB Input Device                                              Not shared
18-4   046d:094c  Brio 100                                                      Not shared

Persisted:
GUID                                  DEVICE
4c7eac3d-67c2-460a-a00b-1390e37a4d20  USB Serial Device (COM8), USB JTAG/serial debug unit
```

### Example

```shell
usbipd bind --busid 12-2

usbipd list
Connected:
BUSID  VID:PID    DEVICE                                                        STATE
2-2    041e:3278  USB Input Device, USB Serial Device (COM3), Sound Blaster X4  Not shared
2-7    8087:0a2b  Intel(R) Wireless Bluetooth(R)                                Not shared
12-2   303a:1001  USB Serial Device (COM4), USB JTAG/serial debug unit          Shared
17-3   1532:026c  USB Input Device, Razer Huntsman V2                           Not shared
17-4   046d:c52b  Logitech USB Input Device, USB Input Device                   Not shared
18-2   1462:3fa4  USB Input Device                                              Not shared
18-4   046d:094c  Brio 100                                                      Not shared

Persisted:
GUID                                  DEVICE
4c7eac3d-67c2-460a-a00b-1390e37a4d20  USB Serial Device (COM8), USB JTAG/serial debug unit

```