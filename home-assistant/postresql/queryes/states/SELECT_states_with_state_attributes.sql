SELECT
	*
FROM
	PUBLIC.STATE_ATTRIBUTES
WHERE
	PUBLIC.STATE_ATTRIBUTES.ATTRIBUTES_ID IN (
		SELECT
			PUBLIC.STATES.ATTRIBUTES_ID
		FROM
			PUBLIC.STATES_META
			LEFT JOIN STATES ON PUBLIC.STATES.METADATA_ID = PUBLIC.STATES_META.METADATA_ID
			LEFT JOIN STATE_ATTRIBUTES ON PUBLIC.STATE_ATTRIBUTES.ATTRIBUTES_ID = PUBLIC.STATES.ATTRIBUTES_ID
		WHERE
			STATES_META.ENTITY_ID IN (
				'sensor.invertor_batteries_charging_monthly_kwh',
				'sensor.invertor_battery_charging_power_kwh'
			)
	);