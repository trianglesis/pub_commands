

# Disable wireless

- https://raspberrytips.com/disable-wifi-raspberry-pi/

```shell
sudo rfkill block wifi
sudo rfkill block bluetooth


sudo vi /boot/firmware/config.txt
dtoverlay=disable-wifi
dtoverlay=disable-bt


sudo systemctl disable wpa_supplicant
# Removed '/etc/systemd/system/dbus-fi.w1.wpa_supplicant1.service'.
# Removed '/etc/systemd/system/multi-user.target.wants/wpa_supplicant.service'.
sudo systemctl disable bluetooth
# Synchronizing state of bluetooth.service with SysV service script with /usr/lib/systemd/systemd-sysv-install.
# Executing: /usr/lib/systemd/systemd-sysv-install disable bluetooth
# Removed '/etc/systemd/system/dbus-org.bluez.service'.
# Removed '/etc/systemd/system/bluetooth.target.wants/bluetooth.service'.
sudo systemctl disable hciuart

```