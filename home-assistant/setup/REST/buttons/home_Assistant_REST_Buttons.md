# Make a REST call

Using: 
- https://www.home-assistant.io/dashboards/actions/
- https://www.home-assistant.io/dashboards/button/
- https://www.home-assistant.io/integrations/rest_command/
- https://www.home-assistant.io/docs/configuration/templating/

## Open a door via POST

Add token and URL into `secrets.yaml`

```yaml
# REST Secrets
pl_url: URL
pl_nonce: TOKEN
```

## REST

Make a rest action one for all.
Rest is imported in `configuration.yaml`

```yaml
# REST
rest_command: !include rest.yaml
```

Method: 

- Cannot concat a string with token yet in HA rest yaml!
  - Use: https://www.home-assistant.io/docs/configuration/templating/ 

```yaml
open_doors:
  url: !secret pl_url
  method: POST
  content_type: "application/x-www-form-urlencoded"
  payload: "dom={{dom}}&gate={{gate}}&mode={{mode}}&nonce={{nonce}}"
```

## Button

Add button with arguments for each door:

```yaml
show_name: true
show_icon: true
type: button
tap_action:
  action: perform-action
  target: {}
  data:
    dom: HOMENAME
    gate: GATENAME
    mode: ANOTHER_ARG
    nonce: TOKEN
  perform_action: rest_command.open_doors
icon: mdi:button-pointer
hold_action:
  action: none
icon_height: 48px
name: Button TST

```