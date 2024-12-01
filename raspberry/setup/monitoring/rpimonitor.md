# Rpi Monitor

- https://github.com/XavierBerger/RPi-Monitor
- https://xavierberger.github.io/RPi-Monitor-docs/11_installation.html


## Install

```shell
sudo apt-get install dirmngr
sudo apt-key adv --recv-keys --keyserver keyserver.ubuntu.com 2C0D3C0F
```

```shell
sudo wget http://goo.gl/vewCLL -O /etc/apt/sources.list.d/rpimonitor.list
sudo apt-get update
sudo apt-get install rpimonitor
```


```shell
sudo /etc/init.d/rpimonitor update

```

Open
-  http://raspberrypi.local:8888


# Add TOP 3 proc table

```shell
cp /usr/share/rpimonitor/web/addons/top3/top3.cron /etc/cron.d/top3
```

## Add to the end of conf file:

```shell
sudo vi /etc/rpimonitor/data.conf
```

```text
# TOP 3
web.addons.1.title=Top3
web.addons.1.addons=top3
web.status.1.content.1.line.1=InsertHTML("/addons/top3/top3.html")
```

## Add custom services monitoring:

People examples:
  - https://xavierberger.github.io/RPi-Monitor-docs/31_configuration_examples.html

### Create a new conf include at the end of the general conf file:

```shell
sudo vi /etc/rpimonitor/data.conf
```

```text
# Custom services:
include=/etc/rpimonitor/template/custom_services.conf
```

#### Custom services content:

