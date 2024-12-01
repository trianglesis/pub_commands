# Test and stress tests

```shell
sudo apt install stress-ng
stress-ng --cpu 4 --cpu-method fft 

# https://wiki.ubuntu.com/Kernel/Reference/stress-ng
stress-ng --matrix 1 -t 1m
stress-ng --matrix 0 -t 1m

# More info
stress-ng --mq 0 -t 30s --times --perf

# Temperature show
stress-ng --cpu 0 --tz -t 60

# Getting the CPU hot
stress-ng --matrix 0 --matrix-size 64 --tz  -t 60
# Forcing memory pressure
stress-ng --brk 2 --stack 2 --bigheap 2

```

# NVMe

- https://medium.com/@timothydmoody/how-to-set-up-and-benchmark-nvmes-on-the-raspberry-pi-5-b0d009f384e3
  

```shell
sudo apt-get install hdparm
sudo hdparm -t --direct /dev/nvme0n1
sudo curl https://raw.githubusercontent.com/TheRemote/PiBenchmarks/master/Storage.sh | sudo bash

```
