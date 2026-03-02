# ============================================
# ÉTAPE 1 — Build
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

# Copie les dépendances d'abord (optimisation cache)
COPY *.csproj ./
RUN dotnet restore

# Copie tout le code
COPY . ./

# Compile pour la production
RUN dotnet publish -c Release -o /app/publish

# ============================================
# ÉTAPE 2 — Runtime
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copie seulement les fichiers compilés
COPY --from=build /app/publish .

EXPOSE 5120

ENTRYPOINT ["dotnet", "Conversion.API.dll"]