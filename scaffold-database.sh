#!/usr/bin/env bash
echo sleeping 5 seconds before scaffolding database
sleep 5
echo restoring and building
export scaffold_path=src/Etdb.UserService.Scaffold/Etdb.UserService.Scaffold.csproj
dotnet restore ${scaffold_path} && dotnet build ${scaffold_path}
echo running context scaffold
sudo dotnet run --project ${scaffold_path} --framework netcoreapp3.0
