DELETE FROM PUBLIC.STATISTICS_SHORT_TERM
WHERE
	PUBLIC.STATISTICS_SHORT_TERM.METADATA_ID IN (
		SELECT
			ID
		FROM
			PUBLIC.STATISTICS_META
		WHERE
			STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
	);

DELETE FROM PUBLIC.STATISTICS
WHERE
	PUBLIC.STATISTICS.METADATA_ID IN (
		SELECT
			ID
		FROM
			PUBLIC.STATISTICS_META
		WHERE
			STATISTICS_META.STATISTIC_ID = 'sensor.invertor_battery_charging_power_kwh'
	);