ARG BUILD_IMAGE=mcr.microsoft.com/dotnet/sdk:8.0-alpine

## Build stage
FROM ${BUILD_IMAGE} AS builder
WORKDIR /app/src

# Copy all the layers csproj files into respective folders
COPY ["src/InvestorFlow.ContactManagement.API/InvestorFlow.ContactManagement.API.csproj", "InvestorFlow.ContactManagement.API/"]
COPY ["src/InvestorFlow.ContactManagement.Domain/InvestorFlow.ContactManagement.Domain.csproj", "InvestorFlow.ContactManagement.Domain/"]
COPY ["src/InvestorFlow.ContactManagement.Application/InvestorFlow.ContactManagement.Application.csproj", "InvestorFlow.ContactManagement.Application/"]
COPY ["src/InvestorFlow.ContactManagement.Infrastructure/InvestorFlow.ContactManagement.Infrastructure.csproj", "InvestorFlow.ContactManagement.Infrastructure/"]

# Restore over API project - this pulls restore over the dependent projects as well
RUN dotnet restore "InvestorFlow.ContactManagement.API/InvestorFlow.ContactManagement.API.csproj"

# Copy application code
COPY src .
 
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet ef database update \
    --project InvestorFlow.ContactManagement.Infrastructure/InvestorFlow.ContactManagement.Infrastructure.csproj \
    --startup-project InvestorFlow.ContactManagement.API/InvestorFlow.ContactManagement.API.csproj \
    --context InvestorFlow.ContactManagement.Infrastructure.Persistence.AppDbContext \
    --configuration Debug  \
    20240925104655_Initial
