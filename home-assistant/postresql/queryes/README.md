# Home Assistant PostreSQL

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
