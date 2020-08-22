#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ScheduleJob.Core/ScheduleJob.Core.csproj", "ScheduleJob.Core/"]
COPY ["ScheduleJob.Core.IRepository/ScheduleJob.Core.IRepository.csproj", "ScheduleJob.Core.IRepository/"]
COPY ["ScheduleJob.Core.Contract/ScheduleJob.Core.Contract.csproj", "ScheduleJob.Core.Contract/"]
COPY ["ScheduleJob.Core.Utility/ScheduleJob.Core.Utility.csproj", "ScheduleJob.Core.Utility/"]
COPY ["ScheduleJob.Core.Services/ScheduleJob.Core.Services.csproj", "ScheduleJob.Core.Services/"]
COPY ["ScheduleJob.Core.IServices/ScheduleJob.Core.IServices.csproj", "ScheduleJob.Core.IServices/"]
COPY ["ScheduleJob.Core.Repository/ScheduleJob.Core.Repository.csproj", "ScheduleJob.Core.Repository/"]
RUN dotnet restore "ScheduleJob.Core/ScheduleJob.Core.csproj"
COPY . .
WORKDIR "/src/ScheduleJob.Core"
RUN dotnet build "ScheduleJob.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduleJob.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScheduleJob.Core.dll"]