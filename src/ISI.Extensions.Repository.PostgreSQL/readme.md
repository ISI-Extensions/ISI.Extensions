docker run --name pgsql -e POSTGRES_PASSWORD=test1234 -d  -p 5432:5432 -v E:/PostgresSQL/Data:/var/lib/postgresql/data postgres

docker exec -it pgsql psql -U postgres


CREATE ROLE masteradmin WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  CREATEDB
  CREATEROLE
  NOREPLICATION
  NOBYPASSRLS
  ENCRYPTED PASSWORD 'SCRAM-SHA-256$4096:LCpApgGxF7cZ+g1L1wgakg==$82SuBgNyZ9MOZXu/SwRfSBXqUA+rGlszxkGn+RK+/Yg=:kmFboulR01CoRVCmOLid0wpWoKigqaHvt9JOe/fcjsA=';

GRANT postgres TO masteradmin;








CREATE USER MasterAdmin --createdb --superuser --login --createrole;
ALTER USER MasterAdmin WITH PASSWORD '9ab831ceb061';
GRANT "Create role" TO MasterAdmin;
GRANT Create DB TO MasterAdmin;
GRANT postgres TO MasterAdmin;

CREATE USER MasterAdmin WITH ENCRYPTED PASSWORD '9ab831ceb061';


REVOKE ALL PRIVILEGES ON DATABASE postgres TO masteradmin;



CREATE USER masteradmin WITH ENCRYPTED PASSWORD '9ab831ceb061';
ALTER USER masteradmin WITH SUPERUSER; 
ALTER ROLE masteradmin CREATEDB	CREATEROLE;

GRANT ALL PRIVILEGES ON DATABASE postgres TO masteradmin;

ALTER USER youruser WITH SUPERUSER; 


docker run --name pgadmin -p 5051:80 -e "PGADMIN_DEFAULT_EMAIL=user@baeldung.com" -e "PGADMIN_DEFAULT_PASSWORD=baeldung" -d dpage/pgadmin4