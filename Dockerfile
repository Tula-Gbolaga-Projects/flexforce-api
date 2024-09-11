FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY *.csproj ./

RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

ENV ASPNETCORE_URLS=http://+8080

ENTRYPOINT ["dotnet", "agency-portal-api.dll"]

