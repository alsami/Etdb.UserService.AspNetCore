# Dev Environment

`dotnet user-secrets set "RedisConfiguration:Connection" "<<host-address>>:<<host-port>>" --id "Etdb_UserService"`

`dotnet user-secrets set "DocumentDbContextOptions:DatabaseName" "<<your-db-name>>" --id "Etdb_UserService"`

`dotnet user-secrets set "DocumentDbContextOptions:ConnectionString" "mongodb://<<your-user-name>>:<<your-password>>@<<host-address>>:<<host-port>>" --id "Etdb_UserService"`