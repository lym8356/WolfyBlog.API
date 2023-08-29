#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
#COPY ["WolfyBlog.API/WolfyBlog.API.csproj", "WolfyBlog.API/"]
COPY ["WolfyBlog.API.csproj", "./"]
#RUN dotnet restore "WolfyBlog.API/WolfyBlog.API.csproj"
RUN dotnet restore "./WolfyBlog.API.csproj"
COPY . .
#WORKDIR "/src/WolfyBlog.API"
WORKDIR "/src/."
RUN dotnet build "WolfyBlog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WolfyBlog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WolfyBlog.API.dll"]
