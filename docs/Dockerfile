FROM nginx:alpine
RUN rm /etc/nginx/conf.d/default.conf
COPY . /usr/share/nginx/html
COPY . /usr/share/nginx/html

# Change permissions so NGINX can write to cache dirs
RUN chmod -R 755 /var/cache/nginx /usr/share/nginx/html

# Make sure user has permission
USER 0

#EXPOSE 80