# Minimal API

## Pull SQL Server 202 container image from Microsoft Container Registry
docker pull mcr.microsoft.com/mssql/server:2022-latest

## Run 
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<password>" `
   -p 1433:1433 --name sql1 --hostname sql1 `
   -d `
   mcr.microsoft.com/mssql/server:2022-latest

## Update Database
Add-Migration InitialCreate
Update-Database

## References
https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&preserve-view=true&tabs=cli&pivots=cs1-powershell
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs