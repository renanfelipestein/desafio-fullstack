# Stage 1: Build
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .

# Recebe o ARG e expõe como ENV para o Vite
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL

RUN npm run build-only

# Stage 2: Serve with Nginx
FROM nginx:alpine AS production
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80