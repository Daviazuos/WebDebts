dotnet ef --startup-project ../MicroServices.WebDebts.Api migrations add init --context DataContext

dotnet ef --startup-project ../MicroServices.WebDebts.Api database update --context DataContext

deploy

heroku container:push web -a web-debts

heroku container:release web -a web-debts


