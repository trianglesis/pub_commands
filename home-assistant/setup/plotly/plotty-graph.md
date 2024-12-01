https://plotly.com/python/reference/layout/

```yaml
type: custom:plotly-graph
entities:
  - entity: sensor.easun_easun_battery_average_voltage
    name: Batt V
    show_value: true
    filters:
      - filter: i=1
      - resample: 10s
    line:
      width: 1
      color: orange
  - entity: sensor.easun_easun_battery_state_of_charge
    name: Batt SoC
    show_value: true
    filters:
      - filter: i>=1
      - resample: 10s
    line:
      width: 1
      color: green
  - entity: number.easun_easun_battery_low_voltage_protection_point_in_off_grid_mode
    name: Batt low V
    line:
      width: 2
      dash: dot
      color: red
  - entity: sensor.easun_easun_operation_mode
    name: Mode
    filters:
      - filter: i=1
      - resample: 10s
    fill: tozeroy
    line:
      color: gray
      dash: dot
      width: 1
  - entity: ""
    name: Now
    yaxis: y9
    showlegend: false
    line:
      width: 1
      dash: dot
      color: deepskyblue
    x: $ex [Date.now(), Date.now()]
    "y":
      - 0
      - 1
hours_to_show: 6
time_offset: 15m
refresh_interval: 15
logarithmic_scale: true
layout:
  yaxis9:
    visible: false
    fixedrange: true
  dragmode: pan
  height: 350
  margin:
    b: 90
  legend:
    itemwidth: 50
  xaxis:
    rangeselector:
      "y": "-.3"
      buttons:
        - count: 20
          step: minute
        - count: 1
          step: hour
        - count: 4
          step: hour
        - count: 12
          step: hour
        - count: 1
          step: day
        - count: 2
          step: day
        - count: 7
          step: day
config:
  scrollZoom: false
  doubleClickDelay: 600
```


```yaml
type: custom:plotly-graph
entities:
  - entity: sensor.easun_easun_battery_average_power
    name: Batt Power
    filters:
      - filter: i>=1
      - resample: 15s
    show_value: true
    line:
      width: 1
      dash: dot
      color: blue
  - entity: sensor.easun_easun_inverter_charging_power
    name: Chrg Power
    filters:
      - filter: i=1
      - resample: 15s
    line:
      width: 1
      dash: dot
      color: cyan
  - entity: sensor.easun_easun_output_active_power
    name: Act Power
    show_value: true
    filters:
      - filter: i>=1
      - resample: 15s
    line:
      width: 1
      color: yellow
  - entity: sensor.easun_easun_battery_state_of_charge
    filters:
      - resample: 15s
    name: Batt SoC
    line:
      width: 1
      dash: dot
      color: green
  - entity: sensor.easun_easun_rated_power
    filters:
      - filter: i=1
      - resample: 15s
    name: Rated W
    line:
      width: 2
      dash: dot
      color: red
  - entity: sensor.easun_easun_operation_mode
    name: Mode
    filters:
      - filter: i=1
      - resample: 10s
    fill: tozeroy
    line:
      color: gray
      dash: dot
      width: 1
  - entity: ""
    name: Now
    yaxis: y9
    showlegend: false
    line:
      width: 1
      dash: dot
      color: deepskyblue
    x: $ex [Date.now(), Date.now()]
    "y":
      - 0
      - 1
hours_to_show: 6
time_offset: 15m
refresh_interval: 15
logarithmic_scale: true
layout_options:
  grid_columns: 5
  grid_rows: 5
fit_y_data: true
min_y_axis: 10
layout:
  yaxis9:
    visible: false
    fixedrange: true
  dragmode: pan
  height: 350
  margin:
    b: 90
  xaxis:
    rangeselector:
      "y": "-.3"
      buttons:
        - count: 20
          step: minute
        - count: 1
          step: hour
        - count: 4
          step: hour
        - count: 12
          step: hour
        - count: 1
          step: day
        - count: 7
          step: day

```

### Show blackout

```yaml
  - entity: sensor.easun_easun_operation_mode
    name: Mode
    filters:
      - filter: i>1
      - resample: 60s
    fill: tozeroy
    line:
      color: gray
      dash: dot
      width: 1
```


### Show Wh

```yaml
  - entity: sensor.easun_easun_output_active_power
    name: Wh
    show_value: true
    filters:
      - filter: i=1
      - resample: 15s
      - integrate: h
    line:
      dash: dot
      width: 1
      color: white
```

### Show Text, no line with markers

```yaml
  - entity: sensor.easun_easun_battery_state_of_charge
    showlegend: false
    name: ""
    mode: markers+text
    filters:
      - filter: i>1
      - resample: 30m
    textposition: top right
    texttemplate: " %{y:.0f}"
    textfont:
      color: rgba ( 0, 221, 51 , 1)
      size: 9
    marker:
      size: 6
      color: rgba(  0, 221, 51  , 1)
    yaxis: y1
```


### Show Text, no line with markers Max and Min

```yaml
  - entity: sensor.easun_easun_battery_state_of_charge
    showlegend: false
    statistic: min
    period:
      0s: 5minute
      6h: 5minute # when the visible range is ≥ 6 hrs
      12h: hour # when the visible range is ≥ 6 hrs
      24h: hour # when the visible range is ≥ 1 day, use the `hour` period
      7d: day # from 7 days on, use `day`
      6M: week # from 6 months on, use weeks. Note Uppercase M! (lower case m means minutes)
      1y: month # from 1 year on, use `month
    name: ""
    mode: markers+text+lines
    textposition: bottom right
    texttemplate: " %{y:.0f}"
    textfont:
      color: darkgreen
      size: 9
    marker:
      size: 6
      color: darkgreen
    line:
      shape: spline
      dash: dot
    yaxis: y1
```

#### Battery SoC

```yaml
  - entity: sensor.easun_easun_battery_state_of_charge
    showlegend: false
    statistic: min
    period:
      0s: 5minute
      1h: 5minute
      4h: hour
      24h: hour
      7d: day
      6M: week
      1y: month
    name: ""
    mode: markers+text+lines
    textposition: bottom right
    texttemplate: " %{y:.0f}%"
    textfont:
      color: lightgreen
      size: 10
    marker:
      size: 4
      color: darkgreen
    line:
      shape: spline
      dash: dot
```

#### Battery Volt

```yaml
  - entity: sensor.easun_easun_battery_average_voltage
    showlegend: false
    statistic: min
    period:
      0s: 5minute
      1h: 5minute
      4h: hour
      24h: hour
      7d: day
      6M: week
      1y: month
    name: ""
    mode: markers+text+lines
    textposition: bottom right
    texttemplate: " %{y:.1f}"
    textfont:
      color: darkorange
      size: 10
    marker:
      size: 4
      color: darkorange
    line:
      shape: spline
      dash: dot
    yaxis: y1
```