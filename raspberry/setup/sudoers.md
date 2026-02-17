# 

https://superuser.com/a/1725243

```shell
tideway ALL=(root) NOPASSWD: /usr/bin/systemctl restart snapserver
tideway ALL=(root) NOPASSWD: /usr/bin/systemctl restart snapclient
tideway ALL=(root) NOPASSWD: /usr/bin/systemctl restart snapclient_2
tideway ALL=(root) NOPASSWD: /usr/bin/systemctl restart snapclient_3
tideway ALL=(root) NOPASSWD: /usr/sbin/reboot
```

```shell
sudo systemctl restart snapserver
sudo systemctl restart snapclient snapclient_2 snapclient_3
sudo systemctl restart snapclient
sudo systemctl restart snapclient_2
sudo systemctl restart snapclient_3
sudo systemctl restart nut-driver-enumerator.service nut-monitor nut-server
sudo reboot now
```