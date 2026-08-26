SELECT `states`.`state_id`,
    `states`.`state`,
    `states`.`last_reported_ts`,
    `states`.`last_updated`,
    `states`.`last_updated_ts`,
    `states`.`attributes_id`,
    `states`.`metadata_id`,
    #`states_meta`.`metadata_id` as k_metadata_id,
    `states_meta`.`entity_id`,
    COUNT(`states_meta`.`entity_id`) as entities
FROM `homeassistant`.`states`, `homeassistant`.`states_meta`
WHERE `states`.`metadata_id` = `states_meta`.`metadata_id`
group by entity_id
ORDER BY entities DESC
;
