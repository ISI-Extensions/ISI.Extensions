docker run -d --name oracle-db -p 1521:1521 -p 5500:5500 -e ORACLE_PWD=test1234 -v E:/Oracle/Data:/opt/oracle/oradata container-registry.oracle.com/database/express:21.3.0-xe


