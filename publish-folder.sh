#!/usr/bin/env bash
dotnet publish src/Etdb.UserService -c Release -o publish
cd publish
zip -r ../app.zip .
