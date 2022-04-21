#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MockItUp.Console/MockItUp.Console.csproj", "MockItUp.Console/"]
COPY ["MockItUp.Common/MockItUp.Common.csproj", "MockItUp.Common/"]
COPY ["MockItUp.Core/MockItUp.Core.csproj", "MockItUp.Core/"]
RUN dotnet restore "MockItUp.Console/MockItUp.Console.csproj"
COPY . .
WORKDIR "/src/MockItUp.Console"
RUN dotnet build "MockItUp.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockItUp.Console.csproj" -c Release -o /app/publish

FROM base AS final
ENV GRPC_DNS_RESOLVER=native
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockItUp.Console.dll"]
