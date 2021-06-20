FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/CustomerManager.Api/CustomerManager.Api.csproj", "src/CustomerManager.Api/"]
COPY ["src/CustomerManager.Services/CustomerManager.Services.csproj", "src/CustomerManager.Services/"]
COPY ["src/CustomerManager.Repositories/CustomerManager.Repositories.csproj", "src/CustomerManager.Repositories/"]
COPY ["src/CustomerManager.Models/CustomerManager.Models.csproj", "src/CustomerManager.Models/"]
RUN dotnet restore "src/CustomerManager.Api/CustomerManager.Api.csproj"
COPY . .
WORKDIR "/src/src/CustomerManager.Api"
RUN dotnet build "CustomerManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerManager.Api.dll"]