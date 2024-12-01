# RabbitMQ

## primary RabbitMQ signing key
rpm --import 'https://github.com/rabbitmq/signing-keys/releases/download/3.0/rabbitmq-release-signing-key.asc'
## modern Erlang repository
rpm --import 'https://github.com/rabbitmq/signing-keys/releases/download/3.0/cloudsmith.rabbitmq-erlang.E495BB49CC4BBE5B.key'
## RabbitMQ server repository
rpm --import 'https://github.com/rabbitmq/signing-keys/releases/download/3.0/cloudsmith.rabbitmq-server.9F4587F226208342.key'


# Install:

- https://www.rabbitmq.com/install-rpm.html

Red Hat 9, CentOS Stream 9, Rocky Linux 9, Alma Linux 9, Modern Fedora Releases

## Repos:

```
vi /etc/yum.repos.d/rabbitmq.repo
```



```
dnf update -y

dnf install socat logrotate -y

dnf install -y erlang rabbitmq-server
```

# Setup:

```
rabbitmqctl add_vhost tentacle
rabbitmqctl add_user octo_user (PASSWORD)
rabbitmqctl set_user_tags octo_user administrator
rabbitmqctl set_permissions -p tentacle octo_user ".*" ".*" ".*"
```

# Start
```
chkconfig rabbitmq-server on
systemctl start rabbitmq-server
systemctl restart rabbitmq-server
```

# Stop
```
rabbitmqctl stop
rabbitmq-server start
```

# Management

```
rabbitmq-plugins enable rabbitmq_management
```


# DELETE\CHANGE
rabbitmqctl delete_vhost localhost
rabbitmqctl delete_vhost /
rabbitmqctl change_password octo_user (PASSWORD)

tail -f -n 100 /var/log/rabbitmq/rabbit@HOSTNAME.log
rabbitmq-server
rabbitmqctl stop

rabbitmqctl start_app

# RESET EVERYTHING: Start from clean:

rabbitmqctl stop_app
rabbitmqctl reset
rabbitmqctl start_app
