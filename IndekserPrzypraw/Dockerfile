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

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet ef migrations bundle -o migration.exe -c SpicesContext
# set to release
RUN dotnet publish -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app ./
COPY --from=build /source/IndekserPrzypraw/appsettings.json ./
COPY --from=build /source/IndekserPrzypraw/migration.exe ./
COPY ./entrypoint.sh ./
ENTRYPOINT ["sh", "entrypoint.sh"]
