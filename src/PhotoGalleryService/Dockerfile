FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

RUN apk add curl
RUN apk --update add gcc make g++
RUN curl https://download.imagemagick.org/ImageMagick/download/ImageMagick.tar.gz --output ImageMagick.tar.gz
RUN tar -xzf ImageMagick.tar.gz && cd ImageMagick-* && ./configure --prefix /usr/local && make install
RUN rm -rf ImageMagick

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080  

COPY out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "PhotoGalleryService.dll"]