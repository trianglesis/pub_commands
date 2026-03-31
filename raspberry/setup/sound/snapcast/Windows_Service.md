# Install with NSSM

## Check sound devices

```shell
E:\Install\Services\nssm-2.24\win64>E:\Install\Snapclient\snapclient_win64\snapclient.exe -l
0: default
USB (USB Audio Device)

1: {0.0.0.00000000}.{11d4f837-f638-4ee5-bae4-05de724822f7}
Speakers (Steam Streaming Speakers)

2: {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6}
USB (USB Audio Device)

3: {0.0.0.00000000}.{f352e4fd-412d-4026-8bf7-f7e3921f3166}
Speakers (Steam Streaming Microphone)

```

Use 0 as default, better.

## Check options

```shell
E:\Install\Snapclient\snapclient.exe --help
Usage: snapclient [options...] [url]

 With 'url' = <tcp|ws|wss>://<snapserver host or IP>[:port]
 For example: 'tcp://192.168.1.1:1704', or 'ws://homeserver.local'

  --help                              Produce help message
  -v, --version                       Show version number
  -h, --host arg                      (deprecated, use [url]) Server hostname or ip address
  -p, --port arg (=1704)              (deprecated, use [url]) Server port
  -i, --instance arg (=1)             Instance id when running multiple instances on the same host
  --hostID arg                        Unique host id, default is MAC address
  --cert arg                          Client certificate file (PEM format)
  --cert-key arg                      Client private key file (PEM format)
  --key-password arg                  Key password (for encrypted private key)
  --server-cert [=arg(="default certificates")]
                                      Verify server with CA certificate (PEM format)
  -l, --list                          List PCM devices
  -s, --soundcard arg (=default)      Index or name of the PCM device
  --latency arg (=0)                  Latency of the PCM device
  --sampleformat arg                  Resample audio stream to <rate>:<bits>:<channels>
  --player arg (=wasapi)              wasapi|file[:<options>|?]
  --sharingmode arg (=shared)         Audio mode to use [shared|exclusive]
  --mixer arg (=software)             software|none|?[:<options>]
  --logsink arg                       Log sink [null,system,stdout,stderr,file:<filename>]
  --logfilter arg (=*:info)           Log filter <tag>:<level>[,<tag>:<level>]* with tag = * or <log tag> and level = [trace,debug,info,notice,warning,error,fatal]
```


## Compose CMD


```shell
# DEBUG
E:\Install\Snapclient\snapclient.exe --latency 250 --soundcard 0 --instance 1 --logsink file:Y:\\logs\\home-mb\\client.log --logfilter snapclient:debug --hostID HomeMB tcp://192.168.1.11:1704

# Normal
E:\Install\Snapclient\snapclient.exe --latency 250 --soundcard 0 --instance 1 --logsink file:"E:\Install\Snapclient\\client.log" --logfilter=*:error --hostID HomeMB tcp://192.168.1.11:1704


E:\Install\Services\nssm-2.24\win64\nssm.exe edit snapclient
E:\Install\Services\nssm-2.24\win64\nssm.exe install snapclient
```


- `E:\Services\snapclient_win64\snapclient.exe --host 192.168.1.11 --port 1704 --hostID SashaPC`
- `E:\Install\Snapclient\snapclient_win64\snapclient.exe --host 192.168.1.11 --port 1704 --soundcard {0.0.0.00000000}.{f87bd6dd-c279-4c57-8ed0-7e72d603a69a} --hostID HomeMB`

{0.0.0.00000000}.{f87bd6dd-c279-4c57-8ed0-7e72d603a69a}

E:\Services\nssm-2.24\win64>nssm.exe install snapclient
Service "snapclient" installed successfully!

E:\Services\nssm-2.24\win64>nssm.exe start snapclient
snapclient: START: The operation completed successfully.

E:\Services\nssm-2.24\win64>nssm.exe status snapclient
SERVICE_RUNNING

# Home MB

E:\Install\Snapclient\snapclient_win64>snapclient.exe -l
0: default
Speakers (USB Audio Device)

1: {0.0.0.00000000}.{11d4f837-f638-4ee5-bae4-05de724822f7}
Speakers (Steam Streaming Speakers)

2: {0.0.0.00000000}.{4e15512d-1b29-4181-ae9f-71e27525e783}
LG TV SSCR2 (Intel(R) Display Audio)

3: {0.0.0.00000000}.{f352e4fd-412d-4026-8bf7-f7e3921f3166}
Speakers (Steam Streaming Microphone)

4: {0.0.0.00000000}.{f87bd6dd-c279-4c57-8ed0-7e72d603a69a}
Speakers (USB Audio Device)
