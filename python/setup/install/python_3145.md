# Install Python 3.14.5+

- <https://docs.python.org/3/using/configure.html>

## Dev Tools

Pre-Requisits:

```shell
sudo yum groupinstall "Development Tools"
# For open ssl too
sudo yum install openssl-devel libffi-devel perl-IPC-Cmd perl-FindBin -y
# ???
libbz2-dev

# Extras
sudo dnf install perl-Time-Piece -y
sudo dnf install perl-FindBin -y
sudo dnf install zlib-devel -y
sudo dnf install readline-devel -y
sudo dnf install libuuid-devel -y
sudo dnf install lzip.x86_64 -y
sudo dnf install sqlite-devel -y
sudo dnf install bzip2-devel -y
sudo dnf install gdbm-libs -y
sudo dnf install ncurses-devel -y
sudo dnf install tk-devel -y
sudo dnf install python3-tkinter -y
sudo dnf install gdbm-devel -y
```

## Optional custom SSL

- <https://openssl-library.org/source/>
- <https://github.com/openssl/openssl/releases/download/openssl-3.5.6/openssl-3.5.6.tar.gz>

```shell
mkdir -p OPENSSL_INSTALL && cd OPENSSL_INSTALL
wget https://github.com/openssl/openssl/releases/download/openssl-3.5.6/openssl-3.5.6.tar.gz
#
# Unpack
#
tar xzf openssl-3.5.6.tar.gz
cd openssl-3.5.6
#
#
./config enable-md2 --prefix=/usr/local --libdir=lib --openssldir=/etc/pki/tls
make -j1 depend
make -j8
sudo make install_sw
#
# Start over
make clean && make distclean
```

### Result

```log
*** Installing engines
install engines/afalg.so -> /usr/local/lib/engines-3/afalg.so
install engines/capi.so -> /usr/local/lib/engines-3/capi.so
install engines/loader_attic.so -> /usr/local/lib/engines-3/loader_attic.so
install engines/padlock.so -> /usr/local/lib/engines-3/padlock.so
created directory `/usr/local/lib/ossl-modules'
*** Installing modules
install providers/legacy.so -> /usr/local/lib/ossl-modules/legacy.so
"make" depend && "make" _build_programs
make[1]: Entering directory '/home/user/OPENSSL_INSTALL/openssl-3.5.6'
make[1]: Leaving directory '/home/user/OPENSSL_INSTALL/openssl-3.5.6'
make[1]: Entering directory '/home/user/OPENSSL_INSTALL/openssl-3.5.6'
make[1]: Nothing to be done for '_build_programs'.
make[1]: Leaving directory '/home/user/OPENSSL_INSTALL/openssl-3.5.6'
*** Installing runtime programs
install apps/openssl -> /usr/local/bin/openssl
install tools/c_rehash -> /usr/local/bin/c_rehash
```



### After install

```shell
/usr/local/bin/openssl -v
# 
# OpenSSL 3.5.6 7 Apr 2026 (Library: OpenSSL 3.5.1 1 Jul 2025)
#
openssl version -a
/usr/local/bin/openssl version -a
#
sudo ldconfig /usr/local/lib64/
sudo ldconfig
```


Place future downloads
`mkdir -p PYTHON_INSTALL`
`cd PYTHON_INSTALL`

## Download Python

-<https://www.python.org/ftp/python/3.14.5/Python-3.14.5.tgz>

```shell
wget https://www.python.org/ftp/python/3.14.5/Python-3.14.5.tgz
# Unpack
tar -xzf Python-3.14.5.tgz
cd Python-3.14.5
```

## Configure

Short

```shell
./configure --enable-optimizations --with-lto --with-openssl=/usr/local/bin/openssl LDFLAGS="-Wl,-rpath /usr/local"
```

Extra with debug

```shell
./configure --prefix=/usr/local \
            --enable-optimizations \
            --with-lto \
            --enable-shared \
            --with-pydebug \
            --enable-loadable-sqlite-extensions \
            --with-openssl-rpath=auto LDFLAGS="-Wl,-rpath /usr/local/lib"
```

## Install

```shell
make -j "$(nproc)"
sudo make altinstall
#
# Start over
make clean && make distclean
# Check
whereis python3.14
# 
# python3.14: /usr/local/bin/python3.14 /usr/local/lib/python3.14
sudo /usr/local/bin/pip3.14 install --upgrade pip
sudo /usr/local/bin/pip3.14 install virtualenv
```

### LD_CONFIG

```shell
vi /etc/ld.so.conf.d/python310.conf
include /usr/local/lib/
```

## Virtualenv

```shell
cd PROJECT_DIR
virtualenv --python=/usr/local/bin/python3.14 venv --system-site-packages
#
# Use
source venv/bin/activate
source venv/bin/deactivate
#
```

## Debug

```shell
# https://stackoverflow.com/questions/22931774/how-to-use-gdb-python-debugging-extension-inside-virtualenv
yum install python3-dbg
python3-dbg -m venv ./env-name
```
