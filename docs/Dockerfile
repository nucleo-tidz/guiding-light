#FROM nginx:alpine
#
## Remove default config (optional if you're using your own)
#RUN rm /etc/nginx/conf.d/default.conf
#
## Copy your static files
#COPY . /usr/share/nginx/html
#
## Optional: copy your own nginx.conf
## COPY nginx.conf /etc/nginx/nginx.conf
#
## Make sure NGINX can write to required paths
#RUN mkdir -p /var/run /var/cache/nginx && \
    #chmod -R 777 /var/run /var/cache/nginx /usr/share/nginx/html
#
## 👇 This forces it to run as root
#USER root
#EXPOSE 80

FROM nginxinc/nginx-unprivileged:1.27.0-alpine
COPY . /usr/share/nginx/html