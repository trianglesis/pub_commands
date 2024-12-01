# WSGI MOD
We should have HTTPD installed!
`
pip3 install mod_wsgi
`

# VENV
- https://virtualenv.pypa.io/en/latest/reference/#virtualenv-command
`
pip3 install virtualenv
`
`
virtualenv --python=/usr/local/bin/python3 core
`

# OR (from added symlink)

```
cd /var/www/octopus
virtualenv --python=/usr/bin/python3 venv --system-site-packages
virtualenv --python=/usr/local/bin/python3.10 venv --system-site-packages
source venv/bin/activate
source venv/bin/deactivate
```

# OR:0
```
cd /var/www/triangle/ && source core/bin/activate
deactivate

pip install -r "" --ignore-installed

pip install mod_wsgi
pip install -r "pip-update.txt" --ignore-installed
```

# WSL
```
cd /mnt/g/Projects/lobster/
virtualenv --python=/usr/bin/python3.10 venv --system-site-packages
virtualenv --python=/usr/local/bin/python3.10 venv --system-site-packages

virtualenv --python=C:\Python310\python.exe win_venv --system-site-packages
pip install -r "" --ignore-installed
```