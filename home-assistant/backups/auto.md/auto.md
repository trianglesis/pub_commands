

# Backups


## Getting states

```yaml

When NAS is Up or Down
{{ states('sensor.truenas_system_uptime') }}

# Advanced_SSH_&_Web_Terminal_Update_from_23.0.2_to_23.0.2
{{ state_attr('update.advanced_ssh_web_terminal_update', 'friendly_name')|replace(' ', '_') }}_from_{{ state_attr('update.advanced_ssh_web_terminal_update', 'installed_version') }}_to_{{ state_attr('update.advanced_ssh_web_terminal_update', 'latest_version') }}

{{ integration_entities('Supervisor')
  | select('match', 'update\.(?!home_assistant)')
  | list }}

# ['update.advanced_ssh_web_terminal_update', 'update.esphome_update', 'update.mosquitto_broker_update', 'update.studio_code_server_update', 'update.pgadmin4_update', 'update.timescaledb_update', 'update.piper_update', 'update.vlc_update', 'update.zigbee2mqtt_update', 'update.music_assistant_beta_update', 'update.samba_share_update', 'update.yt_music_po_token_generator_update', 'update.network_ups_tools_update']


{{ integration_entities('Supervisor')
          | select('match', 'update')
          | reject('match', 'update\.home_assistant')
          | map('device_attr', 'identifiers')
          | flatten
          | reject('eq', 'hassio')
          | list }}

# ['a0d7b954_ssh', '5c53de3b_esphome', 'core_mosquitto', 'a0d7b954_vscode', '77b2833f_pgadmin4', '77b2833f_timescaledb', 'core_piper', 'core_vlc', '45df7312_zigbee2mqtt', 'd5369777_music_assistant_beta', 'core_samba', 'd5369777_ytm_po_token_generator', 'a0d7b954_nut']
```


## Actions

Partial backup for an addon
NOTE: The naming works but only in HA UI!

```yaml
action: hassio.backup_partial
data:
  homeassistant_exclude_database: true
  addons: |-
    {{ integration_entities('Supervisor')
        | select('match', 'update')
        | reject('match', 'update\.home_assistant')
        | map('device_attr', 'identifiers')
        | flatten
        | reject('eq', 'hassio')
        | list }}
  location: HA_Backup
  compressed: false
  name: >
    {{ state_attr('update.advanced_ssh_web_terminal_update', 'friendly_name')|replace(' ', '_') }}_from_{{ state_attr('update.advanced_ssh_web_terminal_update', 'installed_version') }}_to_{{ state_attr('update.advanced_ssh_web_terminal_update', 'latest_version') }}

```