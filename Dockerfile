# build step
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["src/TechDemo.Api/TechDemo.Api.csproj", "TechDemo.Api/"]
COPY ["src/TechDemo.Application/TechDemo.Application.csproj", "TechDemo.Application/"]
COPY ["src/TechDemo.Domain/TechDemo.Domain.csproj", "TechDemo.Domain/"]
COPY ["src/TechDemo.Infrastructure/TechDemo.Infrastructure.csproj", "TechDemo.Infrastructure/"]

RUN dotnet restore "TechDemo.Api/TechDemo.Api.csproj"

COPY src/ ./

WORKDIR /src/TechDemo.Api

RUN dotnet build "TechDemo.Api.csproj" -c Release -o /app/build

# publish step
FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 5001
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechDemo.Api.dll"]