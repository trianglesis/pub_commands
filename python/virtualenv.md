# Install venv

Choose required python:
- system default
- compiled 


```shell
cd /var/www/lobster
virtualenv --python=/usr/local/bin/python3.11 venv --system-site-packages
source venv/bin/activate
source venv/bin/deactivate


virtualenv --python=/usr/local/bin/python3.13 venv --system-site-packages
```

## Debug

```shell
# https://stackoverflow.com/questions/22931774/how-to-use-gdb-python-debugging-extension-inside-virtualenv
yum install python3-dbg
python3-dbg -m venv ./env-name
```