#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/MicroServices.WebDebts.Api/MicroServices.WebDebts.Api.csproj", "src/MicroServices.WebDebts.Api/"]
COPY ["src/MicroServices.WebDebts.DependencyInjection/MicroServices.WebDebts.DependencyInjection.csproj", "src/MicroServices.WebDebts.DependencyInjection/"]
COPY ["src/MicroServices.WebDebts.Domain/MicroServices.WebDebts.Domain.csproj", "src/MicroServices.WebDebts.Domain/"]
COPY ["src/MicroServices.WebDebts.Application/MicroServices.WebDebts.Application.csproj", "src/MicroServices.WebDebts.Application/"]
COPY ["src/MicroServices.WebDebts.Infrastructure/MicroServices.WebDebts.Infrastructure.csproj", "src/MicroServices.WebDebts.Infrastructure/"]
RUN dotnet restore "src/MicroServices.WebDebts.Api/MicroServices.WebDebts.Api.csproj"
COPY . .
WORKDIR "/src/src/MicroServices.WebDebts.Api"
RUN dotnet build "MicroServices.WebDebts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroServices.WebDebts.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "MicroServices.WebDebts.Api.dll"]

CMD ASPNETCORE_URLS=http://*:$PORT dotnet MicroServices.WebDebts.Api.dll