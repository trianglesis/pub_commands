# Repos

## Doc

- YUM: https://docs.oracle.com/en/operating-systems/oracle-linux/software-management/OL-SFW-MGMT.pdf
- EPEL: https://cloudspinx.com/how-to-enable-epel-repository-on-oracle-linux/


## Install

```shell
sudo dnf install -y oraclelinux-release-el9
sudo dnf config-manager --enable ol9_kvm_utils
sudo dnf install https://dl.fedoraproject.org/pub/epel/epel-release-latest-9.noarch.rpm
```

### Custom

```shell
sudo tee /etc/yum.repos.d/ol9-epel.repo<<EOF
[ol9_developer_EPEL]
name= Oracle Linux \$releasever EPEL (\$basearch)
baseurl=https://yum.oracle.com/repo/OracleLinux/OL9/developer/EPEL/\$basearch/
gpgkey=file:///etc/pki/rpm-gpg/RPM-GPG-KEY-oracle
gpgcheck=1
enabled=1
EOF
```

## Manage

```shell
dnf repolist
sudo dnf reinstall repository
sudo dnf config-manager --enable repository

ls -lah /etc/yum.repos.d/
sudo vi /etc/yum.repos.d/

dnf clean all
```


## Search

`uuid-dev lzma-dev liblzma-dev libncurses5-dev readline-dev sqlite-devel`

```shell
dnf search tkinter
```