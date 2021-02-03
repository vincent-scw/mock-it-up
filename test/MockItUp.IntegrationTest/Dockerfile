#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base

WORKDIR /test
COPY ["MockItUp.IntegrationTest/MockItUp.IntegrationTest.csproj", "MockItUp.IntegrationTest/"]
RUN dotnet restore "MockItUp.IntegrationTest/MockItUp.IntegrationTest.csproj"
COPY . .
WORKDIR "/test/MockItUp.IntegrationTest"
RUN dotnet build "MockItUp.IntegrationTest.csproj" -c Release -o /test/artifact

WORKDIR /test/artifact
ENTRYPOINT ["dotnet", "test", "MockItUp.IntegrationTest.dll"]