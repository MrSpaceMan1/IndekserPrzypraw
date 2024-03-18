# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ../*.sln .
COPY ./*.csproj ./IndekserPrzypraw/
RUN dotnet restore ./IndekserPrzypraw

# copy everything else and build app
COPY ./ ./IndekserPrzypraw
WORKDIR /source/IndekserPrzypraw
# set to release
RUN dotnet publish -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "IndekserPrzypraw.dll"]
