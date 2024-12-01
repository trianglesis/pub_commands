# Htop

- https://github.com/htop-dev/htop

# Pre:
`yum install ncurses-devel`

```
wget https://github.com/htop-dev/htop/releases/download/3.2.2/htop-3.2.2.tar.xz
tar -xvf htop-3.2.2.tar.xz
cd htop-3.2.2

./autogen.sh && ./configure && make

make install
```
