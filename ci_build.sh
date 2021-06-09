#! /usr/bin/env bash

if [ -n "$TF_BUILD" ]
then
    # normalise our branches
    echo "Initialising CI branch state"
    git branch --track main origin/main
    case "$BUILD_SOURCEBRANCH" in
        refs/heads/*)
            branch_name=${BUILD_SOURCEBRANCH:11}
            git switch --force-create "$branch_name" HEAD
            ;;
        *)
            git switch --force-create "ci" HEAD
            ;;
    esac
fi

set -euox pipefail

dotnet --version --verbose

dotnet restore
dotnet build --no-restore --configuration Release
dotnet test --no-restore --no-build --configuration Release
dotnet pack --no-restore -o PublishOutput --configuration Release