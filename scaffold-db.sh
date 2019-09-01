#!/usr/bin/env bash
export scaffold_path=src/Etdb.UserService.Scaffolder/Etdb.UserService.Scaffolder.csproj
export dll_path=src/Etdb.UserService.Scaffolder/bin/Debug/netcoreapp3.0/Etdb.UserService.Scaffolder.dll
echo running context scaffold
dotnet build ${scaffold_path}
dotnet ${dll_path}
