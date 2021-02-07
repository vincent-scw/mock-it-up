#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base

WORKDIR /
COPY ["src/mockctl.proto", "src/"]
COPY ["src/MockItUp.Client/MockItUp.Client.csproj", "src/MockItUp.Client/"]
COPY ["test/MockItUp.IntegrationTest/MockItUp.IntegrationTest.csproj", "test/MockItUp.IntegrationTest/"]
RUN dotnet restore "test/MockItUp.IntegrationTest/MockItUp.IntegrationTest.csproj"
COPY . .
WORKDIR "/test/MockItUp.IntegrationTest"
RUN dotnet build "MockItUp.IntegrationTest.csproj" -c Release -o /test/artifact

WORKDIR /test/artifact
ENTRYPOINT ["dotnet", "test", "MockItUp.IntegrationTest.dll"]