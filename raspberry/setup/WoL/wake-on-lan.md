# Wake related devices from Raspberry

Doesn't work!

Read:
- https://pimylifeup.com/raspberry-pi-wake-on-lan-server/


# Install 

```shell
sudo apt install etherwake
```

# Use:

shell```
sudo etherwake MAC:ADDR
```

# Works fine:

```shell
sudo wakeonlan -i LOCAL_IP -p 4343 MAC:ADDR
```