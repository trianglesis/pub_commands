##  

- https://www.home-assistant.io/integrations/utility_meter#yaml-configuration
- https://www.home-assistant.io/integrations/utility_meter#action-utility_meterreset
- https://community.home-assistant.io/t/reset-utility-meter-at-particular-time-of-day/521035/5

## Core config


### Utiliti meters

At: `.storage cat core.config_entries | grep utility_meter`

Do not change, just read.

Example json:

```json
{
    "created_at": "2024-12-04T10:06:37.652377+00:00",
    "data": {},
    "disabled_by": null,
    "discovery_keys": {},
    "domain": "utility_meter",
    "entry_id": "01JE8J5KJMCG74S3VVP063W866",
    "minor_version": 1,
    "modified_at": "2024-12-04T10:06:37.652385+00:00",
    "options": {
        "always_available": false,
        "cycle": "none",
        "delta_values": false,
        "name": "Invertor Batteries Charge Net kWh (UI)",
        "net_consumption": true,
        "offset": 0,
        "periodically_resetting": false,
        "source": "sensor.invertor_battery_average_power_kwh",
        "tariffs": []
    },
    "pref_disable_new_entities": false,
    "pref_disable_polling": false,
    "source": "user",
    "title": "Invertor Batteries Charge Net kWh (UI)",
    "unique_id": null,
    "version": 2
}
```


## Config yaml:

### Counters

Count the power consumption of the inverter in three instances:

- Battery power Increment\Decrement values -\+ kWh
  - Can caltulate estimated kWh usage from batteries, how many left, how charged.
- Invertor power Increment\Decrement values -\+ kWh
  - Can show power losses during charge cycles, just for info.
- Mains Invertor power Usage - simple kWh per time with 0 values during blackouts.
  - Can show the full power usage by the invertor from the Main grid

#### 1. Invertor meters

```yaml
sensor:
  # Invertor Battery power Increase\Decrease
  # Use in utility meter as NET value
  # Count the kWh in the battery on charge\discharge
  # used in utility meter id: 'invertor_battery_charge_auto_kwh'
  # reset utility meter at full charge
  - platform: integration
    name: "Invertor Battery Average Power kWh"
    source: sensor.easun_easun_battery_average_power
    unique_id: invertor_battery_average_power
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  # Full inverter power comsumption: Increase\Decrease
  # Cycle mothtly at utility meter
  # Will show unequality in INCR\DECR values
  # overtime due to circuit power loses
  - platform: integration
    name: "Invertor Average Power kWh"
    source: sensor.easun_easun_average_inverter_power
    unique_id: invertor_average_power
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  # Full invertor power consumption overall.
  # Not NET, can be 0
  - platform: integration
    name: "Invertor Mains Power kWh"
    source: sensor.easun_easun_average_mains_power
    unique_id: invertor_mains_power
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  # Only tracking when charging.
  # 100% charged battery = 0 charging power
  - platform: integration
    name: "Invertor Battery Charging Power kWh"
    source: sensor.easun_easun_inverter_charging_power
    unique_id: invertor_battery_charging_power
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  # Make able to consume battery counters
  # See packages/sensors/easun_in_out.yaml
  # https://community.home-assistant.io/t/home-battery-on-energy-dashboard/463059/6
  - platform: integration
    name: "EASUN Battery In kWh"
    source: sensor.easun_battery_in
    unique_id: invertor_battery_charging_kwh
    method: left
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    name: "EASUN Battery Out kWh"
    source: sensor.easun_battery_out
    unique_id: invertor_battery_discharging_kwh
    method: left
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1
```

#### 2. Other devices meters


```yaml
sensor:
  ## Other
  ##
  - platform: integration
    source: sensor.washing_machine_tuya_din_power
    unique_id: washing_machine_tuya_din_power_kwh
    name: "Washing Machine kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.dishwasher_fridge_power
    unique_id: dishwasher_fridge_power_kwh
    name: "Dishwasher Fridge kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.conditioner_tuya_din_power
    unique_id: conditioner_watt_kwh
    name: "Conditioner kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.tuya_cloud_kitchen_1_tyua_power
    unique_id: tuya_cloud_kitchen_1_tyua_power_kwh
    name: "Kettle kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.coffee_tuya_power
    unique_id: coffee_tuya_power_kwh
    name: "Coffee kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.custom_1_power
    unique_id: custom_1_power_kwh
    name: "custom_1_power kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.custom_2_power
    unique_id: custom_2_power_kwh
    name: "custom_2_power kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.custom_3_power
    unique_id: custom_3_power_kwh
    name: "custom_3_power kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  - platform: integration
    source: sensor.tuya_cloud_towel_power
    unique_id: tuya_cloud_towel_power
    name: "tuya_cloud_towel_power kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  # Do not include in total, we have a Fridge+Dishwasher
  - platform: integration
    source: sensor.dish_washer_watt
    unique_id: dish_washer_watt_kwh
    name: "Dishwasher kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  # Counter only for Fridge without dishwasher
  # packages/sensors/fridge_without_dishwasher.yaml
  - platform: integration
    source: sensor.fridge_power
    unique_id: fridge_power_kwh
    name: "Fridge kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

  # Counter for SUM of all power
  # packages/sensors/total_consumption_sum.yaml
  - platform: integration
    source: sensor.total_consumption
    unique_id: total_consumption
    name: "Total Consumption kWh"
    unit_prefix: k
    unit_time: h
    round: 2
    max_sub_interval:
      minutes: 1

```

