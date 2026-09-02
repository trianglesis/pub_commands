# Old IP tables

`# Warning: iptables-legacy tables present, use iptables-legacy to see them`

Read

- <https://wiki.nftables.org/wiki-nftables/index.php/Moving_from_iptables_to_nftables>
- <https://forum.openwrt.org/t/yamon3-getting-warning-iptables-legacy-tables-present-use-iptables-legacy-to-see-them/159574/3>

Save old, try to convert to new

```shell
% iptables-save > iptables-legacy.txt

% iptables-legacy-save > iptables-legacy.txt
% iptables-nft-restore < iptables-legacy.txt

% iptables-nft-save
nft list ruleset
```

## Old packages

List, remove

```shell
root@OpenWrt:~# opkg list-installed | grep legacy
ip6tables-zz-legacy - 1.8.7-7
iptables-zz-legacy - 1.8.7-7
xtables-legacy - 1.8.7-7
# Removed from UI
```

## Util

Utility that produced warning message

```shell
opkg install mwan3
```
