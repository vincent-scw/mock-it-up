#!/bin/bash
docker build -f ./src/MockItUp.Console/Dockerfile ./src
echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin
docker push edentidus/mockitup:v$VERSION_NUMBER
dotnet nuget push ./src/MockItUp.Client/bin/Release/MockItUp.Client.$VERSION_NUMBER.nupkg --api-key $NUGET_KEY --source https://api/nuget.org/v3/index.json
