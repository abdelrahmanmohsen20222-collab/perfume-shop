FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["PerfumeShopAPI.csproj", "."]
RUN dotnet restore "PerfumeShopAPI.csproj"
COPY . .
RUN dotnet build "PerfumeShopAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PerfumeShopAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PerfumeShopAPI.dll"]