### Meters

**NOTE:** Utility meter uses sensor by its name in snake_case, cannot use `unique_id`, dont know why.

Consists of different meters each:
- Check the battery kWh charged and discharged
  - NET - can be positive\negative
  - reset by automation at battery SoC 100%
- Invertor power usage net, to see power losses
  - NET - can be positive\negative
  - reset by cycle
- Invertor full power usage from main grid
  - can be zero on blackouts
  - montly
  - weekly
  - full


`utility_meters`

```yaml
# Invertor meters
# Source is the name of sensor.integration in snake_case

# https://community.home-assistant.io/t/reset-utility-meter-at-particular-time-of-day/521035/5
utility_meter:
  # Dont use cycle, reset by Automation: utility_meter.calibrate
  # Count +\- of a battery cycle, reset on battery full charge
  # by automation.
  # source: sensor.easun_easun_battery_average_power
  invertor_battery_charge:
    source: sensor.invertor_battery_average_power_kwh
    name: Invertor Batteries Charge Net kWh
    unique_id: invertor_battery_charge_net_kwh
    net_consumption: true

  # DISCHARGE
  # Same as above, but only as monthly counter and not NET
  # use for HA battery system
  # source: sensor.easun_easun_battery_average_power
  invertor_battery_charge_monthly:
    source: sensor.invertor_battery_average_power_kwh
    name: Invertor Batteries Charge (monthly) kWh
    unique_id: invertor_battery_charge_monthly_kwh
    periodically_resetting: true
    cycle: monthly

  # RECHARGE
  # Only active when battery is charging
  # use for HA battery system
  # source: sensor.easun_easun_inverter_charging_power
  invertor_battery_charging_monthly:
    source: sensor.invertor_battery_charging_power_kwh
    name: Invertor Batteries Charging (monthly) kWh
    unique_id: invertor_battery_charging_monthly_kwh
    periodically_resetting: true
    cycle: monthly

  # Count +\- of the inverter cycle, reset monthly
  # Will show inequality due to power loss in the system.
  # source: sensor.easun_easun_average_inverter_power
  invertor_power_net:
    source: sensor.invertor_average_power_kwh
    name: Invertor Power Consumption Net kWh
    unique_id: invertor_power_consumption_net_kwh
    net_consumption: true
    cycle: monthly

  # Just invertor power consumption from the main network
  # source: sensor.easun_easun_average_mains_power
  invertor_power_monthly_sum:
    source: sensor.invertor_mains_power_kwh
    name: Invertor Main Power Consumption (monthly)
    unique_id: invertor_main_power_monthly
    periodically_resetting: true
    cycle: monthly

  # source: sensor.easun_easun_average_mains_power
  invertor_power_weekly_sum:
    source: sensor.invertor_mains_power_kwh
    name: Invertor Main Power Consumption (weekly)
    unique_id: invertor_main_power_weekly
    periodically_resetting: true
    cycle: weekly

  # source: sensor.easun_easun_average_mains_power
  invertor_power_all_sum:
    source: sensor.invertor_mains_power_kwh
    name: Invertor Main Power Consumption (all)
    unique_id: invertor_main_power_all
    periodically_resetting: true

```

### Energy price:

```yaml
# https://community.home-assistant.io/t/how-to-use-a-sensor-for-calculate-my-energy-consumption-costs/545131/2
template:
  - sensor:
      - name: Price kWh
        unique_id: electricity_price_uah
        icon: mdi:currency-uah
        unit_of_measurement: UAH/kWh
        state: >
          {% if now().hour >= 0 and now().hour < 7 %}
            4.32
          {% else %}
            4.32
          {% endif %}
```

### Template sensor to count diff

Dishwasher and Fridge both are metered at DIN rail.
Dishwasher is also metered by smart socket.
Substract Dishwasher wattage from DIN rail to show Frigdge wattage only

