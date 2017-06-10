#! /usr/bin/env bash

set -e

which dotnet

dotnet --info

dotnet restore

dotnet build

for proj in test/**/*.csproj
do
    dotnet test $proj
done
