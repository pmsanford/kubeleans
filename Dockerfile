FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish Kubeleans.WebApp/Kubeleans.WebApp.csproj -c Release -o /app/webapp
RUN dotnet publish Kubeleans.Main/Kubeleans.Main.csproj -c release -o /app/main

FROM microsoft/aspnetcore:2.0
WORKDIR /app/webapp
COPY --from=build-env /app/webapp .
WORKDIR /app/main
COPY --from=build-env /app/main .
WORKDIR /app
ENTRYPOINT ["dotnet"]