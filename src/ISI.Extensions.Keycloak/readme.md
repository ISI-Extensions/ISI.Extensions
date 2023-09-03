  keycloak01:
    restart: always
    container_name: keycloak01
    image: quay.io/keycloak/keycloak:latest
    ports:
    - '8090:8080'
    networks:
      - wrs-default-network
    environment:
      - KEYCLOAK_ADMIN=admin
      - KEYCLOAK_ADMIN_PASSWORD=admin
      - KC_DB=postgres
      - KC_DB_URL=jdbc:postgresql://172.17.0.2:5432/keycloakdatabase
      - KC_DB_USERNAME=keycloakuser
      - KC_DB_PASSWORD=a48838d4-8131-48ea-b7dd-66a433a6491b
      - PROXY_ADDRESS_FORWARDING=true
      - KC_HOSTNAME_URL=https://keycloak.isi-net.com
      - KC_HOSTNAME_ADMIN_URL=https://keycloak.isi-net.com
      - KC_HOSTNAME_STRICT_HTTPS=true

    entrypoint: ["/opt/keycloak/bin/kc.sh"]
    command: start --proxy=edge


CREATE DATABASE keycloakdatabase;
CREATE USER keycloakuser ENCRYPTED PASSWORD 'a48838d4-8131-48ea-b7dd-66a433a6491b';;
GRANT ALL PRIVILEGES ON DATABASE keycloakdatabase TO keycloakuser;
\c keycloakdatabase
GRANT ALL ON SCHEMA public TO keycloakuser;

ALTER USER keycloakuser PASSWORD 'a48838d4-8131-48ea-b7dd-66a433a6491b';
docker run --name keycloak -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:latest /opt/keycloak/bin/kc.sh start-dev --db-url=jdbc:postgresql://localhost:5432/keycloak --db-username=postgres --db-password=postgres --https-key-store-file=server.keystore --https-key-store-password=secret



docker run --name keycloak -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin -e KC_DB=postgres -e KC_DB_URL=jdbc:postgresql://localhost:5432/keycloakdatabase -e KC_DB_USERNAME=keycloakuser -e KC_DB_PASSWORD=a48838d4-8131-48ea-b7dd-66a433a6491b quay.io/keycloak/keycloak:latest start-dev
docker run --name keycloak -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin -e KC_DB=postgres -e KC_DB_URL=jdbc:postgresql://172.17.0.2:5432/keycloakdatabase -e KC_DB_USERNAME=keycloakuser -e KC_DB_PASSWORD=a48838d4-8131-48ea-b7dd-66a433a6491b quay.io/keycloak/keycloak:latest start-dev
