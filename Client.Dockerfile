FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish Kubeleans.WebApp/Kubeleans.WebApp.csproj -c Release -o /app/out

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Kubeleans.WebApp.dll"]