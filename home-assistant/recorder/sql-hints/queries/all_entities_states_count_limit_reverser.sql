SELECT `states_meta`.`entity_id`,
    COUNT(`states`.`metadata_id`) as entities
FROM `homeassistant`.`states`, `homeassistant`.`states_meta`
WHERE `states_meta`.`metadata_id` = `states`.`metadata_id`
GROUP BY `states`.`metadata_id`
order by entities desc
LIMIT 100
;
