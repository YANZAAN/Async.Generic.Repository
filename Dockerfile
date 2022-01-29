FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /app

COPY ["src/NET.Repository/NET.Repository.csproj","./src/NET.Repository/"]
COPY ["src/NET.Repository.Tests/NET.Repository.Tests.csproj","./src/NET.Repository.Tests/"]

RUN dotnet restore "src/NET.Repository.Tests/NET.Repository.Tests.csproj"

COPY . .

RUN dotnet build "src/NET.Repository.Tests/NET.Repository.Tests.csproj" -c Release -o /app/build

FROM build AS test

WORKDIR /app

COPY --from=build /app/build .

ENTRYPOINT ["dotnet", "test", "NET.Repository.Tests.dll"]
