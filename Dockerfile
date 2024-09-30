ARG BUILD_IMAGE=mcr.microsoft.com/dotnet/sdk:8.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:8.0

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
 
RUN dotnet publish  \
    ./InvestorFlow.ContactManagement.API/InvestorFlow.ContactManagement.API.csproj  \
    --no-restore \
    -c Release  \
    -o /app/src/out
 
# Package stage
FROM ${RUNTIME_IMAGE}
WORKDIR /app

# Copy the published output from the build stage
COPY --from=builder /app/src/out .

# For documentation only
EXPOSE 8080

ENTRYPOINT ["dotnet", "InvestorFlow.ContactManagement.API.dll"]
