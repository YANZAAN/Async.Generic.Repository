FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /app

COPY ["GenericRepository/GenericRepository.csproj","./GenericRepository/"]
COPY ["GenericRepository.Tests/GenericRepository.Tests.csproj","./GenericRepository.Tests/"]

RUN dotnet restore "GenericRepository.Tests/GenericRepository.Tests.csproj"

COPY . .

RUN dotnet build "GenericRepository.Tests/GenericRepository.Tests.csproj" -c Release -o /app/build

FROM build AS test

WORKDIR /app

COPY --from=build /app/build .

ENTRYPOINT ["dotnet", "test", "GenericRepository.Tests.dll"]
