#! /usr/bin/env bash

set -euox pipefail

dotnet --version --verbose

dotnet restore
dotnet build --no-restore --configuration Release
dotnet test --no-restore --no-build --configuration Release
dotnet pack --no-restore -o PublishOutput --configuration Release