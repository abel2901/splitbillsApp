# --- Stage 1: Build ---
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copia apenas o csproj para restaurar dependências primeiro
COPY *.csproj ./
RUN dotnet restore

# Copia todo o restante do código
COPY . ./

# Publica a aplicação (sem --no-restore)
RUN dotnet publish -c Release -o out