```text
########################################################################
# Extract information about Services
#
########################################################################

dynamic.4.name=rpimonitor_desc
dynamic.4.source=service rpimonitor status | grep "rpimonitor.service -"
dynamic.4.regexp=- (.*)
dynamic.5.name=rpimonitor_act
dynamic.5.source=service rpimonitor status | grep "Active: "
dynamic.5.regexp=(\(.*\))
dynamic.6.name=rpimonitor_runtime
dynamic.6.source=service rpimonitor status | grep "Active: "
dynamic.6.regexp=; (.*)

dynamic.7.name=smbd_desc
dynamic.7.source=service smbd status | grep "smbd.service - "
dynamic.7.regexp=- (.*)
dynamic.8.name=smbd_act
dynamic.8.source=service smbd status | grep "Active: "
dynamic.8.regexp=(\(.*\))
dynamic.9.name=smbd_runtime
dynamic.9.source=service smbd status | grep "Active: "
dynamic.9.regexp=; (.*)

dynamic.13.name=ssh_desc
dynamic.13.source=service ssh status | grep "ssh.service - "
dynamic.13.regexp=- (.*)
dynamic.14.name=ssh_act
dynamic.14.source=service ssh status | grep "Active: "
dynamic.14.regexp=(\(.*\))
dynamic.15.name=ssh_runtime
dynamic.15.source=service ssh status | grep "Active: "
dynamic.15.regexp=; (.*)

dynamic.16.name=xrdp_desc
dynamic.16.source=service xrdp status | grep "xrdp.service - "
dynamic.16.regexp=- (.*)
dynamic.17.name=xrdp_act
dynamic.17.source=service xrdp status | grep "Active: "
dynamic.17.regexp=(\(.*\))
dynamic.18.name=xrdp_runtime
dynamic.18.source=service xrdp status | grep "Active: "
dynamic.18.regexp=; (.*)

# My added
# NUT UpsMon Logger
dynamic.20.name=upslog_act
dynamic.20.source=service upslog status | grep "Active: "
dynamic.20.regexp=\:\s(.*)\s\(.*\)
dynamic.21.name=upslog_runtime
dynamic.21.source=service upslog status | grep "Active: "
dynamic.21.regexp=; (.*)

dynamic.23.name=nut_server_act
dynamic.23.source=service nut-server status | grep "Active: "
dynamic.23.regexp=(\(.*\))
dynamic.24.name=nut_server_runtime
dynamic.24.source=service nut-server status | grep "Active: "
dynamic.24.regexp=; (.*)

dynamic.26.name=nut_monitor_act
dynamic.26.source=service nut-monitor status | grep "Active: "
dynamic.26.regexp=(\(.*\))
dynamic.27.name=nut_monitor_runtime
dynamic.27.source=service nut-monitor status | grep "Active: "
dynamic.27.regexp=; (.*)

web.status.1.content.1.name=Services
web.status.1.content.1.icon=daemons.png
web.status.1.content.1.line.1="<style type=\"text/css\">.tg331 {border-collapse:collapse;border-spacing:0;}.tg331 tr:nth-child(even){background-color: #f2f2f2}.tg331 table{border: 0px solid #e9e9e9;}.tg331 td{font-family:Arial, sans-serif;font-size:14px;padding:12px 2px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;}.tg331 th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:12px 2px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;}.tg331 .tg331-yw4l{vertical-align:top;text-align: left;}.tg331 hr {display: block;margin-top: 0.5em;margin-bottom: 0.5em;margin-left: auto;margin-right: auto;border-style: inset; border-width: 1px;}</style><table width=\"100%\" class=\"tg331\"><tr><td><b>Status</b></td><td><b>Service Name</b></td><td><b>Service Description</b></td><td><b>Run Time</b></td></tr><tr><td>"+Label(data.rpimonitor_act,"=='(running)'","OK","success")+Label(data.rpimonitor_act,"!='(running)'","KO","danger")+"</td><td><b>rpimonitor</b></td><td>" + data.rpimonitor_desc + "</td><td>"+Label(data.rpimonitor_act,"=='(running)'",data.rpimonitor_runtime,"default")+Label(data.rpimonitor_act,"!='(running)'","-","default")+"</td></tr><tr><td>"+Label(data.smbd_act,"=='(running)'","OK","success")+Label(data.smbd_act,"!='(running)'","KO","danger")+"</td><td><b>smbd</b></td><td>" + data.smbd_desc + "</td><td>"+Label(data.smbd_act,"=='(running)'",data.smbd_runtime,"default")+Label(data.smbd_act,"!='(running)'","-","default")+"</td></tr><tr><td>"+Label(data.ssh_act,"=='(running)'","OK","success")+Label(data.ssh_act,"!='(running)'","KO","danger")+"</td><td><b>SSH</b></td><td>" + data.ssh_desc + "</td><td>"+Label(data.ssh_act,"=='(running)'",data.ssh_runtime,"default")+Label(data.ssh_act,"!='(running)'","-","default")+"</td></tr><tr><td>"+Label(data.xrdp_act,"=='(running)'","OK","success")+Label(data.xrdp_act,"!='(running)'","KO","danger")+"</td><td><b>xrdp</b></td><td>" + data.xrdp_desc + "</td><td>"+Label(data.xrdp_act,"=='(running)'",data.xrdp_runtime,"default")+Label(data.xrdp_act,"!='(running)'","-","default")+"</td></tr><tr><td>"+Label(data.upslog_act,"=='active'","OK","success")+Label(data.upslog_act,"!='active'","KO","danger")+"</td><td><b>upslog</b></td><td>" + "NUT logger" + "</td><td>"+Label(data.upslog_act,"=='active'",data.upslog_runtime,"default")+Label(data.upslog_act,"!='active'","-","default")+"</td></tr><tr><td>"+Label(data.nut_server_act,"=='(running)'","OK","success")+Label(data.nut_server_act,"!='(running)'","KO","danger")+"</td><td><b>nut_server</b></td><td>" + "NUT Server" + "</td><td>"+Label(data.nut_server_act,"=='(running)'",data.nut_server_runtime,"default")+Label(data.nut_server_act,"!='(running)'","-","default")+"</td></tr><tr><td>"+Label(data.nut_monitor_act,"=='(running)'","OK","success")+Label(data.nut_monitor_act,"!='(running)'","KO","danger")+"</td><td><b>nut_monitor</b></td><td>" + "NUT Monitor" + "</td><td>"+Label(data.nut_monitor_act,"=='(running)'",data.nut_monitor_runtime,"default")+Label(data.nut_monitor_act,"!='(running)'","-","default")+"</td></tr></table>"
```


# nVME Temperature:

- [Install cli](SSD_NVMe.md)
- Change the file `/etc/rpimonitor/template/temperature.conf`

```shell
sudo vi /etc/rpimonitor/template/temperature.conf
```

```text
# SSD NVME
dynamic.13.name=nvme_temp_1
dynamic.13.source=sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 1"
dynamic.13.regexp=Temperature\sSensor\s1\s+\:\s(\d+)
dynamic.13.postprocess=sprintf("%.2f", $1/1000)
dynamic.13.rrd=GAUGE

dynamic.14.name=nvme_temp_2
dynamic.14.source=sudo nvme smart-log /dev/nvme0n1 | grep -i "Temperature Sensor 2"
dynamic.14.regexp=Temperature\sSensor\s2\s+\:\s(\d+)
dynamic.14.postprocess=sprintf("%.2f", $1/1000)
dynamic.14.rrd=GAUGE

# Comment: 
# web.status.1.content.4.line.1=JustGageBar("CPU Temperature", data.soc_temp+"°C", 40, data.soc_temp, 80, 100, 80)

# SSD NVMe + CPU
web.status.1.content.4.line.1=JustGageBar("CPU Temperature", data.soc_temp+"°C", 40, data.soc_temp, 80, 100, 80)+" "+JustGageBar("NVMe sensor 1", data.nvme_temp_1+"°C", 40, data.nvme_temp_1, 80, 100, 80)+" "+JustGageBar("NVMe Sensor 2", data.nvme_temp_2+"°C", 40, data.nvme_temp_2, 80, 100, 80)


# SSD NVMe
web.statistics.1.content.9.name=NVMe Sensor 1
web.statistics.1.content.9.graph.1=nvme_temp_1
web.statistics.1.content.9.ds_graph_options.nvme_temp_1.label=NVMe sensor 1 t (°C)
web.statistics.1.content.10.name=NVMe Sensor 2
web.statistics.1.content.10.graph.1=nvme_temp_2
web.statistics.1.content.10.ds_graph_options.nvme_temp_2.label=NVMe sensor 2 t (°C)
```

