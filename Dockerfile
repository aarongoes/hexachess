# Step 1: Use the official .NET Core SDK image to build the app
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Optional: Clear NuGet package cache to avoid issues with cached packages
RUN dotnet nuget locals all --clear

# Copy the solution and project files
COPY Hexachess.sln ./
COPY Hexachess/Hexachess.csproj ./Hexachess/
COPY Model/Model.csproj ./Model/
COPY Repository/Repository.csproj ./Repository/
COPY DAL/DAL.csproj ./DAL/
COPY Factory/Factory.csproj ./Factory/
COPY Logic/Logic.csproj ./Logic/
COPY Tests/Tests.csproj ./Tests/

# Restore all dependencies for the solution
RUN dotnet restore Hexachess.sln

# Copy the entire source code after restoring dependencies
COPY . ./

# Publish the app and ensure all dependencies are included in the output folder
RUN dotnet publish Hexachess/Hexachess.csproj -c Release -o /out

# Step 2: Use the ASP.NET Core runtime image for running the app
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-env /out .

# Expose port 80 for the application
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "Hexachess.dll"]
