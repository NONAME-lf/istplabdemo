# Базовий образ SDK для збірки
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Базовий образ SDK для збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо csproj і відновлюємо залежності
COPY ["ShopInfrastructure.csproj", "ShopInfrastructure/"]
RUN dotnet restore "ShopInfrastructure.csproj"

# Копіюємо весь код і збираємо
COPY . .
WORKDIR "/src/ShopInfrastructure"
RUN dotnet build "ShopInfrastructure.csproj" -c Release -o /app/build

# Публікуємо застосунок
FROM build AS publish
RUN dotnet publish "ShopInfrastructure.csproj" -c Release -o /app/publish

# Публікуємо в фінальному образі
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopInfrastructure.dll"]
