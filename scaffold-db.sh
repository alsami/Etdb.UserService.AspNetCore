#!/usr/bin/env bash

publish() {
    dotnet publish -o publish src/Etdb.UserService.Scaffolder 
}

execute() {
    ./publish/Etdb.UserService.Scaffolder 
}

publish
execute