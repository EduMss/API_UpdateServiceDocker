#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UpdateServiceDocker.csproj", "."]
RUN dotnet restore "./UpdateServiceDocker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./UpdateServiceDocker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UpdateServiceDocker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


# Instalar Docker CLI no contêiner
USER root

#RUN apt-get update && apt-get install -y \
#    apt-transport-https \
#    ca-certificates \
#    curl \
#    gnupg2 \
#    lsb-release \
#    software-properties-common



#RUN curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add -
#RUN add-apt-repository \
#    "deb [arch=amd64] https://download.docker.com/linux/debian \
#    $(lsb_release -cs) \
#    stable"
#RUN apt-get update && apt-get install -y docker-ce-cli
#RUN apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin



# Add Docker's official GPG key:
#RUN apt-get update
#RUN apt-get install -y ca-certificates curl
#RUN install -m 0755 -d /etc/apt/keyrings
#RUN curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc
#RUN chmod a+r /etc/apt/keyrings/docker.asc

# Add the repository to Apt sources:
#RUN echo \
#  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/debian \
#  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
#  RUN tee /etc/apt/sources.list.d/docker.list > /dev/null
#RUN apt-get update








#USER root

ENTRYPOINT ["dotnet", "UpdateServiceDocker.dll"]
