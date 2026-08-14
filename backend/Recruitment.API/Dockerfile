FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Recruitment.Domain/Recruitment.Domain.csproj Recruitment.Domain/
COPY Recruitment.Application/Recruitment.Application.csproj Recruitment.Application/
COPY Recruitment.Infrastructure/Recruitment.Infrastructure.csproj Recruitment.Infrastructure/
COPY Recruitment.API/Recruitment.API.csproj Recruitment.API/

RUN dotnet restore Recruitment.API/Recruitment.API.csproj

COPY . .
WORKDIR /src/Recruitment.API
RUN dotnet build Recruitment.API.csproj -c Release -o /app/build
RUN dotnet publish Recruitment.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Recruitment.API.dll"]
