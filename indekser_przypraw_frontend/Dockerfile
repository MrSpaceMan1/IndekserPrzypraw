FROM node:21-alpine
WORKDIR /app
RUN ["npm", "i", "@rollup/rollup-linux-x64-musl"]
RUN ["npm", "i", "--global", "vite"]
EXPOSE 5173
ENTRYPOINT [ "npm", "run", "dev" ]