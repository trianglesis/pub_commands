# SSH

Change setting at RPi

```shell
sudo vi /etc/ssh/sshd_config
# Permit pubkey and other options needed

sudo systemctl restart sshd

# Add hosts if needed
sudo vi ~/.ssh/config
```

## Generate Keys

Gen the key, add to known hosts of other host

## At RPi

```shell
sudo mkdir -p ~/.ssh/
sudo chmod 700 ~/.ssh/
sudo ssh-keygen -b 2048 -t rsa

sudo cp ~/USER_rpi ~/.ssh/USER_rpi
sudo cp ~/USER_rpi.pub ~/.ssh/USER_rpi.pub
sudo cp ~/USER_rpi /root/.ssh/USER_rpi
sudo cp ~/USER_rpi.pub /root/.ssh/USER_rpi.pub

sudo chown USER:USER ~/.ssh/USER_rpi
sudo chown USER:USER ~/.ssh/USER_rpi.pub
sudo chmod 600 ~/.ssh/USER_rpi
sudo chmod 600 ~/.ssh/USER_rpi.pub
sudo chmod 700 ~/.ssh && chmod 600 ~/.ssh/authorized_keys

cat ~/.ssh/USER_rpi.pub
# Copy content to another host as key in 

# OR USE util: copy from RPi to HA
sudo ssh-copy-id -i ~/.ssh/USER_rpi.pub USER@1.1.1.15
# Test
ssh -i /home/USER/.ssh/USER_rpi 'root@1.1.1.15' 'ls -lah'
```

## From HA to RPI

NOTE: HA wants `id_rsa` file name for a private key at `/config/.ssh` no matter what, so rename the key or generate as it is.

```shell
# Gen the key same as in RPi
cd /config/.ssh
sudo ssh-keygen -b 2048 -t rsa

# Copy HA key into RPi
# use CONFIG's known hosts file, not root!
ssh-copy-id -o UserKnownHostsFile=/config/.ssh/known_hosts -i /config/.ssh/USER_ha.pub USER@1.1.1.11
# Test
ssh -o UserKnownHostsFile=/config/.ssh/known_hosts -i /config/.ssh/USER_ha USER@1.1.1.11 'ls -lah'

# Permissions 0644 for '/config/.ssh/USER_ha.pub' are too open.
chmod 700 /config/.ssh && chmod 600 /config/.ssh/authorized_keys
sudo chmod 600 /config/.ssh/USER_ha
sudo chmod 600 /config/.ssh/USER_ha.pub
```


### From RPi to HA

```log
sudo ssh-copy-id -i ~/.ssh/USER_rpi.pub root@1.1.1.15
/usr/bin/ssh-copy-id: INFO: Source of key(s) to be installed: "/home/USER/.ssh/USER_rpi.pub"
/usr/bin/ssh-copy-id: INFO: attempting to log in with the new key(s), to filter out any that are already installed
/usr/bin/ssh-copy-id: INFO: 1 key(s) remain to be installed -- if you are prompted now it is to install the new keys
root@1.1.1.15's password:

Number of key(s) added: 1

Now try logging into the machine, with: "ssh -i /home/USER/.ssh/USER_rpi 'root@1.1.1.15'"
and check to make sure that only the key(s) you wanted were added.

```
