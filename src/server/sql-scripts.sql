--TODO: Create initdb scripts for migrations?


SELECT 'CREATE DATABASE keycloak;
CREATE USER keycloak WITH ENCRYPTED PASSWORD 'keycloak';
GRANT ALL PRIVILEGES ON DATABASE keycloak to keycloak;'
WHERE NOT EXISTS (
	SELECT FROM pg_database
	WHERE datname = 'keycloak'
)\gexec

--CREATE DATABASE EnterpriseSolution;
