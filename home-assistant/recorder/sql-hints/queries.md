# Info

- <https://smarthomescene.com/guides/optimize-your-home-assistant-database/>

## HA entities

```
{# {% set sorted = states | dictsort(by="value") %} #}
{% for state in states %}
  - {{ state.entity_id -}}
  {% break %}
{% endfor %}
```

## Some queries

- <https://www.home-assistant.io/integrations/sql/>
- <https://www.home-assistant.io/actions/sql.query/>

## Useful queries

You can use: <https://github.com/hassio-addons/app-phpmyadmin>
Or MySQL Workbench, dump or export database using PhPMyadmin

To get all records for all entities:

[All entities states](queries/all_entities_states.sql)

Will show with names from related table:

```log
27	on			1787701131.38095		1787692132.1233423		26	25	binary_sensor.advanced_ssh_web_terminal_running
1306	unavailable			1787692140.8391044		1787692140.8391044		1358	1304	binary_sensor.balcony_box_door_contact
30	off			1787701131.3809948		1787692132.1238434		29	28	binary_sensor.esphome_running
26	on			1787701131.3809352		1787692132.1232028		25	24	binary_sensor.mosquitto_broker_running
```

Now group by each entity and count overall: how many times this entity is saved its state in database:

[With count](queries/all_entities_states_count.sql)

Better version: first count all records, then get entities IDs
[Reverse - faster!](queries/all_entities_states_count_limit_reverser.sql)

Will see how many records for each:

```log
1934	on	1787692159.1642158		1787692143.8672094	1684	1630	automation.bedroom	480
1304	unavailable	1787692140.832167		1787692140.832167	1356	1302	binary_sensor.presencebedroom_presence	259
1956	on	1787692159.175016		1787692143.8700008	1705	1652	automation.sonoff_trvzb_external_temperature_sensor_calibration_bedroom	162
1955	on	1787692159.1749096		1787692143.8698776	1704	1651	automation.sonoff_trvzb_external_temperature_sensor_calibration_kitchen	152

```

We dont need to many, use this result to exclude entities from recorder at `configuration.yaml`

Also use this for automation to purge old records we no longer need.