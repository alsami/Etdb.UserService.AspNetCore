#!/usr/bin/env bash
echo sleeping 5 seconds before scaffolding database
sleep 5
echo running context scaffold
dotnet run --project src/Etdb.UserService.Scaffold/Etdb.UserService.Scaffold.csproj
