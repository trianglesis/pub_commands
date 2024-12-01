# Install Apache2 from sources

## Pre
Pre-Requisits:
```
yum groupinstall "Development Tools"
yum install expat expat-devel

# Probably, but not sure:
yum install libxml2-dev
```


- https://httpd.apache.org/docs/2.4/install.html

```
wget https://dlcdn.apache.org/httpd/httpd-2.4.57.tar.gz

gzip -d httpd-2.4.57.tar.gz

tar xvf httpd-2.4.57.tar

```

# APR and APR-Util

- https://apr.apache.org/download.cgi

```
wget https://dlcdn.apache.org/apr/apr-1.7.4.tar.gz
gzip -d apr-1.7.4.tar.gz
tar xvf apr-1.7.4.tar
```

```
wget https://dlcdn.apache.org/apr/apr-util-1.6.3.tar.gz
gzip -d apr-util-1.6.3.tar.gz
tar xvf apr-util-1.6.3.tar
```

### APR and APR-Util info

Make sure you have APR and APR-Util already installed on your system. 
If you don't, or prefer to not use the system-provided versions, download the latest versions of both APR and APR-Util from Apache APR, unpack them into 

- /httpd_source_tree_root/srclib/apr
- /httpd_source_tree_root/srclib/apr-util

be sure the directory names do not have version numbers; for example, the APR distribution must be under 
- /httpd_source_tree_root/srclib/apr/

### and use option:

`./configure's --with-included-apr`

On some platforms, you may have to install the corresponding -dev packages to allow httpd to build against your installed copy of APR and APR-Util.

#### Move\copy:

```
cp -r ~/apr-1.7.4/ ~/httpd-2.4.57/srclib/apr/
cp -r ~/apr-util-1.6.3/ ~/httpd-2.4.57/srclib/apr-util/
```

Check copies: 

```
ls -lah ~/httpd-2.4.57/srclib/apr/
ls -lah ~/httpd-2.4.57/srclib/apr-util/
```

# PCRE2 install or check system

Perl-Compatible Regular Expressions Library (PCRE)
This library is required but not longer bundled with httpd. 
Download the source code from http://www.pcre.org, or install a Port or Package. 
If your build system can't find the pcre-config script installed by the PCRE build, point to it using the --with-pcre parameter. 
On some platforms, you may have to install the corresponding -dev package to allow httpd to build against your installed copy of PCRE.

- https://github.com/PCRE2Project/pcre2/releases
- https://stackoverflow.com/a/10717222

## Make Install PCRE2

```
wget https://github.com/PCRE2Project/pcre2/releases/download/pcre2-10.42/pcre2-10.42.tar.gz
gzip -d pcre2-10.42.tar.gz
tar xvf pcre2-10.42.tar
cd pcre2-10.42

./configure --prefix=/usr/local/pcre

make
make install
```

## OR use system:
`yum install pcre`
`Package pcre-8.44-3.el9.3.x86_64 is already installed.`


# Make Install Apache2

```
cd httpd-2.4.57
```

`
./configure --prefix=/usr/local/apache --with-included-apr --with-pcre=/usr/local/pcre
`

If: `configure: error: Did not find working script at pcre2-config` then run:

- https://bz.apache.org/bugzilla/show_bug.cgi?id=66000

`
./configure --prefix=/usr/local/apache --with-included-apr --with-pcre=/usr/local/pcre/bin/pcre2-config
`

If all are OK:

```
make
make install
```

## Errors:

configure: error: pcre(2)-config for libpcre not found. PCRE is required and available from http://pcre.org/
- see PCRE Install

# Use

## Conf

Conf files are stored in mentioned new place! 
- /usr/local/apache/

```
vi /usr/local/apache/conf/httpd.conf
vi /usr/local/apache/conf/extra/httpd-vhosts.conf
vi /usr/local/apache/conf/extra/YOUR_SITE
```

## Start\stop
```
/usr/local/apache/bin/apachectl -k start
/usr/local/apache/bin/apachectl -k stop
/usr/local/apache/bin/apachectl -k graceful-stop
```

## Logs:
- /usr/local/apache/logs/
```
tail -f -n50 /usr/local/apache/logs/error_log
tail -f -n50 /usr/local/apache/logs/access_log
```


## ETC:

(2)No such file or directory: [client 111.22.333.44:61934] mod_wsgi (pid=236976): Unable to connect to WSGI daemon process 'daemon' on '/usr/local/apache/run/wsgi.234448.8.1.sock' as user with uid=2

```
mkdir -p /usr/local/apache/run/
```


# Service:

- https://httpd.apache.org/docs/2.4/stopping.html

Add to:
- /etc/systemd/system/

Stop:
- /usr/local/apache/bin/apachectl -k graceful-stop

`
vi /etc/systemd/system/httpd.service
`

```
[Unit]
Description=Apache Web Server
After=network.target remote-fs.target nss-lookup.target

[Service]
Type=forking
PIDFile=/usr/local/apache/logs/httpd.pid
ExecStart=/usr/local/apache/bin/apachectl -k start
ExecStop=/usr/local/apache/bin/apachectl -k graceful-stop
ExecReload=/usr/local/apache/bin/apachectl -k graceful
PrivateTmp=true
LimitNOFILE=infinity

[Install]
WantedBy=multi-user.target
```

## If changed:
`
systemctl daemon-reload
`

## Run:

`
systemctl start httpd.service
`


# User add or pid file path rights

```
chown root:daemon -R /usr/local/apache/run/

grep apache /etc/passwd

apache:x:48:48:Apache:/usr/share/httpd:/sbin/nologin

```

# Autostart:

`
systemctl enable httpd.service
`