# nVME info:

- [Install cli](SSD_NVMe.md)
- Change the file `/etc/rpimonitor/template/sdcard.conf`

```shell
sudo vi /etc/rpimonitor/template/temperature.conf
```

```text

# NVMe info
static.9.name=nvme_available_spare
static.9.source=sudo nvme smart-log /dev/nvme0n1 | grep -i "available_spare" | awk '{print $1,$3}'
static.9.regexp=available_spare\s(\d+\%)
static.10.name=nvme_read_total
static.10.source=sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Read"
static.10.regexp=Data\sUnits\sRead\s+\:\s\S+\s\((.*)\)
static.11.name=nvme_write_total
static.11.source=sudo nvme smart-log /dev/nvme0n1 | grep -i "Data Units Written"
static.11.regexp=Data\sUnits\sWritten\s+\:\s\S+\s\((.*)\)

# NVMe info outputs
web.status.1.content.7.line.1="Spare: <b>"+ data.nvme_available_spare + "</b> " + "Data Units Read: <b>"+ data.nvme_read_total + "</b> " + "Data Units Written: <b>"+ data.nvme_write_total + "</b> "

```

# Battery info:

Additionally see all stat files before:

```shell
ls -lah /var/lib/rpimonitor/stat
```

There may be an issue `.rrd is not a valid RRD archive!` with custom graphs, check your custom graph there and delete if needed.

### Create a new conf include at the end of the general conf file:

```shell
sudo vi /etc/rpimonitor/data.conf
```

```text
# UPS services:
include=/etc/rpimonitor/template/ups_services.conf
```

### Content

```shell
sudo vi /etc/rpimonitor/template/ups_services.conf
```

```text
dynamic.1.name=battery_low
dynamic.1.source=upsc Powercom800W@localhost battery.charge.low 2>&1 | grep -v '^Init SSL'
dynamic.1.regexp=(.*)

dynamic.2.name=battery_charge
dynamic.2.source=upsc Powercom800W@localhost battery.charge 2>&1 | grep -v '^Init SSL'
dynamic.2.regexp=(\d+)

# dynamic.2.postprocess=$1%100
# dynamic.2.rrd=GAUGE

dynamic.3.name=battery_charge_warning
dynamic.3.source=upsc Powercom800W@localhost battery.charge.warning 2>&1 | grep -v '^Init SSL'
dynamic.3.regexp=(.*)
dynamic.4.name=ups_status
dynamic.4.source=upsc Powercom800W@localhost ups.status 2>&1 | grep -v '^Init SSL'
dynamic.4.regexp=(.*)
dynamic.5.name=ups_test_result
dynamic.5.source=upsc Powercom800W@localhost ups.test.result 2>&1 | grep -v '^Init SSL'
dynamic.5.regexp=(.*)
dynamic.6.name=ups_batt_runtime
dynamic.6.source=upsc Powercom800W@localhost battery.runtime 2>&1 | grep -v '^Init SSL'
dynamic.6.regexp=(.*)

# Draw
web.status.1.content.1.name=UPS
web.status.1.content.1.icon=daemons.png
# ProgressBar
web.status.1.content.1.line.1='<b>Battery charge level</b>: ' + data.battery_charge
web.status.1.content.1.line.2=ProgressBar(data.battery_charge,100,50,30)
web.status.1.content.1.line.3='<b>Battery runtime left minutes EST:</b> ' + data.ups_batt_runtime
web.status.1.content.1.line.4=ProgressBar(data.ups_batt_runtime,800,50,10)
# Text
web.status.1.content.1.line.5="Battery low: <b>"+ data.battery_low + "</b>" + "<br>Battery warn: <b>"+ data.battery_charge_warning + "</b>" + "<br>UPS Status: <b>"+ data.ups_status + "</b>" + "<br>UPS Test: <b>"+ data.ups_test_result + "</b>"

# Stats
web.statistics.1.content.11.name=Battery charge
web.statistics.1.content.11.graph.1=battery_charge
web.statistics.1.content.11.ds_graph_options.battery_charge.label=Battery charge level
```

