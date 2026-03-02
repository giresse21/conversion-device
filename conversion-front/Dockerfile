# ============================================
# ÉTAPE 1 — Build
# ============================================
FROM node:22-alpine AS build

WORKDIR /app

# Copie des fichiers de dépendances (optimisation cache)
COPY package.json package-lock.json* ./
RUN npm ci

# Copie du code source
COPY . .

# Build de l'app (sortie dans dist/)
RUN npm run build

# ============================================
# ÉTAPE 2 — Runtime (serveur de fichiers statiques)
# ============================================
FROM nginx:alpine AS runtime

# Copie la config nginx (SPA : fallback sur index.html pour le routing)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copie les fichiers buildés depuis l'étape build
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
