# SSH

## Keys

```shell
sudo mkdir -p ~/.ssh/
sudo chmod 700 ~/.ssh/
sudo ssh-keygen -b 2048 -t rsa

# Keys always 600
sudo chmod 600 ~/.ssh/USER_rpi
sudo chmod 600 ~/.ssh/USER_rpi.pub

# 
sudo chmod 700 ~/.ssh && chmod 600 ~/.ssh/authorized_keys

```

Conf example:

```conf
Host github-trianglesis
        HostName github.com
        IdentityFile ~/.ssh/oleks-pc-gh
        IdentitiesOnly yes
        User git
```

Test

```shell
# General
ssh -vT git@github.com
# Host-specific
ssh -vT github-trianglesis
# Key-specific
ssh -vT git@github.com -i /home/user/.ssh/it\@trianglesis.org.ua
```
