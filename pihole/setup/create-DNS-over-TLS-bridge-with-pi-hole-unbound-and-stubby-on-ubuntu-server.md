# Create DNS-over-TLS bridge with Pi-hole, unbound and stubby on Ubuntu Server

Few months ago, I've made a similar work but I wanted something a little more easier to manage. Please have a look at [here](https://gist.github.com/Jiab77/72c868ecebce1d0027258eeec53b5a0f) for my previous work.

This time, I'm gonna do pretty much the same thing but using Pi-hole as base then modify it to include `unbound` and `stubby`.

This way, I can use the power of Pi-hole with some additional security layers:

* Recursive DNS check (`unbound`)
* DNS-over-TLS (`stubby`)

## Server

### Install OS

Load the Ubuntu Server image you for Raspberry Pi's from here:

* http://cdimage.ubuntu.com/ubuntu/releases/

Version 18.04.5:

* Hard Float (armhf): <http://cdimage.ubuntu.com/ubuntu/releases/18.04/release/ubuntu-18.04.5-preinstalled-server-armhf+raspi3.img.xz>
* 64 Bits (arm64): <http://cdimage.ubuntu.com/ubuntu/releases/18.04/release/ubuntu-18.04.5-preinstalled-server-arm64+raspi3.img.xz>

> If you are using small TFT screen on your Raspberry Pi, it might not work with the latest Ubuntu Server images. In this case, you will have to fallback on the version `18.04.3`.

Version 20.04.2:

* Hard Float (armhf): <http://cdimage.ubuntu.com/ubuntu/releases/20.04/release/ubuntu-20.04.2-preinstalled-server-armhf+raspi.img.xz>
* 64 Bits (arm64): <http://cdimage.ubuntu.com/ubuntu/releases/20.04/release/ubuntu-20.04.2-preinstalled-server-arm64+raspi.img.xz>

### Update OS

```bash
# Update package cache
sudo apt update --fix-missing -y

# Get latest packages version
# Allow downgrades in case of incompatibilities
sudo apt dist-upgrade -y --allow-downgrades

# Clean up leading old packages
sudo apt autoremove --purge -y

# Check if reboot is required
[ -f /var/run/reboot-required ] && cat /var/run/reboot-required
```

### Install Netdata

```
bash <(curl -Ss https://my-netdata.io/kickstart.sh) all --dont-wait --disable-telemetry
```

#### Enable sensors

```
cd /etc/netdata
sudo ./edit-config charts.d.conf
```

Add `sensors=force` at the end of the file and save it.

Restart Netdata to enable Raspberry Pi temperature sensor monitoring.

```
sudo systemctl restart netdata ; systemctl status netdata -l
```

### Install Pi-hole

```
curl -sSL https://install.pi-hole.net | bash
```

#### Initial Upstream DNS

Even we'll disable it later, just select anyone of them as displayed:

