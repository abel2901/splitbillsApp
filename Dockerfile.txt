# --- Stage 1: Build ---
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

# Copia csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia tudo para dentro do container
COPY . ./

# Publica a aplicação
RUN dotnet publish -c Release -o out

# --- Stage 2: Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app
COPY --from=build /app/out ./

# Expõe a porta definida pelo Railway (via variável PORT)
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=true

# Define a porta dinâmica do Railway
ENV ASPNETCORE_URLS=http://+:$PORT

# Inicia a aplicação
ENTRYPOINT ["dotnet", "SplitBillsApi.dll"]
