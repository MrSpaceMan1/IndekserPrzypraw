FROM node:21-alpine AS build
COPY ./indekser_przypraw_frontend /opt/frontend
WORKDIR /opt/frontend
RUN ["npm", "i"]
RUN ["npx", "vite", "build", "--outDir", "/srv/indekser"]

FROM nginx:latest
COPY --from=build /srv/indekser /srv/indekser
COPY ./nginx_certs /etc/nginx/certs/
COPY ./nginx.conf /etc/nginx/nginx.conf

EXPOSE 443
EXPOSE 80