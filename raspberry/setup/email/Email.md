# Emails at RPi

- [Official doc](https://wiki.archlinux.org/title/msmtp)

```shell
sudo apt-get install msmtp msmtp-mta bsd-mailx apt-config-auto-update
sudo chown user:user /var/log/msmtp
```

## Config 

```shell
sudo vi /etc/msmtprc
```

- Add config according to your email provider

```text
# Set default values for all following accounts.
defaults
auth           on
tls            on
tls_starttls   on
tls_trust_file /etc/ssl/certs/ca-certificates.crt
logfile        /var/log/msmtp

# Gmail
account        emails_name
host           mail.adm.tools
port           25
auth           login
from           rp@your_post_domain.com
user           rp@your_post_domain.com
password       <PASSWORD>
# Set a default account
account default : emails_name
```

### Test mail

```shell
echo -e "Subject: UPS ALERT: $NOTIFYTYPE\n\nUPS: $UPSNAME\r\nAlert type: $NOTIFYTYPE\n\n\nups1: network\nups2: system\nups3: Powercom800W" | msmtp user@your.mail.com
# OR
echo "Subject: UPS ALERT $NOTIFYTYPE test!\r\n\nUPS $UPSNAME - $NOTIFYTYPE\n\n$BASIC_INFO\n\nEnd" | msmtp $EMAIL
```