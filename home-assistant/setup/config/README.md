## Setup the config

Hints to modularize your config:

- https://www.home-assistant.io/docs/configuration/splitting_configuration/
- https://community.home-assistant.io/t/where-is-config-information-for-utility-meter-stored/433577/2


## Simple

- https://www.home-assistant.io/docs/configuration/splitting_configuration/#modularization-and-granularity

One yaml for each entity type: `/root/homeassistant/configuration.yaml`

Put sub-configs at the same level dir.

```yaml
# REST
rest_command: !include rest.yaml

# Official CMD line:
command_line: !include command_line.yaml

# SSH Notification sound at RPi5:
# shell_command: https://www.home-assistant.io/integrations/shell_command
shell_command: !include shell_command.yaml
```

## Advanced

- https://www.home-assistant.io/docs/configuration/splitting_configuration/#advanced-usage

### only SINGLE entry per file!

Cannot use more then one entity in each file imported.

```yaml
automation: !include_dir_list automation/presence/
```

### Submodule unpack:

Will 'unpack' a few levels deep into config hierarchy.

```yaml
alexa:
  intents: !include_dir_named alexa/
```

OR just import files as whole:

```yaml
homeassistant:
  packages: !include_dir_named packages
```


### Single module call

Once one `automation:` imported, you cannot import another with the same key!

```yaml
automation: !include_dir_merge_list automation/
```

### Miltiple configs and entities

One key for all type of entitites, but can include miltiple files of this entity with multiple entities in each file.

```yaml
group: !include_dir_merge_named group/
```


## Extra:

- https://www.home-assistant.io/docs/configuration/splitting_configuration/#example-combine-include_dir_merge_list-with-automationsyaml


Keep manual config and UI both.

```yaml
# My own handmade automations
automation manual: !include_dir_merge_list automations/

# Automations I create in the UI
automation ui: !include automations.yaml
```