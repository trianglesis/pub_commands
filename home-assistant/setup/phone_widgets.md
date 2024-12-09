## Android

Use HA App and Widget template


```Jinja2
{% if is_state('sensor.easun_easun_operation_mode_id', '3') %}
<div style="text-align: center;">
<h1>🌑&nbsp;<font color="#b30000">Світла нема!</font></h1>
</div>
{% elif is_state('sensor.easun_easun_operation_mode_id', '2') %}
<div style="text-align: center;">
<h1>☀️&nbsp;<font color="#669900">Світло є!</font></h1>
</div>
{% else %}
Інвертор: {{states('sensor.easun_easun_operation_mode')}}{% endif %}
<div style="text-align: center;">
<h6>{{ time_since(states.sensor.easun_easun_operation_mode_id.last_changed) }}</h6>
<div>
<div style="text-align: start;">
<h6>
🔋&nbsp;{{states('sensor.easun_easun_battery_state_of_charge', rounded=False, with_unit=True)}}&nbsp;
〽️&nbsp;{{states('sensor.easun_easun_battery_average_voltage', rounded=False, with_unit=True)}}&nbsp;
📶&nbsp;{{states('sensor.easun_easun_battery_average_power', rounded=False, with_unit=True)}}&nbsp;
</h6>
</div>
```