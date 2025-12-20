FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SrDoc.Api.csproj", "./"]
RUN dotnet restore "./SrDoc.Api.csproj"

COPY . .
RUN dotnet publish "SrDoc.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "SrDoc.Api.dll"]