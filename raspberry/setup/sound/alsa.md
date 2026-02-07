# ALSA

- https://techie-show.com/alsamixer-not-saving-settings/


Volume:

sudo alsamixer

Save forever
sudo alsactl store


sudo alsactl --file ~/.config/asound.state store
sudo vi ~/.bashrc
alsactl --file ~/.config/asound.state restore