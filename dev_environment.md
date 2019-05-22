# Dev Environment

## Running the tests

In order to run the tests, following user-secrets must be set and a redis- and mongodb-instance is required.

`dotnet user-secrets set "RedisCacheOptions:Configuration" "<host-address>:<host-port>" --id "Etdb_UserService"`

`dotnet user-secrets set "RedisCacheOptions:InstanceName" "<your-db-name>" --id "Etdb_UserService"`

`dotnet user-secrets set "DocumentDbContextOptions:ConnectionString" "mongodb://<your-user-name>:<your-password>@<host-address>>:<host-port>" --id "Etdb_UserService"`

`dotnet user-secrets set "DocumentDbContextOptions:DatabaseName" "Your-Db-Name" --id "Etdb_UserService"`