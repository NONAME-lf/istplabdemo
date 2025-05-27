# Базовий образ ASP.NET для запуску
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Образ SDK для побудови застосунку
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо тільки поточну папку, де зберігається Dockerfile і проект
COPY . .

# Відновлюємо залежності та будуємо
RUN dotnet restore "ShopInfrastructure.csproj"
RUN dotnet build "ShopInfrastructure.csproj" -c Release -o /app/build

# Публікуємо застосунок
FROM build AS publish
RUN dotnet publish "ShopInfrastructure.csproj" -c Release -o /app/publish

# Фінальний образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopInfrastructure.dll"]
