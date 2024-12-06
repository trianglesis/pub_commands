# Home Assistant PostreSQL

- https://www.home-assistant.io/docs/backend/database/#query

```sql
SELECT
	STATES_META.ENTITY_ID,
	COUNT(*) AS COUNT
FROM
	STATES
	INNER JOIN STATES_META ON STATES.METADATA_ID = STATES_META.METADATA_ID
WHERE
	STATES_META.ENTITY_ID IN (
		'sensor.invertor_battery_charging_power_kwh',
		'sensor.invertor_batteries_charging_monthly_kwh'
	)
GROUP BY
	STATES_META.ENTITY_ID
ORDER BY
	ENTITY_ID DESC
```


## States

- Select states by entity ID (name), then id:

```sql
SELECT * FROM public.states_meta
WHERE entity_id = 'sensor.invertor_battery_charging_power_kwh'
```

- Now check states:

```sql
SELECT * FROM public.states
WHERE metadata_id = '775'
```

- More attributes to see

```sql
SELECT * FROM public.state_attributes
WHERE attributes_id = '21768'
```

Compose a full query to select state and attrs, or just states:

- Without attr
[Short](states/SELECT_states_by_entity_id.sql)

- With attr
[Full](states/SELECT_states_by_entity_id_FULL.sql)

As described at HA docs with extra steps:

- [Example from doc](states/SELECT_states_example.sql)

### DELETE

[Delete by entity id](states/DELETE_states_by_entity_id.sql)

## Statistics

Use a similar approach as above.

[Both stats](statistics/SELECT_statistics_by_entity_id.sql)

Or short query:

- Statistics for one sensor

```sql
SELECT
	*
FROM
	PUBLIC.STATISTICS_META
	LEFT JOIN STATISTICS ON STATISTICS_META.ID = STATISTICS.METADATA_ID
WHERE
	STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
```

- Statistics short term for one sensor

```sql
SELECT
	*
FROM
	PUBLIC.STATISTICS_META
	LEFT JOIN STATISTICS_SHORT_TERM ON STATISTICS_META.ID = STATISTICS_SHORT_TERM.METADATA_ID
WHERE
	STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
```


### DELETE

- Use sort selects as a refference:

[Delete](statistics/DELETE_statistics_by_entity_id.sql)

See Update to get rid of old values

### Update

- https://community.home-assistant.io/t/how-to-completely-wipe-sensor-history/441945/2

Better to set to null

Go to: `.storage/core.restore_state`
Find by `"state": {"entity_id":"sensor.invertor_batteries_charging_monthly_kwh"`

Check possible values for all with re: 
- `,"state":"[a-zA-Z0-9^"]+"`
- `"last_valid_state":\s{0,1}`

Change as below:

- "state":"0"
- "native_value": null,
- "last_valid_state": null

```json
{
    "state": {"entity_id":"sensor.invertor_battery_charging_power_kwh","state":"0","attributes":{"state_class":"total","source":"sensor.easun_easun_inverter_charging_power","unit_of_measurement":"kWh","device_class":"energy","friendly_name":"Invertor Battery Charging Power kWh"},"last_changed":"2024-12-06T14:31:05.847613+00:00","last_reported":"2024-12-06T14:31:05.847405+00:00","last_updated":"2024-12-06T14:31:05.847613+00:00","context":{"id":"01JEE639VQNYP6SPXG1H9K00TC","parent_id":null,"user_id":null}},
    "extra_data": {
    "native_value": null,
    "native_unit_of_measurement": "kWh",
    "source_entity": "sensor.easun_easun_inverter_charging_power",
    "last_valid_state": null
    },
    "last_seen": "2024-12-06T14:46:14.476792+00:00"
},
```

- Select to verify

1. Short

```sql
SELECT
	*
FROM
	PUBLIC.STATISTICS_META
	LEFT JOIN STATISTICS_SHORT_TERM ON STATISTICS_META.ID = STATISTICS_SHORT_TERM.METADATA_ID
WHERE
	STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
```

2. Long

```sql
SELECT
	*
FROM
	PUBLIC.STATISTICS_META
	LEFT JOIN STATISTICS ON STATISTICS_META.ID = STATISTICS.METADATA_ID
WHERE
	STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
```

Update SUM

1. Short

```sql
UPDATE STATISTICS_SHORT_TERM 
SET sum=sum-12.23
WHERE STATISTICS_SHORT_TERM.METADATA_ID IN 
(
    SELECT
        ID
    FROM
        PUBLIC.STATISTICS_META
    WHERE
        STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
);
```

2. Long

```sql
UPDATE STATISTICS 
SET sum=sum-12.23
WHERE STATISTICS.METADATA_ID IN 
(
    SELECT
        ID
    FROM
        PUBLIC.STATISTICS_META
    WHERE
        STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
);
```
