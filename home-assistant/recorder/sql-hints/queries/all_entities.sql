SELECT `states`.`state_id`,
    `states`.`state`,
    `states`.`last_changed`,
    `states`.`last_changed_ts`,
    `states`.`last_reported_ts`,
    `states`.`last_updated`,
    `states`.`last_updated_ts`,
    `states`.`old_state_id`,
    `states`.`attributes_id`,
    `states`.`metadata_id`,
    #`states_meta`.`metadata_id` as k_metadata_id,
    `states_meta`.`entity_id`
FROM `homeassistant`.`states`, `homeassistant`.`states_meta`
WHERE `states`.`metadata_id` = `states_meta`.`metadata_id`
order by metadata_id DESC
;
