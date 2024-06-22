#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KdyWeb.CloudParse.SlefHost/KdyWeb.CloudParse.SelfHost.csproj", "KdyWeb.CloudParse.SlefHost/"]
COPY ["KdyWeb.HttpApi/KdyWeb.HttpApi.csproj", "KdyWeb.HttpApi/"]
COPY ["KdyWeb.CommonService/KdyWeb.CommonService.csproj", "KdyWeb.CommonService/"]
COPY ["KdyWeb.ICommonService/KdyWeb.ICommonService.csproj", "KdyWeb.ICommonService/"]
COPY ["KdyWeb.Dto/KdyWeb.Dto.csproj", "KdyWeb.Dto/"]
COPY ["KdyWeb.CloudParse/KdyWeb.CloudParse.csproj", "KdyWeb.CloudParse/"]
COPY ["KdyWeb.BaseInterface/KdyWeb.BaseInterface.csproj", "KdyWeb.BaseInterface/"]
COPY ["KdyWeb.Entity/KdyWeb.Entity.csproj", "KdyWeb.Entity/"]
COPY ["KdyWeb.Utility/KdyWeb.Utility.csproj", "KdyWeb.Utility/"]
COPY ["KdyWeb.PageParse/KdyWeb.PageParse.csproj", "KdyWeb.PageParse/"]
COPY ["KdyWeb.WebParse/KdyWeb.WebParse.csproj", "KdyWeb.WebParse/"]
COPY ["KdyWeb.IRepository/KdyWeb.IRepository.csproj", "KdyWeb.IRepository/"]
COPY ["KdyWeb.MiniProfiler/KdyWeb.MiniProfiler.csproj", "KdyWeb.MiniProfiler/"]
COPY ["KdyWeb.Repository/KdyWeb.Repository.csproj", "KdyWeb.Repository/"]
COPY ["KdyWeb.EntityFramework/KdyWeb.EntityFramework.csproj", "KdyWeb.EntityFramework/"]
COPY ["KdyWeb.Service.FileStore/KdyWeb.Service.FileStore.csproj", "KdyWeb.Service.FileStore/"]
COPY ["KdyWeb.IService.FileStore/KdyWeb.IService.FileStore.csproj", "KdyWeb.IService.FileStore/"]
COPY ["KdyWeb.Service/KdyWeb.Service.csproj", "KdyWeb.Service/"]
COPY ["Kdy.StandardJob/Kdy.StandardJob.csproj", "Kdy.StandardJob/"]
COPY ["KdyWeb.IService/KdyWeb.IService.csproj", "KdyWeb.IService/"]
COPY ["KdyWeb.IService.Selenium/KdyWeb.IService.Selenium.csproj", "KdyWeb.IService.Selenium/"]
COPY ["KdyWeb.Service.CloudParse/KdyWeb.Service.CloudParse.csproj", "KdyWeb.Service.CloudParse/"]
COPY ["KdyWeb.IService.CloudParse/KdyWeb.IService.CloudParse.csproj", "KdyWeb.IService.CloudParse/"]
COPY ["KdyWeb.Service.Selenium/KdyWeb.Service.Selenium.csproj", "KdyWeb.Service.Selenium/"]
RUN dotnet restore "KdyWeb.CloudParse.SlefHost/KdyWeb.CloudParse.SelfHost.csproj"
COPY . .
WORKDIR "/src/KdyWeb.CloudParse.SlefHost"
RUN dotnet build "KdyWeb.CloudParse.SelfHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KdyWeb.CloudParse.SelfHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KdyWeb.CloudParse.SelfHost.dll"]