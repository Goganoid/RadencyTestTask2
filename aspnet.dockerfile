# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./API/API.csproj ./API/
COPY ./Application/Application.csproj ./Application/
COPY ./Domain/Domain.csproj ./Domain/
COPY ./Persistence/Persistence.csproj ./Persistence/
COPY RadencyTestTask2.sln .
RUN dotnet restore --use-current-runtime -r linux-x64

# copy everything else and build app
COPY ./ .
RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained false --no-restore

RUN ls
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "API.dll"]
