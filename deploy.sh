#!/bin/bash
if [ "$TRAVIS_PULL_REQUEST" = "false" -a "$TRAVIS_BRANCH" = "main" ]; then
  docker build -t edentidus/mockitup:v$VERSION_NUMBER -t edentidus/mockitup:latest -f ./src/MockItUp.Console/Dockerfile .
  echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin
  docker push edentidus/mockitup:v$VERSION_NUMBER
  docker push edentidus/mockitup:latest
  dotnet nuget push src/MockItUp.Client\bin\Release\MockItUp.Client.$VERSION_NUMBER.nupkg --api-key $NUGET_KEY --source https://api/nuget.org/v3/index.json
fi