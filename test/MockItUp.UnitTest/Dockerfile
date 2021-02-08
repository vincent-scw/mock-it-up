#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["test/MockItUp.UnitTest/MockItUp.UnitTest.csproj", "test/MockItUp.UnitTest/"]
COPY ["src/MockItUp.Common/MockItUp.Common.csproj", "src/MockItUp.Common/"]
COPY ["src/MockItUp.Core/MockItUp.Core.csproj", "src/MockItUp.Core/"]
RUN dotnet restore "test/MockItUp.UnitTest/MockItUp.UnitTest.csproj"
COPY . .
WORKDIR "/src/test/MockItUp.UnitTest"
RUN dotnet build "MockItUp.UnitTest.csproj" -c Release -o /test/artifact

WORKDIR /test/artifact
ENTRYPOINT ["dotnet", "test", "MockItUp.UnitTest.dll"]