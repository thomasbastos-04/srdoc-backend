FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SrDoc.Api/SrDoc.Api.csproj", "SrDoc.Api/"]
RUN dotnet restore "SrDoc.Api/SrDoc.Api.csproj"
COPY . .
WORKDIR "/src/SrDoc.Api"
RUN dotnet build "SrDoc.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SrDoc.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SrDoc.Api.dll"]