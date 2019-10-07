$migrationName = Read-Host -Prompt 'Enter new migration name'
dotnet ef migrations add $migrationName --project ../  --startup-project ../../PayrollProcessor -c PayrollDbContext 
"Done"