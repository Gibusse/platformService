FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build-env
WORKDIR /source

COPY *.csproj .
RUN dotnet restore --use-current-runtime

COPY . ./
RUN dotnet publish --use-current-runtime -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:7.0-jammy
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT [ "dotnet", "PlatformService.dll" ]