# Other

## Uncomment included modules:

```text
include=/etc/rpimonitor/template/version.conf
include=/etc/rpimonitor/template/uptime.conf
include=/etc/rpimonitor/template/cpu.conf
include=/etc/rpimonitor/template/temperature.conf
include=/etc/rpimonitor/template/memory.conf
include=/etc/rpimonitor/template/swap.conf
include=/etc/rpimonitor/template/sdcard.conf
include=/etc/rpimonitor/template/network.conf
```


##  Remove storage mounts you don't want to monitor

```shell
sudo vi /etc/rpimonitor/template/sdcard.conf
```

- boot section commented
- see commented line replaced by ="" below at the web.status section!

```text
static.7.name=sdcard_root_total
static.7.source=df /
static.7.regexp=\S+\s+(\d+).*\/$
static.7.postprocess=$1/1024

#static.8.name=sdcard_boot_total
#static.8.source=df /boot
#static.8.regexp=\S+\s+(\d+).*\/boot$
#static.8.postprocess=$1/1024

dynamic.6.name=sdcard_root_used
dynamic.6.source=df /
dynamic.6.regexp=\S+\s+\d+\s+(\d+).*\/$
dynamic.6.postprocess=$1/1024
dynamic.6.rrd=GAUGE

#dynamic.7.name=sdcard_boot_used
#dynamic.7.source=df /boot
#dynamic.7.regexp=\S+\s+\d+\s+(\d+).*\/boot$
#dynamic.7.postprocess=$1/1024
#dynamic.7.rrd=GAUGE

web.status.1.content.7.name=SD card
web.status.1.content.7.icon=sd.png
#web.status.1.content.7.line.1="<b>/boot</b> Used: <b>"+KMG(data.sdcard_boot_used,'M')+"</b> (<b>"+Percent(data.sdcard_boot_used,data.sdcard_boot_total,'M')+"</b>) Free: <b>"+KMG(data.sdcard_boot_total-data.sdcard_boot_used,'M')+ "</b> Total: <b>"+ KMG(data.sdcard_boot_total,'M') +"</b>"
#web.status.1.content.7.line.2=ProgressBar(data.sdcard_boot_used,data.sdcard_boot_total,60,80)
web.status.1.content.7.line.1=""
web.status.1.content.7.line.2=""
web.status.1.content.7.line.3="<b>/</b> Used: <b>"+KMG(data.sdcard_root_used,'M') + "</b> (<b>" + Percent(data.sdcard_root_used,data.sdcard_root_total,'M')+"</b>) Free: <b>"+KMG(data.sdcard_root_total-data.sdcard_root_used,'M')+ "</b> Total: <b>"+ KMG(data.sdcard_root_total,'M') + "</b>"
web.status.1.content.7.line.4=ProgressBar(data.sdcard_root_used,data.sdcard_root_total,60,80)

#web.statistics.1.content.3.name=Disks - boot
#web.statistics.1.content.3.graph.1=sdcard_boot_total
#web.statistics.1.content.3.graph.2=sdcard_boot_used
#web.statistics.1.content.3.ds_graph_options.sdcard_boot_total.label=Size of /boot (MB)
#web.statistics.1.content.3.ds_graph_options.sdcard_boot_total.color="#FF7777"
#web.statistics.1.content.3.ds_graph_options.sdcard_boot_used.label=Used on /boot (MB)
#web.statistics.1.content.3.ds_graph_options.sdcard_boot_used.lines={ fill: true }
#web.statistics.1.content.3.ds_graph_options.sdcard_boot_used.color="#7777FF"

web.statistics.1.content.4.name=Disks - root
web.statistics.1.content.4.graph.1=sdcard_root_total
web.statistics.1.content.4.graph.2=sdcard_root_used
web.statistics.1.content.4.ds_graph_options.sdcard_root_total.label=Size of / (MB)
web.statistics.1.content.4.ds_graph_options.sdcard_root_total.color="#FF7777"
web.statistics.1.content.4.ds_graph_options.sdcard_root_used.label=Used on / (MB)
web.statistics.1.content.4.ds_graph_options.sdcard_root_used.lines={ fill: true }
web.statistics.1.content.4.ds_graph_options.sdcard_root_used.color="#7777FF"
```



# Restart

```shell
sudo /etc/init.d/rpimonitor update
sudo systemctl restart rpimonitor.service
```