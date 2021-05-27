FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Team12/Team12.csproj", "Team12/"]
RUN dotnet restore "Team12/Team12.csproj"
COPY . .
WORKDIR "/src/Team12"
RUN dotnet build "Team12.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Team12.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Team12.dll"]
