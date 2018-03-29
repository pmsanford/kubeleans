FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish Kubeleans.Main/Kubeleans.Main.csproj -c Release -o /app/out

FROM microsoft/dotnet:runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Kubeleans.Main.dll"]