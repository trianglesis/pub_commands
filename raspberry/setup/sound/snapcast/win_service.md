# Use NSSM

## MediaBox

```shell
E:\Install\Services\nssm-2.24\win64
nssm.exe edit snapclient

# E:\Install\Snapclient\snapclient_win64\snapclient.exe
# E:\Install\Snapclient\snapclient_win64
# --host 192.168.1.11 --port 1704 --soundcard {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6} --hostID MediaBox
# --host 192.168.1.11 --port 1704 --soundcard {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6} --hostID MediaBox 
E:\Install\Snapclient\snapclient_win64\snapclient.exe --host 192.168.1.11 --port 1704 --soundcard {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6} --logsink file:E:\Install\Snapclient\client.log --hostID HomeMB
E:\Install\Snapclient\snapclient_win64\snapclient.exe --soundcard {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6} --logsink file:E:\Install\Snapclient\client.log --hostID HomeMB tcp://192.168.1.11:1704
# Details: 
# Snapcast Client MediaBox started with NSSM

# Check soundcards

E:\Install\Snapclient\snapclient_win64\snapclient.exe -l

E:\Services\snapclient_win64\snapclient.exe --host 192.168.1.11 --port 1704 --hostID PersPC
# {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6}
E:\Install\Snapclient\snapclient_win64\snapclient.exe --host 192.168.1.11 --port 1704 --soundcard {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6} --hostID MediaBox


nssm.exe install snapclient
# Service "snapclient" installed successfully!

nssm.exe start snapclient
# snapclient: START: The operation completed successfully.

nssm.exe status snapclient
# SERVICE_RUNNING

nssm.exe restart snapclient
# snapclient: STOP: The operation completed successfully.
```

### Devices Media Box

```shell
E:\Install\Snapclient\snapclient_win64>snapclient.exe -l
0: default
Speakers (USB Audio Device)

1: {0.0.0.00000000}.{11d4f837-f638-4ee5-bae4-05de724822f7}
Speakers (Steam Streaming Speakers)

2: {0.0.0.00000000}.{4e15512d-1b29-4181-ae9f-71e27525e783}
LG TV SSCR2 (Intel(R) Display Audio)

3: {0.0.0.00000000}.{5e1d1eeb-46df-4083-ad7c-d3d986ba94d6}
Speakers (USB Audio Device)

4: {0.0.0.00000000}.{f352e4fd-412d-4026-8bf7-f7e3921f3166}
Speakers (Steam Streaming Microphone)

```