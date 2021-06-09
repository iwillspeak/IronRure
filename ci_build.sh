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

dotnet build --configuration Release
dotnet test --no-build --configuration Release  --logger 'trx' --logger 'console;verbosity=normal'
dotnet pack -o PublishOutput --configuration Release
