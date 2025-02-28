# Dev Tools
Pre-Requisits:
```shell
sudo yum groupinstall "Development Tools"
sudo yum install openssl-devel libffi-devel perl-IPC-Cmd perl-FindBin libbz2-dev -y

# Extras
sudo dnf install -y zlib-devel
sudo dnf install -y readline-devel
sudo dnf install -y libuuid-devel
sudo dnf install -y lzip.x86_64
sudo dnf install -y sqlite-devel
sudo dnf install -y bzip2-devel
sudo dnf install -y gdbm-libs
sudo dnf install -y ncurses-devel
sudo dnf install -y tk-devel
sudo dnf install -y python3-tkinter
sudo dnf install -y gdbm-devel
```

- https://docs.python.org/3.10/using/unix.html
- https://docs.python.org/3.13/using/unix.html#building-python
- https://docs.python.org/3.13/using/configure.html#configure-options
- https://www.workaround.cz/howto-build-compile-install-latest-python-310-39-38-37-centos-7-8-9/

# Open SSL

- https://docs.python.org/3.13/using/unix.html#custom-openssl

```text
https://www.openssl.org/source/openssl-1.1.1v.tar.gz
https://www.openssl.org/source/openssl-3.0.10.tar.gz
https://github.com/openssl/openssl/releases/download/openssl-3.4.1/openssl-3.4.1.tar.gz

# System ver
https://github.com/openssl/openssl/releases/download/openssl-3.0.16/openssl-3.0.16.tar.gz
```

```shell
wget https://github.com/openssl/openssl/releases/download/openssl-3.4.1/openssl-3.4.1.tar.gz
tar xzf openssl-3.4.1.tar.gz
pushd openssl-3.4.1
./config enable-md2 --prefix=/usr/local --libdir=lib --openssldir=/etc/pki/tls
make -j1 depend
make -j8
make install_sw
popd
```

### Start over

```shell
# Start over
make clean && make distclean
```

### Optionally:

```shell
sudo ldconfig /usr/local/lib64/
sudo ldconfig
openssl version -a
```


##### ERROR
- (you may need to install the FindBin module)

`dnf install perl-FindBin`


# Python

## Select python sources:
- curl -O https://www.python.org/ftp/python/3.13.2/Python-3.13.2.tgz
  
### Older:
- curl -O https://www.python.org/ftp/python/3.10.5/Python-3.10.5.tgz
- curl -O https://www.python.org/ftp/python/3.11.0/Python-3.11.0b4.tgz

### Best now
- curl -O https://www.python.org/ftp/python/3.11.5/Python-3.11.5.tgz

##### Install for Oracle Linux 9
tar -xzf Python-3.13.2.tgz
cd Python-3.13.2

## Install

#### EASY

```shell
./configure --enable-optimizations --with-lto --with-openssl=/usr/local/bin/custom-openssl LDFLAGS="-Wl,-rpath /usr/local"
```

#### PRO

```shell
./configure --prefix=/usr/local \
            --enable-optimizations \
            --with-lto \
            --enable-shared \
            --with-pydebug \
            --enable-loadable-sqlite-extensions \
            --with-openssl-rpath=auto LDFLAGS="-Wl,-rpath /usr/local/lib"
```

- old

```shell
./configure --prefix=/usr/local \
    --enable-shared \
    --enable-optimizations \
    --enable-loadable-sqlite-extensions \
    --with-lto \
    --without-pymalloc \
    --without-doc-strings \
    --with-openssl=/usr/local/bin/custom-openssl \
    --with-openssl-rpath=auto LDFLAGS="-Wl,-rpath /usr/local"
```

#### Optional

```shell
#  --disable-test-modules \
#  --with-system-ffi \
#  --with-computed-gotos \
#  LDFLAGS="-Wl,-rpath /usr/lib"
```

## Install ALT!


```shell
make -j "$(nproc)"
make altinstall
```

### Start over

```shell
# Start over
make clean && make distclean
```

# Finish:

```shell
whereis python3.11
python3.11: /usr/local/bin/python3.11 /usr/local/lib/python3.11

whereis python3.13
python3.13: /usr/local/bin/python3.13 /usr/local/lib/python3.13
```

# ERRORS

## Missing modules

Find by dnf

```shell
The necessary bits to build these optional modules were not found:
_bz2                      _curses                   _curses_panel
_dbm                      _gdbm                     _tkinter
_uuid                     readline
```


## ibpython3.11.so.1.0

- Probable fix
- error while loading shared libraries: libpython3.11.so.1.0: cannot open shared object file: No such file or directory
ldconfig /usr/local/lib/

```shell
vi /etc/ld.so.conf.d/python310.conf
include /usr/local/lib/
```

## LD_CONFIG 
- NOT REQUIRED IF you don't change sys python

## Linking
```shell
    ln -s /usr/local/bin/python3.11 /usr/bin/python3.11 && \
    ln -s /usr/local/bin/python3.11-config /usr/bin/python3.11-config  && \
    ln -s /usr/local/bin/pydoc3.11 /usr/bin/pydoc3.11  && \
    ln -s /usr/local/bin/idle3.11 /usr/bin/idle3.11  && \
    ln -s /usr/local/bin/pip3.11 /usr/bin/pip3.11
```

## ldconfig
```shell
    /usr/local/lib/libpython3.11.so.1.0
    /usr/local/lib/libpython3.11.so
    /usr/local/lib/libpython3.so
```

## Only for info 

- Run and collect system path to imitate them for new python 3.11
whereis python3

- maybe add:
```shell
export CPATH=/usr/local/include && export LIBRARY_PATH=/usr/local/lib && export LD_LIBRARY_PATH=/usr/local/lib
export LIBRARY_PATH=LIBRARY_PATH:/usr/local/lib && export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:/usr/local/lib
```

## REPEATEDLY failing with python shared libs:

// https://stackoverflow.com/a/43623903
P.S. I came across a similar problem while using virtualenv with python3.6, and I fixed it like so:

First, append include <lib path of python3.x> to /etc/ld.so.conf
(Something like: include /opt/python361/lib or include /usr/local/lib)

Then, activate the new configuration by running
sudo /sbin/ldconfig -v

```shell
rm -rf /usr/local/bin/2to3-3.11 && rm -rf /usr/local/bin/idle3.11 && rm -rf /usr/local/bin/pip3.11 && rm -rf /usr/local/bin/pydoc3.11 && rm -rf /usr/local/bin/python3.11 && rm -rf /usr/local/bin/python3.11-config
rm -rf /usr/local/lib/libpython3.11.so -> libpython3.11.so.1.0 && rm -rf /usr/local/lib/libpython3.11.so.1.0 && rm -rf /usr/local/lib/python3.11
```
