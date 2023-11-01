FROM mcr.microsoft.com/dotnet/aspnet:7.0.2-alpine3.17 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.102-alpine3.17 AS build
WORKDIR /app
COPY ["int3306.Api", "int3306.Api"]
COPY ["int3306.Entities", "int3306.Entities"]
COPY ["int3306.Repository", "int3306.Repository"]
RUN dotnet publish int3306.Api -c Release -o /app/publish

FROM base AS final
ARG PORT
ENV PORT=$PORT
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["./int3306.Api"]
