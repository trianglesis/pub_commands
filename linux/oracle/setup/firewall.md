# firewall

- https://docs.oracle.com/en/operating-systems/oracle-linux/9/firewall/firewall-ConfiguringaPacketFilteringFirewall.html

```
systemctl status firewalld
systemctl reload firewalld
```


## Allow usual ports:

### Web

```
firewall-cmd --permanent --zone=public --add-port=80/tcp
firewall-cmd --permanent --zone=public --add-port=8000/tcp
```


### RabbitMQ

```
firewall-cmd --permanent --zone=public --add-port=5672/tcp
firewall-cmd --permanent --zone=public --add-port=15672/tcp
```

### MySQL

```
firewall-cmd --permanent --zone=public --add-port=3306/tcp
```


# Show active:
firewall-cmd --list-all --zone=public

# Truested:
firewall-cmd --zone=trusted --list-all