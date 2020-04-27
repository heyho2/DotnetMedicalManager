FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 59602
EXPOSE 44321

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Host/Host.csproj", "Host/"]
RUN dotnet restore "Host/Host.csproj"
COPY . .
WORKDIR "/src/Host"
RUN dotnet build "Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Host.dll"]