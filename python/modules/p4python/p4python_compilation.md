# p4 PYTHON NEW

- https://github.com/perforce/p4python/archive/refs/tags/2022.1.0.tar.gz
- http://ftp.perforce.com/perforce/r22.1/

## Usually not working
`wget http://ftp.perforce.com/perforce/r22.1/bin.linux26x86_64/p4python-3.10-x86_64-whl.zip`

# Compile
```
wget ftp://ftp.perforce.com/perforce/r22.1/bin.tools/p4python.tgz
wget ftp://ftp.perforce.com/perforce/r22.1/bin.linux26x86_64/p4api.tgz
```
## Untar
```
tar zxvf p4python.tgz
tar zxvf p4api.tgz
```
## GO
```
cd p4python-2022.1.2299330/
```

## Add
```
python3.10 setup.py build --apidir ../p4api-2022.1.2344699/
python3.10 setup.py install --apidir ../p4api-2022.1.2344699/
```

## Issues
- IF: error: option --apidir not recognized

python3.10 setup.py install

Installed /var/www/lobster/venv/lib/python3.10/site-packages/p4python-2022.1.2299330-py3.10-linux-x86_64.egg
Processing dependencies for p4python==2022.1.2299330
Finished processing dependencies for p4python==2022.1.2299330


