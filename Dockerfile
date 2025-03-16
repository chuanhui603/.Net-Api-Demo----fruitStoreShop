# 使用官方的 .NET SDK 映像來構建應用程式
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# 複製 csproj 並作為獨立層進行還原
COPY *.csproj ./
RUN dotnet restore

# 複製所有其他文件並構建應用程式
COPY . ./
RUN dotnet publish -c Release -o out

# 構建運行時映像
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# 暴露應用程式運行的端口
EXPOSE 80

# 定義容器的入口點
ENTRYPOINT ["dotnet", "水水水果API.dll"]
