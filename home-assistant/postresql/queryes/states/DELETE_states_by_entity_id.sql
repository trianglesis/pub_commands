DELETE FROM PUBLIC.STATES
WHERE
	PUBLIC.STATES.METADATA_ID IN (
		SELECT
			PUBLIC.STATES_META.metadata_id 
		FROM
			PUBLIC.STATES_META
		WHERE
			PUBLIC.STATES_META.ENTITY_ID IN (
		'sensor.invertor_battery_charging_power_kwh',
		'sensor.invertor_battery_average_power_kwh',
		'sensor.invertor_batteries_charging_monthly_kwh'
	)
	);