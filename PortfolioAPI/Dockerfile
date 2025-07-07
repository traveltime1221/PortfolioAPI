# 建置階段：用 .NET 8 SDK 編譯專案
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 複製 csproj 並還原套件
COPY PortfolioAPI/PortfolioAPI.csproj ./PortfolioAPI.csproj
RUN dotnet restore

# 複製所有原始碼並建置
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# 執行階段：用輕量 .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# 若之後需要 Python，可加入：
# RUN apt-get update && apt-get install -y python3 python3-pip

COPY --from=build /app/publish ./

# Render 預設開 port 10000，但你可以照習慣開 80
EXPOSE 80

# 啟動 API
ENTRYPOINT ["dotnet", "PortfolioAPI.dll"]
