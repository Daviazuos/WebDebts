namespace MicroServices.WebDebts.Domain.Models.Enum
{
    public enum Status
    {
        Paid = 0,
        NotPaid = 1
    }

    public enum DebtStatus
    {
        Open = 0,
        Closed = 1
    }

    public enum DebtInstallmentType
    {
        Installment = 0,
        Fixed = 1,
        Simple = 2
    }

    public enum DebtType
    {
        Simple = 0,
        Card = 1
    }

    public enum WalletStatus
    {
        Enable = 0,
        Disable = 1
    }
}
