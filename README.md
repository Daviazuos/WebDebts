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
