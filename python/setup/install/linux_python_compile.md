# Old issues:
https://www.workaround.cz/howto-build-compile-install-latest-python-310-39-38-37-centos-7-8/
http://www.trianglesis.org.ua/centos-python-installation-guide-with-no-pain

# Pre-Requisits:
yum groupinstall "Development Tools"
yum install openssl-devel
yum install libffi-devel


# Build:
https://docs.python.org/3/using/unix.html#building-python
##Config:

https://docs.python.org/3/using/configure.html
```
curl -O https://www.python.org/ftp/python/3.11.5/Python-3.11.5.tgz
tar -xzf Python-3.11.5.tgz
cd Python-3.11.5/
```

### GA
```
curl -O https://www.python.org/ftp/python/3.11.0/Python-3.11.0a1.tgz
tar -xzf Python-3.11.0a1.tgz
cd Python-3.11.0a1
```
### RELEASE
```
curl -O https://www.python.org/ftp/python/3.10.7/Python-3.10.7.tgz
tar -xzf Python-3.10.7.tgz
cd Python-3.10.7
```

## Usual

`./configure --enable-optimizations --enable-shared`

- OR

`CFLAGS="-I/usr/local/include" LDFLAGS="-L/usr/local/lib"`

- OR

`./configure --prefix=/usr/local --enable-optimizations --enable-shared LDFLAGS="-Wl,-rpath /usr/local/lib"`

- WSL
`./configure --prefix=/usr/local --enable-optimizations --enable-shared LDFLAGS="-Wl,-rpath /usr/local/lib"`

### ALT!

`make altinstall`


# ENV
```
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/lib/
```
## PIP3
```
ln -s /usr/local/bin/python3.10 /usr/bin/python3
ln -s /usr/local/bin/pip3.10 /usr/bin/pip3
```
## Virtual Env
```
pip3 install virtualenv
```

# IF issues

1 - Can't connect to HTTPS URL because the SSL module is not available
yum -y install bzip2-devel libffi-devel openssl-devel openssl

# Install OPEN SSL
```
yum install perl-IPC-Cmd

curl -O https://www.openssl.org/source/openssl-3.0.0.tar.gz
tar xzf openssl-3.0.0.tar.gz
pushd openssl-3.0.0
./config --prefix=/usr/local/bin/custom-openssl --libdir=lib --openssldir=/etc/pki/tls
make -j1 depend
make -j8
make install_sw
popd
```

# Install python with SSL Option
```
pushd Python-3.11.5
./configure CFLAGS="-I/usr/local/include" LDFLAGS="-L/usr/local/lib" -C --enable-optimizations --with-lto --enable-shared --with-openssl=/usr/local/bin/custom-openssl --with-openssl-rpath=auto --prefix=/usr/local
make -j8
make altinstall
```

# Something extra
```
/usr/local/lib/python3.10
/usr/local/bin/python3.10 -m pip install --upgrade pip
```

# Open ssl
```
pushd Python-3.11.5
./configure -C --enable-optimizations --with-lto --enable-shared --with-openssl=/usr/local/bin/custom-openssl --with-openssl-rpath=auto --prefix=/usr/local
make -j8
make altinstall
```

# maybe add:
```
export CPATH=/usr/local/include
export LIBRARY_PATH=/usr/local/lib
export LD_LIBRARY_PATH=/usr/local/lib
```

# Probable fix
# error while loading shared libraries: libpython3.10.so.1.0: cannot open shared object file: No such file or directory

`ldconfig /usr/local/lib`

# REPEATEDLY failing with python shared libs:
// https://stackoverflow.com/a/43623903
P.S. I came across a similar problem while using virtualenv with python3.6, and I fixed it like so:

First, append include <lib path of python3.x> to /etc/ld.so.conf (Something like: include /opt/python361/lib or include /usr/local/lib)
Then, activate the new configuration by running sudo /sbin/ldconfig -v.