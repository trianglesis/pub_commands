-- https://www.home-assistant.io/docs/backend/database/#query
SELECT
	STATES_META.ENTITY_ID,
	COUNT(*) AS COUNT
FROM
	STATES
	INNER JOIN STATES_META ON STATES.METADATA_ID = STATES_META.METADATA_ID
WHERE
	STATES_META.ENTITY_ID IN (
		'zone.tren',
		'sensor.invertor_batteries_charging_monthly_kwh',
		'sensor.invertor_battery_charging_power_kwh'
	)
GROUP BY
	STATES_META.ENTITY_ID
ORDER BY
	ENTITY_ID DESC