dotnet ef --startup-project ../MicroServices.WebDebts.Api migrations add init --context DataContext

dotnet ef --startup-project ../MicroServices.WebDebts.Api database update --context DataContext

Payload 
{
  "name": "tv",
  "value": 2000.00,
  "date": "2020-10-14",
  "numberOfInstallments": 10,
  "debtType": "Installment"
}

docker build -t web-debt-api .


deploy

heroku container:push web -a web-debts

heroku container:release web -a web-debts


