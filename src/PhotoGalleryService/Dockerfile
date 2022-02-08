FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
RUN apk add curl
RUN \
curl https://www.imagemagick.org/download/ImageMagick.tar.gz && \
tar -xzf ImageMagick.tar.gz && \
cd ImageMagick && \
./configure --prefix /usr/local && \
make install && \
cd .. && \
rm -rf  ImageMagick

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080  

COPY out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "PhotoGalleryService.dll"]