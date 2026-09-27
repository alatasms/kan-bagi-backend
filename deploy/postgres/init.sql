-- One Postgres server, one database per service. Services never read each other's database.
CREATE DATABASE userservice;
CREATE DATABASE postservice;
CREATE DATABASE keycloak;
CREATE DATABASE matching;
