SELECT 
    `states_meta`.`entity_id`,
    COUNT(`states_meta`.`entity_id`) as entities
FROM `homeassistant`.`states`, `homeassistant`.`states_meta`
WHERE `states`.`metadata_id` = `states_meta`.`metadata_id`
group by entity_id
ORDER BY entities DESC
LIMIT 25
;
