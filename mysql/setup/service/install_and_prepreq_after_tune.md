# Install
- https://kifarunix.com/install-mysql-8-on-oracle-linux/

`
dnf install mysql-server
sudo dnf install https://repo.mysql.com//mysql80-community-release-el9-1.noarch.rpm
`

# Run

`
sudo systemctl enable --now mysqld
`

# Check

`
systemctl status mysqld
systemctl restart mysqld
`

# Connnect

`
mysql --user=root --password
`

# Setup


`
sudo grep 'temporary password' /var/log/mysqld.log
sudo grep 'temporary password' /var/log/mysql/mysqld.log
`

# Users add

```
# Change\set root

CREATE USER 'root'@'%' IDENTIFIED BY '__INSERT_PASSWORD__';
CREATE USER 'root'@'localhost' IDENTIFIED BY '__INSERT_PASSWORD__';

# Set Rights, better in UI:
GRANT ALL PRIVILEGES ON *.* TO 'root'@'%' WITH GRANT OPTION;
update user set password=PASSWORD('__INSERT_PASSWORD__') where User='root';


# Add database users:
CREATE USER 'octo_user'@'%' IDENTIFIED BY '__INSERT_PASSWORD__';
CREATE USER 'octo_user'@'localhost' IDENTIFIED BY '__INSERT_PASSWORD__';

# Set Rights, better in UI:
GRANT ALL PRIVILEGES ON *.* TO 'octo_user'@'%' WITH GRANT OPTION;

#
CREATE USER 'celery_backend'@'%' IDENTIFIED BY '__INSERT_PASSWORD__';
CREATE USER 'celery_backend'@'localhost' IDENTIFIED BY '__INSERT_PASSWORD__';
```

## ERRORS:

### only_full_group_by

`django.db.utils.OperationalError: (1055, "Expression #1 of SELECT list is not in GROUP BY clause and contains nonaggregated column 'dev_copy.tests_last.id' which is not functionally dependent on columns in GROUP BY clause; this is incompatible with sql_mode=only_full_group_by")`

- http://johnemb.blogspot.com/2014/09/adding-or-removing-individual-sql-modes.html

```

SELECT @@sql_mode;

SET GLOBAL sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY',''));

SELECT @@sql_mode;
```

Or my.cnf

```
# New for Django:
sql-mode="STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO"
```
