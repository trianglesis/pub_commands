# Kali

## Start init

- <https://www.kali.org/docs/troubleshooting/common-minimum-setup/>

### GUI

Some GUI for tools

- <https://www.kali.org/docs/wsl/win-kex/>

```shell
sudo apt install -y kali-win-kex

# Start
wsl -d kali-linux kex --esm --wtstart -s
```

To start Win-KeX in Window mode with sound support, run either:

- Inside of Kali WSL: `kex --win -s`
- On Window’s command prompt: `wsl -d kali-linux kex --win -s`

To start Win-KeX in Enhanced Session Mode with sound support and ARM workaround, run either:

Inside of Kali WSL: `kex --esm --ip -s`
On Window’s command prompt: `wsl -d kali-linux kex --esm --ip -s`

To start Win-KeX in Seamless mode with sound support, run, run either:

Inside of Kali WSL: `kex --sl -s`
On Window’s command prompt: `wsl -d kali-linux kex --sl -s`

## Install just all

```shell
sudo apt install -y kali-linux-large
```