```yaml
# packages/sensors/integration.yaml
template:
  - sensor:
      # Substract total power if dishwasher from dishwashwer+fridge DIN counter
      - name: Fridge power
        unique_id: fridge_without_dishwasher
        icon: mdi:counter
        device_class: power
        state_class: measurement
        unit_of_measurement: W
        state: >
          {% if states('sensor.dishwasher_fridge_power')|float >= 0 and states('sensor.tuya_cloud_dishwasher_power')|float >=0 %}
          {{ states('sensor.dishwasher_fridge_power')|float - states('sensor.tuya_cloud_dishwasher_power')|float }}
          {% else %}0{% endif %}
        availability: >
          {{ states('sensor.dishwasher_fridge_power')|is_number}}
```

### Total of all

Collect all actual power meter devices into one

```yaml
# packages/sensors/integration.yaml
# Include invertor MAINS power when charging and when inverter is on
template:
  - sensor:
      - name: Total Consumption
        icon: mdi:counter
        device_class: power
        state_class: measurement
        unit_of_measurement: W
        state: >
          {{  states('sensor.easun_easun_average_mains_power')|float(0) + 
              states('sensor.washing_machine_tuya_din_power')|float(0) + 
              states('sensor.dishwasher_fridge_power')|float(0) + 
              states('sensor.conditioner_tuya_din_power')|float(0) + 
              states('sensor.tuya_cloud_kitchen_1_tyua_power')|float(0) + 
              states('sensor.coffee_tuya_power')|float(0) + 
              states('sensor.custom_1_power')|float(0) + 
              states('sensor.custom_2_power')|float(0) + 
              states('sensor.custom_3_power')|float(0) + 
              states('sensor.tuya_cloud_towel_power')|float(0)
          }}
        availability: >
          {{  states('sensor.easun_easun_average_mains_power')|is_number and 
              states('sensor.washing_machine_tuya_din_power')|is_number and 
              states('sensor.dishwasher_fridge_power')|is_number and 
              states('sensor.conditioner_tuya_din_power')|is_number and 
              states('sensor.tuya_cloud_kitchen_1_tyua_power')|is_number and 
              states('sensor.coffee_tuya_power')|is_number and 
              states('sensor.custom_1_power')|is_number and 
              states('sensor.custom_2_power')|is_number and 
              states('sensor.custom_3_power')|is_number and 
              states('sensor.tuya_cloud_towel_power')|is_number
          }}
```

### Battery module

Read: 

- https://www.home-assistant.io/integrations/template/#sensor
- https://developers.home-assistant.io/docs/core/entity/sensor/#available-state-classes
- https://www.home-assistant.io/integrations/sensor#device-class
- https://www.home-assistant.io/docs/energy/faq/#troubleshooting-missing-entities
- https://community.home-assistant.io/t/home-battery-on-energy-dashboard/463059/2

Seems OK:
- https://community.home-assistant.io/t/home-battery-on-energy-dashboard/463059/6

Preprequisits:
1. Make a template sensor to filter out negative float numbers and other cleaning (below)
   1. Based on actual invertor sensor fro, ESP32 module: `sensor.easun_easun_battery_average_power`
2. Make Invertor counter for Wattage in or out of batteries: [see](#1-invertor-meters)
   1. Based on the step 1. from current module: `sensor.easun_battery_in` and `sensor.easun_battery_out`
3. Add in the battery section at energy page



Invertor reports a battery charge\discharge as a counter with pos\negative values.
Convert negative value to ABS positive and report as a separate discharge sensor.
Report charge sensor for convinience by sorting out any negative float by reporting zero instead.


```yaml
# https://community.home-assistant.io/t/home-battery-on-energy-dashboard/463059/6
template:
  - sensor:
      # If positive int - report, show 0 for a negative int
      # easun_battery_in:
      - name: EASUN Battery In
        unique_id: easun_battery_in
        icon: mdi:battery-charging
        state_class: measurement
        device_class: power
        unit_of_measurement: W
        state: >
          {% if states('sensor.easun_easun_battery_average_power')|float >= 0 %}
          {{ states('sensor.easun_easun_battery_average_power')|float }}
          {% else %}0{% endif %}
        availability: >
          {{ states('sensor.easun_easun_battery_average_power')|is_number}}

        # If negative int - onvert to positive, show 0 for positive int
        # easun_battery_out:
      - name: EASUN Battery Out
        unique_id: easun_battery_out
        icon: mdi:battery-arrow-down
        state_class: measurement
        device_class: power
        unit_of_measurement: W
        state: >
          {% if states('sensor.easun_easun_battery_average_power')|float < 0 %}
          {{ states('sensor.easun_easun_battery_average_power')|float|abs }}
          {% else %}0{% endif %}
        availability: >
          {{ states('sensor.easun_easun_battery_average_power')|is_number}}

```