![image](https://user-images.githubusercontent.com/9881407/119740520-57077880-be84-11eb-9e5c-a751042e2b5a.png)

#### Initial blocklist

We'll add some addiotional ones later but you can already keep the default one enabled.

![image](https://user-images.githubusercontent.com/9881407/119740838-e0b74600-be84-11eb-85d5-cc43d919f4e6.png)

#### Protocols to listen

You can leave the default selection if you don't know or unselect the protocol you are not using.

![image](https://user-images.githubusercontent.com/9881407/119740931-0cd2c700-be85-11eb-927f-aab587829035.png)

> During my initial testing on VM, it seems like __Pi-hole__ will automatically disable the unused protocols.

#### IP address

You can leave the default settings or change them if necessary.

![image](https://user-images.githubusercontent.com/9881407/119741201-85d21e80-be85-11eb-8ac0-44818c22ec97.png)

> The image is not broken, I just preferred to keep the default settings private.

#### IP address warning

Acknowledge the warning if you don't know what it means otherwise read and follow the given instructions.

![image](https://user-images.githubusercontent.com/9881407/119741576-4952f280-be86-11eb-8de9-2065a8cbacae.png)

#### Web interface

You can use __Pi-hole__ without the web interface but I'd recommend to keep as it is much more convenient. You can still use the __CLI__ if you prefer that, it is installed by default, the web interface is purely optional.

![image](https://user-images.githubusercontent.com/9881407/119743044-51606180-be89-11eb-8268-511c2550dda3.png)

#### Web server

The web server will be required if you have decided to use the web interface but if you already have installed another web server, then you might have to say `off`, otherwise leave it to __`on`__.

![image](https://user-images.githubusercontent.com/9881407/119743181-97b5c080-be89-11eb-992b-1648a7ed883e.png)

#### Logging

You can decide to not use it but keeping the default setting will be helpful for debugging in case of issues.

![image](https://user-images.githubusercontent.com/9881407/119743533-65589300-be8a-11eb-981f-0871e8630144.png)

#### Privacy mode

You decide :grin:. But for a private usage I'd say, level `0` otherwise for a privacy respectful public usage, level `3`.

![image](https://user-images.githubusercontent.com/9881407/119743675-b072a600-be8a-11eb-9661-ce732992a704.png)

> As I said, you decide on this one, you are not forced to follow me :wink:.

#### After the configuration wizzard

Once you've have defined the __Privacy Mode__, the configuration wizzard will disappear and the installation process will continue with the given configuration values.

#### DNS resolution issues

You might encounter some DNS resolution issues at the end of the installation process, it is due to conflict with existing DNS resolvers but it can be solved pretty easily.

![image](https://user-images.githubusercontent.com/9881407/119744315-072caf80-be8c-11eb-8044-056346e0fd3f.png)

You will have around __20 seconds__ to do the following:

```bash
# Connect on your device
ssh user@host

# Edit the DNS resolution file
sudo nano /etc/resolv.conf

# Delete the existing content and add the following:
nameserver 127.0.0.1
```

Save the file and normally it should work and continue as expected.

> Unfortunately on my side I didn't had time to proceed but it worked on my test VM.

If you didn't had time to do anything before the timeout, don't worry, you can still proceed to the install, you have not done all of this for nothing :wink:.

![image](https://user-images.githubusercontent.com/9881407/119744867-32fc6500-be8d-11eb-8cfd-6df35a604e75.png)

Here is what to do in this case:

```bash
# Edit the DNS resolution file
sudo nano /etc/resolv.conf

# Delete the existing content, add the following and save the file
nameserver 127.0.0.1

# Test if it works
dig pi-hole.net

# If it does, run the install script again.
# Do it ASAP because the /etc/resolv.conf file will be rewritten by the responsible service until is it disabled later.
curl -sSL https://install.pi-hole.net | bash
```

Once reloaded, the installer script will prompt for execution mode, you can safely decide to leave the default selection or reconfigure everything.

![image](https://user-images.githubusercontent.com/9881407/119745257-0b59cc80-be8e-11eb-81ca-92d48f1f3ab6.png)

As you can see below, the installation finished properly as expected :tada:.

![image](https://user-images.githubusercontent.com/9881407/119745458-773c3500-be8e-11eb-8b95-0414a0a70291.png)

#### Solving DNS resolution issues

As said previously, the `/etc/resolv.conf` file will be rewritten automatically by the responsible and conflicting service and will need to be disabled to let __Pi-hole__ working properly without any further similar issues.

In my case, the culprit is the `systemd-resolved` service that is using a `stub` file instead of the real `/etc/resolv.conf` file. You can recognize it by the default content of the `/etc/resolv.conf` file or the fact that it's `127.0.0.53` that is used by default instead of `127.0.0.1`.

If you are in a similar situation, run the following commands:

```bash
# Check the service status
systemctl status systemd-resolved.service -l

# Disable the service if running
sudo systemctl disable --now systemd-resolved.service ; systemctl status systemd-resolved.service -l

# Remove existing symlink
sudo rm -fv /etc/resolv.conf

# Edit the DNS resolution file
sudo nano /etc/resolv.conf

# Delete the existing content, add the following and save the file
nameserver 127.0.0.1

# Test if it works
dig pi-hole.net
```

> You can also `dig` any other domains if you prefer or either use `getent` or `host` instead of `dig`, the idea of this test is to see if the DNS resolution is repaired or not.

Normally, you should be able to continue without any other similar issues.

#### Reset generated password

If you are like me and had the DNS resolution broken during the installation process, then you have no clue about the generated password that is used to protect the access to the web interface and the `API` so you might wonder how to reset it or set a new one, right?

Don't worry, I'll tell you what to do :wink:.

Simply run the following command(s):

```bash
# Reset the password
pihole -a -p

# And set a new one (yes, it's the same command)
pihole -a -p
```

> Run `pihole --help` or `pihole -a -h` for more details.

#### Access to the web interface

Normally you should be able to access to the web interface with your device IP adress or hostname (_if already defined in `/etc/hosts` on your machine or a global `DNS`_).

If you don't remember the IP address or hostname of your device or simply being lazy to type the address (__*don't worry, I won't blame you*__), you can do the following:

```bash
# Connect using IP address
echo "http://$(hostname -i)/admin/"

# Connect using hostname
echo "http://$(hostname -f)/admin/"
```

Then `[Ctrl + Click]` to open it in your browser :wink:.

![image](https://user-images.githubusercontent.com/9881407/119748339-051b1e80-be95-11eb-9ed3-0a79dd2d3275.png)

#### Put it dark man!

If you are looking at the dark mode, log as `admin` and go to __Settings__ on the left then:

![image](https://user-images.githubusercontent.com/9881407/119748602-9094af80-be95-11eb-8a2f-25bfb7663cc5.png)

#### Add more blocklists (_optional_)

If you want to add some others blocklists like [EasyList](https://easylist.to/) used by [AdBlock Plus](https://adblockplus.org/) for example, you can use the converted version for __Pi-hole__ from <https://github.com/justdomains/blocklists>.

In my case, I'm gonna use the following ones:

* EasyList: <https://justdomains.github.io/blocklists/lists/easylist-justdomains.txt>
* EasyPrivacy: <https://justdomains.github.io/blocklists/lists/easyprivacy-justdomains.txt>

If you are looking for something else or different ones, here are some references that you can find useful:

* <https://firebog.net/>
* <https://github.com/StevenBlack/hosts>
* <https://github.com/Laicure/hosts>

So, to proceed, go to __Group Management__ on the left then do as displayed below:

![image](https://user-images.githubusercontent.com/9881407/119750242-2f6edb00-be99-11eb-9492-a158db1cb949.png)

Once you've finished to add all your additional blocklists, go to __Tools__ then __Update Gravity__ and click on the __Update__ button.

![image](https://user-images.githubusercontent.com/9881407/119750472-b1f79a80-be99-11eb-9bbe-a1235258f898.png)

You should get something similar if the update worked properly:

![image](https://user-images.githubusercontent.com/9881407/119750523-d0f62c80-be99-11eb-9dd0-98c974780311.png)

Now you can take the time to discover the dashboard before continuing or not :grin:.

#### Change blocking mode (_optional_)

You should not change the blocking mode if you have no idea about possible `DNS` replies, otherwise proceed as follow:

```bash
# Edit the /etc/pihole/pihole-FTL.conf file
sudo nano /etc/pihole/pihole-FTL.conf

# Add or change the following and save the file
BLOCKINGMODE=NXDOMAIN
```

> See [here](https://docs.pi-hole.net/ftldns/blockingmode/) for more details about other possible values.
>
> By default, [Quad9](https://www.quad9.net) is using also using `NXDOMAIN` as `DNS` reply to block unwanted / malware domains. To know more about that, check [here](https://www.quad9.net/support/faq) and search for `NXDOMAIN` in the page.

Once done, run the following command:

```
sudo killall -SIGHUP pihole-FTL
```

Or go to __Settings__ then click on the __Restart DNS resolver__ button:

![image](https://user-images.githubusercontent.com/9881407/119753453-1537fb80-be9f-11eb-9a0b-98cfcc4d08ea.png)

#### View stats in Netdata

As we have installed __Netdata__ at the beginning, we can now see some statistics about __Pi-hole__ in __Netdata__ but we have to restart the __Netdata__ service first:

```bash
# Restart the service
sudo systemctl restart netdata ; systemctl status netdata -l

# Connect to the Netdata web interface
echo "http://$(hostname -f):19999/#menu_pihole"

# Or with this one for using the IP address instead of the hostname
echo "http://$(hostname -i):19999/#menu_pihole"
```

Then `[Ctrl + Click]` to open the URL in your browser:

![image](https://user-images.githubusercontent.com/9881407/119751623-c76dc400-be9b-11eb-9fb4-44d61d83bda8.png)

### Install Unbound and Stubby

```
sudo apt install unbound stubby
```

#### Disable Unbound resolver

The first thing you will have to do when using __Unbound__ with __Pi-hole__ is to disable the installed internal resolver that will create the same DNS resolution issue we had previously... To do so, simply run the following command:

```
sudo systemctl disable --now unbound-resolvconf.service ; systemctl status unbound-resolvconf.service -l
```

Then verify that the content of the `/etc/resolv.conf` file is similar to this:

```
$ cat /etc/resolv.conf 
nameserver 127.0.0.1
```

At this state, __Unbound__ will not working correctly but it's normal, it's because the default port `53` is already used by [FTLDNS](https://docs.pi-hole.net/ftldns/) in __Pi-hole__.

We need now to configure __Unbound__ and make it listening on another port than `53`.

> The port `53` is the default one for `DNS`.

#### Configure Unbound

Create a new configuration file that will be used to hold the main settings related to your setup without touching the default ones.

In this case, I'll use `/etc/unbound/unbound.conf.d/pi-hole-dot.conf` but you can use another name.

```
sudo nano /etc/unbound/unbound.conf.d/pi-hole-dot.conf
```

> The new configuration file must be stored in the `/etc/unbound/unbound.conf.d/` folder.

Add the following content:

```yaml
server:
    # Network settings
    interface: 127.0.0.1
    port: 5335
    do-ip4: yes
    do-udp: yes
    do-tcp: yes

    # May be set to yes if you have IPv6 connectivity
    do-ip6: no

    # You want to leave this to no unless you have *native* IPv6. With 6to4 and
    # Terredo tunnels your web browser should favor IPv4 for the same reasons
    prefer-ip6: no

    # Use this only when you downloaded the list of primary root servers!
    # If you use the default dns-root-data package, unbound will find it automatically
    #root-hints: "/var/lib/unbound/root.hints"

    # Trust glue only if it is within the server's authority
    harden-glue: yes

    # Require DNSSEC data for trust-anchored zones, if such data is absent, the zone becomes BOGUS
    harden-dnssec-stripped: yes

    # Don't use Capitalization randomization as it known to cause DNSSEC issues sometimes
    # see https://discourse.pi-hole.net/t/unbound-stubby-or-dnscrypt-proxy/9378 for further details
    use-caps-for-id: no

    # Reduce EDNS reassembly buffer size.
    # Suggested by the unbound man page to reduce fragmentation reassembly problems
    edns-buffer-size: 1472

    # Perform prefetching of close to expired message cache entries
    # This only applies to domains that have been frequently queried
    prefetch: yes

    # One thread should be sufficient, can be increased on beefy machines. In reality for most users running on small networks or on a single machine, it should be unnecessary to seek performance enhancement by increasing num-threads above 1.
    num-threads: 2

    # Ensure kernel buffer is large enough to not lose messages in traffic spikes
    #so-rcvbuf: 1m

    # Ensure privacy of local IP ranges
    private-address: 192.168.0.0/16
    private-address: 169.254.0.0/16
    private-address: 172.16.0.0/12
    private-address: 10.0.0.0/8
    private-address: fd00::/8
    private-address: fe80::/10
```

> It's a customized version from the one described [here](https://docs.pi-hole.net/guides/dns/unbound/).

Now restart the service and check the status:

```
sudo systemctl restart unbound ; systemctl status unbound -l
```

Unfortunately, it's not finished yet... There is another blocking service that is also trying to listen on the port `53` which is __Stubby__. Don't worry, we'll also solve this issue but we must do some other changes in the __Unbound__ configuration before moving to the __Stubby__ part.

Now we'll need to forward all the `DNS` requests that will be passed from `FTLDNS` to __Unbound__ to finally, __Stubby__.

Why that? Simply because:

1. `FTLDNS` is listening on port `53` to handle all `DNS` requests
2. These requests will be then forwarded to __Unbound__ for the recursive `DNS` checking.
3. To apply the `DNS-over-TLS` we need then to forward requests from __Unbound__ to __Stubby__ that will then forward them to the defined Upstream DNS in the configuration file.
4. To finish, `FTLDNS` will then cache the `DNS` replies transmitted with `DNS-over-TLS` from __Stubby__.

> It's sounds complicated but once in place it works perfectly :wink:.

To forward `DNS` requests to __Stubby__, we'll need to create a new configuration file named `/etc/unbound/unbound.conf.d/stubby.conf`:

```
sudo nano /etc/unbound/unbound.conf.d/stubby.conf
```

With the following content:

```yaml
server:
    do-not-query-localhost: no
forward-zone:
  name: "."
    forward-addr: 127.0.0.1@8053
    forward-addr: ::1@8053
```

Save the file and... You're mostly done for the __Unbound__ part! :sunglasses:

#### Increase DNS privacy

To improve the DNS privacy with __Unbound__, we just need to create some additional configuration files:

* `privacy.conf`
* `qname-minimisation.conf` (_should already exist_)

With the following content:

* `/etc/unbound/unbound.conf.d/privacy.conf`

```yaml
server:
    minimal-responses: yes
    hide-identity: yes
    hide-version: yes
```

* `/etc/unbound/unbound.conf.d/qname-minimisation.conf`

```yaml
server:
    # Send minimum amount of information to upstream servers to enhance
    # privacy. Only sends minimum required labels of the QNAME and sets
    # QTYPE to NS when possible.

    # See RFC 7816 "DNS Query Name Minimisation to Improve Privacy" for
    # details.

    qname-minimisation: yes
```

Optionally, you can also change the default logging settings by creating another config file:

```
sudo nano /etc/unbound/unbound.conf.d/logs.conf
```

With the following content:

```yaml
# enable query logging
server:
    # If no logfile is specified, syslog is used
    # logfile: "/var/log/unbound/unbound.log"
    log-time-ascii: yes
    log-queries: yes
    log-replies: yes
    verbosity: 2
```

> Feel free to use different settings and set verbosity level to `0` for example with `log-queries` and `log-replies` set to `no`.

#### Configure Stubby

Now we need to configure __Stubby__ to stop using the default port `53` and listen on another one.

We'll also need to define the wanted Upstream DNS to use internaly.

> The configuration file below has been customized to use [Quad9](https://www.quad9.net) Upstream DNS with [DNSSEC](https://en.wikipedia.org/wiki/Domain_Name_System_Security_Extensions) and [ECS](https://en.wikipedia.org/wiki/EDNS_Client_Subnet) enabled. See [here](https://www.quad9.net/support/faq) for more details.
>
> Feel free to change the defined settings according to your needs.

Create a backup of the existing file:

```
cd /etc/stubby
sudo mv -v stubby.yml stubby.yml.old
sudo nano stubby.yml
```

and paste the content from the customized version:

```yaml
#
# This is a yaml version of the stubby configuration file (it replaces the 
# json based stubby.conf file used in earlier versions of getdns/stubby).
#
# For more information see
# https://dnsprivacy.org/wiki/display/DP/Configuring+Stubby
#
# This format does not fully support all yaml features - the restrictions are:
#   - the outer-most data structure must be a yaml mapping
#   - mapping keys must be yaml scalars
#   - plain scalars will be converted to json unchanged
#   - non-plain scalars (quoted, double-quoted, wrapped) will be interpreted
#     as json strings, i.e. double quoted. 
#   - yaml tags are not supported
#   - IPv6 addresses ending in :: are not yet supported (use ::0)
#
# Note that we plan to introduce a more compact format for defining upstreams
# in future: https://github.com/getdnsapi/stubby/issues/79

# Logging is currently configured at runtime using command line arguments. See
# > stubby -h
# for details.

# Specifies whether to run as a recursive or stub resolver 
# For stubby this MUST be set to GETDNS_RESOLUTION_STUB
resolution_type: GETDNS_RESOLUTION_STUB

# Ordered list composed of one or more transport protocols: 
# GETDNS_TRANSPORT_UDP, GETDNS_TRANSPORT_TCP or GETDNS_TRANSPORT_TLS
# If only one transport value is specified it will be the only transport used. 
# Should it not be available basic resolution will fail.
# Fallback transport options are specified by including multiple values in the
# list.  Strict mode (see below) should use only GETDNS_TRANSPORT_TLS.
dns_transport_list:
  - GETDNS_TRANSPORT_TLS

# Selects Strict or Opportunistic Usage profile as described in
# https://datatracker.ietf.org/doc/draft-ietf-dprive-dtls-and-tls-profiles/
# Strict mode requires that authentication information for the upstreams is
# specified below. Opportunistic may fallback to clear text DNS if UDP or TCP
# is included in the transport list above.
# For Strict use        GETDNS_AUTHENTICATION_REQUIRED
# For Opportunistic use GETDNS_AUTHENTICATION_NONE
tls_authentication: GETDNS_AUTHENTICATION_REQUIRED

# EDNS0 option to pad the size of the DNS query to the given blocksize
# 128 is currently recommended by 
# https://tools.ietf.org/html/draft-ietf-dprive-padding-policy-03
tls_query_padding_blocksize: 128

# EDNS0 option for ECS client privacy as described in Section 7.1.2 of
# https://tools.ietf.org/html/rfc7871
edns_client_subnet_private : 1

# EDNS0 option for keepalive idle timeout in ms as specified in
# https://tools.ietf.org/html/rfc7828
# This keeps idle TLS connections open to avoid the overhead of opening a new 
# connection for every query.
idle_timeout: 10000

# Set the listen addresses for the stubby DAEMON. This specifies localhost IPv4
# and IPv6. It will listen on port 53 by default. Use <IP_address>@<port> to 
# specify a different port
listen_addresses:
  - 127.0.0.1@8053
  -  0::1@8053

# Instructs stubby to distribute queries across all available name servers. 
# Set to 0 to treat the upstreams below as an ordered list and use a single
# upstream until it becomes unavailable, then use the next one.
round_robin_upstreams: 1

# Require DNSSEC validation. For releases earlier than 1.2 a trust anchor must
# be configured configured manually. This can be done with unbound-anchor.
dnssec_return_status: GETDNS_EXTENSION_TRUE

# Specify the location of the installed trust anchor file (leave commented out
# for zero configuration DNSSEC)
# dnssec_trust_anchors: "/etc/unbound/getdns-root.key"
dnssec_trust_anchors: "/var/lib/unbound/root.key"

# Control the maximum number of connection failures that will be permitted
# before Stubby backs-off from using an individual upstream (default 2)
# tls_connection_retries: 5

# Control the maximum time in seconds Stubby will back-off from using an
# individual upstream after failures under normal circumstances (default 3600)
# tls_backoff_time: 300

# Limit the total number of outstanding queries permitted
# limit_outstanding_queries: 100

# Specify the timeout on getting a response to an individual request
# (default 5s)
# timeout: 1

# Specify the list of upstream recursive name servers to send queries to
# In Strict mode upstreams need either a tls_auth_name or a tls_pubkey_pinset
# so the upstream can be authenticated.
# The list below includes all the available test servers but only has the subset
# operated the stubby/getdns developers enabled. You can enable any of the
# others you want to use by uncommenting the relevant section. See:
# https://dnsprivacy.org/wiki/display/DP/DNS+Privacy+Test+Servers
# If you don't have IPv6 then comment then out those upstreams.
# In Opportunistic mode they only require an IP address in address_data.
# The information for an upstream can include the following:
# - address_data: IPv4 or IPv6 address of the upstream
#   port: Port for UDP/TCP (default is 53)
#   tls_auth_name: Authentication domain name checked against the server
#                  certificate
#   tls_pubkey_pinset: An SPKI pinset verified against the keys in the server
#                      certificate
#     - digest: Only "sha256" is currently supported
#       value: Base64 encoded value of the sha256 fingerprint of the public
#              key
#   tls_port: Port for TLS (default is 853)
upstream_recursive_servers:
# IPv4 addresses
# The Surfnet/Sinodun servers
#  - address_data: 145.100.185.15
#    tls_auth_name: "dnsovertls.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 62lKu9HsDVbyiPenApnc4sfmSYTHOVfFgL3pyB+cBL4=
#  - address_data: 145.100.185.16
#    tls_auth_name: "dnsovertls1.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: cE2ecALeE5B+urJhDrJlVFmf38cJLAvqekONvjvpqUA=
# The getdnsapi.net server
#  - address_data: 185.49.141.37
#    tls_auth_name: "getdnsapi.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: foxZRnIh9gZpWnl+zEiKa0EJ2rdCGroMWm02gaxSc9Q=
# IPv6 addresses
# The Surfnet/Sinodun servers
#  - address_data: 2001:610:1:40ba:145:100:185:15
#    tls_auth_name: "dnsovertls.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 62lKu9HsDVbyiPenApnc4sfmSYTHOVfFgL3pyB+cBL4=
#  - address_data: 2001:610:1:40ba:145:100:185:16
#    tls_auth_name: "dnsovertls1.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: cE2ecALeE5B+urJhDrJlVFmf38cJLAvqekONvjvpqUA=
# The getdnsapi.net server
#  - address_data: 2a04:b900:0:100::38
#    tls_auth_name: "getdnsapi.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: foxZRnIh9gZpWnl+zEiKa0EJ2rdCGroMWm02gaxSc9Q=


# Additional servers

# IPv4 addresses
## Quad 9 'secure' service - Filters, does DNSSEC, does ECS
  - address_data: 9.9.9.11
    tls_auth_name: "dns.quad9.net"
  - address_data: 149.112.112.11
    tls_auth_name: "dns.quad9.net"
## Quad 9 'secure' service - Filters, does DNSSEC, doesn't send ECS
  - address_data: 9.9.9.9
    tls_auth_name: "dns.quad9.net"
  - address_data: 149.112.112.112
    tls_auth_name: "dns.quad9.net"
## Quad 9 'insecure' service - No filtering, does DNSSEC, may send ECS (it is 
## unclear if it honours the edns_client_subnet_private request from stubby)
#  - address_data: 9.9.9.10
#    tls_auth_name: "dns.quad9.net"
## The Uncensored DNS servers
#  - address_data: 89.233.43.71
#    tls_auth_name: "unicast.censurfridns.dk"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: wikE3jYAA6jQmXYTr/rbHeEPmC78dQwZbQp6WdrseEs=
## A Surfnet/Sinodun server supporting TLS 1.2 and 1.3
#  - address_data: 145.100.185.18
#    tls_auth_name: "dnsovertls3.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 5SpFz7JEPzF71hditH1v2dBhSErPUMcLPJx1uk2svT8=
## A Surfnet/Sinodun server using Knot resolver. Warning - has issue when used 
## for DNSSEC
#  - address_data: 145.100.185.17
#    tls_auth_name: "dnsovertls2.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: NAXBESvpjZMnPWQcrxa2KFIkHV/pDEIjRkA3hLWogSg=
## dns.cmrg.net server using Knot resolver. Warning - has issue when used for
## DNSSEC.
#  - address_data: 199.58.81.218
#    tls_auth_name: "dns.cmrg.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 3IOHSS48KOc/zlkKGtI46a9TY9PPKDVGhE3W2ZS4JZo=
#      - digest: "sha256"
#        value: 5zFN3smRPuHIlM/8L+hANt99LW26T97RFHqHv90awjo=
## dns.larsdebruin.net (formerly dns1.darkmoon.is)
#  - address_data: 51.15.70.167
#    tls_auth_name: "dns.larsdebruin.net "
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: AAT+rHoKx5wQkWhxlfrIybFocBu3RBrPD2/ySwIwmvA=
## securedns.eu
#  - address_data: 146.185.167.43
#    tls_auth_name: "securedns.eu"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 2EfbwDyk2zSnAbBJSpCSWZKKGUD+a6p/yg2bxdC+x2A=
## dns-tls.bitwiseshift.net
#  - address_data: 81.187.221.24
#    tls_auth_name: "dns-tls.bitwiseshift.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: YmcYWZU5dd2EoblZHNf1jTUPVS+uK3280YYCdz4l4wo=
## ns1.dnsprivacy.at
#  - address_data: 94.130.110.185
#    tls_auth_name: "ns1.dnsprivacy.at"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: vqVQ9TcoR9RDY3TpO0MTXw1YQLjF44zdN3/4PkLwtEY=
## ns2.dnsprivacy.at
#  - address_data: 94.130.110.178
#    tls_auth_name: "ns2.dnsprivacy.at"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: s5Em89o0kigwfBF1gcXWd8zlATSWVXsJ6ecZfmBDTKg=
## dns.bitgeek.in 
#  - address_data: 139.59.51.46
#    tls_auth_name: "dns.bitgeek.in"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: FndaG4ezEBQs4k0Ya3xt3z4BjFEyQHd7B75nRyP1nTs=
## Lorraine Data Network  (self-signed cert).
#  - address_data: 80.67.188.188
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: WaG0kHUS5N/ny0labz85HZg+v+f0b/UQ73IZjFep0nM=
## dns.neutopia.org
#  - address_data: 89.234.186.112
#    tls_auth_name: "dns.neutopia.org"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: wTeXHM8aczvhRSi0cv2qOXkXInoDU+2C+M8MpRyT3OI=
## NIC Chile (self-signed cert)
#  - address_data: 200.1.123.46
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: sG6kj+XJToXwt1M6+9BeCz1SOj/1/mdZn56OZvCyZZc=
## # OARC. Note: this server currently doesn't support strict mode!
##   - address_data: 184.105.193.78
##     tls_auth_name: "tls-dns-u.odvr.dns-oarc.net"
##     tls_pubkey_pinset:
##       - digest: "sha256"
##         value: pOXrpUt9kgPgbWxBFFcBTbRH2heo2wHwXp1fd4AEVXI=

#IPv6 addresses
## Quad 9 'secure' service - Filters, does DNSSEC, does ECS
  - address_data: 2620:fe::11
    tls_auth_name: "dns.quad9.net"
  - address_data: 2620:fe::fe:11
    tls_auth_name: "dns.quad9.net"
## Quad 9 'secure' service - Filters, does DNSSEC, doesn't send ECS
  - address_data: 2620:fe::fe
    tls_auth_name: "dns.quad9.net"
  - address_data: 2620:fe::9
    tls_auth_name: "dns.quad9.net"
## Quad 9 'insecure' service - No filtering, does DNSSEC, may send ECS (it is 
## unclear if it honours the edns_client_subnet_private request from stubby)
#  - address_data: 2620:fe::10
#    tls_auth_name: "dns.quad9.net"
## The Uncensored DNS server
#  - address_data: 2a01:3a0:53:53::0
#    tls_auth_name: "unicast.censurfridns.dk"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: wikE3jYAA6jQmXYTr/rbHeEPmC78dQwZbQp6WdrseEs=
## A Surfnet/Sinodun server supporting TLS 1.2 and 1.3
#  - address_data: 2001:610:1:40ba:145:100:185:18
#    tls_auth_name: "dnsovertls3.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 5SpFz7JEPzF71hditH1v2dBhSErPUMcLPJx1uk2svT8=
## A Surfnet/Sinodun server using Knot resolver. Warning - has issue when used 
## for DNSSEC
#  - address_data: 2001:610:1:40ba:145:100:185:17
#    tls_auth_name: "dnsovertls2.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: NAXBESvpjZMnPWQcrxa2KFIkHV/pDEIjRkA3hLWogSg=
## dns.cmrg.net server using Knot resolver. Warning - has issue when used for
## DNSSEC.
#  - address_data: 2001:470:1c:76d::53
#    tls_auth_name: "dns.cmrg.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 3IOHSS48KOc/zlkKGtI46a9TY9PPKDVGhE3W2ZS4JZo=
#      - digest: "sha256"
#        value: 5zFN3smRPuHIlM/8L+hANt99LW26T97RFHqHv90awjo=
## securedns.eu
#  - address_data: 2a03:b0c0:0:1010::e9a:3001
#    tls_auth_name: "securedns.eu"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 2EfbwDyk2zSnAbBJSpCSWZKKGUD+a6p/yg2bxdC+x2A=
## dns-tls.bitwiseshift.net
#  - address_data: 2001:8b0:24:24::24
#    tls_auth_name: "dns-tls.bitwiseshift.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: YmcYWZU5dd2EoblZHNf1jTUPVS+uK3280YYCdz4l4wo=
## ns1.dnsprivacy.at
#  - address_data: 2a01:4f8:c0c:3c03::2
#    tls_auth_name: "ns1.dnsprivacy.at"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: vqVQ9TcoR9RDY3TpO0MTXw1YQLjF44zdN3/4PkLwtEY=
## ns2.dnsprivacy.at
#  - address_data: 2a01:4f8:c0c:3bfc::2
#    tls_auth_name: "ns2.dnsprivacy.at"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: s5Em89o0kigwfBF1gcXWd8zlATSWVXsJ6ecZfmBDTKg=
## Go6Lab
#  - address_data: 2001:67c:27e4::35
#    tls_auth_name: "privacydns.go6lab.si"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: g5lqtwHia/plKqWU/Fe2Woh4+7MO3d0JYqYJpj/iYAw=
## Lorraine Data Network  (self-signed cert). 
#  - address_data: 2001:913::8
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: WaG0kHUS5N/ny0labz85HZg+v+f0b/UQ73IZjFep0nM=
## dns.neutopia.org
#  - address_data: 2a00:5884:8209::2
#    tls_auth_name: "dns.neutopia.org"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: wTeXHM8aczvhRSi0cv2qOXkXInoDU+2C+M8MpRyT3OI=
## NIC Chile (self-signed cert)
#  - address_data: 2001:1398:1:0:200:1:123:46
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: sG6kj+XJToXwt1M6+9BeCz1SOj/1/mdZn56OZvCyZZc=
## Yeti. Note the servers use a different root trust anchor for DNSSEC!
#  - address_data: 2001:4b98:dc2:43:216:3eff:fea9:41a
#    tls_auth_name: "dns-resolver.yeti.eu.org"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: YxtXAorQNSo+333ko1ctuXcnpMcplPaOI/GCM+YeMQk=
## # OARC. Note: this server currently doesn't support strict mode!
##   - address_data: 2620:ff:c000:0:1::64:25
##     tls_auth_name: "tls-dns-u.odvr.dns-oarc.net"
##     tls_pubkey_pinset:
##       - digest: "sha256"
##         value: pOXrpUt9kgPgbWxBFFcBTbRH2heo2wHwXp1fd4AEVXI=

## Servers that listen on port 443 (IPv4 and IPv6)
## Surfnet/Sinodun servers
#  - address_data: 145.100.185.15
#    tls_port: 443
#    tls_auth_name: "dnsovertls.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 62lKu9HsDVbyiPenApnc4sfmSYTHOVfFgL3pyB+cBL4=
#  - address_data: 145.100.185.16
#    tls_port: 443
#    tls_auth_name: "dnsovertls1.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: cE2ecALeE5B+urJhDrJlVFmf38cJLAvqekONvjvpqUA=
## dns.cmrg.net server using Knot resolver
#  - address_data: 199.58.81.218
#    tls_port: 443
#    tls_auth_name: "dns.cmrg.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 3IOHSS48KOc/zlkKGtI46a9TY9PPKDVGhE3W2ZS4JZo=
#      - digest: "sha256"
#        value: 5zFN3smRPuHIlM/8L+hANt99LW26T97RFHqHv90awjo=
## Lorraine Data Network  (self-signed cert)
#  - address_data: 80.67.188.188
#    tls_port: 443
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: WaG0kHUS5N/ny0labz85HZg+v+f0b/UQ73IZjFep0nM=
## dns.neutopia.org
#  - address_data: 89.234.186.112
#    tls_port: 443
#    tls_auth_name: "dns.neutopia.org"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: wTeXHM8aczvhRSi0cv2qOXkXInoDU+2C+M8MpRyT3OI=
## The Surfnet/Sinodun servers
#  - address_data: 2001:610:1:40ba:145:100:185:15
#    tls_port: 443
#    tls_auth_name: "dnsovertls.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 62lKu9HsDVbyiPenApnc4sfmSYTHOVfFgL3pyB+cBL4=
#  - address_data: 2001:610:1:40ba:145:100:185:16
#    tls_port: 443
#    tls_auth_name: "dnsovertls1.sinodun.com"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: cE2ecALeE5B+urJhDrJlVFmf38cJLAvqekONvjvpqUA=
## dns.cmrg.net server using Knot resolver
#  - address_data: 2001:470:1c:76d::53
#    tls_port: 443
#    tls_auth_name: "dns.cmrg.net"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: 3IOHSS48KOc/zlkKGtI46a9TY9PPKDVGhE3W2ZS4JZo=
#      - digest: "sha256"
#        value: 5zFN3smRPuHIlM/8L+hANt99LW26T97RFHqHv90awjo=
## Lorraine Data Network (self-signed cert)
#  - address_data: 2001:913::8
#    tls_port: 443
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: WaG0kHUS5N/ny0labz85HZg+v+f0b/UQ73IZjFep0nM=
## dns.neutopia.org
#  - address_data: 2a00:5884:8209::2
#    tls_port: 443
#    tls_auth_name: "dns.neutopia.org"
#    tls_pubkey_pinset:
#      - digest: "sha256"
#        value: wTeXHM8aczvhRSi0cv2qOXkXInoDU+2C+M8MpRyT3OI=
```

#### Restart required services

Now that every is configuration properly, we need to restart __Unbound__ and __Stubby__ to apply the new changes:

```
sudo systemctl restart unbound stubby ; systemctl status unbound stubby -l
```

You should see something similar if it worked without issues:

```
● unbound.service - Unbound DNS server
   Loaded: loaded (/lib/systemd/system/unbound.service; enabled; vendor preset: enabled)
   Active: active (running) since Thu 2021-05-27 03:38:06 UTC; 26ms ago
     Docs: man:unbound(8)
  Process: 13861 ExecStartPre=/usr/lib/unbound/package-helper root_trust_anchor_update (code=exited, statu
  Process: 13858 ExecStartPre=/usr/lib/unbound/package-helper chroot_setup (code=exited, status=0/SUCCESS)
 Main PID: 13865 (unbound)
    Tasks: 2 (limit: 2149)
   CGroup: /system.slice/unbound.service
           └─13865 /usr/sbin/unbound -d

May 27 03:38:06 [REDACTED] systemd[1]: Starting Unbound DNS server...
May 27 03:38:06 [REDACTED] package-helper[13861]: /var/lib/unbound/root.key has content
May 27 03:38:06 [REDACTED] package-helper[13861]: success: the anchor is ok
● unbound.service - Unbound DNS server
   Loaded: loaded (/lib/systemd/system/unbound.service; enabled; vendor preset: enabled)
   Active: active (running) since Thu 2021-05-27 03:38:06 UTC; 26ms ago
     Docs: man:unbound(8)
  Process: 13861 ExecStartPre=/usr/lib/unbound/package-helper root_trust_anchor_update (code=exited, statu
  Process: 13858 ExecStartPre=/usr/lib/unbound/package-helper chroot_setup (code=exited, status=0/SUCCESS)
 Main PID: 13865 (unbound)
    Tasks: 2 (limit: 2149)
   CGroup: /system.slice/unbound.service
           └─13865 /usr/sbin/unbound -d

May 27 03:38:06 [REDACTED] systemd[1]: Starting Unbound DNS server...
May 27 03:38:06 [REDACTED] package-helper[13861]: /var/lib/unbound/root.key has content
May 27 03:38:06 [REDACTED] package-helper[13861]: success: the anchor is ok
May 27 03:38:06 [REDACTED] package-helper[13861]: success: the anchor is ok
May 27 03:38:06 [REDACTED] unbound[13865]: [13865:0] notice: init module 0: subnet
May 27 03:38:06 [REDACTED] unbound[13865]: [13865:0] notice: init module 1: validator
May 27 03:38:06 [REDACTED] unbound[13865]: [13865:0] notice: init module 2: iterator
May 27 03:38:06 [REDACTED] unbound[13865]: [13865:0] info: start of service (unbound 1.6.7).
May 27 03:38:06 [REDACTED] systemd[1]: Started Unbound DNS server.

● stubby.service - DNS Privacy Stub Resolver
   Loaded: loaded (/lib/systemd/system/stubby.service; enabled; vendor preset: enabled)
   Active: active (running) since Thu 2021-05-27 03:38:06 UTC; 268ms ago
     Docs: https://dnsprivacy.org/wiki/display/DP/DNS+Privacy+Daemon+-+Stubby
 Main PID: 13857 (stubby)
    Tasks: 1 (limit: 2149)
   CGroup: /system.slice/stubby.service
           └─13857 /usr/bin/stubby

May 27 03:38:06 [REDACTED] systemd[1]: Started DNS Privacy Stub Resolver.
May 27 03:38:06 [REDACTED] stubby[13857]: [03:38:06.118897] STUBBY: Read config from file /etc/stubby/stu
May 27 03:38:06 [REDACTED] stubby[13857]: [03:38:06.124157] STUBBY: DNSSEC Validation is ON
May 27 03:38:06 [REDACTED] stubby[13857]: [03:38:06.124223] STUBBY: Transport list is:
May 27 03:38:06 [REDACTED] stubby[13857]: [03:38:06.124239] STUBBY:   - TLS
May 27 03:38:06 [REDACTED] stubby[13857]: [03:38:06.124255] STUBBY: Privacy Usage Profile is Strict (Auth
May 27 03:38:06 [REDACTED] stubby[13857]: [03:38:06.124269] STUBBY: (NOTE a Strict Profile only applies w
May 27 03:38:06 [REDACTED] stubby[13857]: [03:38:06.124286] STUBBY: Starting DAEMON....
```

#### Change DNS settings

```
sudo nano /etc/dhcpcd.conf
```

Comment the line similar to:

```
static domain_name_servers=9.9.9.9 149.112.112.112
```

That way:

```
# static domain_name_servers=9.9.9.9 149.112.112.112
```

Then save the file. Once done, restart the service:

```
sudo systemctl restart dhcpcd.service ; systemctl status dhcpcd.service -l
```

Then go in the web interface and add __Unbound__ as local DNS resolver:

![image](https://user-images.githubusercontent.com/9881407/119764112-20942280-beb1-11eb-9bd4-0a578c327d74.png)
![image](https://user-images.githubusercontent.com/9881407/119764180-3c97c400-beb1-11eb-9f05-98aa22290d57.png)

Save the new settings with the __Save__ button at the bottom right of the page.

> If something goes wrong here, the system will automatically reset to the previously defined values.

Now you can test all the involved services, all should work correctly:

```bash
# Check Unbound
dig pi-hole.net @127.0.0.1 -p 5335

# Check Stubby
dig pi-hole.net @127.0.0.1 -p 8053

# Check FTLDNS
dig pi-hole.net @127.0.0.1 -p 53
```

To finish, we can now validate if `DNSSEC` is also working as expected:

```bash
# Should return SERVAIL
dig sigfail.verteiltesysteme.net @127.0.0.1 -p 5335

# Should return NOERROR
dig sigok.verteiltesysteme.net @127.0.0.1 -p 5335
```

#### Configuration summary

1. __FTLDNS__: `127.0.0.1:53`
2. __Unbound__: `127.0.0.1:5335`
3. __Stubby__: `127.0.0.1:8053`
4. __Web Interface__: `http://hostname/admin`
5. __API__: `127.0.0.1:4711`
6. __Netdata__: `http://hostname:19999`

### Enable Unbound monitoring (_optional_)

To enable the __Unbound__ monitoring in __Netdata__, we'll just create a new configuration file for __Unbound__ and give read permission to __Netdata__ on `unbound-control` configuration files.

1. Create new __Unbound__ config file for __Netdata__:

```bash
# Create the dedicated file to host Netdata related settings for Unbound
sudo nano /etc/unbound/unbound.conf.d/netdata.conf
```

And add the following content:

```yaml
server:
    # Enable extended statistics.
    statistics-interval: 0
    extended-statistics: yes
    # Set to yes if graphing tool needs it
    statistics-cumulative: yes

# enable remote-control
remote-control:
    control-enable: yes
```

Restart the `unbound` service:

```
sudo systemctl restart unbound ; systemctl status unbound -l
```

2. Assign read permission on `unbound-control` files for __Netdata__:

```bash
# Move to the unbound configuration directory
cd /etc/unbound

# Assign read permision via file ACLs
sudo setfacl -m user:netdata:r unbound.conf
sudo setfacl -m user:netdata:r unbound_control.key
sudo setfacl -m user:netdata:r unbound_control.pem

# Check applied file ACLs permissions
getfacl unbound.conf
getfacl unbound_control.key
getfacl unbound_control.pem
```

3. Restart __Netdata__ to apply the changes.

```bash
# Restart the service
sudo systemctl restart netdata ; systemctl status netdata -l

# Connect to the Netdata web interface
echo "http://$(hostname -f):19999/#menu_unbound_local"

# Or with this one for using the IP address instead of the hostname
echo "http://$(hostname -i):19999/#menu_unbound_local"
```

Then `[Ctrl + Click]` to open the URL in your browser:

![image](https://user-images.githubusercontent.com/9881407/120015608-ce9fe980-bfe3-11eb-8151-76ae4837e615.png)

### Enable additional monitoring (_optional_)

> Coming soon.

### Bonus: Autologin + Screen + Pi-hole Chronometer

In this section, I'll explain how to setup the `autologin` + `screen` + __Pi-hole__ `Chronometer`.

`Chronometer` is a way to display stats to LCD display.

> I can't capture the result as it will contain too many private details.

#### Autologin

It's very simple to setup the `autologin`:

```bash
# Check the getty path
which getty

# Edit the getty service
sudo systemctl edit getty@tty1
```

Add the following content:

```ini
[Service]
ExecStart=
ExecStart=-/sbin/agetty --autologin [username] --noclear %I $TERM
```

Do the following changes:

1. Replace `/sbin/getty` by the path given with the `which` command
2. Replace `[username]` by the username you want to autologin

You can find more details here: https://wiki.archlinux.org/index.php/Getty#Automatic_login_to_virtual_console

#### Screen

Normally `screen` should be already installed on your systemd but if not, just run the following command:

```bash
sudo apt install screen
```

#### Launch script

Now that all dependencies are installed, you can create a small launch script that will called when the session will automatically open:

```bash
#!/bin/bash

# Create an attached Screen session usable from SSH
screen -S pi-hole-mon bash -c '/usr/local/bin/pihole -c'
```

> You can also add `-r [refresh delay in seconds]` but on my side using `-r 1` was creating too much screen flickering so I left the default refresh delay.

Save the script as `pi-hole-mon.sh` and make it executable: `chmod -v +x pi-hole-mon.sh`.

Once done, add the following content in the `.bashrc` file to call it when the session is started:

```bash
# Load screen + pi-hole chronometer
if [ $(tty) == /dev/tty1 ]; then
  for I in {5..1} ; do echo "Initializing monitor in $I seconds..." ; sleep 1 ; done
  ~/pi-hole-mon.sh
fi
```

Now you can reboot to see the result :grin:

#### Connect to the screen session

If everything has worked correctly, you should have a running `screen` session created that display the __Pi-hole__ `chronometer` stats :metal:

To connect on that session, run the following commands:

```bash
# Connect via SSH on the remote host
ssh user@host

# List existing screen sessions
screen -ls

# Connect on the existing session
screen -x

# To disconnect without killing the screen session
[Ctrl+a] d
```

You can find more details here: https://linoxide.com/linux-how-to/screen-remote-ssh/

## Client

In this section, I'll try to explain how to setup several clients to connect to our fresh new __Pi-hole__ (`DoT`) deployment.

### Systemd-resolved

Simply edit the `/etc/systemd/resolved.conf` file and add or change the following:

```ini
#  This file is part of systemd.
#
#  systemd is free software; you can redistribute it and/or modify it
#  under the terms of the GNU Lesser General Public License as published by
#  the Free Software Foundation; either version 2.1 of the License, or
#  (at your option) any later version.
#
# Entries in this file show the compile time defaults.
# You can change settings by editing this file.
# Defaults can be restored by simply deleting this file.
#
# See resolved.conf(5) for details

[Resolve]
DNS=[YOUR-SERVER-IP]
#DNSOverTLS=opportunistic
FallbackDNS=9.9.9.11 149.112.112.11
Domains=~.
#LLMNR=no
#MulticastDNS=no
DNSSEC=yes
#Cache=yes
#DNSStubListener=yes
```

> Replace `[YOUR-SERVER-IP]` by the IP address assigned to your __Pi-hole__ (`DoT`) device.
>
> __You can also define any other Upstream DNS than the ones from [Quad9](https://www.quad9.net/) that you can see here.__

Additionally, you can also try to enable the `DNSSEC` option but according to this [test](https://en.internet.nl/connection/), it works better when disabled.

> I had some local `DNS` resolution issues when `DNSSEC` was not enabled, so I put it back on.

Regarding the `DNSOverTLS` option, make sure to have a `systemd` version upper than __`239`__. As far as I know, it has been supported starting from version `242`.

To finish and apply your changes, simply run the following commands:

```
sudo systemctl restart systemd-resolved.service ; systemctl status systemd-resolved.service -l
```

You can now get more details and very the status with:

```
$ systemd-resolve --status
Global
       LLMNR setting: no
MulticastDNS setting: no
  DNSOverTLS setting: opportunistic
      DNSSEC setting: yes
    DNSSEC supported: yes
         DNS Servers: [REDACTED]
Fallback DNS Servers: 9.9.9.11
                      149.112.112.11
          DNS Domain: ~.
          DNSSEC NTA: 10.in-addr.arpa
                      16.172.in-addr.arpa
                      168.192.in-addr.arpa
                      17.172.in-addr.arpa
                      18.172.in-addr.arpa
                      19.172.in-addr.arpa
                      20.172.in-addr.arpa
                      21.172.in-addr.arpa
                      22.172.in-addr.arpa
                      23.172.in-addr.arpa
                      24.172.in-addr.arpa
                      25.172.in-addr.arpa
                      26.172.in-addr.arpa
                      27.172.in-addr.arpa
                      28.172.in-addr.arpa
                      29.172.in-addr.arpa
                      30.172.in-addr.arpa
                      31.172.in-addr.arpa
                      corp
                      d.f.ip6.arpa
                      home
                      internal
                      intranet
                      lan
                      local
                      private
                      test
...
```

> The output has been limited for privacy reasons.

Then finally, run the following command to verify that the DNS resolution is still working correctly:

```
$ dig pi-hole.net @127.0.0.53

; <<>> DiG 9.11.3-1ubuntu1.15-Ubuntu <<>> pi-hole.net @127.0.0.53
;; global options: +cmd
;; Got answer:
;; ->>HEADER<<- opcode: QUERY, status: NOERROR, id: 59201
;; flags: qr rd ra; QUERY: 1, ANSWER: 1, AUTHORITY: 0, ADDITIONAL: 1

;; OPT PSEUDOSECTION:
; EDNS: version: 0, flags:; udp: 65494
;; QUESTION SECTION:
;pi-hole.net.			IN	A

;; ANSWER SECTION:
pi-hole.net.		900	IN	A	178.128.134.214

;; Query time: 557 msec
;; SERVER: 127.0.0.53#53(127.0.0.53)
;; WHEN: Thu May 27 21:03:04 CEST 2021
;; MSG SIZE  rcvd: 56
```

> The IP address `127.0.0.53` is the stub DNS resolver address used by `systemd` instead of the classical `127.0.0.1`.

As you can see in the status, it says `NOERROR` and so means that it's normally working :grin:.

### Netplan

Go to your `/etc/netplan` config folder and edit the `YAML` config file:

```
sudo nano /etc/netplan/50-cloud-init.yaml
```

And add or change the following:

```yaml
nameservers:
    addresses: [1.2.3.4]
```

in the `ethernets` and/or `wifis` sections.

> Replace `1.2.3.4` by the real IP address of your __Pi-hole__ device.

Then save the file and run the following commands:

```bash
# Generate new config files
sudo netplan --debug generate

# Try the new changes
sudo netplan --debug try

# If the test worked, apply changes permanently
sudo netplan --debug apply
```

Now you can try if that worked:

```
dig pi-hole.net @1.2.3.4
```

> Replace `1.2.3.4` by the real IP address of your __Pi-hole__ device.

### NetworkManager

> Coming soon.

## References

* <https://learn.netdata.cloud/guides/monitor/pi-hole-raspberry-pi>
* <https://github.com/pi-hole/pi-hole/#one-step-automated-install>
* <https://github.com/pi-hole/AdminLTE>
* <https://docs.pi-hole.net/main/prerequisites/>
* <https://www.reddit.com/r/pihole/comments/8kw5z8/dashboard_default_password/>
* <https://docs.pi-hole.net/guides/misc/whitelist-blacklist/>
* <https://github.com/StevenBlack>
* <https://firebog.net/>
* <https://github.com/justdomains/blocklists>
* <https://github.com/Laicure/hosts>
* <https://docs.pi-hole.net/database/gravity/example/>
* <https://docs.pi-hole.net/ftldns/configfile/>
* <https://docs.pi-hole.net/ftldns/blockingmode/>
* <https://docs.pi-hole.net/ftldns/privacylevels/>
* <https://docs.pi-hole.net/guides/dns/unbound/>
* <https://learn.netdata.cloud/guides/collect-unbound-metrics>
* <https://learn.netdata.cloud/docs/agent/collectors/go.d.plugin/modules/unbound>
* <https://docs.pi-hole.net/guides/dns/upstream-dns-providers/>
* <https://wikileaks.org/wiki/Alternative_DNS>
* <https://gist.github.com/Jiab77/72c868ecebce1d0027258eeec53b5a0f>
* <https://wiki.archlinux.org/title/Stubby>
* <https://dnsprivacy.org/wiki/display/DP/Configuring+Stubby>
* <https://www.linuxbabe.com/ubuntu/ubuntu-stubby-dns-over-tls>
* <https://dfarq.homeip.net/dns-over-tls-protect-your-network-with-ubuntu/>
* <https://www.quad9.net/support/faq>
* <https://www.quad9.net/news/blog/enable-private-dns-using-quad9-on-android-9/>
* <https://dnsprivacy.org/wiki/display/DP/DNS+Privacy+-+The+Solutions#DNSPrivacyTheSolutions-DNS-over-TLS(DoT)>
* <https://dnsprivacy.org/wiki/display/DP/DNS+Privacy+Test+Servers>