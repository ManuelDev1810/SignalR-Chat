FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY JobSityChat.UI.csproj ./
RUN dotnet restore "./JobSityChat.UI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "JobSityChat.UI.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "JobSityChat.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobSityChat.UI.dll"]