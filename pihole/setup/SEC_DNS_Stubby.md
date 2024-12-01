# Stubby

- https://github.com/getdnsapi/stubby
- https://gist.github.com/Jiab77/1cdc2896f22791c4db492e87bbf609ff


```shell
sudo apt install stubby
# OR
sudo apt install unbound stubby
```


# Config:


```shell
vi /etc/stubby/stubby.yml
```

# Start

```shell
user@raspberrypi:~ $ sudo stubby
[14:58:02.878686] STUBBY: Stubby version: Stubby 0.3.0
[14:58:02.881099] STUBBY: Read config from file /etc/stubby/stubby.yml
[14:58:02.895084] STUBBY: DNSSEC Validation is OFF
[14:58:02.895095] STUBBY: Transport list is:
[14:58:02.895099] STUBBY:   - TLS
[14:58:02.895101] STUBBY: Privacy Usage Profile is Strict (Authentication required)
[14:58:02.895105] STUBBY: (NOTE a Strict Profile only applies when TLS is the ONLY transport!!)
[14:58:02.895108] STUBBY: Starting DAEMON....
```

# As service:

```shell
user@raspberrypi:~ $ sudo systemctl enable stubby
user@raspberrypi:~ $ sudo systemctl start stubby
```