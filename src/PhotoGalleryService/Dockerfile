FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

RUN apk update
RUN apk add --upgrade imagemagick

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080  

COPY out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "PhotoGalleryService.dll"]